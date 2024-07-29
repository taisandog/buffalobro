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
        AsyncLocal<T> _asyncValue = new AsyncLocal<T>();
        public T Value
        {
            get 
            {
                if (Thread.CurrentThread.IsThreadPoolThread) 
                {
                    
                    return _asyncValue.Value;
                }
                return _thdValue.Value;
            }
            set 
            {
                if (Thread.CurrentThread.IsThreadPoolThread)
                {
                    _asyncValue.Value = value;
                    return;
                }
                _thdValue.Value = value;
            }
        }

    } 
}
