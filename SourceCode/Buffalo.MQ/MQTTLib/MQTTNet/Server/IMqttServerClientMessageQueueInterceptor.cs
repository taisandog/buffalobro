using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerClientMessageQueueInterceptor
    {
        Task InterceptClientMessageQueueEnqueueAsync(MqttClientMessageQueueInterceptorContext context);
    }
}
