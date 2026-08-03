using Buffalo.DB.CacheManager;
using Buffalo.DB.CacheManager.CacheCollection;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.QueryCache.RedisCollections
{
    public class RedisLock : QueryCacheLock
    {
        private readonly IDatabase _client;
        private readonly CommandFlags _commandFlags;

        public RedisLock(IDatabase client, string key, CommandFlags commandFlags) : base(key)
        {
            _client = client;
            _commandFlags = commandFlags;
        }

        protected override LockResult LockObject(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            NormalizeTimeout(ref millisecondsTimeout, ref pollingMillisecond);
            long pollingCount = millisecondsTimeout / pollingMillisecond;

            NewGuidHash();
            TimeSpan expiration = TimeSpan.FromMilliseconds(millisecondsTimeout);
            for (long i = 0; i < pollingCount; i++)
            {
                if (_client.LockTake(_key, _guidHash, expiration, _commandFlags))
                {
                    return LockResult.Success;
                }
                Thread.Sleep(pollingMillisecond);
            }
            return LockResult.AlreadyLocked;
        }

        protected override UnlockResult UnLockUser()
        {
            RedisValue value = _client.LockQuery(_key, _commandFlags);
            string lockId = RedisConverter.RedisValueToValue<string>(value, "");
            if (lockId != _guidHash)
            {
                return UnlockResult.Expired;
            }
            return _client.LockRelease(_key, value, _commandFlags)
                ? UnlockResult.Success
                : UnlockResult.Failed;
        }

        internal static void NormalizeTimeout(ref long millisecondsTimeout, ref int pollingMillisecond)
        {
            if (millisecondsTimeout <= 0)
            {
                millisecondsTimeout = 1000;
            }
            if (pollingMillisecond <= 0)
            {
                pollingMillisecond = (int)(millisecondsTimeout / 10);
            }
        }
    }

    public class RedisLockAsync : QueryCacheLockAsync
    {
        private readonly IDatabase _client;
        private readonly CommandFlags _commandFlags;

        public RedisLockAsync(IDatabase client, string key, CommandFlags commandFlags) : base(key)
        {
            _client = client;
            _commandFlags = commandFlags;
        }

        protected override async Task<LockResult> LockObjectAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            RedisLock.NormalizeTimeout(ref millisecondsTimeout, ref pollingMillisecond);
            long pollingCount = millisecondsTimeout / pollingMillisecond;

            NewGuidHash();
            TimeSpan expiration = TimeSpan.FromMilliseconds(millisecondsTimeout);
            for (long i = 0; i < pollingCount; i++)
            {
                if (await _client.LockTakeAsync(_key, _guidHash, expiration, _commandFlags))
                {
                    return LockResult.Success;
                }
                await Task.Delay(pollingMillisecond);
            }
            return LockResult.AlreadyLocked;
        }

        protected override async Task<UnlockResult> UnLockUserAsync()
        {
            RedisValue value = await _client.LockQueryAsync(_key, _commandFlags);
            string lockId = RedisConverter.RedisValueToValue<string>(value, "");
            if (lockId != _guidHash)
            {
                return UnlockResult.Expired;
            }
            return await _client.LockReleaseAsync(_key, value, _commandFlags)
                ? UnlockResult.Success
                : UnlockResult.Failed;
        }
    }
}
