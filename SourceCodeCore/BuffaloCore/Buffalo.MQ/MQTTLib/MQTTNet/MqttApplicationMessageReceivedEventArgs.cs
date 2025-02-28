﻿using System;

namespace Buffalo.MQ.MQTTLib.MQTTnet
{
    public class MqttApplicationMessageReceivedEventArgs : EventArgs
    {
        public MqttApplicationMessageReceivedEventArgs(string clientId, MqttApplicationMessage applicationMessage)
        {
            ClientId = clientId;
            ApplicationMessage = applicationMessage ?? throw new ArgumentNullException(nameof(applicationMessage));
        }

        public string ClientId { get; }

        public MqttApplicationMessage ApplicationMessage { get; }

        public bool ProcessingFailed { get; set; }
    }
}
