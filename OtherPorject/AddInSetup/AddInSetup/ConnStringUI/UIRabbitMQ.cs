using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buffalo.ArgCommon;
using AddInSetup.Unit;
using Buffalo.DB.CacheManager;
using System.Web;
using System.Diagnostics;

namespace AddInSetup.ConnStringUI
{
    public partial class UIRabbitMQ : UIConnBase
    {
        public UIRabbitMQ()
        {
            InitializeComponent();
            ShowProxy = false;
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
                APIResault res = MQHelper.TestMQ(name, key, "rabbitmq",false, sbStr);
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

            if (!string.IsNullOrWhiteSpace(txtVirtualHost.Text))
            {
                sbStr.Append("vhost=");
                sbStr.Append(HttpUtility.UrlEncode(txtVirtualHost.Text));
                sbStr.Append(";");
            }

            if (!string.IsNullOrWhiteSpace(txtUid.Text))
            {
                sbStr.Append("uid=");
                sbStr.Append(HttpUtility.UrlEncode(txtUid.Text));
                sbStr.Append(";");
            }
            if (!string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                sbStr.Append("pwd=");
                sbStr.Append(HttpUtility.UrlEncode(txtPwd.Text));
                sbStr.Append(";");
            }

            if (!string.IsNullOrWhiteSpace(cmbExchangeMode.Text))
            {
                sbStr.Append("exchangeMode=");
                sbStr.Append(cmbExchangeMode.Text);
                sbStr.Append(";");
            }

            if (!string.IsNullOrWhiteSpace(txtExchangeName.Text))
            {
                sbStr.Append("exchangeName=");
                sbStr.Append(HttpUtility.UrlEncode(txtExchangeName.Text));
                sbStr.Append(";");
            }

            sbStr.Append("autoDelete=");
            sbStr.Append(chkAutoDelete.Checked ? "1" : "0");
            sbStr.Append(";");

            sbStr.Append("queueName=");
            sbStr.Append(HttpUtility.UrlEncode(txtQueue.Text));
            sbStr.Append(";");


            sbStr.Append("deliveryMode=");
            sbStr.Append(chkDeliveryMode.Checked?"2":"1");
            sbStr.Append(";");


            return sbStr.ToString();
        }


    }
}


