using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ��ȡֵ�ľ��
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public delegate object ContextGetHandle(string key);
    /// <summary>
    /// ����ֵ�ľ��
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public delegate void ContextSetHandle(string key,object value);
    /// <summary>
    /// ɾ��ֵ�ľ��
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public delegate void ContextDeleteHandle(string key);
    /// <summary>
    /// ������ֵ
    /// </summary>
    public class ContextValue
    {
        /// <summary>
        /// ��ǰ�߳�������
        /// </summary>
        public readonly static ContextValue Current = new ContextValue();

        ///// <summary>
        ///// ��ȡ�ķ���
        ///// </summary>
        //private ContextGetHandle _gethandle;
        ///// <summary>
        ///// ���õķ���
        ///// </summary>
        //private ContextSetHandle _sethandle;
        ///// <summary>
        ///// ���õķ���
        ///// </summary>
        //private ContextDeleteHandle _deletehandle;

        ///// <summary>
        ///// ���߳�������
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
        ///// Web��ʽ��ȡֵ
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private object WebGetValue(string key) 
        //{
        //    return System.Web.HttpContext.Current.Items[key];
        //}
        ///// <summary>
        ///// Web��ʽ����ֵ
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private void WebSetValue(string key, object value)
        //{
        //    System.Web.HttpContext.Current.Items[key] = value;
        //}
        ///// <summary>
        ///// Web��ʽɾ��ֵ
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private void WebDeleteValue(string key)
        //{
        //    System.Web.HttpContext.Current.Items.Remove(key);
        //}
        ///// <summary>
        ///// Win������ʽ��ȡֵ
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private object AppGetValue(string key)
        //{
        //    return System.Runtime.Remoting.Messaging.CallContext.GetData(key);
        //}
        ///// <summary>
        ///// Win������ʽ����ֵ
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private void AppSetValue(string key, object value)
        //{
        //    System.Runtime.Remoting.Messaging.CallContext.SetData(key, value);
        //}
        ///// <summary>
        ///// Win������ʽɾ��ֵ
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private void AppDeleteValue(string key)
        //{
        //    System.Runtime.Remoting.Messaging.CallContext.SetData(key, null);
        //}
        /// <summary>
        /// ������ֵ
        /// </summary>
        /// <param name="key">��ȡֵ�ļ�</param>
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
        /// ɾ��ֵ
        /// </summary>
        /// <param name="key"></param>
        public void DeleteValue(string key)
        {
            //_deletehandle(key);
            System.Runtime.Remoting.Messaging.CallContext.SetData(key, null);
        }
    }
}
