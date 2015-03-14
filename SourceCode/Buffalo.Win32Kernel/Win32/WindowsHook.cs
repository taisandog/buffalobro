using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Buffalo.Kernel.Win32;

namespace Buffalo.Win32Kernel.Win32
{

    public class WindowsHook:IDisposable
    {
        // ************************************************************************
        // Filter function delegate
        //public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
        // ************************************************************************

        // ************************************************************************
        // Internal properties
        protected IntPtr m_hhook = IntPtr.Zero;
        protected WindowsAPI.HookProc m_filterFunc = null;
        protected HookType m_hookType;
        // ************************************************************************

        // ************************************************************************
        // Event delegate
        //public delegate void HookEventHandler(object sender, HookEventArgs e);
        // ************************************************************************

        // ************************************************************************
        // Event: HookInvoked 
        public event WindowsAPI.HookEventHandler HookInvoked;
        protected void OnHookInvoked(Buffalo.Kernel.Win32.WindowsAPI.HookEventArgs e)
        {
            if (HookInvoked != null)
                HookInvoked(this, e);
        }
        // ************************************************************************

        // ************************************************************************
        // Class constructor(s)
        public WindowsHook(HookType hook)
        {
            m_hookType = hook;
            m_filterFunc = new WindowsAPI.HookProc(this.CoreHookProc);
        }
        public WindowsHook(HookType hook, WindowsAPI.HookProc func)
        {
            m_hookType = hook;
            m_filterFunc = func;
        }
        // ************************************************************************

        // ************************************************************************
        // Default filter function
        protected IntPtr CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
                return WindowsAPI.CallNextHookEx(m_hhook, code, wParam, lParam);

            // Let clients determine what to do
            Buffalo.Kernel.Win32.WindowsAPI.HookEventArgs e = new Buffalo.Kernel.Win32.WindowsAPI.HookEventArgs();
            e.HookCode = code;
            e.wParam = wParam;
            e.lParam = lParam;
            
            OnHookInvoked(e);
            

            // Yield to the next hook in the chain
            return WindowsAPI.CallNextHookEx(m_hhook, code, wParam, lParam);
        }
        // ************************************************************************

        // ************************************************************************
        // Install the hook
        public void Install()
        {
            if (m_hhook != IntPtr.Zero) 
            {
                throw new Exception("钩子已经启动");
            }
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                m_hhook = WindowsAPI.SetWindowsHookEx((int)m_hookType, m_filterFunc, WindowsAPI.GetModuleHandle(curModule.ModuleName), 0);
            }
            //m_hhook = WindowsAPI.SetWindowsHookEx(
            //    (int)m_hookType,
            //    m_filterFunc,
            //    IntPtr.Zero,
            //    (int)Thread.CurrentThread.ManagedThreadId);
            if (m_hhook == IntPtr.Zero)
            {
                throw new Exception("挂钩失败");
            }
        }
        // ************************************************************************

        // ************************************************************************
        // Uninstall the hook
        public void Uninstall()
        {
            if (m_hhook != IntPtr.Zero)
            {
                WindowsAPI.UnhookWindowsHookEx(m_hhook);
                m_hhook = IntPtr.Zero;
            }
        }
        // ************************************************************************

        #region IDisposable 成员

        public void Dispose()
        {
            Uninstall();
        }

        #endregion

        ~WindowsHook() 
        {
            Uninstall();
        }
    }
}
