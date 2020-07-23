using System.Collections.Generic;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    public class MqttSubAckPacketProperties
    {
        public string ReasonString { get; set; }

        public List<MqttUserProperty> UserProperties { get; set; }
    }
}
