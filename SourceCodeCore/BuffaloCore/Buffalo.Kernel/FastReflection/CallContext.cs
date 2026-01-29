using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel.FastReflection
{



    public class CallContext<T>
    {
        ThreadLocal<T> _thdValue = new ThreadLocal<T>();
        AsyncLocal<AsyncLocalValue<T>> _asyncValue = new AsyncLocal<AsyncLocalValue<T>>();
       

        

        public T Value
        {
            get 
            {
                if (CallContextAsyncTag.IsInAsync) 
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
                if (CallContextAsyncTag.IsInAsync)
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
    /// 上下文异步标记
    /// </summary>
    public class CallContextAsyncTag 
    {
        private static AsyncLocal<AsyncLocalValue<int>> _isAsync = new AsyncLocal<AsyncLocalValue<int>>();
        /// <summary>
        /// 是否异步调用标志，-1表示未设置，0表示同步调用，1表示异步调用
        /// </summary>
        private static int IsAsync
        {
            get
            {
                AsyncLocalValue<int> val = _isAsync.Value;
                if (val == null)
                {
                    return -1;
                }
                return val.Value;
            }
            set
            {
                if (value < 0) 
                {
                    _isAsync.Value = null;
                    return;
                }

                AsyncLocalValue<int> val = _isAsync.Value;
                if (val == null)
                {
                    val = new AsyncLocalValue<int>();
                    _isAsync.Value = val;
                }
                val.Value = value;
            }
        }
        /// <summary>
        /// 设置是否异步调用标志
        /// </summary>
        /// <param name="isAsync">是否异步</param>
        public static void SetAsyncNx(bool isAsync) 
        {

            int val = IsAsync;
            if (val < 0)
            {
                IsAsync = isAsync ? 1 : 0;
            }
        }
        /// <summary>
        /// 清除是否异步调用标志
        /// </summary>
        public static void ClearAsync()
        {
            IsAsync = -1;


        }
        /// <summary>
        /// 判断是否在异步调用中
        /// </summary>
        public static bool IsInAsync
        {
            get
            {
                int val = IsAsync;
                if (val < 0)
                {
                    return Thread.CurrentThread.IsThreadPoolThread;
                }
                return val == 1;
            }
        }
    }
}
