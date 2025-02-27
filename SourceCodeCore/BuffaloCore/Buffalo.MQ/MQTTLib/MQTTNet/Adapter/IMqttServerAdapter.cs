using System;
using System.Threading.Tasks;
using Buffalo.MQ.MQTTLib.MQTTnet.Server;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Adapter
{
    public interface IMqttServerAdapter : IDisposable
    {
        Func<IMqttChannelAdapter, Task> ClientHandler { get; set; }

        Task StartAsync(IMqttServerOptions options);
        Task StopAsync();
    }
}
