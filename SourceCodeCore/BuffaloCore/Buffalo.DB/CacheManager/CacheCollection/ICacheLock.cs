using Buffalo.Kernel;
using Buffalo.Kernel.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    /// <summary>
    /// 同步缓存锁。
    /// </summary>
    public abstract class QueryCacheLock : IDisposable
    {
        protected static readonly LockObjects<string> _lokKey = new LockObjects<string>();
        private static readonly ThreadLocal<Dictionary<string, bool>> _lockMarks =
            new ThreadLocal<Dictionary<string, bool>>();

        protected bool _islock;
        protected readonly string _key;
        protected string _guidHash;

        protected QueryCacheLock(string key)
        {
            _key = key;
        }

        public bool Islock
        {
            get { return _islock; }
        }

        protected void NewGuidHash()
        {
            _guidHash = CommonMethods.GuidToString(Guid.NewGuid());
        }

        private static Dictionary<string, bool> GetLockMarks()
        {
            Dictionary<string, bool> marks = _lockMarks.Value;
            if (marks == null)
            {
                marks = new Dictionary<string, bool>();
                _lockMarks.Value = marks;
            }
            return marks;
        }

        public LockResult Lock(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            if (_islock)
            {
                return LockResult.Success;
            }

            Dictionary<string, bool> marks = GetLockMarks();
            if (marks.ContainsKey(_key))
            {
                return LockResult.Success;
            }

            LockResult result;
            object localLock = _lokKey.GetObject(_key);
            lock (localLock)
            {
                result = LockObject(millisecondsTimeout, pollingMillisecond);
                if (result == LockResult.Success)
                {
                    marks[_key] = true;
                    _islock = true;
                }
            }
            return result;
        }

        public UnlockResult UnLock()
        {
            if (!_islock)
            {
                return UnlockResult.Success;
            }

            UnlockResult result = UnLockUser();
            if (result == UnlockResult.Success)
            {
                GetLockMarks().Remove(_key);
                _islock = false;
            }
            return result;
        }

        public void Dispose()
        {
            UnLock();
        }

        protected abstract LockResult LockObject(long millisecondsTimeout = -1, int pollingMillisecond = -1);
        protected abstract UnlockResult UnLockUser();
    }

    /// <summary>
    /// 异步缓存锁。
    /// </summary>
    public abstract class QueryCacheLockAsync : IAsyncDisposable
    {
        private static readonly AsyncLocal<Dictionary<string, bool>> _lockMarks =
            new AsyncLocal<Dictionary<string, bool>>();

        private readonly Dictionary<string, bool> _currentLockMarks;
        protected bool _islock;
        protected readonly string _key;
        protected string _guidHash;

        protected QueryCacheLockAsync(string key)
        {
            _key = key;

            // 构造函数在调用方上下文同步执行。在进入 LockAsync 前创建集合，
            // 确保后续 await 和嵌套锁继承同一个锁标记集合。
            _currentLockMarks = _lockMarks.Value;
            if (_currentLockMarks == null)
            {
                _currentLockMarks = new Dictionary<string, bool>();
                _lockMarks.Value = _currentLockMarks;
            }
        }

        public bool Islock
        {
            get { return _islock; }
        }

        protected void NewGuidHash()
        {
            _guidHash = CommonMethods.GuidToString(Guid.NewGuid());
        }

        public async Task<LockResult> LockAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            if (_islock)
            {
                return LockResult.Success;
            }
            if (_currentLockMarks.ContainsKey(_key))
            {
                return LockResult.Success;
            }

            LockResult result = await LockObjectAsync(millisecondsTimeout, pollingMillisecond);
            if (result == LockResult.Success)
            {
                _currentLockMarks[_key] = true;
                _islock = true;
            }
            return result;
        }

        public async Task<UnlockResult> UnLockAsync()
        {
            if (!_islock)
            {
                return UnlockResult.Success;
            }

            UnlockResult result = await UnLockUserAsync();
            if (result == UnlockResult.Success)
            {
                _currentLockMarks.Remove(_key);
                _islock = false;
            }
            return result;
        }

        public async ValueTask DisposeAsync()
        {
            await UnLockAsync();
        }

        protected abstract Task<LockResult> LockObjectAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1);
        protected abstract Task<UnlockResult> UnLockUserAsync();
    }
}
