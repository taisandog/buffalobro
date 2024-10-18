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
    public class RedisLock : ICacheLock
    {
        private static LockObjects<string> _lokKey = new LockObjects<string>();



        /// <summary>
        /// 要锁的键
        /// </summary>
        private string _key = null;

        /// <summary>
        /// 标记为本锁的guid
        /// </summary>
        private int _guidHash;

        private static ThreadLocal<Dictionary<string, bool>> _threadContextHandle = new ThreadLocal<Dictionary<string, bool>>();
        /// <summary>
        /// 获取线程上下文集合
        /// </summary>
        private Dictionary<string, bool> _threadContext;
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
        private IDatabase _client;


        private CommandFlags _commanfFlags;

        public RedisLock(IDatabase client, string key, CommandFlags commanfFlags)
        {
            _client = client;
            _commanfFlags = commanfFlags;
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

            object lok = _lokKey.GetObject(_key);
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
                pollingMillisecond = (int)(millisecondsTimeout / 10);
            }
            long pollingCount = millisecondsTimeout / pollingMillisecond;
            Random random = new Random(Guid.NewGuid().GetHashCode());
            _guidHash = random.Next(0, int.MaxValue);
            bool ret = false;

            TimeSpan ts = TimeSpan.FromMilliseconds(millisecondsTimeout);
            //ret = _client.LockTake(_key, _guidHash, ts, _commanfFlags);

            for (int i = 0; i < pollingCount; i++)
            {
                ret = _client.LockTake(_key, _guidHash, ts, _commanfFlags);
                if (ret)
                {
                    HasLock = true;

                    break;
                }
                Thread.Sleep(pollingMillisecond);
            }

            return ret;
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
            Random random = new Random(Guid.NewGuid().GetHashCode());
            _guidHash = random.Next(0, int.MaxValue);
            bool ret = false;

            TimeSpan ts = TimeSpan.FromMilliseconds(millisecondsTimeout);
            //ret = _client.LockTake(_key, _guidHash, ts, _commanfFlags);

            for (int i = 0; i < pollingCount; i++)
            {
                ret = await _client.LockTakeAsync(_key, _guidHash, ts, _commanfFlags);
                if (ret)
                {
                    HasLock = true;

                    break;
                }
                await Task.Delay(pollingMillisecond);
            }

            return ret;
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
        private async Task<bool> UnLockUserAsync()
        {
            bool ret = false;

            RedisValue data = await _client.LockQueryAsync(_key, _commanfFlags);
            int val = RedisConverter.RedisValueToValue<int>(data, -1);
            if (val != _guidHash)
            {
                return false;
            }
            ret = await _client.LockReleaseAsync(_key, data, _commanfFlags);
            if (!ret)
            {
                return false;
            }
            DeleteLock();
            return ret;

        }
        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        private bool UnLockUser()
        {
            bool ret = false;

            RedisValue data = _client.LockQuery(_key, _commanfFlags);
            int val = RedisConverter.RedisValueToValue<int>(data, -1);
            if (val != _guidHash)
            {
                return false;
            }
            ret = _client.LockRelease(_key, data, _commanfFlags);
            if (!ret)
            {
                return false;
            }
            DeleteLock();
            return ret;

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
            
            using (AsyncTaskLock<string> aslok = new AsyncTaskLock<string>(_key))
            {
                if (!(await aslok.LockAsync()))
                {
                    return false;
                }
                _islock = await LockObjectAsync(millisecondsTimeout, pollingMillisecond);
            }
            return _islock;
        }

        public async Task<bool> UnLockAsync()
        {
            if (_islock)
            {
                bool ret = await UnLockUserAsync();
                _islock = !ret;
            }

            return !_islock;
        }

        public async ValueTask DisposeAsync()
        {
            await UnLockAsync();
        }
    }
}
