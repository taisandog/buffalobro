using Buffalo.MQ.MQTTLib.MQTTnet.Client.Connecting;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Disconnecting;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Options;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Publishing;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Subscribing;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Unsubscribing;
using Buffalo.MQ.MQTTLib.MQTTnet.Packets;
using Buffalo.MQ.MQTTLib.MQTTnet.Server;
using MqttClientSubscribeResult = Buffalo.MQ.MQTTLib.MQTTnet.Client.Subscribing.MqttClientSubscribeResult;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Formatter
{
    public interface IMqttDataConverter
    {
        MqttPublishPacket CreatePublishPacket(MqttApplicationMessage applicationMessage);

        MqttPubAckPacket CreatePubAckPacket(MqttPublishPacket publishPacket);

        MqttApplicationMessage CreateApplicationMessage(MqttPublishPacket publishPacket);

        MqttClientAuthenticateResult CreateClientConnectResult(MqttConnAckPacket connAckPacket);

        MqttConnectPacket CreateConnectPacket(MqttApplicationMessage willApplicationMessage, IMqttClientOptions options);

        MqttConnAckPacket CreateConnAckPacket(MqttConnectionValidatorContext connectionValidatorContext);

        MqttClientSubscribeResult CreateClientSubscribeResult(MqttSubscribePacket subscribePacket, MqttSubAckPacket subAckPacket);

        MqttClientUnsubscribeResult CreateClientUnsubscribeResult(MqttUnsubscribePacket unsubscribePacket, MqttUnsubAckPacket unsubAckPacket);

        MqttSubscribePacket CreateSubscribePacket(MqttClientSubscribeOptions options);

        MqttUnsubscribePacket CreateUnsubscribePacket(MqttClientUnsubscribeOptions options);

        MqttDisconnectPacket CreateDisconnectPacket(MqttClientDisconnectOptions options);

        MqttClientPublishResult CreatePublishResult(MqttPubAckPacket pubAckPacket);

        MqttClientPublishResult CreatePublishResult(MqttPubRecPacket pubRecPacket, MqttPubCompPacket pubCompPacket);
    }
}
