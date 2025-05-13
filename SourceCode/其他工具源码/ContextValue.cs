using Buffalo.Kernel.FastReflection;
using System;
using System.Collections.Generic;
using System.Security;
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
                return CallContext.GetData(key);
            }
            set 
            {
                //_sethandle(key, value);
                CallContext.SetData(key, value);
            }
        }
        /// <summary>
        /// ɾ��ֵ
        /// </summary>
        /// <param name="key"></param>
        public void DeleteValue(string key)
        {
            //_deletehandle(key);
            CallContext.SetData(key, null);
        }


       
    }
}
