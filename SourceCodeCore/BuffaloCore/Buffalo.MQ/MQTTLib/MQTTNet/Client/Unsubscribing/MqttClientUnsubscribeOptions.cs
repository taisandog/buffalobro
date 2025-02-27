using System.Collections.Generic;
using Buffalo.MQ.MQTTLib.MQTTnet.Packets;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Unsubscribing
{
    public class MqttClientUnsubscribeOptions
    {
        public List<string> TopicFilters { get; set; } = new List<string>();

        public List<MqttUserProperty> UserProperties { get; set; } = new List<MqttUserProperty>();
    }
}
