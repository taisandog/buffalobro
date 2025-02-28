﻿using System;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Connecting
{
    public class MqttClientConnectedEventArgs : EventArgs
    {
        public MqttClientConnectedEventArgs(MqttClientAuthenticateResult authenticateResult)
        {
            AuthenticateResult = authenticateResult ?? throw new ArgumentNullException(nameof(authenticateResult));
        }

        public MqttClientAuthenticateResult AuthenticateResult { get; }
    }
}
