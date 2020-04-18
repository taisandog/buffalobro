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
        /// <param name="connectionString">连接字符串</param>
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
            if (_subscriber == null)
            {
                _subscriber = _redis.GetSubscriber();
            }
        }



        public override bool IsOpen
        {
            get
            {
                return _redis != null;
            }


        }

        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected override APIResault SendMessage(string routingKey, byte[] body)
        {
            RedisValue value = body;

            _subscriber.Publish(routingKey, value, _config.CommanfFlags);


            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 删除队列(Rabbit可用)
        /// </summary>
        /// <param name="queueName">队列名，如果为null则全删除</param>
        public override void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        {
            
        }
        /// <summary>
        /// 删除交换器
        /// </summary>

        public override void DeleteTopic(bool ifUnused)
        {
            
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        protected override void Close()
        {
            if (_redis != null)
            {
                _redis.Close();
                _redis.Dispose();
                _redis = null;
            }
            
            _subscriber = null;
        }



        protected override APIResault StartTran()
        {
            return ApiCommon.GetSuccess();
        }

        protected override APIResault CommitTran()
        {
            return ApiCommon.GetSuccess();
        }

        protected override APIResault RoolbackTran()
        {
            return ApiCommon.GetSuccess();
        }

        ~RedisMQConnection()
        {
            Close();
            
        }
    }
}
