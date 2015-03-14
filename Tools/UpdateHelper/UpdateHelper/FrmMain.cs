using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UpdateHelper
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<CopyPath> lst = CopyPath.GetPath();
            foreach (CopyPath cp in lst) 
            {
                cp.DoCopy();
            }
            MessageBox.Show("¿½±´Íê±Ï");
        }
    }
}