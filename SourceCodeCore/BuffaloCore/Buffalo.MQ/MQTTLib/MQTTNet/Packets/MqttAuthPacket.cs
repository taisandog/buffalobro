﻿using Buffalo.MQ.MQTTLib.MQTTnet.Protocol;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    /// <summary>
    /// Added in MQTTv5.0.0.
    /// </summary>
    public class MqttAuthPacket : MqttBasePacket
    {
        public MqttAuthenticateReasonCode ReasonCode { get; set; }

        public MqttAuthPacketProperties Properties { get; set; }
    }
}
