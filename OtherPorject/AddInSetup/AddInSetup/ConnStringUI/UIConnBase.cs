using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel;
using System.Web;

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
        /// 填充代理信息
        /// </summary>
        /// <param name="sb"></param>
        protected void FillProxyInfo(StringBuilder sbStr)
        {
            if (string.IsNullOrWhiteSpace(txtProxyHost.Text))
            {
                return;
            }
            sbStr.Append("ProxyHost=");
            sbStr.Append(HttpUtility.UrlEncode(txtProxyHost.Text));
            sbStr.Append(";");
            sbStr.Append("ProxyPort=");
            sbStr.Append(((int)txtProxyPort.Value).ToString());
            sbStr.Append(";");

            if (string.IsNullOrWhiteSpace(txtProxyUser.Text))
            {
                return;
            }
            sbStr.Append("ProxyUser=");
            sbStr.Append(HttpUtility.UrlEncode(txtProxyUser.Text));
            sbStr.Append(";");
            sbStr.Append("ProxyPass=");
            sbStr.Append(HttpUtility.UrlEncode(txtProxyPass.Text));
            sbStr.Append(";");
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
        /// <summary>
        /// 显示代理
        /// </summary>
        protected bool ShowProxy
        {
            get
            {
                return gbProxy.Visible;
            }
            set
            {
                gbProxy.Visible = value;
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
