using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Buffalo.Win32Kernel.Win32
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class SystemInfo
    {
        public delegate bool DelIsWow64Process(IntPtr pHandle, ref bool is64);
        /// <summary>
        /// 是否64位系统
        /// </summary>
        /// <returns></returns>
        public static bool Is64System()
        {
            //bool ret = false;
            //IntPtr pModule = WindowsAPI.GetModuleHandle("kernel32.dll");
            //IntPtr ptrPro = WindowsAPI.GetProcAddress(pModule, "IsWow64Process");

            //DelIsWow64Process delIs64 = Marshal.GetDelegateForFunctionPointer(ptrPro, typeof(DelIsWow64Process)) as DelIsWow64Process;
            //if (delIs64 != null)
            //{
                
            //    IntPtr proPtr = Process.GetCurrentProcess().Handle;
            //    delIs64(proPtr, ref ret);
            //}
            //return ret;

            return IntPtr.Size == 8;
        }

        /// <summary>
        /// Gets a value indicating if the operating system is a Windows 2000 or a newer one.
        /// </summary>
        public static bool IsWindows2000OrNewer
        {
            get { return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major >= 5); }
        }

        /// <summary>
        /// Gets a value indicating if the operating system is a Windows XP or a newer one.
        /// </summary>
        public static bool IsWindowsXpOrNewer
        {
            get
            {
                return
                    (Environment.OSVersion.Platform == PlatformID.Win32NT) &&
                    (
                        (Environment.OSVersion.Version.Major >= 6) ||
                        (
                            (Environment.OSVersion.Version.Major == 5) &&
                            (Environment.OSVersion.Version.Minor >= 1)
                        )
                    );
            }
        }
        /// <summary>
        /// 把文件移到回收站
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int SendToRecycleBin(string file)
        {
            SHFILEOPSTRUCT lpFileOp = new SHFILEOPSTRUCT();
            lpFileOp.wFunc = WFunc.FO_DELETE;
            lpFileOp.pFrom = file + "\0";
            lpFileOp.fFlags = FILEOP_FLAGS.FOF_NOCONFIRMATION | FILEOP_FLAGS.FOF_NOERRORUI | FILEOP_FLAGS.FOF_SILENT;
            lpFileOp.fFlags |= FILEOP_FLAGS.FOF_ALLOWUNDO;//允许撤销即为放进回收站
            lpFileOp.fAnyOperationsAborted = false;

            int n = WindowsAPI.SHFileOperation(ref lpFileOp);
            return n;


        }
        /// <summary>
        /// Gets a value indicating if the operating system is a Windows Vista or a newer one.
        /// </summary>
        public static bool IsWindowsVistaOrNewer
        {
            get { return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major >= 6); }
        }
    }
}
