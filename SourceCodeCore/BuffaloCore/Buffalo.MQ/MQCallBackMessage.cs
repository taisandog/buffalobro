using System;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    /// <summary>
    /// 消费回调消息。Commit/CommitAsync 是 Ack/AckAsync 的兼容别名。
    /// </summary>
    public abstract class MQCallBackMessage : IDisposable
    {
        private readonly SemaphoreSlim _settlementLock = new SemaphoreSlim(1, 1);
        private MQSettlementState _settlementState = MQSettlementState.Pending;
        private int _maxDeliveryCount = int.MaxValue;
        private bool _deadLetterEnabled = true;
        private Func<MQCallBackMessage, MQSettlementState, Task> _settlementObserver;

        protected bool _isOldMessage;
        protected string _topic;
        protected byte[] _body;

        protected MQCallBackMessage(string topic, byte[] body)
        {
            _topic = topic;
            _body = body;
        }

        /// <summary>
        /// Broker 消息标识。没有原生标识的后端会生成一个本地标识。
        /// </summary>
        public string MessageId { get; protected set; }

        /// <summary>
        /// 死信对应的原主题。普通消息为空。
        /// </summary>
        public string OriginalTopic { get; internal set; }

        public string OriginalMessageId { get; internal set; }

        public string DeadLetterReason { get; internal set; }

        /// <summary>
        /// 当前已投递次数，第一次为 1。
        /// </summary>
        public int DeliveryCount { get; internal set; } = 1;

        public bool IsOldMessage
        {
            get { return _isOldMessage; }
            internal set { _isOldMessage = value; }
        }

        public bool IsRedelivered { get; internal set; }

        public MQSettlementState SettlementState
        {
            get { return _settlementState; }
        }

        public MQFailureType FailureType { get; private set; }

        public string FailureReason { get; private set; }

        public string RoutingKey { get { return _topic; } }

        public string Topic { get { return _topic; } }

        public byte[] Body { get { return _body; } }

        internal void ConfigureSettlement(int maxDeliveryCount, bool deadLetterEnabled,
            Func<MQCallBackMessage, MQSettlementState, Task> settlementObserver)
        {
            _maxDeliveryCount = Math.Max(1, maxDeliveryCount);
            _deadLetterEnabled = deadLetterEnabled;
            _settlementObserver = settlementObserver;
        }

        internal void SetFailure(MQFailureType failureType, string reason)
        {
            FailureType = failureType;
            FailureReason = reason;
        }

        public virtual void Ack()
        {
            AckAsync().GetAwaiter().GetResult();
        }

        public Task AckAsync()
        {
            return SettleAsync(MQSettlementState.Acked, MQFailureType.None, null, null);
        }

        /// <summary>
        /// 请求重新投递。达到最大投递次数时自动转入死信。
        /// </summary>
        public Task RetryAsync(string reason = null, TimeSpan? delay = null)
        {
            if (_deadLetterEnabled && DeliveryCount >= _maxDeliveryCount)
            {
                return SettleAsync(MQSettlementState.DeadLettered,
                    MQFailureType.RetryExceeded, reason, delay);
            }
            MQFailureType failureType = FailureType == MQFailureType.None
                ? MQFailureType.ExplicitRetry : FailureType;
            return SettleAsync(MQSettlementState.RetryRequested,
                failureType, reason, delay);
        }

        public Task DeadLetterAsync(string reason = null)
        {
            return SettleAsync(MQSettlementState.DeadLettered,
                MQFailureType.ExplicitReject, reason, null);
        }

        /// <summary>
        /// 兼容旧版 API。
        /// </summary>
        public virtual void Commit()
        {
            Ack();
        }

        /// <summary>
        /// 兼容旧版 API。
        /// </summary>
        public virtual Task CommitAsync()
        {
            return AckAsync();
        }

        private async Task SettleAsync(MQSettlementState state, MQFailureType failureType,
            string reason, TimeSpan? delay)
        {
            Func<MQCallBackMessage, MQSettlementState, Task> observer = null;
            await _settlementLock.WaitAsync().ConfigureAwait(false);
            try
            {
                if (_settlementState != MQSettlementState.Pending)
                {
                    return;
                }

                switch (state)
                {
                    case MQSettlementState.Acked:
                        await AckCoreAsync().ConfigureAwait(false);
                        break;
                    case MQSettlementState.RetryRequested:
                        await RetryCoreAsync(reason, delay).ConfigureAwait(false);
                        break;
                    case MQSettlementState.DeadLettered:
                        await DeadLetterCoreAsync(reason).ConfigureAwait(false);
                        break;
                    default:
                        throw new InvalidOperationException("不支持的消息结算状态:" + state);
                }

                FailureType = failureType;
                FailureReason = reason;
                _settlementState = state;
                observer = _settlementObserver;
            }
            finally
            {
                _settlementLock.Release();
            }
            if (observer != null)
            {
                await observer(this, state).ConfigureAwait(false);
            }
        }

        protected abstract Task AckCoreAsync();

        protected abstract Task RetryCoreAsync(string reason, TimeSpan? delay);

        protected abstract Task DeadLetterCoreAsync(string reason);

        public virtual void Dispose()
        {
            _body = null;
            _topic = null;
            _settlementObserver = null;
            GC.SuppressFinalize(this);
        }
    }
}
