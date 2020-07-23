using System.Collections.Generic;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Unsubscribing
{
    public class MqttClientUnsubscribeResult
    {
        public List<MqttClientUnsubscribeResultItem> Items { get; }  =new List<MqttClientUnsubscribeResultItem>();
    }
}
