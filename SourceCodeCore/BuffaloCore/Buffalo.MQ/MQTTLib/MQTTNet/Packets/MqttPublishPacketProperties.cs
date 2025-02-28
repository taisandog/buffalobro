﻿using System.Collections.Generic;
using Buffalo.MQ.MQTTLib.MQTTnet.Protocol;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    public class MqttPublishPacketProperties
    {
        public MqttPayloadFormatIndicator? PayloadFormatIndicator { get; set; }

        public uint? MessageExpiryInterval { get; set; }

        public ushort? TopicAlias { get; set; }

        public string ResponseTopic { get; set; }

        public byte[] CorrelationData { get; set; }

        public List<MqttUserProperty> UserProperties { get; set; }

        public List<uint> SubscriptionIdentifiers { get; set; }

        public string ContentType { get; set; }
    }
}
