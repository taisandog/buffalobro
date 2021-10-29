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
                APIResault res=MQHelper.TestMQ(name,key, "redismq",false, sbStr);
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

            if (chkUseQueue.Checked)
            {
                sbStr.Append("useQueue=1");
                sbStr.Append(";");
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
            int messageMode = (int)cmbMessageMode.SelectedValue;
            if (messageMode > 0)
            {
                sbStr.Append("MessageMode=");
                sbStr.Append(messageMode.ToString());
                sbStr.Append(";");
            }
            int pInterval = (int)nupMessageMode.Value;
            if (pInterval > 0)
            {
                sbStr.Append("pInterval=");
                sbStr.Append(pInterval.ToString());
                sbStr.Append(";");
            }
            sbStr.Append("database=");
            sbStr.Append(((int)txtDatabase.Value).ToString());
            return sbStr.ToString();
        }


    }
}
