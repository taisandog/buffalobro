using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    public class RedisCallbackMessage : MQCallBackMessage
    {
        private IDatabase _db;
        private string _consumerGroup;
        private RedisValue _messId;
        private CommandFlags _commandFlags;
        private string _deadLetterSuffix;
        private string _defaultDataKey;
        private readonly Func<Task> _ackHandler;
        private readonly Func<string, TimeSpan?, Task> _retryHandler;
        private readonly Func<string, Task> _deadLetterHandler;

        public RedisCallbackMessage(string topic, byte[] body,
            Func<Task> ackHandler = null,
            Func<string, TimeSpan?, Task> retryHandler = null,
            Func<string, Task> deadLetterHandler = null) : base(topic, body)
        {
            MessageId = Guid.NewGuid().ToString("N");
            _ackHandler = ackHandler;
            _retryHandler = retryHandler;
            _deadLetterHandler = deadLetterHandler;
        }

        public RedisCallbackMessage(string topic, byte[] body, IDatabase db,
            string consumerGroup, RedisValue messId, CommandFlags commandFlags,
            string deadLetterSuffix, string defaultDataKey, int deliveryCount = 1) :
            base(topic, body)
        {
            _db = db;
            _messId = messId;
            _commandFlags = commandFlags;
            _consumerGroup = consumerGroup;
            _deadLetterSuffix = deadLetterSuffix;
            _defaultDataKey = defaultDataKey;
            MessageId = messId.ToString();
            DeliveryCount = Math.Max(1, deliveryCount);
        }

        public RedisCallbackMessage(string topic, byte[] body, IDatabase db,
            string consumerGroup, RedisValue messId, CommandFlags commandFlags) :
            this(topic, body, db, consumerGroup, messId, commandFlags,
                ".DLQ", "bufmq.data")
        {
        }

        protected override async Task AckCoreAsync()
        {
            if (_ackHandler != null)
            {
                await _ackHandler().ConfigureAwait(false);
                return;
            }
            if (_db != null)
            {
                await _db.StreamAcknowledgeAsync(_topic, _consumerGroup, _messId,
                    _commandFlags).ConfigureAwait(false);
            }
        }

        protected override async Task RetryCoreAsync(string reason, TimeSpan? delay)
        {
            if (_retryHandler != null)
            {
                await _retryHandler(reason, delay).ConfigureAwait(false);
            }
            // Stream 消息保持在 PEL；监听器会在 AckTimeout 后通过 XAUTOCLAIM 重新领取。
        }

        protected override async Task DeadLetterCoreAsync(string reason)
        {
            if (_deadLetterHandler != null)
            {
                await _deadLetterHandler(reason).ConfigureAwait(false);
                return;
            }
            if (_db == null)
            {
                throw new NotSupportedException("当前 Redis 消费模式没有配置死信处理器");
            }

            string deadLetterKey = _topic + _deadLetterSuffix;
            await _db.StreamAddAsync(deadLetterKey, new[]
            {
                new NameValueEntry(_defaultDataKey, _body),
                new NameValueEntry("bufmq.originalTopic", _topic),
                new NameValueEntry("bufmq.originalMessageId", MessageId ?? string.Empty),
                new NameValueEntry("bufmq.deliveryCount", DeliveryCount),
                new NameValueEntry("bufmq.failureReason", reason ?? string.Empty),
                new NameValueEntry("bufmq.deadLetterTime", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            }, flags: _commandFlags).ConfigureAwait(false);

            await _db.StreamAcknowledgeAsync(_topic, _consumerGroup, _messId,
                _commandFlags).ConfigureAwait(false);
        }

        public override void Dispose()
        {
            _db = null;
            _messId = RedisValue.Null;
            _commandFlags = CommandFlags.None;
            _consumerGroup = null;
            _deadLetterSuffix = null;
            _defaultDataKey = null;
            base.Dispose();
        }

        ~RedisCallbackMessage()
        {
            Dispose();
        }
    }
}
