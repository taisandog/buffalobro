using Buffalo.Kernel;
using System;
using System.Collections.Generic;

namespace Buffalo.MQ
{
    /// <summary>
    /// 消息确认方式。
    /// </summary>
    public enum MQAckMode
    {
        /// <summary>
        /// 由业务代码调用 AckAsync/CommitAsync。
        /// </summary>
        Manual = 0,
        /// <summary>
        /// 回调正常结束且消息尚未结算时自动确认。
        /// </summary>
        OnSuccess = 1
    }

    /// <summary>
    /// 消息结算状态。
    /// </summary>
    public enum MQSettlementState
    {
        Pending = 0,
        Acked = 1,
        RetryRequested = 2,
        DeadLettered = 3
    }

    /// <summary>
    /// 消息失败类型。AckTimeout/ConsumerLost 都只能说明消息未完成，不能证明业务处理失败。
    /// </summary>
    public enum MQFailureType
    {
        None = 0,
        HandlerException = 1,
        ExplicitRetry = 2,
        ExplicitReject = 3,
        AckTimeout = 4,
        ConsumerLost = 5,
        RetryExceeded = 6
    }

    /// <summary>
    /// 消费重试及死信配置。
    /// </summary>
    public sealed class MQRetryOptions
    {
        public MQAckMode AckMode { get; set; } = MQAckMode.Manual;

        public bool RetryEnabled { get; set; } = true;

        public bool RetryOnHandlerException { get; set; } = true;

        /// <summary>
        /// 最大投递次数，包含第一次投递。
        /// </summary>
        public int MaxDeliveryCount { get; set; } = 5;

        public int RetryDelayMilliseconds { get; set; } = 1000;

        /// <summary>
        /// 未确认多久后可被其他消费者重新领取。主要用于 Redis Stream。
        /// </summary>
        public int AckTimeoutMilliseconds { get; set; } = 30000;

        public int PendingScanIntervalMilliseconds { get; set; } = 5000;

        public bool DeadLetterEnabled { get; set; } = true;

        public string DeadLetterSuffix { get; set; } = ".DLQ";

        internal static MQRetryOptions FromConfig(Dictionary<string, string> configs)
        {
            MQRetryOptions options = new MQRetryOptions();
            string value = configs.GetDicValue<string, string>("ackMode");
            if (string.Equals(value, "onSuccess", StringComparison.OrdinalIgnoreCase) || value == "1")
            {
                options.AckMode = MQAckMode.OnSuccess;
            }

            value = configs.GetDicValue<string, string>("retryEnabled");
            if (!string.IsNullOrWhiteSpace(value))
            {
                options.RetryEnabled = value == "1";
            }

            value = configs.GetDicValue<string, string>("retryOnException");
            if (!string.IsNullOrWhiteSpace(value))
            {
                options.RetryOnHandlerException = value == "1";
            }

            options.MaxDeliveryCount = Math.Max(1,
                configs.GetDicValue<string, string>("maxRetry").ConvertTo<int>(4) + 1);
            options.RetryDelayMilliseconds = Math.Max(0,
                configs.GetDicValue<string, string>("retryDelay").ConvertTo<int>(1000));
            options.AckTimeoutMilliseconds = Math.Max(1000,
                configs.GetDicValue<string, string>("ackTimeout").ConvertTo<int>(30000));
            options.PendingScanIntervalMilliseconds = Math.Max(500,
                configs.GetDicValue<string, string>("pendingScanInterval").ConvertTo<int>(5000));

            value = configs.GetDicValue<string, string>("deadLetterEnabled");
            if (!string.IsNullOrWhiteSpace(value))
            {
                options.DeadLetterEnabled = value == "1";
            }

            value = configs.GetDicValue<string, string>("deadLetterSuffix");
            if (!string.IsNullOrWhiteSpace(value))
            {
                options.DeadLetterSuffix = value;
            }
            return options;
        }

        public string GetDeadLetterTopic(string topic)
        {
            return topic + DeadLetterSuffix;
        }
    }
}
