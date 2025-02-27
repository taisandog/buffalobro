using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerClientDisconnectedHandler
    {
        Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs);
    }
}
