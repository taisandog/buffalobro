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
        private readonly bool _deleteOnAck;
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
            string deadLetterSuffix, string defaultDataKey, int deliveryCount = 1,
            bool deleteOnAck = false) :
            base(topic, body)
        {
            _db = db;
            _messId = messId;
            _commandFlags = commandFlags;
            _consumerGroup = consumerGroup;
            _deadLetterSuffix = deadLetterSuffix;
            _defaultDataKey = defaultDataKey;
            _deleteOnAck = deleteOnAck;
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
            // 自定义 ACK 处理器用于需要自行定义确认动作的消息实现。
            if (_ackHandler != null)
            {
                await _ackHandler().ConfigureAwait(false);
                return;
            }
            // 只有 Stream 消息带有数据库、消费组和消息 ID，才能发送 XACK。
            // 简单收发模式的 _db 为空，此处按设计不向 Redis 执行任何确认命令。
            if (_db != null)
            {
                await CompleteSourceAsync().ConfigureAwait(false);
            }
        }

        protected override async Task RetryCoreAsync(string reason, TimeSpan? delay)
        {
            // 保留自定义处理器入口，供明确实现了重入队逻辑的消息类型使用。
            if (_retryHandler != null)
            {
                await _retryHandler(reason, delay).ConfigureAwait(false);
                return;
            }
            // 简单收发模式创建消息时不会传入 Stream 上下文（_db 为空）。消息已经从
            // List 取走或通过 pub/sub 投递，框架无法可靠地执行 Broker 级重试。
            if (_db == null)
            {
                throw new NotSupportedException(
                    "Redis 只有 Stream 模式支持 RetryAsync；当前模式的异常请由业务处理");
            }
            // Stream 消息保持在 PEL；监听器会在 AckTimeout 后通过 XAUTOCLAIM 重新领取。
        }

        protected override async Task DeadLetterCoreAsync(string reason)
        {
            // 如果调用方提供了专用死信处理器，优先采用调用方定义的存储方式。
            if (_deadLetterHandler != null)
            {
                await _deadLetterHandler(reason).ConfigureAwait(false);
                return;
            }
            // 没有 Stream 上下文，就无法完成“写入死信后再确认原消息”的处理流程。
            if (_db == null)
            {
                throw new NotSupportedException(
                    "Redis 只有 Stream 模式支持 DeadLetterAsync；当前模式的异常请由业务处理");
            }

            // Stream 死信仍写入 Stream，随后确认（或确认并删除）原消息，避免原消息
            // 同时留在 Pending 列表中被再次认领。
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

            await CompleteSourceAsync().ConfigureAwait(false);
        }

        private async Task CompleteSourceAsync()
        {
            if (!_deleteOnAck)
            {
                // 常规确认只执行 XACK：消息退出 Pending，但仍保留在 Stream 中，
                // 后续由 maxLength/maxAge 保留策略统一清理。
                await _db.StreamAcknowledgeAsync(_topic, _consumerGroup, _messId,
                    _commandFlags).ConfigureAwait(false);
                return;
            }

            // deleteOnAck 会永久删除 Stream 记录。先确认当前消费组满足安全条件，再把
            // XACK 和 XDEL 放进同一 Redis 事务，避免只完成其中一个动作。
            await RedisStreamRetention.EnsureDeleteOnAckAllowedAsync(_db, _topic,
                _consumerGroup, _commandFlags).ConfigureAwait(false);
            ITransaction transaction = _db.CreateTransaction();
            Task<long> acknowledgeTask = transaction.StreamAcknowledgeAsync(_topic,
                _consumerGroup, _messId, _commandFlags);
            Task<long> deleteTask = transaction.StreamDeleteAsync(_topic,
                new[] { _messId }, _commandFlags);
            if (!await transaction.ExecuteAsync(_commandFlags).ConfigureAwait(false))
            {
                throw new InvalidOperationException("Redis Stream XACK/XDEL 事务执行失败");
            }
            await acknowledgeTask.ConfigureAwait(false);
            await deleteTask.ConfigureAwait(false);
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
