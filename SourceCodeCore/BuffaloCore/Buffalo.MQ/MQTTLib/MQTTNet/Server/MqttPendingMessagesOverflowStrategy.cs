namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public enum MqttPendingMessagesOverflowStrategy
    {
        DropOldestQueuedMessage,
        DropNewMessage
    }
}
