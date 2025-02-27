using Buffalo.MQ.MQTTLib.MQTTnet.Client.Receiving;

namespace Buffalo.MQ.MQTTLib.MQTTnet
{
    public interface IApplicationMessageReceiver
    {
        IMqttApplicationMessageReceivedHandler ApplicationMessageReceivedHandler { get; set; }
    }
}
