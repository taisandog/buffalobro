using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WordFilter
{
    static class Program
    {
        private static FrmMain _mainForm;
        /// <summary>
        /// 主窗体
        /// </summary>
        public static FrmMain MainForm
        {
            get { return Program._mainForm; }
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (_mainForm = new FrmMain())
            {
                Application.Run(_mainForm);
            }
        }
    }
}