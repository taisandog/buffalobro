using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buffalo.MQ;
using Buffalo.DB.CacheManager;
using AddInSetup.Unit;
using Buffalo.ArgCommon;
using System.Web;
using System.Diagnostics;
using Buffalo.Kernel;
using StackExchange.Redis;
using Buffalo.MQ.RedisMQ;

namespace AddInSetup.ConnStringUI
{
    public partial class UIRedisMQ : UIConnBase
    {
        public UIRedisMQ()
        {
            InitializeComponent();
            ShowProxy = false;
            BindFlags();
            BindMessageMode();
            nupXTrimMaxLength.Maximum = int.MaxValue;
        }
        protected override void OnHelp()
        {
            Process.Start(BasePath + "Buffalo.MQ.docx");
        }
        protected override void OnTest()
        {
            string sbStr = GetConnectionString();
            string name = txtName.Text;
            string key = "buffalo.testmq";
            try
            {
                APIResault res = MQHelper.TestMQ(name, key, "redismq", false, sbStr);
                if (!res.IsSuccess)
                {
                    MessageBox.Show(res.Message, "测试失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("测试成功", "测试成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }
        private void BindFlags()
        {
            List<EnumInfo> lstEnum = EnumUnit.GetEnumInfos(typeof(CommandFlags));
            cmbCommandFlags.DataSource = lstEnum;
            cmbCommandFlags.DisplayMember = "FieldName";
            cmbCommandFlags.ValueMember = "Value";
            cmbCommandFlags.SelectedValue = CommandFlags.None;
        }
        private void BindMessageMode()
        {
            List<EnumInfo> lstEnum = EnumUnit.GetEnumInfos(typeof(RedisMQMessageMode));
            cmbMessageMode.DataSource = lstEnum;
            cmbMessageMode.DisplayMember = "FieldName";
            cmbMessageMode.ValueMember = "Value";
            cmbMessageMode.SelectedValue = RedisMQMessageMode.Subscriber;
        }
        protected override void OnConnOut()
        {
            string sbStr = GetConnectionString();
            OutText.Text = sbStr;

            StringBuilder sbCode = new StringBuilder();
            sbCode.Append("string name=\"");
            sbCode.Append(txtName.Text);
            sbCode.Append("\";");
            sbCode.AppendLine("");

            sbCode.Append("string connectString=\"");
            sbCode.Append(sbStr);
            sbCode.Append("\";");
            sbCode.AppendLine("");

            sbCode.Append("string type=\"redismq\"");
            sbCode.AppendLine("");

            sbCode.Append("MQUnit.SetMQInfo(name, type, connectString);;");

            CodeText.Text = sbCode.ToString();

        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            StringBuilder sbStr = new StringBuilder();
            sbStr.Append("server=");
            sbStr.Append(HttpUtility.UrlEncode(txtServer.Text));
            sbStr.Append(";");

            if (txtExpir.Value > 0)
            {
                sbStr.Append("expir=");
                sbStr.Append(((int)txtExpir.Value).ToString());
                sbStr.Append(";");
            }
            if (!string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                sbStr.Append("pwd=");
                sbStr.Append(HttpUtility.UrlEncode(txtPwd.Text));
                sbStr.Append(";");
            }
            if (chkSSL.Checked)
            {
                sbStr.Append("ssl=1");
                sbStr.Append(";");
            }
            if (chkSkipCert.Checked)
            {
                sbStr.Append("skipCert=1");
                sbStr.Append(";");
            }

            int messageMode = (int)cmbMessageMode.SelectedValue;
            if (messageMode > 0)
            {
                sbStr.Append("MessageMode=");
                sbStr.Append(messageMode.ToString());
                sbStr.Append(";");
            }
            RedisMQMessageMode mode = (RedisMQMessageMode)messageMode;
            if (mode == RedisMQMessageMode.Subscriber)
            {
                if (chkUseQueue.Checked)
                {
                    sbStr.Append("useQueue=1");
                    sbStr.Append(";");
                }
            }



            int commanfFlags = (int)cmbCommandFlags.SelectedValue;
            if (commanfFlags > 0)
            {
                sbStr.Append("commanfFlags=");
                sbStr.Append(commanfFlags.ToString());
                sbStr.Append(";");
            }

            if (nupSyncTimeout.Value > 0)
            {
                sbStr.Append("syncTimeout=");
                sbStr.Append(((int)nupSyncTimeout.Value).ToString());
                sbStr.Append(";");
            }

            int pInterval = (int)nupMessageMode.Value;
            if (pInterval > 0)
            {
                sbStr.Append("pInterval=");
                sbStr.Append(pInterval.ToString());
                sbStr.Append(";");
            }

            if (mode == RedisMQMessageMode.Stream)
            {
                sbStr.Append("consumerName=");
                sbStr.Append(txtConsumerName.Text);
                sbStr.Append(";");

                sbStr.Append("consumerGroupName=");
                sbStr.Append(txtConsumerGroupName.Text);
                sbStr.Append(";");

                sbStr.Append("loadNoAck=");
                sbStr.Append(chkNoAck.Checked?"1":"0");
                sbStr.Append(";");

                sbStr.Append("topicMaxLength=");
                sbStr.Append(((int)nupXTrimMaxLength.Value).ToString());
                sbStr.Append(";");
            }

            sbStr.Append("database=");
            sbStr.Append(((int)txtDatabase.Value).ToString());
            return sbStr.ToString();
        }

        private void btnRemarkPolling_Click(object sender, EventArgs e)
        {
            string text = "Polling模式时：此为每次轮询到空队列时候的睡眠时间(越小越实时，但负担更重，为0时候是50ms)\r\n\r\nBlockQueue和Stream模式时：此为brPop和XREADGROUP的超时时间(最小1秒，尽量大，值为0时候是30秒)\r\n\r\n\r\n";
            ShowRemark(text, "轮询间隔");
        }

        private void ShowRemark(string msg, string title)
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnMessageModeRemark_Click(object sender, EventArgs e)
        {
            string text = "Polling:轮询模式,按照间隔获取队列数据\r\n\r\nSubscriber为订阅发布模式,使用pub/sub实现发布,如果勾选使用队列存储信息,则pub/sub只推送通知，实际数据还是从队列获取，以防止消息丢失\r\n\r\nBlockQueue:阻塞队列模式，利用brpop指令实现阻塞读取队列\r\n\r\nStream:阻塞Stream模式，利用XREADGROUP指令实现Stream消息队列，支持Ack";
            ShowRemark(text, "推送模式");
        }

        private void btnNoAckRemark_Click(object sender, EventArgs e)
        {
            string text = "启动MQListener时候加载一次已读但没正确Ack的消息";
            ShowRemark(text, "启动加载未Ack的消息");

        }

        private void btnXTrimMaxLengthRemarl_Click(object sender, EventArgs e)
        {
            string text = "自动修剪话题的最大记录数\r\n设置为0则不自动修剪\r\n如果不设置可能会一直耗尽内存";
            ShowRemark(text, "最大记录");
        
        }
    }
}
