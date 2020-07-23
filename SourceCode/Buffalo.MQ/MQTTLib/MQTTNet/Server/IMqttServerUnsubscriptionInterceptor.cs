using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerUnsubscriptionInterceptor
    {
        Task InterceptUnsubscriptionAsync(MqttUnsubscriptionInterceptorContext context);
    }
}
