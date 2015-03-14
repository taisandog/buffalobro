using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Kernel;
using System.Diagnostics;

namespace URLToForms
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            CheckReg();
            if (args != null && args.Length > 0)
            {

                    ShowModel(args);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        public static string CurrentFileName 
        {
            get 
            {
                return Process.GetCurrentProcess().MainModule.FileName.Replace("vshost.", "");
            }
        }

        /// <summary>
        /// 检查注册表
        /// </summary>
        private static void CheckReg() 
        {
            if (!ProcessUrlRegistry.CheckReg("openform")) 
            {
                ProcessUrlRegistry.RegistryTo("openform", CurrentFileName);
            }
        }

        private static void ShowModel(string[] args) 
        {
            string head="openform://";
            string str = args[0];
            string formId = str.Substring(head.Length, str.Length - head.Length).Trim('/');
            bool find = false;
            foreach (Process pro in Process.GetProcesses()) 
            {
                try
                {
                    if (pro.MainModule.FileName.Equals(CurrentFileName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        IntPtr mainFormHandle = WindowsAPI.FindWindow(null, "主窗体");
                        if (mainFormHandle != IntPtr.Zero)
                        {
                            byte[] sarr = System.Text.Encoding.Default.GetBytes(formId);
                            int len = sarr.Length;
                            COPYDATASTRUCT cds;
                            cds.dwData = (IntPtr)100;
                            cds.lpData = formId;
                            cds.cbData = len + 1;
                            WindowsAPI.SendMessage(mainFormHandle, Msg.WM_COPYDATA, 0, ref cds);
                            find = true;
                        }
                    }
                }
                catch(Exception ex) 
                {
                }
            }
            if (!find) 
            {
                MessageBox.Show("请先打开应用程序");
            }
        }
    }
}