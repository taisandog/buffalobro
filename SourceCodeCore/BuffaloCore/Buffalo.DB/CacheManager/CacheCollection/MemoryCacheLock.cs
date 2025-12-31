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
    public class MemoryCacheLock : QueryCacheLock
    {
        private static LockObjects<string> _lok = new LockObjects<string>();
        private static AsyncTaskLock<string> _asyncLock = null;
        object _curLockobj = null;
        public MemoryCacheLock(string key) :base(key)
        {
            
        }

        protected override LockResult LockObject(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            bool ret = false;
            LockResult retRes = LockResult.Success;
            _curLockobj = _lok.GetObject(_key);

            if (millisecondsTimeout > 0)
            {
                ret = Monitor.TryEnter(_curLockobj, (int)millisecondsTimeout);
                if (!ret)
                {
                    retRes = LockResult.AlreadyLocked;
                }
                
            }
            else
            {
                Monitor.Enter(_curLockobj, ref ret);
                
            }
            return retRes;
        }

        protected override UnlockResult UnLockUser()
        {
            if (_curLockobj != null )
            {
                Monitor.Exit(_curLockobj);
            }
            return UnlockResult.Success;
        }


        protected override async Task<LockResult> LockObjectAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            bool ret = false;
            LockResult retRes = LockResult.Success;
            _curLockobj = _lok.GetObject(_key);
            _asyncLock = new AsyncTaskLock<string>(_key);


            ret= await _asyncLock.LockAsync();
            if (!ret) 
            {
                return LockResult.AlreadyLocked;
            }
           

            return LockResult.Success;

        }

        protected override async Task<UnlockResult> UnLockUserAsync()
        {
            if (_asyncLock != null) 
            {
                _asyncLock.ReleaseLock();
                _asyncLock = null;
            }
            return UnlockResult.Success;
        }

        
    }
}
