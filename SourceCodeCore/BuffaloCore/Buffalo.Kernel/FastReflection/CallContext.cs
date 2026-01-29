using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel.FastReflection
{


    /// <summary>
    /// 上下文变量，支持异步调用传递，在线程池且不需要异步时候设置CallContextSyncTag.SetSync()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CallContext<T>
    {
        ThreadLocal<T> _thdValue = new ThreadLocal<T>();
        AsyncLocal<AsyncLocalValue<T>> _asyncValue = new AsyncLocal<AsyncLocalValue<T>>();
        public T Value
        {
            get 
            {
                if (!CallContextSyncTag.IsInSync) 
                {
                    AsyncLocalValue<T> val = _asyncValue.Value;
                    if(val == null) 
                    {
                        return default(T);
                    }
                    return val.Value;
                }
                return _thdValue.Value;
            }
            set 
            {
                if (!CallContextSyncTag.IsInSync)
                {
                    AsyncLocalValue<T> val = _asyncValue.Value;
                    if (val == null)
                    {
                        val = new AsyncLocalValue<T>();
                        _asyncValue.Value = val;
                    }
                    val.Value = value;
                    return;
                }
                _thdValue.Value = value;
            }



        }

    }

    public class AsyncLocalValue<T> 
    {
        public T Value;
    }
    /// <summary>
    /// 上下文同步标记，在线程池且不需要异步时候设置CallContextSyncTag.SetSync()
    /// </summary>
    public class CallContextSyncTag 
    {
        private static ThreadLocal<AsyncLocalValue<bool>> _isSync = new ThreadLocal<AsyncLocalValue<bool>>();

        /// <summary>
        /// 设置为同步调用标志
        /// </summary>
        public static void SetSync()
        {
            AsyncLocalValue<bool> valObj = _isSync.Value;

            if (valObj == null)
            {
                valObj = new AsyncLocalValue<bool>();
                _isSync.Value = valObj;
            }
            valObj.Value = true;
            
        }
        /// <summary>
        /// 清除是否同步调用标志
        /// </summary>
        public static void ClearSync()
        {
            _isSync.Value = null;
        }
        /// <summary>
        /// 判断是否在同步调用中
        /// </summary>
        public static bool IsInSync
        {
            get
            {
                AsyncLocalValue<bool> valObj = _isSync.Value;
               
                if (valObj==null)
                {
                    return (!Thread.CurrentThread.IsThreadPoolThread);
                }
                return valObj.Value;
            }
        }
    }
}
