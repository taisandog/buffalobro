using Buffalo.DB.CacheManager;
using Buffalo.DB.CacheManager.CacheCollection;
using Buffalo.Kernel;
using Buffalo.Kernel.Collections;
using Buffalo.Kernel.Defaults;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.QueryCache.RedisCollections
{
    public class MemcachedLock : QueryCacheLock
    {
       

        
       
        private MemcachedClient _client;



        public MemcachedLock(MemcachedClient client,string key) :base(key)
        {
            _client = client;
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
                pollingMillisecond = (int)(millisecondsTimeout/10);
            }
            long pollingCount = millisecondsTimeout / pollingMillisecond;

            NewGuidHash();
            bool ret = false;

            TimeSpan ts = TimeSpan.FromMilliseconds(millisecondsTimeout);
            for (long i = 0; i < pollingCount; i++)
            {
                ret = _client.Store(StoreMode.Add, _key, _guidHash, ts);
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
            for (long i = 0; i < pollingCount; i++)
            {
                ret = await _client.StoreAsync(StoreMode.Add, _key, _guidHash, ts);
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
        protected override UnlockResult UnLockUser()
        {
            bool ret = false;

            object objval = _client.Get(_key);
            string val=ValueConvertExtend.ConvertValue<string>(objval, "");

           
            if (val != _guidHash)
            {
                return UnlockResult.Expired;
            }
            ret = _client.Remove(_key);
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
        protected override async Task<UnlockResult> UnLockUserAsync()
        {
            bool ret = false;

            object objval =await _client.GetAsync(_key);
            string val = ValueConvertExtend.ConvertValue<string>(objval, "");


            if (val != _guidHash)
            {
                return UnlockResult.Expired;
            }
            ret = await _client.RemoveAsync(_key);
            if (!ret)
            {
                return UnlockResult.Failed;
            }
            return UnlockResult.Success;

        }
       

    }
}
