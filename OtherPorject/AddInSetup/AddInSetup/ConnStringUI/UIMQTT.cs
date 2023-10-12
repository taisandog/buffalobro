using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using AddInSetup.Unit;
using Buffalo.ArgCommon;
using Buffalo.Kernel;
using Buffalo.MQ.MQTTLib.MQTTnet.Protocol;
using System.Web;
using Buffalo.MQ.MQTTLib.MQTTnet.Formatter;

namespace AddInSetup.ConnStringUI
{
    public partial class UIMQTT : UIConnBase
    {
        public UIMQTT()
        {
            InitializeComponent();
        }

        private static string MQType = "mqttmq";
        protected override void OnLoad(EventArgs e)
        {
            ShowProxy = false;

            base.OnLoad(e);
            nulKeepAlivePeriod.Maximum = int.MaxValue - 1;
            nupSessionExpiry.Maximum = int.MaxValue - 1;
            BindQualityOfServiceLevel();
            BindRetainHandling();
            BindProtocolVersion();
        }

        private void BindQualityOfServiceLevel()
        {
            List<EnumInfo> lstInfo = new List<EnumInfo>();
            EnumInfo info = new EnumInfo();
            info.DisplayName = "[默认]";
            info.Description = "[默认]";
            info.FieldName = "None";
            info.Value = -1;
            lstInfo.Add(info);

            info = new EnumInfo();
            info.DisplayName = "至多一次(AtMostOnce)";
            info.Description = info.DisplayName;
            info.FieldName = "AtMostOnce";
            info.Value = (int)MqttQualityOfServiceLevel.AtMostOnce;
            lstInfo.Add(info);

            info = new EnumInfo();
            info.DisplayName = "至少一次(AtLeastOnce)";
            info.Description = info.DisplayName;
            info.FieldName = "AtLeastOnce";
            info.Value = (int)MqttQualityOfServiceLevel.AtLeastOnce;
            lstInfo.Add(info);

            info = new EnumInfo();
            info.DisplayName = "确保只有一次(ExactlyOnce)";
            info.Description = info.DisplayName;
            info.FieldName = "ExactlyOnce";
            info.Value = (int)MqttQualityOfServiceLevel.ExactlyOnce;
            lstInfo.Add(info);


            //List<EnumInfo> lstInfo = EnumUnit.GetEnumInfos(typeof(MqttQualityOfServiceLevel));
            cmbQualityOfServiceLevel.DisplayMember = "Description";
            cmbQualityOfServiceLevel.ValueMember = "Value";
            cmbQualityOfServiceLevel.DataSource = lstInfo;
            cmbQualityOfServiceLevel.SelectedValue = -1;
        }
        private void BindProtocolVersion()
        {
            List<EnumInfo> lstInfo = new List<EnumInfo>();
            EnumInfo info = new EnumInfo();
            info.DisplayName = "[默认]";
            info.Description = "[默认]";
            info.FieldName = "None";
            info.Value = -1;
            lstInfo.Add(info);

            info = new EnumInfo();
            info.DisplayName = "V3.10协议";
            info.Description = info.DisplayName;
            info.FieldName = "V310";
            info.Value = (int)MqttProtocolVersion.V310;
            lstInfo.Add(info);

            info = new EnumInfo();
            info.DisplayName = "V3.11协议(V311)";
            info.Description = info.DisplayName;
            info.FieldName = "V311";
            info.Value = (int)MqttProtocolVersion.V311;
            lstInfo.Add(info);

            info = new EnumInfo();
            info.DisplayName = "V5.0协议(V500)";
            info.Description = info.DisplayName;
            info.FieldName = "V500";
            info.Value = (int)MqttProtocolVersion.V500;
            lstInfo.Add(info);


            //List<EnumInfo> lstInfo = EnumUnit.GetEnumInfos(typeof(MqttQualityOfServiceLevel));
            cmbProtocolVersion.DisplayMember = "Description";
            cmbProtocolVersion.ValueMember = "Value";
            cmbProtocolVersion.DataSource = lstInfo;
            cmbProtocolVersion.SelectedValue = -1;
        }

        private void BindRetainHandling()
        {

            List<EnumInfo> lstInfo = new List<EnumInfo>();
            EnumInfo info = new EnumInfo();
            info.DisplayName = "[无]";
            info.Description = "[无]";
            info.FieldName = "[无]";
            info.Value = -1;
            lstInfo.Add(info);


            info = new EnumInfo();
            info.DisplayName = "订阅发送(SendAtSubscribe)";
            info.Description = info.DisplayName;
            info.FieldName = "SendAtSubscribe";
            info.Value = (int)MqttRetainHandling.SendAtSubscribe;
            lstInfo.Add(info);

            info = new EnumInfo();
            info.DisplayName = "仅在订阅时发送新订阅(SendAtSubscribeIfNewSubscriptionOnly)";
            info.Description = info.DisplayName;
            info.FieldName = "SendAtSubscribeIfNewSubscriptionOnly";
            info.Value = (int)MqttRetainHandling.SendAtSubscribeIfNewSubscriptionOnly;
            lstInfo.Add(info);

            info = new EnumInfo();
            info.DisplayName = "订阅时不发送(DoNotSendOnSubscribe)";
            info.Description = info.DisplayName;
            info.FieldName = "DoNotSendOnSubscribe";
            info.Value = (int)MqttRetainHandling.DoNotSendOnSubscribe;
            lstInfo.Add(info);

            cmbRetainHandling.DisplayMember = "Description";
            cmbRetainHandling.ValueMember = "Value";
            cmbRetainHandling.DataSource = lstInfo;
            cmbRetainHandling.SelectedValue = -1;
        }

        protected override void OnHelp()
        {
            Process.Start(BasePath + "Buffalo.MQ.docx");
        }
        protected override void OnTest()
        {
            string sbStrConn = GetConnectionString("testCliConn");
            string sbStrLis = GetConnectionString("testCliLis");
            string name = txtName.Text;

            string key = "buffalotestmq";

            try
            {
                APIResault res = MQHelper.TestMQ(name, key, MQType, false, sbStrConn, name + "_lis", sbStrLis);
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

            sbCode.Append("string type=\"");
            sbCode.Append(MQType);
            sbCode.Append("\"");
            sbCode.AppendLine("");

            sbCode.Append("MQUnit.SetMQInfo(name, type, connectString);");

            CodeText.Text = sbCode.ToString();

        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString(string testClientId = null)
        {
            StringBuilder sbStr = new StringBuilder();
            object selected = null;
            string svalue = txtServer.Text;
            sbStr.Append("server=");
            sbStr.Append(HttpUtility.UrlEncode(svalue));
            sbStr.Append(";");

            svalue = txtUid.Text;
            if (!string.IsNullOrWhiteSpace(svalue))
            {
                sbStr.Append("uid=");
                sbStr.Append(HttpUtility.UrlEncode(svalue));
                sbStr.Append(";");

                svalue = txtPwd.Text;
                if (!string.IsNullOrWhiteSpace(svalue))
                {
                    sbStr.Append("pwd=");
                    sbStr.Append(HttpUtility.UrlEncode(svalue));
                    sbStr.Append(";");
                }
            }

            svalue = txtClientId.Text;
            if (!string.IsNullOrWhiteSpace(testClientId))
            {
                sbStr.Append("clientId=");
                sbStr.Append(testClientId);
                sbStr.Append(";");
            }
            else if (!string.IsNullOrWhiteSpace(svalue))
            {
                sbStr.Append("clientId=");
                sbStr.Append(HttpUtility.UrlEncode(svalue));
                sbStr.Append(";");
            }

            svalue = txtWebSocketServer.Text;
            if (!string.IsNullOrWhiteSpace(svalue))
            {
                sbStr.Append("webSocketServer=");
                sbStr.Append(HttpUtility.UrlEncode(svalue));
                sbStr.Append(";");
            }

            svalue = txtProxy.Text;
            if (!string.IsNullOrWhiteSpace(svalue))
            {
                sbStr.Append("proxy=");
                sbStr.Append(HttpUtility.UrlEncode(svalue));
                sbStr.Append(";");

                svalue = txtProxyUserName.Text;
                if (!string.IsNullOrWhiteSpace(svalue))
                {
                    sbStr.Append("proxyUserName=");
                    sbStr.Append(HttpUtility.UrlEncode(svalue));
                    sbStr.Append(";");

                    svalue = txtProxyPassword.Text;
                    if (!string.IsNullOrWhiteSpace(svalue))
                    {
                        sbStr.Append("proxyPassword=");
                        sbStr.Append(HttpUtility.UrlEncode(svalue));
                        sbStr.Append(";");
                    }


                }
                svalue = txtDomain.Text;
                if (!string.IsNullOrWhiteSpace(svalue))
                {
                    sbStr.Append("domain=");
                    sbStr.Append(HttpUtility.UrlEncode(svalue));
                    sbStr.Append(";");
                }
            }





            int ivalue = nupSessionExpiry.Value.ConvertTo<int>();
            if (ivalue > 0)
            {
                sbStr.Append("sessionExpiry=");
                sbStr.Append(ivalue.ToString());
                sbStr.Append(";");
            }

            ivalue = nulKeepAlivePeriod.Value.ConvertTo<int>();
            if (ivalue > 0)
            {
                sbStr.Append("keepAlivePeriod=");
                sbStr.Append(ivalue.ToString());
                sbStr.Append(";");
            }



            selected = cmbQualityOfServiceLevel.SelectedValue;
            if (selected != null)
            {
                int val = (int)selected;
                if (val >= 0)
                {
                    sbStr.Append("QualityOfServiceLevel=");
                    sbStr.Append(val.ToString());
                    sbStr.Append(";");
                }
            }

            selected = cmbRetainHandling.SelectedValue;
            if (selected != null)
            {
                int val = (int)selected;
                if (val >= 0)
                {
                    sbStr.Append("RetainHandling=");
                    sbStr.Append(val.ToString());
                    sbStr.Append(";");
                }
            }



            sbStr.Append("RetainAsPublished=");
            sbStr.Append(chkRetainAsPublished.Checked ? "1" : "0");
            sbStr.Append(";");

            sbStr.Append("NoLocal=");
            sbStr.Append(chkNoLocal.Checked ? "1" : "0");
            sbStr.Append(";");

            sbStr.Append("keepAlive=");
            sbStr.Append(chkKeepAlive.Checked ? "1" : "0");
            sbStr.Append(";");

            selected = cmbProtocolVersion.SelectedValue;
            if (selected != null)
            {
                int val = (int)selected;
                if (val >= 0)
                {
                    sbStr.Append("ProtocolVersion=");
                    sbStr.Append(val.ToString());
                    sbStr.Append(";");
                }
            }


            sbStr.Append("CleanSession=");
            sbStr.Append(chkCleanSession.Checked ? "1" : "0");
            sbStr.Append(";");

            ivalue = nupSessionExpiryInterval.Value.ConvertTo<int>();
            if (ivalue > 0)
            {
                sbStr.Append("SessionExpiryInterval=");
                sbStr.Append(ivalue.ToString());
                sbStr.Append(";");
            }

            return sbStr.ToString();
        }



    }
}


