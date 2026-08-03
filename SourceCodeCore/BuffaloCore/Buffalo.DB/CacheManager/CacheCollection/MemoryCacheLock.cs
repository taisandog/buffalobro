using Buffalo.Kernel;
using Buffalo.Kernel.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    public class MemoryCacheLock : QueryCacheLock
    {
        private static readonly LockObjects<string> _locks = new LockObjects<string>();
        private object _currentLock;

        public MemoryCacheLock(string key) : base(key)
        {
        }

        protected override LockResult LockObject(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            bool lockTaken = false;
            _currentLock = _locks.GetObject(_key);

            if (millisecondsTimeout > 0)
            {
                lockTaken = Monitor.TryEnter(_currentLock, (int)millisecondsTimeout);
                return lockTaken ? LockResult.Success : LockResult.AlreadyLocked;
            }

            Monitor.Enter(_currentLock, ref lockTaken);
            return LockResult.Success;
        }

        protected override UnlockResult UnLockUser()
        {
            if (_currentLock != null)
            {
                Monitor.Exit(_currentLock);
                _currentLock = null;
            }
            return UnlockResult.Success;
        }
    }

    public class MemoryCacheLockAsync : QueryCacheLockAsync
    {
        private AsyncTaskLock<string> _asyncLock;

        public MemoryCacheLockAsync(string key) : base(key)
        {
        }

        protected override async Task<LockResult> LockObjectAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            _asyncLock = new AsyncTaskLock<string>(_key);
            bool locked = await _asyncLock.LockAsync();
            return locked ? LockResult.Success : LockResult.AlreadyLocked;
        }

        protected override Task<UnlockResult> UnLockUserAsync()
        {
            if (_asyncLock != null)
            {
                _asyncLock.ReleaseLock();
                _asyncLock = null;
            }
            return Task.FromResult(UnlockResult.Success);
        }
    }
}
