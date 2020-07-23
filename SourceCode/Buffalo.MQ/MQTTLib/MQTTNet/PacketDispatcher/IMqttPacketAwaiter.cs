using Buffalo.MQ.MQTTLib.MQTTnet.Packets;
using System;

namespace Buffalo.MQ.MQTTLib.MQTTnet.PacketDispatcher
{
    public interface IMqttPacketAwaiter : IDisposable
    {
        void Complete(MqttBasePacket packet);

        void Fail(Exception exception);

        void Cancel();
    }
}