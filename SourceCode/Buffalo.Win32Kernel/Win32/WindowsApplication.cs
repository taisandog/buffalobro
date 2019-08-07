using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace Buffalo.Win32Kernel.Win32
{
    /// <summary>
    /// 系统程序
    /// </summary>
    public class WindowsApplication
    {
        /// <summary>
        /// 用Debug模式关闭进程(本进程也会关闭)
        /// </summary>
        /// <param name="id"></param>
        public static void DebugKillProcessById(int id) 
        {
            IntPtr hProcess=WindowsAPI.OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS, 0, id);//以所有模式打开进程
            
            int ret = WindowsAPI.DebugActiveProcess(id);//调试此进程ID
            if (ret == 0)
            {
                int err = WindowsAPI.GetLastError();
            }
            //WindowsAPI.FatalExit(0);//本进程强制结束
        }
        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="id"></param>
        public static void KillProcess(int id)
        {
            IntPtr hProcess = WindowsAPI.OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS, 0, id);//以所有模式打开进程
            WindowsAPI.TerminateProcess(hProcess, 0);
        }

        public static void SendClose(Process proc) 
        {

            //foreach (ProcessThread thd in proc.Threads)
            //{
            //    int code=WindowsAPI.GetExitCodeThread(thd.
            //    System.Threading.Thread
            //    WindowsAPI.TerminateThread(thd.Id, 0);
            //}
        }

        /// <summary>
        /// 提升Debug权限
        /// </summary>
        /// <param name="enable"></param>
        public static void EnablePrivilege(bool enable)
        {
            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();

            IntPtr tokenHaldle = IntPtr.Zero;
            bool secc = WindowsAPI.OpenProcessToken(WindowsAPI.GetCurrentProcess(), TOKEN.TOKEN_ADJUST_PRIVILEGES, ref tokenHaldle);
            secc = WindowsAPI.LookupPrivilegeValue(null, SystemName.SE_DEBUG_NAME, ref tp.Privileges.Luid);
            tp.PrivilegeCount = 1;
            if (enable)
            {
                tp.Privileges.Attributes = ConstValues.SE_PRIVILEGE_ENABLED;
            }
            else
            {
                tp.Privileges.Attributes = 0;
            }

            secc = WindowsAPI.AdjustTokenPrivileges(tokenHaldle, false, ref tp, Marshal.SizeOf(tp), IntPtr.Zero, IntPtr.Zero);
            WindowsAPI.CloseHandle(tokenHaldle);
        }
        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <param name="picRoot">壁纸路径</param>
        public static void SetWallPaper(string picRoot)
        {
            IntPtr pRoot= Marshal.StringToHGlobalAuto(picRoot);
            int nResult = WindowsAPI.SystemParametersInfo(SPIAction.SPI_SETDESKWALLPAPER, 1, pRoot, 0x1 | 0x2);
            Marshal.FreeHGlobal(pRoot);
        }

        /// <summary>
        /// 截取屏幕
        /// </summary>
        /// <param name="rect">开始截取位置</param>
        /// <returns></returns>
        public static Image PrintScreen(Rectangle rect)
        {
            //Rectangle rect = Screen.PrimaryScreen.Bounds;
            Image memory = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(memory);
            g.CopyFromScreen(rect.X + 1, rect.Y + 1, 0, 0, rect.Size);
            return memory;
        }
        /// <summary>
        /// 截取屏幕
        /// </summary>
        /// <param name="rect">开始截取位置</param>
        /// <returns></returns>
        public static Image PrintScreen()
        {
            Screen screen = Screen.PrimaryScreen;
            return PrintScreen(screen.Bounds);
        }


        /// <summary>
        /// 刷新注册表
        /// </summary>
        public static void RefreashRegist()
        {
            WindowsAPI.SHChangeNotify(NotifyEvent.SHCNE_ASSOCCHANGED, NotifyEvent.SHCNF_IDLIST, 0, 0);
        }
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="flg">标识</param>
        public static bool DoExitWin(EWX flg)
        {
            bool ok;
            TOKEN_PRIVILEGES tp=new TOKEN_PRIVILEGES();
            IntPtr hproc = Process.GetCurrentProcess().Handle;
            IntPtr htok = IntPtr.Zero;
            ok = WindowsAPI.OpenProcessToken(hproc, TOKEN.TOKEN_ADJUST_PRIVILEGES | TOKEN.TOKEN_QUERY, ref htok);
            tp.PrivilegeCount = 1;
            tp.Privileges.Luid.HighPart = 0;
            tp.Privileges.Luid.LowPart = 0;
            tp.Privileges.Attributes =ConstValues.SE_PRIVILEGE_ENABLED;
            ok = WindowsAPI.LookupPrivilegeValue(null, ConstValues.SE_SHUTDOWN_NAME, ref tp.Privileges.Luid);
            ok = WindowsAPI.AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ok = WindowsAPI.ExitWindowsEx(flg, 0);
            return ok;
        }


    }
}
