using System.Threading;
using System.Threading.Tasks;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Publishing;

namespace Buffalo.MQ.MQTTLib.MQTTnet
{
    public interface IApplicationMessagePublisher
    {
        Task<MqttClientPublishResult> PublishAsync(MqttApplicationMessage applicationMessage, CancellationToken cancellationToken);
    }
}
