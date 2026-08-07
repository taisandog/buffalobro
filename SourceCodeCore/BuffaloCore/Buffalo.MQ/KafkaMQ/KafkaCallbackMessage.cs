using Confluent.Kafka;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.KafkaMQ
{
    public class KafkaCallbackMessage : MQCallBackMessage
    {
        protected int _partition;
        protected long _offset;
        protected IConsumer<byte[], byte[]> _consumer;
        protected ConsumeResult<byte[], byte[]> _consumeResult;
        private IProducer<byte[], byte[]> _deadLetterProducer;
        private string _deadLetterSuffix;
        private Action _completed;

        public int Partition { get { return _partition; } }

        public long Offset { get { return _offset; } }

        public IConsumer<byte[], byte[]> Consumer { get { return _consumer; } }

        public ConsumeResult<byte[], byte[]> ConsumeResult { get { return _consumeResult; } }

        public KafkaCallbackMessage(string topic, byte[] body, int partition, long offset,
            IConsumer<byte[], byte[]> consumer, ConsumeResult<byte[], byte[]> consumeResult,
            IProducer<byte[], byte[]> deadLetterProducer, string deadLetterSuffix,
            int deliveryCount, Action completed) : base(topic, body)
        {
            _consumer = consumer;
            _consumeResult = consumeResult;
            _partition = partition;
            _offset = offset;
            _deadLetterProducer = deadLetterProducer;
            _deadLetterSuffix = deadLetterSuffix;
            _completed = completed;
            DeliveryCount = Math.Max(1, deliveryCount);
            IsRedelivered = DeliveryCount > 1;
            IsOldMessage = IsRedelivered;
            MessageId = topic + ":" + partition + ":" + offset;
            OriginalTopic = ReadHeader(consumeResult.Message.Headers,
                "x-buffalo-original-topic");
            OriginalMessageId = ReadHeader(consumeResult.Message.Headers,
                "x-buffalo-original-message-id");
            DeadLetterReason = ReadHeader(consumeResult.Message.Headers,
                "x-buffalo-failure-reason");
        }

        public KafkaCallbackMessage(string topic, byte[] body, int partition, long offset,
            IConsumer<byte[], byte[]> consumer,
            ConsumeResult<byte[], byte[]> consumeResult) :
            this(topic, body, partition, offset, consumer, consumeResult,
                null, ".DLQ", 1, null)
        {
        }

        protected override Task AckCoreAsync()
        {
            EnsureConsumer();
            _consumer.Commit(_consumeResult);
            _completed?.Invoke();
            return Task.CompletedTask;
        }

        protected override async Task RetryCoreAsync(string reason, TimeSpan? delay)
        {
            EnsureConsumer();
            if (delay.GetValueOrDefault() > TimeSpan.Zero)
            {
                await Task.Delay(delay.Value).ConfigureAwait(false);
            }
            _consumer.Seek(_consumeResult.TopicPartitionOffset);
        }

        protected override async Task DeadLetterCoreAsync(string reason)
        {
            EnsureConsumer();
            if (_deadLetterProducer == null)
            {
                throw new InvalidOperationException("Kafka 死信生产者尚未初始化");
            }

            Headers headers = new Headers();
            if (_consumeResult.Message.Headers != null)
            {
                foreach (IHeader header in _consumeResult.Message.Headers)
                {
                    headers.Add(header.Key, header.GetValueBytes());
                }
            }
            headers.Add("x-buffalo-original-topic", Encoding.UTF8.GetBytes(_topic));
            headers.Add("x-buffalo-original-partition", Encoding.UTF8.GetBytes(_partition.ToString()));
            headers.Add("x-buffalo-original-offset", Encoding.UTF8.GetBytes(_offset.ToString()));
            headers.Add("x-buffalo-original-message-id", Encoding.UTF8.GetBytes(MessageId));
            headers.Add("x-buffalo-delivery-count", Encoding.UTF8.GetBytes(DeliveryCount.ToString()));
            headers.Add("x-buffalo-failure-reason", Encoding.UTF8.GetBytes(reason ?? string.Empty));

            Message<byte[], byte[]> deadLetter = new Message<byte[], byte[]>
            {
                Key = _consumeResult.Message.Key,
                Value = _body,
                Headers = headers,
                Timestamp = _consumeResult.Message.Timestamp
            };
            await _deadLetterProducer.ProduceAsync(_topic + _deadLetterSuffix, deadLetter)
                .ConfigureAwait(false);
            _consumer.Commit(_consumeResult);
            _completed?.Invoke();
        }

        private void EnsureConsumer()
        {
            if (_consumer == null || _consumeResult == null)
            {
                throw new ObjectDisposedException(nameof(KafkaCallbackMessage));
            }
        }

        private static string ReadHeader(Headers headers, string name)
        {
            byte[] value = headers?.GetLastBytes(name);
            return value == null ? null : Encoding.UTF8.GetString(value);
        }

        public override void Dispose()
        {
            _consumer = null;
            _consumeResult = null;
            _deadLetterProducer = null;
            _completed = null;
            base.Dispose();
        }

        ~KafkaCallbackMessage()
        {
            Dispose();
        }
    }
}
