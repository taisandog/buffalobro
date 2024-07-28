using Buffalo.Kernel.FastReflection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 异步锁
    /// </summary>
    public class AsyncLock:IDisposable
    {
        /// <summary>
        /// 当前任务的变量
        /// </summary>
        private static CallContext<Dictionary<object, bool>> _dicCurLock = new CallContext<Dictionary<object, bool>>();
        /// <summary>
        /// 所有资源变量
        /// </summary>
        private static Dictionary<object, bool> _dicAllKey = new Dictionary<object, bool>();

        
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
        protected object _key;
        /// <summary>
        /// 是否锁定
        /// </summary>
        private bool _hasLock = false;
       
        /// <summary>
        /// 被锁的键
        /// </summary>
        public object Key
        {
            get { return _key; }
        }
        public AsyncLock(object key)
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
        /// <param name="pollingMillisecond">轮询次数(小于等于0则为默认值)</param>
        /// <param name="millisecondsTimeout">超时时间(小于等于0则为默认值)</param>
        /// <returns></returns>
        public async Task<bool> LockAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1) 
        {
            Dictionary<object, bool> curLock = _dicCurLock.Value;
            if(curLock == null) 
            {
                curLock = new Dictionary<object, bool>();
                _dicCurLock.Value = curLock;
            }
            if (curLock.ContainsKey(_key)) //已经被锁过，不重复锁
            {
                return true;
            }
            if (millisecondsTimeout <= 0)
            {
                millisecondsTimeout = 2000;
            }
            if (pollingMillisecond <= 0)
            {
                pollingMillisecond = (int)(millisecondsTimeout / 10);
            }
            long pollingCount = millisecondsTimeout / pollingMillisecond;
            for (int i = 0; i < pollingCount; i++) 
            {
                if (TryLock(_key)) 
                {
                    _hasLock = true;
                    break;
                }
                await Task.Delay(pollingMillisecond);
            }
            if (!_hasLock) 
            {
                return false;
            }
            curLock[_key] = true;
            return true;
        }
        /// <summary>
        /// 尝试锁定
        /// </summary>
        /// <returns></returns>
        private bool TryLock(object key) 
        {
            lock (_dicAllKey)
            {

                bool isSucess = _dicAllKey.TryAdd(_key, true);
               
                return isSucess;
            }
           
        }
        /// <summary>
        /// 释放键
        /// </summary>
        public void ReleaseLock() 
        {
            if (_hasLock) 
            {
                return;
            }
            Dictionary<object, bool> curLock = _dicCurLock.Value;
            curLock.Remove(_key);
            lock (_dicAllKey)
            {
                _dicAllKey.Remove(_key);
                _hasLock = false;
            }
            _key = null;
        }
    }
}
