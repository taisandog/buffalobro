using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WordFilter
{
    public partial class FrmQRResault : Form
    {
        public FrmQRResault()
        {
            InitializeComponent();
        }

        private void FrmQRResault_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        /// <summary>
        /// ÏÔÊ¾½á¹û¿ò
        /// </summary>
        /// <param name="content"></param>
        public static void ShowBox(string content) 
        {
            FrmQRResault frm = new FrmQRResault();
            frm.txtContent.Text = content;
            
            frm.Show();
            //Rectangle curRec=Screen.GetBounds(frm);
            int x = Cursor.Position.X - (frm.Width/2);
            int y = Cursor.Position.Y ;
            
            frm.Location = new Point(x, y);
            frm.TopMost = true;
            frm.TopMost = false;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Program.MainForm._isSys = true;
                Clipboard.SetText(this.txtContent.Text);
            }
            catch { }
            finally 
            {
                Program.MainForm._isSys = false;
            }
        }

        private void txtContent_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}