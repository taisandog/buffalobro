﻿using System.Collections.Generic;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    public class MqttDisconnectPacketProperties
    {
        public uint? SessionExpiryInterval { get; set; }

        public string ReasonString { get; set; }

        public List<MqttUserProperty> UserProperties { get; set; }

        public string ServerReference { get; set; }
    }
}