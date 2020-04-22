using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    /// <summary>
    /// Redis队列的未发送信息
    /// </summary>
    public class MQRedisMessage
    {
        /// <summary>
        /// 键
        /// </summary>
        public string RoutingKey;
        /// <summary>
        /// 值
        /// </summary>
        public byte[] Value;

    }
}
