using Buffalo.MQ.MQTTLib.MQTTnet.Client.Connecting;
using Buffalo.MQ.MQTTLib.MQTTnet.Exceptions;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Adapter
{
    public class MqttConnectingFailedException : MqttCommunicationException
    {
        public MqttConnectingFailedException(MqttClientAuthenticateResult result)
            : base($"Connecting with MQTT server failed ({result.ResultCode.ToString()}).")
        {
            Result = result;
        }

        public MqttClientAuthenticateResult Result { get; }
        public MqttClientConnectResultCode ResultCode => Result.ResultCode;
    }
}
