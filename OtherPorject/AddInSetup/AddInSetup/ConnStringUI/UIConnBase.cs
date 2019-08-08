using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel;

namespace AddInSetup.ConnStringUI
{
    public partial class UIConnBase : UserControl
    {
        public UIConnBase()
        {
            InitializeComponent();
        }
        protected static string BasePath = CommonMethods.GetBaseRoot() + "\\doc\\";
        private void gpOutput_SizeChanged(object sender, EventArgs e)
        {
            if (gpOutput != null && gpOutput.Width > 10)
            {
                scOut.SplitterDistance = gpOutput.Width / 2;
            }
        }
        /// <summary>
        /// 显示帮助按钮
        /// </summary>
        protected bool ShowHelp
        {
            get
            {
                return btnTech.Visible;
            }
            set
            {
                btnTech.Visible = value;
            }
        }

        protected virtual void OnConnOut()
        {

        }

        public RichTextBox OutText
        {
            get
            {
                return txtOutConn;
            }
        }
        public RichTextBox CodeText
        {
            get
            {
                return txtOutCode;
            }
        }
        protected virtual void OnTest()
        {

        }
        protected virtual void OnHelp()
        {

        }
        private void btnOut_Click(object sender, EventArgs e)
        {
            OnConnOut();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            OnTest();
        }

        private void btnTech_Click(object sender, EventArgs e)
        {
            OnHelp();
        }

        private void tsCopy_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as System.Windows.Forms.ToolStripMenuItem;
            if (item==null)
            {
                return;
            }
            System.Windows.Forms.ContextMenuStrip ms = item.Owner as System.Windows.Forms.ContextMenuStrip;
            if (ms == null)
            {
                return;
            }
            TextBoxBase txt = ms.SourceControl as TextBoxBase;
            if (txt == null)
            {
                return;
            }
            Clipboard.SetDataObject(txt.Text);
        }
    }
}
