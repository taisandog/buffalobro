using Buffalo.Kernel;
using Buffalo.Kernel.Collections;
using Buffalo.Kernel.FastReflection;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    
    /// <summary>
    /// 缓存锁
    /// </summary>
    public abstract class QueryCacheLock:IDisposable,IAsyncDisposable
    {
        protected static LockObjects<string> _lokKey = new LockObjects<string>();
        protected static ThreadLocal<Dictionary<string, bool>> _thdValue = new ThreadLocal<Dictionary<string, bool>>();
        public static AsyncLocal<Dictionary<string, bool>> _asyncValue = new AsyncLocal<Dictionary<string, bool>>();

        protected bool _islock = false;
        /// <summary>
        /// 当前类是否被锁过
        /// </summary>
        public bool Islock
        {
            get { return _islock; }
        }
        /// <summary>
        /// 要锁的键
        /// </summary>
        protected string _key = null;
        /// <summary>
        /// 缓存锁
        /// </summary>
        /// <param name="key">要锁的key</param>
        public QueryCacheLock(string key)
        {
            _key = key;
            Dictionary<string, bool> threadContext = GetThreadContext(true);//预热异步数据
        }
        /// <summary>
        /// 标记为本锁的guid
        /// </summary>
        protected string _guidHash;
        /// <summary>
        /// 创建新的guid标记
        /// </summary>
        protected void NewGuidHash()
        {
            _guidHash = CommonMethods.GuidToString(Guid.NewGuid());
        }

        

        
        /// <summary>
        /// 获取线程上下文集合（获取本线程是否已经锁过这个值）
        /// </summary>
        /// <param name="isSync">是否异步</param>
        /// <returns></returns>
        protected  Dictionary<string, bool> GetThreadContext(bool isSync)
        {
            
            if (isSync)
            {
                // 如果这里返回 null，说明异步流断了。
                // 我们在这里初始化，确保它能流向后续的嵌套调用（如 lok1）
                if (_asyncValue.Value == null)
                {
                    _asyncValue.Value = new Dictionary<string, bool>();
                }
                return _asyncValue.Value;
            }
            else
            {
                // ThreadLocal 确实不能跨线程，线程变了它必为 null
                if (_thdValue.Value == null)
                {
                    _thdValue.Value = new Dictionary<string, bool>();
                }
                return _thdValue.Value;
            }
        }

        /// <summary>
        /// 获取此会话是否已经锁定了
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected bool GetHasLock(bool isSync)
        {
            Dictionary<string, bool> ht = GetThreadContext(true);
            return ht.ContainsKey(_key);
        }
        /// <summary>
        /// 删除当前会话已锁标记
        /// </summary>
        private void DeleteLockMark(bool isSync)
        {

            Dictionary<string, bool> ht = GetThreadContext(true);
            ht.Remove(_key);
        }

        /// <summary>
        /// 标记当前会话已开启锁
        /// </summary>
        private void AddLockMark(bool isSync)
        {
            Dictionary<string, bool> ht = GetThreadContext(true);
            ht[_key] = true;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            UnLock();
        }

        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="millisecondsTimeout">等待毫秒数</param>
        /// <param name="pollingMillisecond">轮询间隔</param>
        /// <returns></returns>
        public LockResult Lock(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {
            if (_islock)
            {
                return LockResult.Success;
            }
            if (GetHasLock(false))
            {
                return LockResult.Success;
            }
            LockResult res = LockResult.Failed;
            object lok = _lokKey.GetObject(_key);
            lock (lok)
            {
                res = LockObject(millisecondsTimeout, pollingMillisecond);
                if (res == LockResult.Success)
                {
                    AddLockMark(false);
                    _islock = true;
                }
            }
            return res;
        }
        /// <summary>
        /// 解锁
        /// </summary>
        /// <returns></returns>
        public UnlockResult UnLock()
        {
            if (_islock)
            {
                UnlockResult res = UnLockUser();
                if (res == UnlockResult.Success)
                {
                    AddLockMark(false);
                    _islock = false;
                }
                return res;
            }

            return UnlockResult.Success;
        }

        
        /// <summary>
        /// 异步锁定
        /// </summary>
        /// <param name="millisecondsTimeout">轮询毫秒数</param>
        /// <param name="pollingMillisecond">间隔毫秒数</param>
        /// <returns></returns>
        public async Task<LockResult> LockAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1)
        {

            if (_islock)
            {
                return LockResult.Success;
            }
            if (GetHasLock(true))
            {
                return LockResult.Success;
            }
            LockResult res = LockResult.Failed;
            string lockKey ="lok."+ _key;
            //using (AsyncTaskLock<string> aslok = new AsyncTaskLock<string>(lockKey))
            //{
            //    if (!(await aslok.LockAsync()))
            //    {
            //        return LockResult.Failed;
            //    }
                res = await LockObjectAsync(millisecondsTimeout, pollingMillisecond);
                if (res == LockResult.Success)
                {
                    AddLockMark(true);
                    _islock = true;
                }
            //}

            return res;
        }
        /// <summary>
        /// 异步解锁
        /// </summary>
        /// <returns></returns>
        public async Task<UnlockResult> UnLockAsync()
        {
            if (_islock)
            {
                UnlockResult res = await UnLockUserAsync();
                if (res == UnlockResult.Success)
                {
                    DeleteLockMark(true);
                    _islock = false;
                }
                return res;
            }

            return UnlockResult.Success;

        }
        /// <summary>
        /// 异步释放资源
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await UnLockAsync();
        }

        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="pollingMillisecond"></param>
        /// <returns></returns>
        protected abstract LockResult LockObject(long millisecondsTimeout = -1, int pollingMillisecond = -1);

        /// <summary>
        /// 异步锁定
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="pollingMillisecond"></param>
        /// <returns></returns>
        protected abstract Task<LockResult> LockObjectAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1);

        /// <summary>
        /// 异步解锁
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        protected abstract Task<UnlockResult> UnLockUserAsync();

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        protected abstract UnlockResult UnLockUser();
    }
}
