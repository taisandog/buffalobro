using Buffalo.Kernel.Collections;
using Buffalo.Kernel.FastReflection;
using Microsoft.VisualStudio.Threading;
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
    public class AsyncLock<T>:IDisposable
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
        /// 被锁的键
        /// </summary>
        public T Key
        {
            get { return _key; }
        }
        public AsyncLock(T key)
        {
            
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
            Dictionary<T, bool> curLock = _dicCurLock.Value;
            if(curLock == null) 
            {
                curLock = new Dictionary<T, bool>();
                _dicCurLock.Value = curLock;
            }
            if (curLock.ContainsKey(_key)) //已经被锁过，不重复锁
            {
                return true;
            }
            if (await TryLockAsync(_key, cancellationToke))
            {
                curLock[_key] = true;
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
            AsyncAutoResetEvent ret = null;
            ret = _dicAllKey.GetObject(_key);
            

            await ret.WaitAsync(cancellationToken);
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
            Dictionary<T, bool> curLock = _dicCurLock.Value;
            curLock.Remove(_key);

            _key =default(T);
        }
    }


    public class AutoResetLockObject<T>: LockObjects<T, AsyncAutoResetEvent>
    {
        protected override AsyncAutoResetEvent CreateInstance()
        {
            AsyncAutoResetEvent ret= new AsyncAutoResetEvent(false);
            ret.Set();
            return ret;
        }
        protected override void ReleaseValue(LockItem<T, AsyncAutoResetEvent> value)
        {
           
            
        }
    }
}
