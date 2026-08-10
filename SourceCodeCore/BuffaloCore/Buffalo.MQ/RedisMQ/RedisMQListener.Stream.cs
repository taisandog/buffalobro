using Buffalo.Kernel;
using Buffalo.Kernel.Collections;
using Buffalo.Kernel.TreadPoolManager;
using Confluent.Kafka;
using MQTTnet.Internal;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    public partial class RedisMQListener
    {
        
        /// <summary>
        /// Stream方式监听
        /// </summary>
        /// <param name="objKeys"></param>
        public void DoStreamListening(object objKeys)
        {
            string listenKey = objKeys as string;
            if (string.IsNullOrWhiteSpace(listenKey))
            {
                return;
            }
            int timeout = _config.PollingInterval;
            if (timeout < 1000)
            {
                timeout = 1000;
            }

            _config.Options.SyncTimeout = (timeout ) + 2000;
            string pkey = listenKey;

            using (ConnectionMultiplexer connection = RedisMQConnection.CreateManager(_config.Options))//必须开启独立连接进行监听，否则会堵塞其他指令
            {
                _queRedis.Enqueue(connection);
                IDatabase db = connection.GetDatabase(_config.UseDatabase);
                bool newGroup = false;
                //强行创建组
                try
                {
                    newGroup= db.StreamCreateConsumerGroup(pkey, _config.ConsumerGroupName,
                        _config.ConsumerGroupPosition, _config.CommanfFlags);
                }
                catch (Exception ex) 
                {
                    newGroup = false;
                }
                if (newGroup)
                {
                    DoNewGroup(db, pkey);
                }
                if (_config.RetentionPolicy.CleanupMode == MQCleanupMode.DeleteOnAck)
                {
                    RedisStreamRetention.EnsureDeleteOnAckAllowedAsync(db, pkey,
                        _config.ConsumerGroupName, _config.CommanfFlags)
                        .GetAwaiter().GetResult();
                }
                LoadMQMessage(db,pkey,timeout);

            }

        }
        /// <summary>
        /// 修建话题长度
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pkey"></param>
        private void ApplyRetentionPolicy(IDatabase db, string pkey)
        {
            RedisStreamRetention.ApplyAsync(db, pkey, _config.RetentionPolicy,
                _config.CommanfFlags).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 加载消息
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="pkey">话题键</param>
        /// <param name="timeout">等待时间</param>
        private void LoadPendingMessages(IDatabase db, string pkey)
        {
            RedisValue nextStartId = "0-0";
            do
            {
                StreamAutoClaimResult claimResult = db.StreamAutoClaim(
                    pkey,
                    _config.ConsumerGroupName,
                    _config.ConsumerName,
                    _config.RetryOptions.AckTimeoutMilliseconds,
                    nextStartId,
                    _config.StreamPageSize,
                    _config.CommanfFlags);
                if (claimResult.IsNull)
                {
                    break;
                }
                nextStartId = claimResult.NextStartId;
                foreach (StreamEntry message in claimResult.ClaimedEntries)
                {
                    byte[] body = GetStreamBody(message);
                    if (body == null)
                    {
                        CommitEmptyMessageId(db, pkey, message.Id);
                        continue;
                    }
                    int deliveryCount = GetDeliveryCount(db, pkey, message.Id);
                    RedisCallbackMessage mess = CreateStreamMessage(
                        db, pkey, message.Id, body, deliveryCount, true);
                    ApplyStreamMetadata(mess, message.Values);
                    if (_config.RetryOptions.DeadLetterEnabled &&
                        deliveryCount > _config.RetryOptions.MaxDeliveryCount)
                    {
                        mess.DeadLetterAsync("未确认消息超过最大投递次数")
                            .GetAwaiter().GetResult();
                    }
                    else
                    {
                        CallBack(mess).GetAwaiter().GetResult();
                    }
                }
            } while (_pollrunning && nextStartId != "0-0");

        }

        private int GetDeliveryCount(IDatabase db, string pkey, RedisValue messageId)
        {
            StreamPendingMessageInfo[] pending = db.StreamPendingMessages(
                pkey, _config.ConsumerGroupName, 1, _config.ConsumerName,
                messageId, messageId, _config.CommanfFlags);
            if (pending.Length == 0)
            {
                return 1;
            }
            return Math.Max(1, pending[0].DeliveryCount);
        }

        private byte[] GetStreamBody(StreamEntry entry)
        {
            RedisValue value = entry[_config.DefaultStreamDataKey];
            if (!value.IsNull)
            {
                return (byte[])value;
            }
            foreach (NameValueEntry field in entry.Values)
            {
                if (!field.Name.ToString().StartsWith("bufmq.", StringComparison.Ordinal) &&
                    !field.Value.IsNull)
                {
                    return (byte[])field.Value;
                }
            }
            return null;
        }

        private RedisCallbackMessage CreateStreamMessage(IDatabase db, string topic,
            RedisValue messageId, byte[] body, int deliveryCount, bool oldMessage)
        {
            RedisCallbackMessage message = new RedisCallbackMessage(topic, body, db,
                _config.ConsumerGroupName, messageId, _config.CommanfFlags,
                _config.RetryOptions.DeadLetterSuffix, _config.DefaultStreamDataKey,
                deliveryCount,
                _config.RetentionPolicy.CleanupMode == MQCleanupMode.DeleteOnAck);
            message.IsOldMessage = oldMessage;
            message.IsRedelivered = oldMessage || deliveryCount > 1;
            return message;
        }

        private static void ApplyStreamMetadata(RedisCallbackMessage message,
            NameValueEntry[] values)
        {
            foreach (NameValueEntry value in values)
            {
                string name = value.Name.ToString();
                if (name == "bufmq.originalTopic")
                {
                    message.OriginalTopic = value.Value.ToString();
                }
                else if (name == "bufmq.originalMessageId")
                {
                    message.OriginalMessageId = value.Value.ToString();
                }
                else if (name == "bufmq.failureReason")
                {
                    message.DeadLetterReason = value.Value.ToString();
                }
            }
        }
        /// <summary>
        /// 加载消息
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="pkey">话题键</param>
        /// <param name="timeout">等待时间</param>
        private void LoadMQMessage(IDatabase db ,string pkey, int timeout) 
        {
            DateTime lastTrimLen = DateTime.MinValue;
            DateTime nextPendingScan = DateTime.UtcNow;
            int trimTime = _config.RetentionPolicy.CleanupMode == MQCleanupMode.MaxLength ||
                _config.RetentionPolicy.CleanupMode == MQCleanupMode.MaxAge
                ? (int)Math.Min(int.MaxValue,
                    _config.RetentionPolicy.CleanupInterval.TotalMilliseconds)
                : 0;
            DateTime dtNow = DateTime.Now;
            RedisResult res = null;
            object[] argsArr = new object[] { "GROUP", _config.ConsumerGroupName, _config.ConsumerName, "COUNT", _config.StreamPageSize,
                "BLOCK",timeout,"STREAMS",pkey,_config.ReadGroupPosition};//构建参数
            while (_pollrunning)
            {
                try
                {
                    dtNow= DateTime.Now;
                    if (_config.LoadNoAck && DateTime.UtcNow >= nextPendingScan)
                    {
                        LoadPendingMessages(db, pkey);
                        nextPendingScan = DateTime.UtcNow.AddMilliseconds(
                            _config.RetryOptions.PendingScanIntervalMilliseconds);
                    }
                    if (trimTime > 0 &&
                        dtNow.Subtract(lastTrimLen).TotalMilliseconds > trimTime)
                    {
                        ApplyRetentionPolicy(db, pkey);
                        lastTrimLen = dtNow;
                    }
                    res = db.Execute("XREADGROUP", argsArr);
                    
                    if (res == null || res.IsNull)
                    {
                        continue;
                    }

                    var streams = (RedisResult[])res; // 外层数组：流键和消息
                    foreach (var stream in streams)
                    {
                        var streamData = (RedisResult[])stream; // 流键和消息列表
                        if (IsResultNull(streamData, 2))
                        {
                            continue;
                        }

                        var streamKey = (string)streamData[0]; // 流键
                        var messages = (RedisResult[])streamData[1]; // 消息数组


                        foreach (var message in messages)
                        {
                            var messageData = (RedisResult[])message; // 消息 ID 和字段值
                            if (IsResultNull(messageData, 2))
                            {
                                continue;
                            }
                            var messageId = (string)messageData[0]; // 消息 ID
                            var fields = (RedisResult[])messageData[1]; // 字段值对
                           

                            byte[] messageBody = null;
                            byte[] fallbackBody = null;
                            string originalTopic = null;
                            string originalMessageId = null;
                            string deadLetterReason = null;
                            for (int i = 0; i + 1 < fields.Length; i += 2)
                            {
                                RedisResult field = fields[i];
                                RedisResult value = fields[i + 1];
                                if (IsResultObjectNull(field) || IsResultObjectNull(value))
                                {
                                    continue;
                                }
                                string fieldName = (string)field;
                                if (fieldName == "bufmq.originalTopic")
                                {
                                    originalTopic = (string)value;
                                    continue;
                                }
                                if (fieldName == "bufmq.originalMessageId")
                                {
                                    originalMessageId = (string)value;
                                    continue;
                                }
                                if (fieldName == "bufmq.failureReason")
                                {
                                    deadLetterReason = (string)value;
                                    continue;
                                }
                                byte[] fieldValue = (byte[])value;
                                if (fieldValue == null)
                                {
                                    continue;
                                }
                                if (string.Equals(fieldName, _config.DefaultStreamDataKey,
                                    StringComparison.Ordinal))
                                {
                                    messageBody = fieldValue;
                                }
                                else if (!fieldName.StartsWith("bufmq.",
                                    StringComparison.Ordinal) && fallbackBody == null)
                                {
                                    fallbackBody = fieldValue;
                                }
                            }
                            messageBody ??= fallbackBody;
                            if (messageBody == null)
                            {
                                CommitEmptyMessageId(db, pkey, messageId);
                                continue;
                            }
                            RedisCallbackMessage mess = CreateStreamMessage(db, streamKey,
                                messageId, messageBody, 1, false);
                            mess.OriginalTopic = originalTopic;
                            mess.OriginalMessageId = originalMessageId;
                            mess.DeadLetterReason = deadLetterReason;
                            CallBack(mess).GetAwaiter().GetResult();
                        }
                    }
                }

                catch (Exception e)
                {
                    OnException(e).GetAwaiter().GetResult();
                    Thread.Sleep(300);
                }
            }
        }

        private byte[] EmpeyByte=new byte[0];
        /// <summary>
        /// 提交空的消息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pkey"></param>
        /// <param name="messageId"></param>
        private void CommitEmptyMessageId(IDatabase db, string pkey,string messageId) 
        {
            RedisCallbackMessage messEmpty = CreateStreamMessage(db, pkey, messageId,
                EmpeyByte, 1, false);

            messEmpty.Commit();
        }
        /// <summary>
        /// 发现新组让组注册到这个话题
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pkey"></param>
        private void DoNewGroup(IDatabase db, string pkey)
        {

            //插入一条起始数据让新话题不出错
           RedisValue newVal = db.StreamAdd(pkey, new NameValueEntry[]
           {
               new NameValueEntry(_config.DefaultStreamDataKey, new byte[]{ })
           });
            //直接出掉
            StreamEntry[] entries = db.StreamReadGroup(
                     pkey,
                     _config.ConsumerGroupName,
                     _config.ConsumerName,
                     _config.ReadGroupPosition, // 从未处理的消息开始读取
                     count:1 // 每次读取1条
                     );
            RedisValue tmpval = RedisValue.Null;
            byte[] svalue = null;

            foreach (StreamEntry entry in entries) 
            {
                tmpval = entry[_config.DefaultStreamDataKey];
                svalue = tmpval;
                if (svalue == null || svalue.Length <= 0)
                {
                    CommitEmptyMessageId(db, pkey, entry.Id);
                    continue;
                }
                RedisCallbackMessage mess = CreateStreamMessage(db, pkey, entry.Id,
                    svalue, 1, false);
                CallBack(mess).GetAwaiter().GetResult();

            }
            
        }
        /// <summary>
        /// 结果是否为空
        /// </summary>
        /// <param name="res">结果集</param>
        /// <param name="valLength">结果集最少长度</param>
        /// <returns></returns>
        private bool IsResultNull(RedisResult[] res,int valLength) 
        {
            if ((valLength> res.Length))
            {
                return true;
            }
            foreach (var result in res) 
            {
                if (IsResultObjectNull(result))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 结果是否为空
        /// </summary>
        /// <param name="res">结果集</param>
        /// <param name="valLength">结果集最少长度</param>
        /// <returns></returns>
        private bool IsResultObjectNull(RedisResult res)
        {

            if (res == null || res.IsNull)
            {
                return true;
            }

            return false;
        }

        
    }
}
