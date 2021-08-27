using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerApplicationMessageInterceptor
    {
        Task InterceptApplicationMessagePublishAsync(MqttApplicationMessageInterceptorContext context);
    }
}
