using System.Collections.Generic;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    public class MqttAuthPacketProperties
    {
        public string AuthenticationMethod { get; set; }

        public byte[] AuthenticationData { get; set; }

        public string ReasonString { get; set; }

        public List<MqttUserProperty> UserProperties { get; set; }
    }
}
