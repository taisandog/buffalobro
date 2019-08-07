using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel
{
    
    /// <summary>
    /// 上下文作用域的巢状锁
    /// </summary>
    public class NestedLock : IDisposable
    {
        /// <summary>
        /// 当前锁
        /// </summary>
        private Lock _currentLock;

        /// <summary>
        /// 当前锁了的对象
        /// </summary>
        private object _lockObject;
        /// <summary>
        /// 在上下文中的Key
        /// </summary>
        private static readonly string ContextKey = "__!!Buffalo.NestedLock.Key";
        /// <summary>
        /// 获取线程上下文集合
        /// </summary>
        private Hashtable GetThreadContext()
        {
            Hashtable tab = ContextValue.Current[ContextKey] as Hashtable;
            if (tab == null)
            {
                tab = new Hashtable();
                ContextValue.Current[ContextKey] = tab;
            }
            return tab;
        }
        /// <summary>
        /// 线程级巢状锁(不超时)
        /// </summary>
        /// <param name="obj">要锁的对象</param>
        public NestedLock(object obj):this(obj,0)
        {
            
        }

        /// <summary>
        /// 线程级巢状锁
        /// </summary>
        /// <param name="obj">要锁的对象</param>
        /// <param name="timeout">超时时间(小于等于0则不超时，一直等待)</param>
        public NestedLock(object obj, int timeout )
        {
            if (obj == null)
            {
                throw new NullReferenceException("被锁定对象不能为空");
            }
            Hashtable hs = GetThreadContext();

            Lock lok = hs[obj] as Lock;
            if (lok != null)//如果已经被锁则不再锁
            {
                return;
            }
            try
            {
                _lockObject = obj;
                _currentLock = new Lock(_lockObject, timeout, true);
                hs[_lockObject] = _currentLock;
               
            }
            catch { }
        }
        /// <summary>
        /// 是否有锁
        /// </summary>
        public bool HasLock
        {
            get
            {
                return _currentLock != null;
            }
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            if (_currentLock != null)
            {
                Hashtable hs = GetThreadContext();
                hs.Remove(_lockObject);
                _currentLock.Dispose();
            }
        }
    }
}
