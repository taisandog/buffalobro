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
                    //if (typeof(T).FullName== "Buffalo.DB.DbCommon.DataBaseOperate")
                    //{
                    //    Debug.WriteLine("线程池获取值");
                    //    T ret = _asyncValue.Value;
                    //    if (ret == null)
                    //    {
                    //        Debug.WriteLine("线程池空值");
                    //    }
                    //}
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
