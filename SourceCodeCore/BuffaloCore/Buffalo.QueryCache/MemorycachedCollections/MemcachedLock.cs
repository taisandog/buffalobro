using Buffalo.DB.CacheManager;
using Buffalo.DB.CacheManager.CacheCollection;
using Buffalo.Kernel.Defaults;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.QueryCache.RedisCollections
{
    public class MemcachedLock : QueryCacheLock
    {
        private readonly MemcachedClient _client;

        public MemcachedLock(MemcachedClient client, string key) : base(key)
        {
            _client = client;
        }

        protected override LockResult LockObject(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            NormalizeTimeout(ref millisecondsTimeout, ref pollingMillisecond);
            long pollingCount = millisecondsTimeout / pollingMillisecond;

            NewGuidHash();
            TimeSpan expiration = TimeSpan.FromMilliseconds(millisecondsTimeout);
            for (long i = 0; i < pollingCount; i++)
            {
                if (_client.Store(StoreMode.Add, _key, _guidHash, expiration))
                {
                    return LockResult.Success;
                }
                Thread.Sleep(pollingMillisecond);
            }
            return LockResult.AlreadyLocked;
        }

        protected override UnlockResult UnLockUser()
        {
            object value = _client.Get(_key);
            string lockId = ValueConvertExtend.ConvertValue<string>(value, "");
            if (lockId != _guidHash)
            {
                return UnlockResult.Expired;
            }
            return _client.Remove(_key) ? UnlockResult.Success : UnlockResult.Failed;
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

    public class MemcachedLockAsync : QueryCacheLockAsync
    {
        private readonly MemcachedClient _client;

        public MemcachedLockAsync(MemcachedClient client, string key) : base(key)
        {
            _client = client;
        }

        protected override async Task<LockResult> LockObjectAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            MemcachedLock.NormalizeTimeout(ref millisecondsTimeout, ref pollingMillisecond);
            long pollingCount = millisecondsTimeout / pollingMillisecond;

            NewGuidHash();
            TimeSpan expiration = TimeSpan.FromMilliseconds(millisecondsTimeout);
            for (long i = 0; i < pollingCount; i++)
            {
                if (await _client.StoreAsync(StoreMode.Add, _key, _guidHash, expiration))
                {
                    return LockResult.Success;
                }
                await Task.Delay(pollingMillisecond);
            }
            return LockResult.AlreadyLocked;
        }

        protected override async Task<UnlockResult> UnLockUserAsync()
        {
            object value = await _client.GetAsync(_key);
            string lockId = ValueConvertExtend.ConvertValue<string>(value, "");
            if (lockId != _guidHash)
            {
                return UnlockResult.Expired;
            }
            return await _client.RemoveAsync(_key) ? UnlockResult.Success : UnlockResult.Failed;
        }
    }
}
