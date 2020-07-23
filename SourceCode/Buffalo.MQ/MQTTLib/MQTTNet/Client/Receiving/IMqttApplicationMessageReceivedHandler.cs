using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Receiving
{
    public interface IMqttApplicationMessageReceivedHandler
    {
        Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs);
    }
}
