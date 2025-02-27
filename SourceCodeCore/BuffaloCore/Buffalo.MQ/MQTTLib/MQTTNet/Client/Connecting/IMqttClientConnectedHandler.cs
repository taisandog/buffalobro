using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Connecting
{
    public interface IMqttClientConnectedHandler
    {
        Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs);
    }
}
