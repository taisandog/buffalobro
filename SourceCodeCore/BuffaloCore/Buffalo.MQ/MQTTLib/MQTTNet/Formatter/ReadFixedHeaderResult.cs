﻿namespace Buffalo.MQ.MQTTLib.MQTTnet.Formatter
{
    public class ReadFixedHeaderResult
    {
        public bool ConnectionClosed { get; set; }

        public MqttFixedHeader FixedHeader { get; set; }
    }
}
