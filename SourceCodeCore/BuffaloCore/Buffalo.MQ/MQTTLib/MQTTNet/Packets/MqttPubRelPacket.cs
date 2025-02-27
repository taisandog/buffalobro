using Buffalo.MQ.MQTTLib.MQTTnet.Protocol;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    public class MqttPubRelPacket : MqttBasePublishPacket
    {
        #region Added in MQTTv5

        public MqttPubRelReasonCode? ReasonCode { get; set; }

        public MqttPubRelPacketProperties Properties { get; set; }

        #endregion

        public override string ToString()
        {
            return string.Concat("PubRel: [PacketIdentifier=", PacketIdentifier, "] [ReasonCode=", ReasonCode, "]");
        }
    }
}
