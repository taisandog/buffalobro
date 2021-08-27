
using System.Collections.Generic;
using Buffalo.MQ.MQTTLib.MQTTnet.Packets;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Publishing
{
    public class MqttClientPublishResult
    {
        public ushort? PacketIdentifier { get; set; }

        public MqttClientPublishReasonCode ReasonCode { get; set; } = MqttClientPublishReasonCode.Success;

        public string ReasonString { get; set; }

        public List<MqttUserProperty> UserProperties { get; set; }
    }
}
