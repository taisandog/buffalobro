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

        ///// <summary>
        ///// 获取的方法
        ///// </summary>
        //private ContextGetHandle _gethandle;
        ///// <summary>
        ///// 设置的方法
        ///// </summary>
        //private ContextSetHandle _sethandle;
        ///// <summary>
        ///// 设置的方法
        ///// </summary>
        //private ContextDeleteHandle _deletehandle;

        ///// <summary>
        ///// 本线程上下文
        ///// </summary>
        //public ContextValue() 
        //{
        //    if (CommonMethods.IsWebContext) 
        //    {
        //        _gethandle = WebGetValue;
        //        _sethandle = WebSetValue;
        //        _deletehandle = WebDeleteValue;
        //    }
        //    else 
        //    {
        //        _gethandle = AppGetValue;
        //        _sethandle = AppSetValue;
        //        _deletehandle = AppDeleteValue;
        //    }
        //}

        ///// <summary>
        ///// Web形式获取值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private object WebGetValue(string key) 
        //{
        //    return System.Web.HttpContext.Current.Items[key];
        //}
        ///// <summary>
        ///// Web形式设置值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private void WebSetValue(string key, object value)
        //{
        //    System.Web.HttpContext.Current.Items[key] = value;
        //}
        ///// <summary>
        ///// Web形式删除值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private void WebDeleteValue(string key)
        //{
        //    System.Web.HttpContext.Current.Items.Remove(key);
        //}
        ///// <summary>
        ///// Win程序形式获取值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private object AppGetValue(string key)
        //{
        //    return System.Runtime.Remoting.Messaging.CallContext.GetData(key);
        //}
        ///// <summary>
        ///// Win程序形式设置值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private void AppSetValue(string key, object value)
        //{
        //    System.Runtime.Remoting.Messaging.CallContext.SetData(key, value);
        //}
        ///// <summary>
        ///// Win程序形式删除值
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private void AppDeleteValue(string key)
        //{
        //    System.Runtime.Remoting.Messaging.CallContext.SetData(key, null);
        //}
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
            //_deletehandle(key);
            System.Runtime.Remoting.Messaging.CallContext.SetData(key, null);
        }
    }
}
