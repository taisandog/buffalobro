using Buffalo.MQ.MQTTLib.MQTTnet.Protocol;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    public class MqttDisconnectPacket : MqttBasePacket
    {
        #region Added in MQTTv5

        public MqttDisconnectReasonCode? ReasonCode { get; set; }

        public MqttDisconnectPacketProperties Properties { get; set; }

        #endregion

        public override string ToString()
        {
            return string.Concat("Disconnect: [ReasonCode=", ReasonCode, "]");
        }
    }
}
