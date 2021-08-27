using System.Collections.Generic;
using Buffalo.MQ.MQTTLib.MQTTnet.Client;
using Buffalo.MQ.MQTTLib.MQTTnet.Diagnostics;
using Buffalo.MQ.MQTTLib.MQTTnet.Server;

namespace Buffalo.MQ.MQTTLib.MQTTnet
{
    public interface IMqttFactory : IMqttClientFactory, IMqttServerFactory
    {
        IMqttNetLogger DefaultLogger { get; }

        IDictionary<object, object> Properties { get; }
    }
}
