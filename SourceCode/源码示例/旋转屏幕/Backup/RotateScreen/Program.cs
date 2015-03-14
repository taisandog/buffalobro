using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CheckSQLiteDB;

namespace RotateScreen
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DisplayOrientation ori = ScreenSetting.GetScreenOrientation();
            if (ori == DisplayOrientation.DmdoDEFAULT)
            {
                ori = DisplayOrientation.Dmdo90;
            }
            else
            {
                ori = DisplayOrientation.DmdoDEFAULT;
            }
            ScreenSetting.Orientation(ori);

            //Application.Run(new Form1());
        }
    }
}