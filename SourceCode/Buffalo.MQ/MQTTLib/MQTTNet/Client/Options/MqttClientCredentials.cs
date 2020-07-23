namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Options
{
    public class MqttClientCredentials : IMqttClientCredentials
    {
        public string Username { get; set; }

        public byte[] Password { get; set; }
    }
}
