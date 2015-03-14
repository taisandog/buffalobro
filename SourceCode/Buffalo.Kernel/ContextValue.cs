using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 上下文值
    /// </summary>
    public class ContextValue
    {
        public readonly static ContextValue Current = new ContextValue();

        /// <summary>
        /// 是否Web项目
        /// </summary>
        private bool _isWeb = CommonMethods.IsWebContext;

        /// <summary>
        /// 上下文值
        /// </summary>
        /// <param name="key">获取值的键</param>
        /// <returns></returns>
        public object this[string key]
        {
            get 
            {
                if (_isWeb)
                {
                    return System.Web.HttpContext.Current.Items[key];
                }
                else
                {
                    return System.Runtime.Remoting.Messaging.CallContext.GetData(key);
                }
            }
            set 
            {
                if (_isWeb)
                {
                    System.Web.HttpContext.Current.Items[key]=value;
                }
                else
                {
                    System.Runtime.Remoting.Messaging.CallContext.SetData(key,value);
                }
            }
        }
    }
}
