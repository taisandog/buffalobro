using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AutoServices;

namespace AutoTest
{
    static class Program
    {
        static AutoServicesManager man;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            man = new AutoServicesManager(1000);
            man.Init();
            man.Start();
            Application.Run(new Form1());
            man.Stop();
        }
    }
}