﻿namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public class MqttServerTcpEndpointOptions : MqttServerTcpEndpointBaseOptions
    {
        public MqttServerTcpEndpointOptions()
        {
            IsEnabled = true;
            Port = 1883;
        }
    }
}
