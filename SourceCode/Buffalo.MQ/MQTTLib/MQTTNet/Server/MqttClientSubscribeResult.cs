using Buffalo.MQ.MQTTLib.MQTTnet.Packets;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public class MqttClientSubscribeResult
    {
        public MqttSubAckPacket ResponsePacket { get; set; }

        public bool CloseConnection { get; set; }
    }
}
