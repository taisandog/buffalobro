using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerSubscriptionInterceptor
    {
        Task InterceptSubscriptionAsync(MqttSubscriptionInterceptorContext context);
    }
}
