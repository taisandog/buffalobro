using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 会自动释放的锁，可设置等待超时
    /// </summary>
    public class Lock : IDisposable
    {
        /// <summary>
        /// 默认超时设置
        /// </summary>
        public static int defaultMillisecondsTimeout = 15000; // 15S
        private object _obj;

        /// <summary>
        /// 构造 
        /// </summary>
        /// <param name="obj">想要锁住的对象</param>
        public Lock(object obj)
        {
            TryGet(obj, defaultMillisecondsTimeout, true);
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="obj">想要锁住的对象</param>
        /// <param name="millisecondsTimeout">超时设置</param>
        public Lock(object obj, int millisecondsTimeout)
        {
            TryGet(obj, millisecondsTimeout, true);
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="obj">想要锁住的对象</param>
        /// <param name="millisecondsTimeout">超时设置</param>
        /// <param name="throwTimeoutException">是否抛出超时异常</param>
        public Lock(object obj, int millisecondsTimeout, bool throwTimeoutException)
        {
            TryGet(obj, millisecondsTimeout, throwTimeoutException);
        }

        private void TryGet(object obj, int millisecondsTimeout, bool throwTimeoutException)
        {
            if (Monitor.TryEnter(obj, millisecondsTimeout))
            {
                _obj = obj;
            }
            else
            {
                if (throwTimeoutException)
                {
                    throw new TimeoutException("锁定项:"+obj.ToString()+"已经超时");
                }
            }
        }

        /// <summary>
        /// 销毁，并释放锁
        /// </summary>
        public void Dispose()
        {
            if (_obj != null)
            {
                Monitor.Exit(_obj);
            }
        }

        /// <summary>
        /// 获取在获取锁时是否发生等待超时
        /// </summary>
        public bool IsTimeout
        {
            get
            {
                return _obj == null;
            }
        }
    }
}
