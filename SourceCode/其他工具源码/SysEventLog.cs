using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ϵͳ�¼�
    /// </summary>
    public class SysEventLog
    {
        string sourceName;
        string logName;

        /// <summary>
        /// ϵͳ��־
        /// </summary>
        /// <param name="sourceName">Դ����</param>
        /// <param name="logName">��־��</param>
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
        /// д����־
        /// </summary>
        /// <param name="message"></param>
        public void WriteLog(string message) 
        {
            EventLog.WriteEntry(sourceName, message);
        }
    }
}
