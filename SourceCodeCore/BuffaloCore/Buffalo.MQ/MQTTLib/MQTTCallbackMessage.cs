
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.MQ.MQTTLib
{
    public class MQTTCallbackMessage: MQCallBackMessage
    {
        protected MqttApplicationMessageReceivedEventArgs _receivedEventArgs;

        public MqttApplicationMessageReceivedEventArgs ReceivedEventArgs
        {
            get { return _receivedEventArgs; }
        }
       
        public MQTTCallbackMessage(string topic,  byte[] body, 
            MqttApplicationMessageReceivedEventArgs receivedEventArgs) :
            base(topic,  body)
        {
            _receivedEventArgs=receivedEventArgs;

        }

        public override void Commit()
        {
           
        }

        public override void Dispose()
        {
            _receivedEventArgs = null;
            base.Dispose();
        }

        ~MQTTCallbackMessage()
        {
            Dispose();
        }
    }
}
