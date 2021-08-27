using System.Collections.Generic;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    public class MqttUnsubscribePacketProperties
    {
        public List<MqttUserProperty> UserProperties { get; set; }
    }
}
