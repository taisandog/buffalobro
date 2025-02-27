using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Disconnecting
{
    public interface IMqttClientDisconnectedHandler
    {
        Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs);
    }
}
