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
                if (Thread.CurrentThread.IsThreadPoolThread) 
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
                if (Thread.CurrentThread.IsThreadPoolThread)
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
}
