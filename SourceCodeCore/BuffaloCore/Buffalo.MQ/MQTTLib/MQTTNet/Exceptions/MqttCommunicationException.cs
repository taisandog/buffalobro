﻿using System;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Exceptions
{
    public class MqttCommunicationException : Exception
    {
        protected MqttCommunicationException()
        {
        }

        public MqttCommunicationException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public MqttCommunicationException(string message)
            : base(message)
        {
        }
    }
}
