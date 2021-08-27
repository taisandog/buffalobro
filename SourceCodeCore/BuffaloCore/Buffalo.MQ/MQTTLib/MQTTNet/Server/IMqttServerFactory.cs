using System;
using System.Collections.Generic;
using Buffalo.MQ.MQTTLib.MQTTnet.Adapter;
using Buffalo.MQ.MQTTLib.MQTTnet.Diagnostics;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerFactory
    {
        IList<Func<IMqttFactory, IMqttServerAdapter>> DefaultServerAdapters { get; }

        IMqttServer CreateMqttServer();

        IMqttServer CreateMqttServer(IMqttNetLogger logger);

        IMqttServer CreateMqttServer(IEnumerable<IMqttServerAdapter> adapters);

        IMqttServer CreateMqttServer(IEnumerable<IMqttServerAdapter> adapters, IMqttNetLogger logger);
    }
}