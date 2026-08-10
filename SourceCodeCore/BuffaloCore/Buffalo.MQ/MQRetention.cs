using System;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    /// <summary>
    /// MQ 后端支持的消息保留能力。
    /// </summary>
    [Flags]
    public enum MQRetentionCapabilities
    {
        None = 0,
        /// <summary>ACK 后 Broker 会移除当前队列中的消息。</summary>
        AckRemovesMessage = 1,
        /// <summary>支持按消息条数保留。</summary>
        MaxLength = 2,
        /// <summary>支持按时间保留。</summary>
        MaxAge = 4,
        /// <summary>支持按字节数保留。</summary>
        MaxBytes = 8,
        /// <summary>支持 ACK 后物理删除持久化记录。</summary>
        DeleteOnAck = 16,
        /// <summary>保留和清理由 Broker 执行。</summary>
        BrokerManaged = 32
    }

    /// <summary>
    /// 自动清理模式。
    /// </summary>
    public enum MQCleanupMode
    {
        /// <summary>不由 Buffalo.MQ 自动清理。</summary>
        None = 0,
        /// <summary>按最大记录数清理。</summary>
        MaxLength = 1,
        /// <summary>按消息年龄清理。</summary>
        MaxAge = 2,
        /// <summary>ACK 后物理删除。仅适合确认只有一个消费组的存储模型。</summary>
        DeleteOnAck = 3
    }

    /// <summary>
    /// 消息保留策略。后端只应用其能力范围内的字段。
    /// </summary>
    public sealed class MQRetentionPolicy
    {
        public MQCleanupMode CleanupMode { get; set; } = MQCleanupMode.None;

        /// <summary>最大记录数，Redis Stream 的 MaxLength 模式使用。</summary>
        public long MaxLength { get; set; }

        /// <summary>最大存储字节数，Kafka Topic 使用。</summary>
        public long MaxBytes { get; set; }

        /// <summary>最大消息年龄。</summary>
        public TimeSpan MaxAge { get; set; }

        /// <summary>客户端执行保留策略的间隔；Broker 管理型后端可忽略。</summary>
        public TimeSpan CleanupInterval { get; set; } = TimeSpan.FromMinutes(30);

        public MQRetentionPolicy Clone()
        {
            return new MQRetentionPolicy
            {
                CleanupMode = CleanupMode,
                MaxLength = MaxLength,
                MaxBytes = MaxBytes,
                MaxAge = MaxAge,
                CleanupInterval = CleanupInterval
            };
        }
    }

    /// <summary>
    /// 应用保留策略的结果。
    /// </summary>
    public sealed class MQRetentionResult
    {
        public bool Applied { get; internal set; }

        /// <summary>立即清理的记录数；仅配置 Broker 策略时为 0。</summary>
        public long RemovedCount { get; internal set; }

        public string Message { get; internal set; }

        internal static MQRetentionResult NotApplied(string message)
        {
            return new MQRetentionResult { Applied = false, Message = message };
        }

        internal static MQRetentionResult Success(string message, long removedCount = 0)
        {
            return new MQRetentionResult
            {
                Applied = true,
                RemovedCount = removedCount,
                Message = message
            };
        }
    }

    /// <summary>
    /// MQ 保留策略管理接口。它与消息 ACK 分离，不承诺所有后端都能逐消息删除。
    /// </summary>
    public interface IMQRetentionManager
    {
        MQRetentionCapabilities RetentionCapabilities { get; }

        MQRetentionResult ApplyRetentionPolicy(string topic, MQRetentionPolicy policy);

        Task<MQRetentionResult> ApplyRetentionPolicyAsync(string topic,
            MQRetentionPolicy policy);
    }
}
