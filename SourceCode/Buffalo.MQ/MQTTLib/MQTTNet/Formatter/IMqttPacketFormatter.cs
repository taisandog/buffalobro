using System;
using Buffalo.MQ.MQTTLib.MQTTnet.Adapter;
using Buffalo.MQ.MQTTLib.MQTTnet.Packets;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Formatter
{
    public interface IMqttPacketFormatter
    {
        IMqttDataConverter DataConverter { get; }

        ArraySegment<byte> Encode(MqttBasePacket mqttPacket);

        MqttBasePacket Decode(ReceivedMqttPacket receivedMqttPacket);

        void FreeBuffer();
    }
}