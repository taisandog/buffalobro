using Buffalo.Kernel.Collections;
using Buffalo.Kernel.FastReflection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 异步锁
    /// </summary>
    public class AsyncTaskLock<T>:IDisposable
    {
        /// <summary>
        /// 当前任务的变量
        /// </summary>
        private static CallContext<Dictionary<T, bool>> _dicCurLock = new CallContext<Dictionary<T, bool>>();
        /// <summary>
        /// 所有资源变量
        /// </summary>
        private static AutoResetLockObject<T> _dicAllKey = new AutoResetLockObject<T>();

        
        /// <summary>
        /// 每次轮询的时间
        /// </summary>
        protected int _pollingMillisecond = -1;
        /// <summary>
        /// 超时时间
        /// </summary>
        protected int _millisecondsTimeout = -1;
        /// <summary>
        /// 被锁的键
        /// </summary>
        protected T _key;
        /// <summary>
        /// 是否锁定
        /// </summary>
        private bool _hasLock = false;
        /// <summary>
        /// 当前锁
        /// </summary>
        private SemaphoreSlim _curLock;
        /// <summary>
        /// 当前任务已经锁过的
        /// </summary>
        private Dictionary<T, bool> _curTaskLocked;
        /// <summary>
        /// 被锁的键
        /// </summary>
        public T Key
        {
            get { return _key; }
        }
        public AsyncTaskLock(T key)
        {
            _curTaskLocked = _dicCurLock.Value;
            if (_curTaskLocked == null)
            {
                _curTaskLocked = new Dictionary<T, bool>();
                _dicCurLock.Value = _curTaskLocked;
            }
            
            _key = key;
        }

        public void Dispose()
        {
            ReleaseLock();
            
        }


        /// <summary>
        /// 锁定资源
        /// </summary>
        /// <param name="cancellationToke">取消Token</param>
        /// <returns></returns>
        public async Task<bool> LockAsync(CancellationToken cancellationToke) 
        {
            
            
            if (_curTaskLocked.ContainsKey(_key)) //已经被锁过，不重复锁
            {
                return true;
            }
            if (await TryLockAsync(_key, cancellationToke))
            {
                _curTaskLocked[_key] = true;
                _hasLock = true;
            }
            return true;
        }
        /// <summary>
        /// 锁定资源
        /// </summary>
        /// <param name="cancellationToke">取消Token</param>
        /// <returns></returns>
        public Task<bool> LockAsync()
        {
            
            return LockAsync(CancellationToken.None);
        }
        /// <summary>
        /// 尝试锁定
        /// </summary>
        /// <returns></returns>
        private async Task<bool> TryLockAsync(T key, CancellationToken cancellationToken) 
        {
            _curLock = _dicAllKey.GetObject(_key);
            

            await _curLock.WaitAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// 释放键
        /// </summary>
        public void ReleaseLock() 
        {
            if (!_hasLock) 
            {
                return;
            }
            _curLock.Release();

            _curTaskLocked.Remove(_key);

            _key =default(T);
        }
    }


    public class AutoResetLockObject<T>: LockObjects<T, SemaphoreSlim>
    {
        
        protected override SemaphoreSlim CreateInstance()
        {
            SemaphoreSlim ret = new SemaphoreSlim(1,1);
            
            return ret;
        }
        protected override void ReleaseValue(LockItem<T, SemaphoreSlim> value)
        {
            if (value == null)
            {
                return;
            }
            SemaphoreSlim smt= value.LockObject;
            if (smt == null) 
            {
                return;
            }
            smt.Dispose();
        }
    }
}
