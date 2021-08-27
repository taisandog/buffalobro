using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerClientConnectedHandler
    {
        Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs);
    }
}
