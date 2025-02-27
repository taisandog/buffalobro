using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerConnectionValidator
    {
        Task ValidateConnectionAsync(MqttConnectionValidatorContext context);
    }
}
