using Buffalo.DB.CacheManager;
using Buffalo.DB.CacheManager.CacheCollection;
using Buffalo.Kernel;
using Buffalo.Kernel.Collections;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.QueryCache.RedisCollections
{
    public class RedisLock : QueryCacheLock
    {
       

      
        private IDatabase _client;
        private CommandFlags _commanfFlags;

        public RedisLock(IDatabase client ,string key, CommandFlags commanfFlags) :base(key)
        {
            _client = client;
            _commanfFlags = commanfFlags;
        }
        /// <summary>
        /// 锁定Key
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        protected override LockResult LockObject(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            if (millisecondsTimeout <= 0)
            {
                millisecondsTimeout = 1000;
            }
            if (pollingMillisecond <= 0)
            {
                pollingMillisecond = (int)(millisecondsTimeout / 10);
            }
            long pollingCount = millisecondsTimeout / pollingMillisecond;

            NewGuidHash();
            bool ret = false;

            TimeSpan ts = TimeSpan.FromMilliseconds(millisecondsTimeout);

            for (int i = 0; i < pollingCount; i++)
            {
                ret = _client.LockTake(_key, _guidHash, ts, _commanfFlags);
                if (ret)
                {
                    return LockResult.Success;
                }
                Thread.Sleep(pollingMillisecond);
            }

            return LockResult.AlreadyLocked;
        }
        /// <summary>
        /// 锁定Key
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        protected override async Task<LockResult> LockObjectAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            if (millisecondsTimeout <= 0)
            {
                millisecondsTimeout = 1000;
            }
            if (pollingMillisecond <= 0)
            {
                pollingMillisecond = (int)(millisecondsTimeout / 10);
            }
            long pollingCount = millisecondsTimeout / pollingMillisecond;
            NewGuidHash();
            bool ret = false;

            TimeSpan ts = TimeSpan.FromMilliseconds(millisecondsTimeout);


            for (int i = 0; i < pollingCount; i++)
            {
                ret = await _client.LockTakeAsync(_key, _guidHash, ts, _commanfFlags);
                if (ret)
                {
                    

                    return LockResult.Success;
                }
                await Task.Delay(pollingMillisecond);
            }

            return LockResult.AlreadyLocked;
        }



        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        protected override async Task<UnlockResult> UnLockUserAsync()
        {
            bool ret = false;
            
            RedisValue data = await _client.LockQueryAsync(_key, _commanfFlags);
            string val = RedisConverter.RedisValueToValue<string>(data, "");
            if (val != _guidHash)
            {
                return UnlockResult.Expired;
            }
            ret = _client.LockRelease(_key, data, _commanfFlags);
            if (!ret)
            {
                return UnlockResult.Failed;
            }
            return UnlockResult.Success;

        }
        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        protected override UnlockResult UnLockUser()
        {
            bool ret = false;

            RedisValue data = _client.LockQuery(_key, _commanfFlags);
            string val = RedisConverter.RedisValueToValue<string>(data, "");
            if (val != _guidHash)
            {
                return UnlockResult.Expired;
            }
            ret = _client.LockRelease(_key, data, _commanfFlags);
            if (!ret)
            {
                return UnlockResult.Failed;
            }
            return UnlockResult.Success;

        }
        
    }
}
