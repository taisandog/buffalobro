namespace Buffalo.MQ.MQTTLib.MQTTnet.Client.Options
{
    public interface IMqttClientCredentials
    {
        string Username { get; }
        byte[] Password { get; }
    }
}