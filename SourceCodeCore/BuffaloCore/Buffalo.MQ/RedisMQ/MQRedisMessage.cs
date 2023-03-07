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
    public class MQRedisMessage : MQSendMessage
    {
        public MQRedisMessage(string topic, byte[] value) : base(topic, value)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        ~MQRedisMessage()
        {
            Dispose();
        }
    }
}
