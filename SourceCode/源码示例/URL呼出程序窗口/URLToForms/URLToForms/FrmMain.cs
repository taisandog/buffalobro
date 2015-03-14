using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Win32Kernel;

namespace URLToForms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == (int)Msg.WM_COPYDATA) 
            {
                COPYDATASTRUCT dataInfo = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                string str = dataInfo.lpData;
                Type objType = FrmCollection.GetFrmByID(str);
                if (objType == null) 
                {
                    MessageBox.Show("找不到界面:" + str);
                    return;
                }
                Form frm=SingleForms.GetControl(objType) as Form;
                frm.Show();
            }
            base.DefWndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ProcessUrlRegistry.RegistryTo("openform", Program.CurrentFileName);
        }
    }
}