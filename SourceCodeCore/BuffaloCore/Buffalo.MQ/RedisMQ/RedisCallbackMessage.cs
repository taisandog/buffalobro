using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    public class RedisCallbackMessage : MQCallBackMessage
    {


        public RedisCallbackMessage(string topic,  byte[] body) :
            base(topic, body)
        {
            
        }

        public override void Commit()
        {
           
        }

        public override void Dispose()
        {
           
            base.Dispose();
        }

        public override async Task CommitAsync()
        {
            
        }

        ~RedisCallbackMessage()
        {
            Dispose();
        }
    }
}
