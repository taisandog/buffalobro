namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public class PrepareClientSessionResult
    {
        public bool IsExistingSession { get; set; }

        public MqttClientConnection Session { get; set; }
    }
}
