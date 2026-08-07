using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;

namespace Buffalo.MQ.RabbitMQ
{
    public class RabbitCallbackMessage : MQCallBackMessage
    {
        internal const string DeliveryCountHeader = "x-buffalo-delivery-count";
        internal const string FailureReasonHeader = "x-buffalo-failure-reason";

        protected IChannel _channel;
        protected BasicDeliverEventArgs _basicDeliverEventArgs;
        protected string _exchange;
        private string _queueName;
        private string _retryQueueName;
        private string _deadLetterQueueName;
        private int _retryDelayMilliseconds;

        public IChannel Channel { get { return _channel; } }

        public BasicDeliverEventArgs BasicDeliverEventArgs
        {
            get { return _basicDeliverEventArgs; }
        }

        public string Exchange { get { return _exchange; } }

        public string QueueName { get { return _queueName; } }

        public RabbitCallbackMessage(string topic, string exchange, string queueName,
            string retryQueueName, string deadLetterQueueName, byte[] body,
            IChannel channel, BasicDeliverEventArgs basicDeliverEventArgs,
            int retryDelayMilliseconds = 1000) :
            base(topic, body)
        {
            _channel = channel;
            _basicDeliverEventArgs = basicDeliverEventArgs;
            _exchange = exchange;
            _queueName = queueName;
            _retryQueueName = retryQueueName;
            _deadLetterQueueName = deadLetterQueueName;
            _retryDelayMilliseconds = Math.Max(0, retryDelayMilliseconds);
            DeliveryCount = ReadDeliveryCount(basicDeliverEventArgs.BasicProperties.Headers);
            IsRedelivered = basicDeliverEventArgs.Redelivered || DeliveryCount > 1;
            IsOldMessage = IsRedelivered;
            MessageId = basicDeliverEventArgs.BasicProperties.MessageId;
            if (string.IsNullOrWhiteSpace(MessageId))
            {
                MessageId = exchange + ":" + topic + ":" + basicDeliverEventArgs.DeliveryTag;
            }
            OriginalTopic = ReadHeaderString(basicDeliverEventArgs.BasicProperties.Headers,
                "x-buffalo-original-routing-key");
            OriginalMessageId = basicDeliverEventArgs.BasicProperties.MessageId;
            DeadLetterReason = ReadHeaderString(basicDeliverEventArgs.BasicProperties.Headers,
                FailureReasonHeader);
        }

        public RabbitCallbackMessage(string topic, string exchange, byte[] body,
            IChannel channel, BasicDeliverEventArgs basicDeliverEventArgs) :
            this(topic, exchange, string.Empty, string.Empty, string.Empty,
                body, channel, basicDeliverEventArgs)
        {
        }

        protected override async Task AckCoreAsync()
        {
            EnsureDelivery();
            await _channel.BasicAckAsync(_basicDeliverEventArgs.DeliveryTag, false)
                .ConfigureAwait(false);
        }

        protected override async Task RetryCoreAsync(string reason, TimeSpan? delay)
        {
            EnsureDelivery();
            if (string.IsNullOrWhiteSpace(_retryQueueName))
            {
                await _channel.BasicNackAsync(_basicDeliverEventArgs.DeliveryTag,
                    false, true).ConfigureAwait(false);
                return;
            }
            BasicProperties properties = CreateProperties(reason, DeliveryCount + 1);
            double requestedDelay = delay?.TotalMilliseconds ?? _retryDelayMilliseconds;
            properties.Expiration = Math.Max(0, (long)requestedDelay)
                .ToString(CultureInfo.InvariantCulture);
            await _channel.BasicPublishAsync(string.Empty, _retryQueueName, false,
                properties, _body).ConfigureAwait(false);
            await _channel.BasicAckAsync(_basicDeliverEventArgs.DeliveryTag, false)
                .ConfigureAwait(false);
        }

        protected override async Task DeadLetterCoreAsync(string reason)
        {
            EnsureDelivery();
            if (string.IsNullOrWhiteSpace(_deadLetterQueueName))
            {
                await _channel.BasicNackAsync(_basicDeliverEventArgs.DeliveryTag,
                    false, false).ConfigureAwait(false);
                return;
            }
            BasicProperties properties = CreateProperties(reason, DeliveryCount);
            properties.Headers["x-buffalo-original-exchange"] =
                Encoding.UTF8.GetBytes(_exchange ?? string.Empty);
            properties.Headers["x-buffalo-original-routing-key"] =
                Encoding.UTF8.GetBytes(_topic ?? string.Empty);
            properties.Headers["x-buffalo-dead-letter-time"] =
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            await _channel.BasicPublishAsync(string.Empty, _deadLetterQueueName, false,
                properties, _body).ConfigureAwait(false);
            await _channel.BasicAckAsync(_basicDeliverEventArgs.DeliveryTag, false)
                .ConfigureAwait(false);
        }

        private BasicProperties CreateProperties(string reason, int deliveryCount)
        {
            IReadOnlyBasicProperties source = _basicDeliverEventArgs.BasicProperties;
            BasicProperties properties = new BasicProperties
            {
                Persistent = true,
                MessageId = string.IsNullOrWhiteSpace(source.MessageId)
                    ? MessageId : source.MessageId,
                ContentType = source.ContentType,
                ContentEncoding = source.ContentEncoding,
                CorrelationId = source.CorrelationId,
                Type = source.Type,
                AppId = source.AppId,
                Headers = source.Headers == null
                    ? new Dictionary<string, object>()
                    : new Dictionary<string, object>(source.Headers)
            };
            properties.Headers[DeliveryCountHeader] = deliveryCount;
            properties.Headers[FailureReasonHeader] =
                Encoding.UTF8.GetBytes(reason ?? string.Empty);
            return properties;
        }

        private static int ReadDeliveryCount(IDictionary<string, object> headers)
        {
            if (headers == null || !headers.TryGetValue(DeliveryCountHeader, out object value))
            {
                return 1;
            }
            if (value is int intValue) return Math.Max(1, intValue);
            if (value is long longValue) return (int)Math.Max(1, longValue);
            if (value is byte byteValue) return Math.Max(1, (int)byteValue);
            if (value is byte[] bytes && int.TryParse(Encoding.UTF8.GetString(bytes), out int parsed))
            {
                return Math.Max(1, parsed);
            }
            return 1;
        }

        private static string ReadHeaderString(IDictionary<string, object> headers,
            string name)
        {
            if (headers == null || !headers.TryGetValue(name, out object value))
            {
                return null;
            }
            if (value is byte[] bytes)
            {
                return Encoding.UTF8.GetString(bytes);
            }
            return value?.ToString();
        }

        private void EnsureDelivery()
        {
            if (_channel == null || _basicDeliverEventArgs == null)
            {
                throw new ObjectDisposedException(nameof(RabbitCallbackMessage));
            }
        }

        public override void Dispose()
        {
            _channel = null;
            _basicDeliverEventArgs = null;
            _exchange = null;
            _queueName = null;
            _retryQueueName = null;
            _deadLetterQueueName = null;
            _retryDelayMilliseconds = 0;
            base.Dispose();
        }

        ~RabbitCallbackMessage()
        {
            Dispose();
        }
    }
}
