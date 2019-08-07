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
    /// ϵͳ����
    /// </summary>
    public class WindowsApplication
    {
        /// <summary>
        /// ��Debugģʽ�رս���(������Ҳ��ر�)
        /// </summary>
        /// <param name="id"></param>
        public static void DebugKillProcessById(int id) 
        {
            IntPtr hProcess=WindowsAPI.OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS, 0, id);//������ģʽ�򿪽���
            
            int ret = WindowsAPI.DebugActiveProcess(id);//���Դ˽���ID
            if (ret == 0)
            {
                int err = WindowsAPI.GetLastError();
            }
            //WindowsAPI.FatalExit(0);//������ǿ�ƽ���
        }
        /// <summary>
        /// �رս���
        /// </summary>
        /// <param name="id"></param>
        public static void KillProcess(int id)
        {
            IntPtr hProcess = WindowsAPI.OpenProcess(ProcessAccess.PROCESS_ALL_ACCESS, 0, id);//������ģʽ�򿪽���
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
        /// ����DebugȨ��
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
        /// ���ñ�ֽ
        /// </summary>
        /// <param name="picRoot">��ֽ·��</param>
        public static void SetWallPaper(string picRoot)
        {
            IntPtr pRoot= Marshal.StringToHGlobalAuto(picRoot);
            int nResult = WindowsAPI.SystemParametersInfo(SPIAction.SPI_SETDESKWALLPAPER, 1, pRoot, 0x1 | 0x2);
            Marshal.FreeHGlobal(pRoot);
        }

        /// <summary>
        /// ��ȡ��Ļ
        /// </summary>
        /// <param name="rect">��ʼ��ȡλ��</param>
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
        /// ��ȡ��Ļ
        /// </summary>
        /// <param name="rect">��ʼ��ȡλ��</param>
        /// <returns></returns>
        public static Image PrintScreen()
        {
            Screen screen = Screen.PrimaryScreen;
            return PrintScreen(screen.Bounds);
        }


        /// <summary>
        /// ˢ��ע���
        /// </summary>
        public static void RefreashRegist()
        {
            WindowsAPI.SHChangeNotify(NotifyEvent.SHCNE_ASSOCCHANGED, NotifyEvent.SHCNF_IDLIST, 0, 0);
        }
        /// <summary>
        /// �˳�ϵͳ
        /// </summary>
        /// <param name="flg">��ʶ</param>
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
