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
            InitMQReliabilitySettings(MQBackend.Redis);
            BindFlags();
            BindMessageMode();
            nupXTrimMaxLength.Maximum = int.MaxValue;
            cmbMessageMode.SelectedValueChanged += (_, __) => UpdateMessageModeControls();
            // BindMessageMode 已设置默认值，事件注册较晚，因此这里主动同步一次初始状态。
            UpdateMessageModeControls();
        }
        protected override void OnHelp()
        {
            OpenDocument("Buffalo.MQ.docx");
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

        /// <summary>
        /// 根据 Redis 消息模式切换可用控件：Stream 才能设置消费组、ACK、重试、
        /// 死信和保留策略；useQueue 只对 Subscriber 有意义。
        /// </summary>
        private void UpdateMessageModeControls()
        {
            if (cmbMessageMode.SelectedValue == null)
            {
                return;
            }
            RedisMQMessageMode mode = (RedisMQMessageMode)(int)cmbMessageMode.SelectedValue;
            bool isStream = mode == RedisMQMessageMode.Stream;
            // groupBox1 内是消费组、未 ACK 加载和 Stream 最大长度等专用配置。
            groupBox1.Enabled = isStream;
            chkUseQueue.Enabled = mode == RedisMQMessageMode.Subscriber;
            SetMQReliabilitySettingsEnabled(isStream,
                isStream ? "可靠消费与保留设置" : "仅Stream支持可靠设置");
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
            // 必须连 Polling=0 一起输出。库端未提供 MessageMode 时会按 Subscriber
            // 兼容旧连接串，若省略 0，界面选择 Polling 后会被错误解析为 Subscriber。
            sbStr.Append("MessageMode=");
            sbStr.Append(messageMode.ToString());
            sbStr.Append(";");
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
            sbStr.Append(";");
            // 非 Stream 模式不支持这些语义，因此不把可靠消费参数写入连接字符串。
            if (mode == RedisMQMessageMode.Stream)
            {
                FillMQReliabilityInfo(sbStr);
            }
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
            string text = "Polling：轮询 List，消息取出即删除。\r\n\r\n" +
                "Subscriber：使用 pub/sub 推送；勾选队列存储时数据存放在 List。\r\n\r\n" +
                "BlockQueue：使用 BRPOP 阻塞读取，消息取出即删除。\r\n\r\n" +
                "以上三种模式均为简单收发，不提供 ACK、自动重试和死信，异常由业务处理。\r\n\r\n" +
                "Stream：使用 XREADGROUP，支持 ACK、Pending 认领、自动重试、死信和保留策略。";
            ShowRemark(text, "推送模式");
        }

        private void btnNoAckRemark_Click(object sender, EventArgs e)
        {
            string text = "启动MQListener时候加载一次已读但没正确Ack的消息";
            ShowRemark(text, "启动加载未Ack的消息");

        }

        private void btnXTrimMaxLengthRemarl_Click(object sender, EventArgs e)
        {
            string text = "仅在“可靠消费与保留设置”中选择 maxLength 时生效。\r\n" +
                "设置为0表示不按长度修剪。\r\n" +
                "修剪前会确认所有消费组没有 Pending 且 Lag 为0，避免删除离线消费组尚未读取的数据。\r\n" +
                "如需单消费组 ACK 后立即删除，请在“可靠消费与保留设置”中选择 deleteOnAck。";
            ShowRemark(text, "最大记录");
        
        }
    }
}
