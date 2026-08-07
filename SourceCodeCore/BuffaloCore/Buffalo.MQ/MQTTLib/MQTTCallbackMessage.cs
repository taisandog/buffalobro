using MQTTnet;
using MQTTnet.Packets;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib
{
    public class MQTTCallbackMessage : MQCallBackMessage
    {
        private const string DeliveryCountProperty = "x-buffalo-delivery-count";
        protected MqttApplicationMessageReceivedEventArgs _receivedEventArgs;
        private MqttClient _client;
        private string _deadLetterSuffix;

        public MqttApplicationMessageReceivedEventArgs ReceivedEventArgs
        {
            get { return _receivedEventArgs; }
        }

        public MQTTCallbackMessage(string topic, byte[] body,
            MqttApplicationMessageReceivedEventArgs receivedEventArgs,
            MqttClient client, string deadLetterSuffix) : base(topic, body)
        {
            _receivedEventArgs = receivedEventArgs;
            _client = client;
            _deadLetterSuffix = deadLetterSuffix;
            DeliveryCount = ReadDeliveryCount(receivedEventArgs.ApplicationMessage);
            IsRedelivered = receivedEventArgs.ApplicationMessage.Dup || DeliveryCount > 1;
            IsOldMessage = IsRedelivered;
            MessageId = receivedEventArgs.ClientId + ":" +
                receivedEventArgs.PacketIdentifier;
            OriginalTopic = ReadUserProperty(receivedEventArgs.ApplicationMessage,
                "x-buffalo-original-topic");
            OriginalMessageId = ReadUserProperty(receivedEventArgs.ApplicationMessage,
                "x-buffalo-original-message-id");
            DeadLetterReason = ReadUserProperty(receivedEventArgs.ApplicationMessage,
                "x-buffalo-failure-reason");
        }

        public MQTTCallbackMessage(string topic, byte[] body,
            MqttApplicationMessageReceivedEventArgs receivedEventArgs) :
            this(topic, body, receivedEventArgs, null, ".DLQ")
        {
        }

        protected override async Task AckCoreAsync()
        {
            EnsureReceivedMessage();
            await _receivedEventArgs.AcknowledgeAsync(CancellationToken.None)
                .ConfigureAwait(false);
        }

        protected override async Task RetryCoreAsync(string reason, TimeSpan? delay)
        {
            EnsureMessage();
            if (delay.GetValueOrDefault() > TimeSpan.Zero)
            {
                await Task.Delay(delay.Value).ConfigureAwait(false);
            }
            MqttApplicationMessage retryMessage = CreateMessage(
                _topic, DeliveryCount + 1, reason);
            await _client.PublishAsync(retryMessage, CancellationToken.None)
                .ConfigureAwait(false);
            await _receivedEventArgs.AcknowledgeAsync(CancellationToken.None)
                .ConfigureAwait(false);
        }

        protected override async Task DeadLetterCoreAsync(string reason)
        {
            EnsureMessage();
            MqttApplicationMessage deadLetterMessage = CreateMessage(
                _topic + _deadLetterSuffix, DeliveryCount, reason);
            MqttApplicationMessageBuilder builder = new MqttApplicationMessageBuilder()
                .WithTopic(deadLetterMessage.Topic)
                .WithPayload(deadLetterMessage.Payload)
                .WithQualityOfServiceLevel(deadLetterMessage.QualityOfServiceLevel)
                .WithUserProperty("x-buffalo-original-topic", Encoding.UTF8.GetBytes(_topic))
                .WithUserProperty("x-buffalo-original-message-id",
                    Encoding.UTF8.GetBytes(MessageId ?? string.Empty))
                .WithUserProperty("x-buffalo-dead-letter-time", Encoding.UTF8.GetBytes(
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()));
            foreach (MqttUserProperty property in deadLetterMessage.UserProperties)
            {
                builder.WithUserProperty(property.Name, property.ValueBuffer);
            }
            await _client.PublishAsync(builder.Build(), CancellationToken.None)
                .ConfigureAwait(false);
            await _receivedEventArgs.AcknowledgeAsync(CancellationToken.None)
                .ConfigureAwait(false);
        }

        private MqttApplicationMessage CreateMessage(string topic, int deliveryCount,
            string reason)
        {
            MqttApplicationMessage source = _receivedEventArgs.ApplicationMessage;
            MqttApplicationMessageBuilder builder = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(_body)
                .WithQualityOfServiceLevel(source.QualityOfServiceLevel)
                .WithRetainFlag(false);
            if (source.UserProperties != null)
            {
                foreach (MqttUserProperty property in source.UserProperties)
                {
                    if (!string.Equals(property.Name, DeliveryCountProperty,
                        StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(property.Name, "x-buffalo-failure-reason",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        builder.WithUserProperty(property.Name, property.ValueBuffer);
                    }
                }
            }
            builder.WithUserProperty(DeliveryCountProperty,
                Encoding.UTF8.GetBytes(deliveryCount.ToString()));
            builder.WithUserProperty("x-buffalo-failure-reason",
                Encoding.UTF8.GetBytes(reason ?? string.Empty));
            return builder.Build();
        }

        private static int ReadDeliveryCount(MqttApplicationMessage message)
        {
            MqttUserProperty property = message.UserProperties?.LastOrDefault(item =>
                string.Equals(item.Name, DeliveryCountProperty,
                    StringComparison.OrdinalIgnoreCase));
            if (property != null && int.TryParse(
                Encoding.UTF8.GetString(property.ValueBuffer.Span), out int count))
            {
                return Math.Max(1, count);
            }
            return 1;
        }

        private static string ReadUserProperty(MqttApplicationMessage message, string name)
        {
            MqttUserProperty property = message.UserProperties?.LastOrDefault(item =>
                string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
            return property == null ? null :
                Encoding.UTF8.GetString(property.ValueBuffer.Span);
        }

        private void EnsureMessage()
        {
            EnsureReceivedMessage();
            if (_client == null)
            {
                throw new ObjectDisposedException(nameof(MQTTCallbackMessage));
            }
        }

        private void EnsureReceivedMessage()
        {
            if (_receivedEventArgs == null)
            {
                throw new ObjectDisposedException(nameof(MQTTCallbackMessage));
            }
        }

        public override void Dispose()
        {
            _receivedEventArgs = null;
            _client = null;
            _deadLetterSuffix = null;
            base.Dispose();
        }

        ~MQTTCallbackMessage()
        {
            Dispose();
        }
    }
}
