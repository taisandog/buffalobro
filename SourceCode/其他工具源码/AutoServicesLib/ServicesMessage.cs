using Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.AutoServicesLib
{
    /// <summary>
    /// 服务运行信息
    /// </summary>
    public class ServicesMessage
    {
        private IShowMessage _messageShow;
        /// <summary>
        /// 信息类
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="message"></param>
        /// <param name="tag"></param>
        public ServicesMessage(AbsServicesHandle handle, DateTime nowDate, IShowMessage messageShow) 
        {
            _handle = handle;
            _nowDate = nowDate;
            _messageShow = messageShow;
            //_tag = tag;
        }
        /// <summary>
        /// 是否输出日志
        /// </summary>
        public bool IsShowLog
        {
            get
            {
                return _messageShow != null && _messageShow.ShowLog;
            }
        }
        /// <summary>
        /// 是否输出错误
        /// </summary>
        public bool IsShowError
        {
            get
            {
                return _messageShow != null && _messageShow.ShowError;
            }
        }
        /// <summary>
        /// 是否输出警告
        /// </summary>
        public bool IsShowWarning
        {
            get
            {
                return _messageShow != null && _messageShow.ShowWarning;
            }
        }
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            if(IsShowLog)
            {
                _messageShow.Log(FormatMessage(message));
            }
        }
        /// <summary>
        /// 输出错误
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            if (IsShowError)
            {
                
                _messageShow.LogError(FormatMessage(message));
            }
        }

        private string FormatMessage(string message)
        {
            StringBuilder sbRet = new StringBuilder(32);
            sbRet.Append("[");
            sbRet.Append(_handle.ServicesID);
            sbRet.Append("-");
            sbRet.Append(_handle.ServicesName);
            sbRet.Append("]:");
            sbRet.Append(message);
            return sbRet.ToString();
        }
        /// <summary>
        /// 输出警告
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message)
        {
            if (IsShowWarning)
            {
                _messageShow.LogWarning(FormatMessage(message));
            }
        }



        private DateTime _nowDate;
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime NowDate
        {
            get { return _nowDate; }
            
        }

        

        private AbsServicesHandle _handle;
        /// <summary>
        /// 服务类
        /// </summary>
        public AbsServicesHandle Handle
        {
            get { return _handle; }
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
