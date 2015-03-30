using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buffalo.Kernel.AutoServicesLib
{
    /// <summary>
    /// 服务运行信息
    /// </summary>
    public class ServicesMessage
    {
        /// <summary>
        /// 信息类
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="message"></param>
        /// <param name="tag"></param>
        public ServicesMessage(AbsServicesHandle handle) 
        {
            _handle = handle;
            //_message = message;
            //_tag = tag;
        }
        

        private AbsServicesHandle _handle;
        /// <summary>
        /// 服务类
        /// </summary>
        public AbsServicesHandle Handle
        {
            get { return _handle; }
        }
        /// <summary>
        /// 信息
        /// </summary>
        private string _message;
        /// <summary>
        /// 信息
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        
        private object _tag;
        /// <summary>
        /// 其他信息
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
    }
}
