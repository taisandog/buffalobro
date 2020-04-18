using Buffalo.Kernel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    public partial class RedisMQListener : MQListener
    {
        private ConnectionMultiplexer _redis = null;

        /// <summary>
        /// 配置
        /// </summary>
        RedisMQConfig _config;
        /// <summary>
        /// 发布者
        /// </summary>
        public ISubscriber _subscriber;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RedisMQListener(RedisMQConfig config)
        {
            _config = config;
        }
        
       
        /// <summary>
        /// 打来连接
        /// </summary>
        public void Open()
        {
            if (_redis == null)
            {
                _redis =RedisMQConnection.CreateManager(_config.Options);
            }
            
        }

        private void OnRedisCallback(RedisChannel key, RedisValue value)
        {
            CallBack(key.ToString(), key.ToString(), (byte[])value,0,0);
        }

        public override void StartListend(IEnumerable<string> listenKeys)
        {
            Open();

            _subscriber = _redis.GetSubscriber();
            foreach (string lis in listenKeys)
            {
                _subscriber.Subscribe(lis, OnRedisCallback, _config.CommanfFlags);
            }
        }
        public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        {
            StartListend(MQUnit.GetLintenKeys(listenKeys));
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            if (_redis != null)
            {
                _redis.Close();
                _redis.Dispose();
                _redis = null;
            }
        }

        public override void Dispose()
        {
            Close();
        }

        

        ~RedisMQListener()
        {
            Close();
            GC.SuppressFinalize(this);
        }
    }
}
