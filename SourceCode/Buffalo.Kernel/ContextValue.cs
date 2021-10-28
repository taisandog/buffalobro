using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 获取值的句柄
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public delegate object ContextGetHandle(string key);
    /// <summary>
    /// 设置值的句柄
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public delegate void ContextSetHandle(string key,object value);
    /// <summary>
    /// 删除值的句柄
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public delegate void ContextDeleteHandle(string key);
    /// <summary>
    /// 上下文值
    /// </summary>
    public class ContextValue
    {
        /// <summary>
        /// 当前线程上下文
        /// </summary>
        public readonly static ContextValue Current = new ContextValue();

        
        /// <summary>
        /// 上下文值
        /// </summary>
        /// <param name="key">获取值的键</param>
        /// <returns></returns>
        public object this[string key]
        {
            get 
            {

                //return _gethandle(key);
                return System.Runtime.Remoting.Messaging.CallContext.GetData(key);
            }
            set 
            {
                //_sethandle(key, value);
                System.Runtime.Remoting.Messaging.CallContext.SetData(key, value);
            }
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key"></param>
        public void DeleteValue(string key)
        {
            System.Runtime.Remoting.Messaging.CallContext.SetData(key, null);
        }
    }
}
