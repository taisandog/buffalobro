﻿using Buffalo.Kernel;
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
        private static AsyncTaskLock<string> _asyncLock = null;
        private object _lokObj = null;
        private string _key;
        public MemoryCacheLock(string key) 
        {
            _lokObj = _lok.GetObject(key);
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
            _asyncLock = new AsyncTaskLock<string>(_key);
            return _asyncLock.LockAsync();
        }

        public async Task<bool> UnLockAsync()
        {
            _asyncLock.ReleaseLock();
            return true;
        }

        public async ValueTask DisposeAsync()
        {
            await UnLockAsync();

        }
    }
}
