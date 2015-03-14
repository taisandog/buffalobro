using System;
using System.Runtime.InteropServices;
using Buffalo.Kernel.Win32;
namespace Buffalo.Kernel
{
    /// <summary>
    /// DllInvoke 的摘要说明
    /// </summary>
    public class DllInvoke:IDisposable
    {
        private IntPtr hLib;
        public DllInvoke(String DLLPath)
        {
            hLib = WindowsAPI.LoadLibrary(DLLPath);
            if (hLib == IntPtr.Zero) 
            {
                throw new Exception("加载" + DLLPath + "失败\n错误码：" + WindowsAPI.GetLastError());
            }

        }
        ~DllInvoke()
        {
            Dispose();
        }
        /// <summary>
        /// 将要执行的函数转换为委托
        /// </summary>
        /// <param name="APIName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public Delegate Invoke(String APIName, Type t)
        {
            IntPtr api = WindowsAPI.GetProcAddress(hLib, APIName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            WindowsAPI.FreeLibrary(hLib);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}