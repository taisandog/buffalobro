using System.Collections.Generic;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Subscribing
{
    public class MqttClientSubscribeResult
    {
        public List<MqttClientSubscribeResultItem> Items { get; } = new List<MqttClientSubscribeResultItem>();
    }
}
