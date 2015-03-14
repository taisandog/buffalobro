using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Win32Kernel.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Buffalo.Kernel.Win32;

namespace Buffalo.Win32Kernel
{
    /// <summary>
    /// 窗体的信息
    /// </summary>
    public class FormInfo
    {
        int targetProcessId = 0;
        int curThreadId = WindowsAPI.GetCurrentThreadId();

        IntPtr pWin = IntPtr.Zero;
        public FormInfo(IntPtr pWin) 
        {
            this.pWin = pWin;
        }

        public void OpenThreadInput()
        {

            //IntPtr pWin = WindowsAPI.GetForegroundWindow();
            int id = 0;
            targetProcessId = WindowsAPI.GetWindowThreadProcessId(pWin, ref id);

            if (targetProcessId != Process.GetCurrentProcess().Id)
            {
                WindowsAPI.AttachThreadInput(curThreadId, targetProcessId, 1);
            }
        }

        public void CloseThreadInput()
        {
            if (targetProcessId != Process.GetCurrentProcess().Id)
            {
                WindowsAPI.AttachThreadInput(curThreadId, targetProcessId, 0);
                targetProcessId = 0;
            }
        }

        /// <summary>
        /// 获取控件的文本值
        /// </summary>
        /// <param name="txthandle"></param>
        /// <returns></returns>
        public static string GetContorlText(IntPtr txtHandle)
        {
            IntPtr pLen = WindowsAPI.SendMessage(txtHandle, Msg.WM_GETTEXTLENGTH, 0, IntPtr.Zero);
            int len = pLen.ToInt32() * 2 + 1;
            if (len <= 0) 
            {
                return "";
            }
            IntPtr pStr = Marshal.AllocHGlobal(len);
            string str = null;
            try
            {
                IntPtr read = WindowsAPI.SendMessage(txtHandle, Msg.WM_GETTEXT, len, pStr);
                
                str = Marshal.PtrToStringAuto(pStr);
            }
            finally
            {
                Marshal.FreeHGlobal(pStr);
            }
            return str;
        }
    }
}
