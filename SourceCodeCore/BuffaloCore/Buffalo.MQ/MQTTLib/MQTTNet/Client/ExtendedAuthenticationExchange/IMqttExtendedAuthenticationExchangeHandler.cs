using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.ExtendedAuthenticationExchange
{
    public interface IMqttExtendedAuthenticationExchangeHandler
    {
        Task HandleRequestAsync(MqttExtendedAuthenticationExchangeContext context);
    }
}
