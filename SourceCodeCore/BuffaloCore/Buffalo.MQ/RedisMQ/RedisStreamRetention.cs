using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    internal static class RedisStreamRetention
    {
        public static async Task<MQRetentionResult> ApplyAsync(IDatabase database,
            RedisKey streamKey, MQRetentionPolicy policy, CommandFlags flags)
        {
            RedisMQConfig.ValidateRetentionPolicy(policy);
            if (policy.CleanupMode == MQCleanupMode.None)
            {
                return MQRetentionResult.NotApplied("Redis Stream 自动清理未启用");
            }
            if (policy.CleanupMode == MQCleanupMode.DeleteOnAck)
            {
                return MQRetentionResult.NotApplied("deleteOnAck 在消息 ACK 时执行");
            }
            if (!await database.KeyExistsAsync(streamKey, flags).ConfigureAwait(false))
            {
                return MQRetentionResult.NotApplied("Stream 尚不存在");
            }

            StreamGroupInfo[] groups = await database.StreamGroupInfoAsync(streamKey, flags)
                .ConfigureAwait(false);
            if (groups.Any(group => group.PendingMessageCount > 0 || group.Lag != 0))
            {
                return MQRetentionResult.NotApplied(
                    "存在 Pending 或尚未读取消息，为避免多消费组数据丢失，本次不清理");
            }

            RedisResult result;
            if (policy.CleanupMode == MQCleanupMode.MaxLength)
            {
                result = await database.ExecuteAsync("XTRIM",
                    new object[] { streamKey, "MAXLEN", policy.MaxLength }, flags)
                    .ConfigureAwait(false);
            }
            else if (policy.CleanupMode == MQCleanupMode.MaxAge)
            {
                long minimumMilliseconds = DateTimeOffset.UtcNow
                    .Subtract(policy.MaxAge).ToUnixTimeMilliseconds();
                result = await database.ExecuteAsync("XTRIM",
                    new object[] { streamKey, "MINID", minimumMilliseconds + "-0" }, flags)
                    .ConfigureAwait(false);
            }
            else
            {
                throw new NotSupportedException("Redis Stream 不支持清理模式: " +
                    policy.CleanupMode);
            }

            return MQRetentionResult.Success("Redis Stream 清理完成", (long)result);
        }

        public static async Task EnsureDeleteOnAckAllowedAsync(IDatabase database,
            RedisKey streamKey, RedisValue consumerGroup, CommandFlags flags)
        {
            StreamGroupInfo[] groups = await database.StreamGroupInfoAsync(streamKey, flags)
                .ConfigureAwait(false);
            if (groups.Length != 1 || groups[0].Name != consumerGroup)
            {
                throw new InvalidOperationException(
                    "cleanupMode=deleteOnAck 只允许 Stream 中存在当前一个消费组；" +
                    "多消费组请使用 maxLength、maxAge 或 none");
            }
        }
    }
}
