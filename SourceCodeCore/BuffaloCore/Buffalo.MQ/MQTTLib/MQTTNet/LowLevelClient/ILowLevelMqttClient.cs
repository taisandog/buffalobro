using Buffalo.MQ.MQTTLib.MQTTnet.Client.Options;
using Buffalo.MQ.MQTTLib.MQTTnet.Packets;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.LowLevelClient
{
    public interface ILowLevelMqttClient : IDisposable
    {
        Task ConnectAsync(IMqttClientOptions options, CancellationToken cancellationToken);

        Task DisconnectAsync(CancellationToken cancellationToken);

        Task SendAsync(MqttBasePacket packet, CancellationToken cancellationToken);

        Task<MqttBasePacket> ReceiveAsync(CancellationToken cancellationToken);
    }
}
