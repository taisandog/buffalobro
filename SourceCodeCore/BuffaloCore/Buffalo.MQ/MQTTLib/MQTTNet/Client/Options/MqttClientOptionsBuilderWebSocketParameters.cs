using System.Collections.Generic;
using System.Net;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Options
{
    public class MqttClientOptionsBuilderWebSocketParameters
    {
        public IDictionary<string, string> RequestHeaders { get; set; }

        public CookieContainer CookieContainer { get; set; }
    }
}
