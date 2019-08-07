using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 系统事件
    /// </summary>
    public class SysEventLog
    {
        string sourceName;
        string logName;

        /// <summary>
        /// 系统日志
        /// </summary>
        /// <param name="sourceName">源名称</param>
        /// <param name="logName">日志名</param>
        public SysEventLog(string sourceName, string logName) 
        {
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, logName);
            }
            this.sourceName = sourceName; 
            this.logName = logName;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message"></param>
        public void WriteLog(string message) 
        {
            EventLog.WriteEntry(sourceName, message);
        }
    }
}
