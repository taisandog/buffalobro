using Buffalo.DB.CacheManager;
using Buffalo.DB.CacheManager.CacheCollection;
using Buffalo.Kernel.Collections;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.QueryCache.RedisCollections
{
    public class MemcachedLock : ICacheLock
    {
        private static LockObjects<string> _lokKey = new LockObjects<string>();

        /// <summary>
        /// 在上下文中的Key
        /// </summary>
        private static readonly string ContextKey = "__!!Obj.Buf.LokKey!__";
        
        /// <summary>
        /// 要锁的键
        /// </summary>
        private string _key = null;

        /// <summary>
        /// 标记为本锁的guid
        /// </summary>
        private int _guidHash;
        /// <summary>
        /// 获取线程上下文集合
        /// </summary>
        private Dictionary<string, bool> _threadContext;

        private static ThreadLocal<Dictionary<string, bool>> _threadContextHandle = new ThreadLocal<Dictionary<string, bool>>();
        /// <summary>
        /// 获取线程上下文集合（获取本线程是否已经锁过这个值）
        /// </summary>
        private Dictionary<string, bool> GetThreadContext()
        {
            if (_threadContext != null) 
            {
                return _threadContext;
            }
            
            _threadContext = _threadContextHandle.Value;
            if (_threadContext == null)
            {
                _threadContext = new Dictionary<string, bool>();
                _threadContextHandle.Value = _threadContext;
            }
            return _threadContext;
        }
        private MemcachedClient _client;



        public MemcachedLock(MemcachedClient client,string key) 
        {
            _client = client;
            _key = key;
        }

        private bool _islock = false;
        public bool Islock 
        {
            get { return _islock; }
        }

        public void Dispose()
        {
            UnLock();
        }
        /// <summary>
        /// 获取此会话是否已经锁定了
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool HasLock
        {
            get
            {
                
                Dictionary<string, bool> ht = GetThreadContext();
                return ht.ContainsKey(_key);
            }
            set
            {
                
                Dictionary<string, bool> ht = GetThreadContext();
                if (value)
                {
                    ht[_key] = true;
                }
                else
                {
                    ht.Remove(_key);
                }
            }
        }

        public bool Lock(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            if (_islock)
            {
                return true;
            }
            if (HasLock)
            {
                return true;
            }

            object lok = _lokKey.GetObject(_key) ;
            lock (lok)
            {
                _islock = LockObject(millisecondsTimeout, pollingMillisecond);
            }
            return _islock;

        }

        /// <summary>
        /// 锁定Key
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private bool LockObject(long millisecondsTimeout = -1, int pollingMillisecond = -1)
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

            _guidHash = Guid.NewGuid().GetHashCode();
            bool ret = false;

            TimeSpan ts = TimeSpan.FromMilliseconds(millisecondsTimeout);
            for (long i = 0; i < pollingCount; i++)
            {
                ret = _client.Store(StoreMode.Add, _key, _guidHash, ts);
                if (ret)
                {
                    HasLock = true;
                    return true;
                }
                Thread.Sleep(pollingMillisecond);
            }

            return false;
        }
        /// <summary>
        /// 锁定Key
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private async Task<bool> LockObjectAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
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

            _guidHash = Guid.NewGuid().GetHashCode();
            bool ret = false;

            TimeSpan ts = TimeSpan.FromMilliseconds(millisecondsTimeout);
            for (long i = 0; i < pollingCount; i++)
            {
                ret = await _client.StoreAsync(StoreMode.Add, _key, _guidHash, ts);
                if (ret)
                {
                    HasLock = true;
                    return true;
                }
                Thread.Sleep(pollingMillisecond);
            }

            return false;
        }
        public bool UnLock()
        {
            if (_islock)
            {
                _islock = !UnLockUser();
            }

            return !_islock;
        }

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private bool UnLockUser()
        {
            _client.Remove(_key);
            DeleteLock();
            return true;

        }
        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private async Task<bool> UnLockUserAsync()
        {
            await _client.RemoveAsync(_key);
            DeleteLock();
            return true;

        }
        // <summary>
        /// 删除锁
        /// </summary>
        private void DeleteLock()
        {
            
            Dictionary<string, bool> ht = GetThreadContext();
            ht.Remove(_key);
        }

        public async Task<bool> LockAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            if (_islock)
            {
                return true;
            }
            if (HasLock)
            {
                return true;
            }

            _islock = await LockObjectAsync(millisecondsTimeout, pollingMillisecond);
            
            return _islock;
        }

        public async Task<bool> UnLockAsync()
        {
            if (_islock)
            {
                bool ret=await UnLockUserAsync();
                _islock = !ret;
            }

            return !_islock;
        }
    }
}
