using System;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Exceptions
{
    public class MqttConfigurationException : Exception
    {
        protected MqttConfigurationException()
        {
        }

        public MqttConfigurationException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public MqttConfigurationException(string message)
            : base(message)
        {
        }
    }
}
