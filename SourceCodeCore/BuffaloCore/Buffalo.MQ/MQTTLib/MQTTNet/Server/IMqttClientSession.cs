using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttClientSession
    {
        string ClientId { get; }

        Task StopAsync();
    }
}