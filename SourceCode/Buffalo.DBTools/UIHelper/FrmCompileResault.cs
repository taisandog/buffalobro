using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace Buffalo.DBTools.UIHelper
{
    public partial class FrmCompileResault : Form
    {
        public FrmCompileResault()
        {
            InitializeComponent();
        }

        private void FrmCompileError_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="error">错误</param>
        /// <param name="title">标题</param>
        public static void ShowCompileResault(string code, string error,string title) 
        {
            FrmCompileResault frm = new FrmCompileResault();
            if (!string.IsNullOrEmpty(code))
            {
                frm.txtCode.Text = code;
            }
            else 
            {
                frm.spInfo.Panel1Collapsed = true;
            }
            if (!string.IsNullOrEmpty(error))
            {
                frm.txtError.Text = error;
                
            }
            else 
            {
                
                frm.spInfo.Panel2Collapsed = true;
            }
            frm.Text = title+ToolVersionInfo.ToolVerInfo;
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}