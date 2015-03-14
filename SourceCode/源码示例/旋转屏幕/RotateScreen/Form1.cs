using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CheckSQLiteDB;

namespace RotateScreen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplayOrientation ori=ScreenSetting.GetScreenOrientation();
            if(ori==DisplayOrientation.DmdoDEFAULT)
            {
                ori=DisplayOrientation.Dmdo90;
            }else
            {
                ori=DisplayOrientation.DmdoDEFAULT;
            }
            ScreenSetting.Orientation(ori);
        }
    }
}