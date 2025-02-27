using System;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Diagnostics
{
    public interface IMqttNetScopedLogger
    {
        IMqttNetScopedLogger CreateScopedLogger(string source);

        void Publish(MqttNetLogLevel logLevel, string message, object[] parameters, Exception exception);
    }
}
