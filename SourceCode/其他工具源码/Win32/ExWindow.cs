using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Win32
{
    /// <summary>
    /// �Ӵ���չ��
    /// </summary>
    public class ExWindow
    {
        private int _processId;

        public int ProcessId
        {
            get { return _processId; }
        }

        /// <summary>
        /// �Ӵ���չ��
        /// </summary>
        /// <param name="processId">����ID</param>
        public ExWindow(int processId) 
        {
            _processId = processId;
        }

        private List<IntPtr> lstMainWindowHandle=new List<IntPtr>();
        /// <summary>
        /// ��ȡ��ʾ����ľ��
        /// </summary>
        /// <returns></returns>
        public List<IntPtr> GetMainWindowHandle()
        {
            lstMainWindowHandle.Clear();

            WindowsAPI.EnumWindows(EnumWindowsItem, _processId);

            return new List<IntPtr>(lstMainWindowHandle);
        }
        private bool EnumWindowsItem(IntPtr hWnd, int lParam)
        {
            int PID=0;
            WindowsAPI.GetWindowThreadProcessId(hWnd, ref PID);

            if (PID == lParam &&
                WindowsAPI.IsWindowVisible(hWnd) &&
                WindowsAPI.GetWindow(hWnd, GetWindowType.GW_OWNER) == IntPtr.Zero)
            {
                lstMainWindowHandle.Add(hWnd);
                //return false;
            }

            return true;
        }

        /// <summary>
        /// ���ø�����
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="childHandle"></param>
        private static void SetParent(IntPtr parentHandle, IntPtr childHandle) 
        {
            WindowsAPI.SetParent(childHandle, parentHandle);
        }
    }
}
