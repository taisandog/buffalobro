using Buffalo.ArgCommon;
using Buffalo.Kernel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    public partial class RedisMQConnection : MQConnection
    {

        private ConnectionMultiplexer _redis = null;
        /// <summary>
        /// 发布者
        /// </summary>
        ISubscriber _subscriber;
        /// <summary>
        /// 配置
        /// </summary>
        RedisMQConfig _config;
        
        private Queue<MQRedisMessage> _que = null;
        private IDatabase _db;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RedisMQConnection(RedisMQConfig config)
        {
            _config = config;
        }


        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="options">连接字符串</param>
        /// <returns></returns>
        internal static ConnectionMultiplexer CreateManager(ConfigurationOptions options)
        {
            
            return ConnectionMultiplexer.Connect(options);
        }

        /// <summary>
        /// 打来连接
        /// </summary>
        protected override void Open()
        {
            if (_redis == null)
            {
                _redis = CreateManager(_config.Options);
            }
            if (_config.Mode == RedisMQMessageMode.Subscriber)
            {
                if (_subscriber == null)
                {
                    _subscriber = _redis.GetSubscriber();
                }
            }
        }
        /// <summary>
        /// 获取Redis操作类
        /// </summary>
        /// <returns></returns>
        private IDatabase GetDB()
        {
            if (_db == null)
            {
                _db = _redis.GetDatabase(_config.UseDatabase);
            }
            return _db;
        }


        public override bool IsOpen
        {
            get
            {
                return _redis != null;
            }


        }

        protected override APIResault SendMessage(MQSendMessage mess)
        {
            if (_que != null)
            {
                _que.Enqueue(mess as MQRedisMessage);
            }
            else
            {
                SendToPublic(mess.Topic, mess.Value);
            }

            return ApiCommon.GetSuccess();
        }

        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected override APIResault SendMessage(string topic, byte[] body)
        {
            RedisValue value = body;
            if (_que != null)
            {
                MQRedisMessage mess = new MQRedisMessage(topic, body);
                
                _que.Enqueue(mess);
            }
            else
            {
                SendToPublic(topic, body);
            }

            return ApiCommon.GetSuccess();
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        private void SendToPublic(string topic, byte[] body)
        {
            if (_config.Mode == RedisMQMessageMode.Subscriber)
            {
                if (_config.SaveToQueue)
                {

                    string key = _config.GetDefaultQueueKey(topic);
                    IDatabase db = GetDB();
                    db.ListLeftPush(key, body);
                    _subscriber.Publish(topic, RedisMQConfig.PublicTag, _config.CommanfFlags);
                }
                else
                {
                    _subscriber.Publish(topic, body, _config.CommanfFlags);
                }
            }
            else 
            {
                string key = _config.GetDefaultQueueKey(topic);
                IDatabase db = GetDB();
                db.ListLeftPush(key, body);
            }
        }
        ///// <summary>
        ///// 删除队列(Rabbit可用)
        ///// </summary>
        ///// <param name="queueName">队列名，如果为null则全删除</param>
        //public override void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        //{
            
        //}
        ///// <summary>
        ///// 删除交换器
        ///// </summary>

        //public override void DeleteTopic(bool ifUnused)
        //{
            
        //}

        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            if (_db != null)
            {
                
                _db = null;
            }
            if (_redis != null)
            {
                _redis.Close();
                _redis.Dispose();
                _redis = null;
            }
            if (_que != null)
            {
                _que.Clear();
            }
            _que = null;
            _subscriber = null;
        }

        


        protected override APIResault StartTran()
        {
            _que = new Queue<MQRedisMessage>();
            return ApiCommon.GetSuccess();
        }

        protected override APIResault CommitTran()
        {
            if (_que != null)
            {
                MQRedisMessage mess = null;
                while (_que.Count > 0)
                {
                    mess = _que.Dequeue();
                    SendToPublic(mess.Topic, mess.Value);

                }
            }
            return ApiCommon.GetSuccess();
        }

        protected override APIResault RoolbackTran()
        {
            if (_que != null)
            {
                _que.Clear();
            }
            return ApiCommon.GetSuccess();
        }

        ~RedisMQConnection()
        {
            Close();
            
        }
    }
}
