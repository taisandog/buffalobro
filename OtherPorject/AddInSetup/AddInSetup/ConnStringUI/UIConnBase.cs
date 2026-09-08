using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace AddInSetup.ConnStringUI
{
    public partial class UIConnBase : UserControl
    {
        private MQReliabilitySettings _mqReliabilitySettings;
        // 保存按钮引用，便于派生界面根据具体消息模式切换入口状态。
        private Button _mqReliabilitySettingsButton;

        public UIConnBase()
        {
            InitializeComponent();
        }
        protected static string DocumtntPath = Path.Combine(CommonMethods.GetBaseRoot(), "doc");

        protected static string GetDocumtntPath(string fileName) 
        {
            return Path.Combine(DocumtntPath, fileName);
        }

        /// <summary>
        /// 使用 Windows 默认关联程序打开帮助文档。
        /// </summary>
        public static void OpenDocument(string fileName)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = GetDocumtntPath(fileName),
                UseShellExecute = true
            });
        }

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
        /// 初始化 MQ 可靠消费设置入口。
        /// </summary>
        protected void InitMQReliabilitySettings(MQBackend backend)
        {
            _mqReliabilitySettings = new MQReliabilitySettings(backend);
            _mqReliabilitySettingsButton = new Button
            {
                Text = "可靠消费与保留设置",
                Name = "btnMQReliabilitySettings",
                Location = new Point(347, 8),
                Size = new Size(145, 39),
                Font = new Font("微软雅黑", 10F),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                UseVisualStyleBackColor = true
            };
            _mqReliabilitySettingsButton.Click += (_, __) =>
            {
                using (MQReliabilitySettingsForm form =
                    new MQReliabilitySettingsForm(_mqReliabilitySettings))
                {
                    form.ShowDialog(this);
                }
            };
            panel2.Controls.Add(_mqReliabilitySettingsButton);
            _mqReliabilitySettingsButton.BringToFront();
        }

        /// <summary>
        /// 设置 MQ 可靠消费入口状态。用于只在后端特定模式下提供可靠消费能力。
        /// </summary>
        /// <param name="enabled">当前后端/模式是否支持可靠消费设置。</param>
        /// <param name="text">需要同时更新时使用的按钮文字；为空则保持原文字。</param>
        protected void SetMQReliabilitySettingsEnabled(bool enabled, string text = null)
        {
            if (_mqReliabilitySettingsButton == null)
            {
                return;
            }
            _mqReliabilitySettingsButton.Enabled = enabled;
            if (!string.IsNullOrWhiteSpace(text))
            {
                _mqReliabilitySettingsButton.Text = text;
            }
        }

        /// <summary>
        /// 输出 MQ 可靠消费配置。
        /// </summary>
        protected void FillMQReliabilityInfo(StringBuilder sbStr)
        {
            _mqReliabilitySettings?.AppendTo(sbStr);
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
