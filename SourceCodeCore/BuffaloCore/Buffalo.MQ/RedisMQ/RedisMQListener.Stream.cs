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
                StreamInfo info = db.StreamInfo(pkey);
                if (_config.LoadNoAck)
                {
                    LoadPendingMessages(db, pkey);
                }
                LoadMQMessage(db,pkey,timeout);

            }

        }
        /// <summary>
        /// 修建话题长度
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pkey"></param>
        private void XTrimMaxLength(IDatabase db, string pkey)
        {
            if (_config.TopicMaxLength <= 0) 
            {
                return;
            }
            db.Execute("XTRIM", new object[] { pkey, "MAXLEN", _config.TopicMaxLength });
        }

        /// <summary>
        /// 加载消息
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="pkey">话题键</param>
        /// <param name="timeout">等待时间</param>
        private void LoadPendingMessages(IDatabase db, string pkey)
        {
            List<RedisValue> lstMessageId = null;
            do
            {
                lstMessageId = new List<RedisValue>();
                foreach (var pending in db.StreamPendingMessages(pkey, _config.ConsumerGroupName, _config.StreamPageSize, _config.ConsumerName)) // 获取所有消费者的未确认消息
                {
                    lstMessageId.Add(pending.MessageId);
                }
                if (lstMessageId.Count <= 0) 
                {
                    break;
                }
                var claimedMessages = db.StreamClaim(pkey, _config.ConsumerGroupName, _config.ConsumerName, 0,
                    lstMessageId.ToArray());

                foreach (var message in claimedMessages)
                {

                    foreach (var field in message.Values)
                    {
                        if (field.Value.IsNull)
                        {
                            CommitEmptyMessageId(db, pkey, message.Id);
                            continue;
                        }
                        RedisCallbackMessage mess = new RedisCallbackMessage(pkey, field.Value,
                                       db, _config.ConsumerGroupName, message.Id, _config.CommanfFlags);
                        mess.IsOldMessage= true;
                        CallBack(mess).Wait();
                    }

                }
            }while (lstMessageId.Count>= _config.StreamPageSize);

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
            int trimTime = _config.XTrimTimeout;
            byte[] svalue = null;
            DateTime dtNow = DateTime.Now;
            RedisResult res = null;
            object[] argsArr = new object[] { "GROUP", _config.ConsumerGroupName, _config.ConsumerName, "COUNT", _config.StreamPageSize,
                "BLOCK",timeout,"STREAMS",pkey,_config.ReadGroupPosition};//构建参数
            while (_pollrunning)
            {
                try
                {
                    dtNow= DateTime.Now;
                    if (dtNow.Subtract(lastTrimLen).TotalMilliseconds> trimTime) 
                    {
                        XTrimMaxLength(db, pkey);
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
                           

                            for (int i = 0; i < fields.Length; i += 2)
                            {
                                RedisResult field = fields[i];
                                RedisResult value = fields[i + 1];
                                if (IsResultObjectNull(field) || IsResultObjectNull(value))
                                {
                                    CommitEmptyMessageId(db,pkey, messageId);
                                    continue;
                                }
                                svalue = (byte[])value;
                                if (svalue == null)
                                {
                                    CommitEmptyMessageId(db, pkey, messageId);
                                    continue;
                                }
                                RedisCallbackMessage mess = new RedisCallbackMessage(streamKey, svalue,
                                    db, _config.ConsumerGroupName, messageId, _config.CommanfFlags);
                                CallBack(mess).Wait();
                            }
                        }
                    }
                }

                catch (Exception e)
                {
                    OnException(e).Wait();
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
            RedisCallbackMessage messEmpty = new RedisCallbackMessage(pkey, EmpeyByte, db, _config.ConsumerGroupName, messageId,
                                    _config.CommanfFlags);

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
                RedisCallbackMessage mess = new RedisCallbackMessage(pkey, svalue, db, _config.ConsumerGroupName, entry.Id,
                                _config.CommanfFlags);
                CallBack(mess).Wait();

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
