using System;
using System.Collections.Generic;
using System.Text;

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

        ~RedisCallbackMessage()
        {
            Dispose();
        }
    }
}
