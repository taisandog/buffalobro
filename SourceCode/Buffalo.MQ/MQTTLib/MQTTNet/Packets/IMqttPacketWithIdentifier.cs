namespace Buffalo.MQ.MQTTLib.MQTTnet.Packets
{
    public interface IMqttPacketWithIdentifier
    {
        ushort? PacketIdentifier { get; set; }
    }
}
