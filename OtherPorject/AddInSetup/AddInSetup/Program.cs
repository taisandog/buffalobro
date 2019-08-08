using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AddInSetup
{
    static class Program
    {
        public static FrmMain CurrentMainForm;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CurrentMainForm = new FrmMain();
            Application.Run(CurrentMainForm);
        }
    }
}