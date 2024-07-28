using Buffalo.Kernel;
using Buffalo.Kernel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    public class MemoryCacheLock : ICacheLock
    {
        private static LockObjects<string> _lok = new LockObjects<string>();
        private static AsyncLock _asyncLick = null;
        private object _lokObj = null;
        
        public MemoryCacheLock(string key) 
        {
            _lokObj = _lok.GetObject(key);
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

        public bool Lock(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            bool ret = false;
            if (millisecondsTimeout > 0)
            {
                ret = Monitor.TryEnter(_lokObj, (int)millisecondsTimeout);
            }
            else
            {
                Monitor.Enter(_lokObj, ref ret);
            }
            _islock = ret;
            return ret;
        }

        public bool UnLock()
        {
            if (_lokObj != null && _islock)
            {
                Monitor.Exit(_lokObj);
                _islock = false;
                return true;
            }
            
            return false;
        }

        public Task<bool> LockAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            _asyncLick = new AsyncLock(_lokObj);
            return _asyncLick.LockAsync(millisecondsTimeout, pollingMillisecond);
        }

        public async Task<bool> UnLockAsync()
        {
            _asyncLick.ReleaseLock();
            return true;
        }

        public async ValueTask DisposeAsync()
        {
            await UnLockAsync();

        }
    }
}
