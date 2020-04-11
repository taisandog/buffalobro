using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    public partial class RedisMQConnection
    {
        
        /// <summary>
        /// 打开事件监听
        /// </summary>
        public override void StartListend()
        {
            Open();
            
            _subscriber = _redis.GetSubscriber();
            _subscriber.Subscribe(_listen,OnRedisCallback, _commanfFlags);


        }

        private void OnRedisCallback(RedisChannel key, RedisValue value)
        {
            CallBack(key.ToString(),"", (byte[])value);
        }
    }
}
