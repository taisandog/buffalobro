using MQTTnet;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.MQ.MQTTLib
{
    /// <summary>
    /// MQTT消息
    /// </summary>
    public class MQTTMessage : MQSendMessage
    {
        protected MqttApplicationMessage _message;
        /// <summary>
        /// 消息
        /// </summary>
        public MqttApplicationMessage Message 
        {
            get { return _message; }
        }
        public MQTTMessage(MqttApplicationMessage message)
            : base(message.Topic, message.Payload.ToArray())
        {
            
            _message = message;
        }

        public override void Dispose()
        {
            
            _message = null;
            base.Dispose();
        }

        ~MQTTMessage()
        {
            Dispose();
        }
    }
}
