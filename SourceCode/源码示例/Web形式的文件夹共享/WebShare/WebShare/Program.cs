using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Buffalo.Win32Kernel;

namespace WebShare
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _config = new ConfigManager();
            _config.LoadConfig();
            using (FrmMain frm = SingleForms.GetForm<FrmMain>())
            {
                if (args != null && args.Length > 0) 
                {
                    foreach (string arg in args) 
                    {
                        if (arg.Equals("-s", StringComparison.CurrentCultureIgnoreCase)) 
                        {
                            frm.AutoStart = true;
                        }
                    }
                }
                Application.Run(frm);
            }
        }

        private static ConfigManager _config;
        /// <summary>
        /// 配置信息
        /// </summary>
        public static ConfigManager Config
        {
            get { return Program._config; }
        }

    }
}