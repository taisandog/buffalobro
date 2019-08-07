using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Buffalo.Win32Kernel.Win32;
using System.Threading;



namespace Buffalo.Win32Kernel
{
    public class ThreadCycle
    {
        ulong starCycle;
        ulong endCycle;
        IntPtr currentThreadHandle;
        public ThreadCycle() 
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            currentThreadHandle = WindowsAPI.GetCurrentThread();
        }

        /// <summary>
        /// 获取当前线程周期
        /// </summary>
        /// <returns></returns>
        public ulong GetCurTnreadCycle() 
        {
            ulong ret=0;
            if (SystemInfo.IsWindowsVistaOrNewer)
            {
                WindowsAPI.QueryThreadCycleTime(currentThreadHandle, ref ret);
            }
            else 
            {
                long l;
                long kernelTime;
                long userTimer;
                WindowsAPI.GetThreadTimes(currentThreadHandle, out l, out l, out kernelTime,
                   out userTimer);
                ret = (ulong)(kernelTime + userTimer);
            }
            return ret;
        }
        /// <summary>
        /// 开始
        /// </summary>
        public void Star() 
        {
            starCycle = GetCurTnreadCycle();
            endCycle = 0;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop() 
        {
            endCycle = GetCurTnreadCycle();
        }

        /// <summary>
        /// 获取一共用了多少CPU周期
        /// </summary>
        /// <returns></returns>
        public ulong TotalCycle
        {
            get
            {
                if (endCycle <= 0)
                {
                    throw new Exception("计时器还没停止");
                }
                return endCycle - starCycle;
            }
        }

    }
}
