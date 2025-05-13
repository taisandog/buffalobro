using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AddInSetup.Unit;
using Buffalo.ArgCommon;
using System.Web;
using Buffalo.Kernel;
using Confluent.Kafka;
using System.Diagnostics;

namespace AddInSetup.ConnStringUI
{
    public partial class UIKafkaMQ : UIConnBase
    {
        public UIKafkaMQ()
        {
            InitializeComponent();
           

        }
        protected override void OnLoad(EventArgs e)
        {
            ShowProxy = false;
            nupInterval.Maximum = Int32.MaxValue;
            nupSessionTimeout.Maximum = Int32.MaxValue;
            InitAutoOffsetReset();
            InitSecurityProtocol();
            InitSaslMechanism();
            base.OnLoad(e);
        }
        /// <summary>
        /// 初始化AutoOffset
        /// </summary>
        protected void InitAutoOffsetReset()
        {
            List<EnumInfo> lstInfo = EnumUnit.GetEnumInfos(typeof(AutoOffsetReset));
            cmbAutoOffsetReset.DisplayMember = "FieldName";
            cmbAutoOffsetReset.ValueMember = "Value";
            cmbAutoOffsetReset.DataSource = lstInfo;
            cmbAutoOffsetReset.SelectedValue = AutoOffsetReset.Earliest;
        }
        /// <summary>
        /// 初始化AutoOffset
        /// </summary>
        protected void InitSecurityProtocol()
        {
            List<EnumInfo> lstInfo = EnumUnit.GetEnumInfos(typeof(SecurityProtocol));
            cmbSecurityProtocol.DisplayMember = "FieldName";
            cmbSecurityProtocol.ValueMember = "Value";
            cmbSecurityProtocol.DataSource = lstInfo;
            cmbSecurityProtocol.SelectedValue = SecurityProtocol.Plaintext;

        }
        /// <summary>
        /// 初始化AutoOffset
        /// </summary>
        protected void InitSaslMechanism()
        {
            List<EnumInfo> lstInfo = EnumUnit.GetEnumInfos(typeof(SaslMechanism));
            EnumInfo info = new EnumInfo();
            info.FieldName = "None";
            info.Value = 0;
            lstInfo.Insert(0, info);
            cmbSaslMechanism.DisplayMember = "FieldName";
            cmbSaslMechanism.ValueMember = "Value";
            cmbSaslMechanism.DataSource = lstInfo;
            cmbSaslMechanism.SelectedValue = 0;
        }
        protected override void OnHelp()
        {
            Process.Start(BasePath + "Buffalo.MQ.docx");
        }
        protected override void OnTest()
        {
            string sbStr = GetConnectionString();
            string name = txtName.Text;
            string key = "buffalotestmq";
            try
            {
                APIResault res = MQHelper.TestMQ(name, key, "kafkamq",true, sbStr);
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

            sbCode.Append("string type=\"kafkamq\"");
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

            string svalue = txtServer.Text;
            sbStr.Append("server=");
            sbStr.Append(HttpUtility.UrlEncode(svalue));
            sbStr.Append(";");

            svalue = txtSaslUsername.Text;
            if (!string.IsNullOrWhiteSpace(svalue))
            {
                sbStr.Append("saslUsername=");
                sbStr.Append(HttpUtility.UrlEncode(svalue));
                sbStr.Append(";");
            }

            svalue = txtSaslPassword.Text;
            if (!string.IsNullOrWhiteSpace(svalue))
            {
                sbStr.Append("saslPassword=");
                sbStr.Append(HttpUtility.UrlEncode(svalue));
                sbStr.Append(";");
            }
            
            svalue = txtGroupId.Text;
            if (!string.IsNullOrWhiteSpace(svalue))
            {
                sbStr.Append("groupId=");
                sbStr.Append(HttpUtility.UrlEncode(svalue));
                sbStr.Append(";");
            }



            object selected = cmbSaslMechanism.SelectedValue;
            if (selected!=null)
            {
                int val = (int)selected;
                if (val > 0)
                {
                    sbStr.Append("saslMechanism=");
                    sbStr.Append(val.ToString());
                    sbStr.Append(";");
                }
            }

            selected = cmbSecurityProtocol.SelectedValue;
            if (selected != null)
            {
                int val = (int)selected;
                if (val >= 0)
                {
                    sbStr.Append("securityProtocol=");
                    sbStr.Append(val.ToString());
                    sbStr.Append(";");
                }
            }

            selected = cmbAutoOffsetReset.SelectedValue;
            if (selected != null)
            {
                int val = (int)selected;
                if (val >= 0)
                {
                    sbStr.Append("offsetType=");
                    sbStr.Append(val.ToString());
                    sbStr.Append(";");
                }
            }

            sbStr.Append("autoCommit=");
            sbStr.Append(chkAutoCommit.Checked ? "1" : "0");
            sbStr.Append(";");



            decimal dvalue = nupInterval.Value;
            if (dvalue>0)
            {
                sbStr.Append("interval=");
                sbStr.Append(dvalue.ToString());
                sbStr.Append(";");
            }

            dvalue = nupSessionTimeout.Value;
            if (dvalue > 0)
            {
                sbStr.Append("sessionTimeout=");
                sbStr.Append(dvalue.ToString());
                sbStr.Append(";");
            }

            
            return sbStr.ToString();
        }

        private void Labkafka_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path = CommonMethods.GetBaseRoot() + "\\AddIns\\Resource\\Kafka\\librdkafka.redist\\";
            Process.Start(path);
        }

        
    }
}


