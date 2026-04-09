using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel.FastReflection
{


    /// <summary>
    /// 上下文变量，支持异步调用传递，在线程池且不需要异步时候设置CallContextSyncTag.SetAsync()
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
                if (CallContextSyncTag.IsAsync) 
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
                if (CallContextSyncTag.IsAsync)
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
    /// 上下文同步标记，在线程池且不需要异步时候设置CallContextSyncTag.SetAsync()
    /// </summary>
    public class CallContextSyncTag 
    {
        private static ThreadLocal<int> _isAsync = new ThreadLocal<int>();

        /// <summary>
        /// 设置为异步调用标志
        /// </summary>
        /// <param name="isAsync">是否异步</param>
        public static void SetAsync(bool isAsync)
        {

            _isAsync.Value = isAsync ? 1:2;
            
        }
       
        /// <summary>
        /// 清除是否同步调用标志
        /// </summary>
        public static void ClearSetting()
        {
            _isAsync.Value = 0;
        }
        /// <summary>
        /// 判断是否在异步调用中
        /// </summary>
        public static bool IsAsync
        {
            get
            {
                int valObj = _isAsync.Value;
               
                if (valObj<=0)
                {
                    return Thread.CurrentThread.IsThreadPoolThread;
                }
                return valObj==1;
            }
        }
    }
}
