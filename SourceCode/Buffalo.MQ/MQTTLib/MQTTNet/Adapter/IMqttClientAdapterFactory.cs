using Buffalo.MQ.MQTTLib.MQTTnet.Client.Options;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Adapter
{
    public interface IMqttClientAdapterFactory
    {
        IMqttChannelAdapter CreateClientAdapter(IMqttClientOptions options);
    }
}
