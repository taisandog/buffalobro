using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.Win32
{
    public class AsmManager
    {
        /// <summary>
        /// 开启新线程运行asm代码
        /// </summary>
        /// <param name="asmCode"></param>
        /// <param name="pid"></param>
        public static void RunAsm(byte[] asmCode,int pid)
        {
            IntPtr hwnd = IntPtr.Zero;
            IntPtr addre=IntPtr.Zero;
            IntPtr threadhwnd = IntPtr.Zero;
            if (pid != 0)
            {
                IntPtr hProcess = WindowsAPI.OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS | ProcessAccess.PROCESS_CREATE_THREAD | ProcessAccess.PROCESS_VM_WRITE, 0, pid);
                hwnd = hProcess;
                if (hwnd != IntPtr.Zero)
                {
                    addre = WindowsAPI.VirtualAllocEx(hwnd, 0, asmCode.Length, ProcessAccess.MEM_COMMIT, ProcessAccess.PAGE_EXECUTE_READWRITE);
                    WindowsAPI.WriteProcessMemory(hwnd, addre, asmCode, asmCode.Length, 0);
                    threadhwnd = WindowsAPI.CreateRemoteThread(hwnd, 0, 0, addre, 0, 0, ref pid);
                    WindowsAPI.VirtualFreeEx(hwnd, addre, asmCode.Length, ProcessAccess.MEM_RELEASE);
                    WindowsAPI.CloseHandle(threadhwnd);
                    WindowsAPI.CloseHandle(hwnd);
                }
            }
        }
    }
}
