using System;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Exceptions
{
    public class MqttProtocolViolationException : Exception
    {
        public MqttProtocolViolationException(string message)
            : base(message)
        {
        }
    }
}
