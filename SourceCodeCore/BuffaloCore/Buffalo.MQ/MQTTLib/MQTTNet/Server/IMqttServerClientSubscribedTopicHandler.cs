using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerClientSubscribedTopicHandler
    {
        Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs eventArgs);
    }
}
