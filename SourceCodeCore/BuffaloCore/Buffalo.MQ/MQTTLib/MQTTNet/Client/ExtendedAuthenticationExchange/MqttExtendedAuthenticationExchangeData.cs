using System.Collections.Generic;
using Buffalo.MQ.MQTTLib.MQTTnet.Packets;
using Buffalo.MQ.MQTTLib.MQTTnet.Protocol;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.ExtendedAuthenticationExchange
{
    public class MqttExtendedAuthenticationExchangeData
    {
        public MqttAuthenticateReasonCode ReasonCode { get; set; }

        public string ReasonString { get; set; }

        public byte[] AuthenticationData { get; set; }

        public List<MqttUserProperty> UserProperties { get; }
    }
}
