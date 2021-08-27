using System.Collections.Generic;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    public class MqttPubAckPacketProperties
    {
        public string ReasonString { get; set; }

        public List<MqttUserProperty> UserProperties { get; set; }
    }
}
