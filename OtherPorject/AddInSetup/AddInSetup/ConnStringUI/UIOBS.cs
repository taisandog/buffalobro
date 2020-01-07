using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buffalo.Storage;
using System.Web;
using System.Diagnostics;

namespace AddInSetup.ConnStringUI
{
    public partial class UIOBS : UIConnBase
    {
        public UIOBS()
        {
            InitializeComponent();
        }

        protected override void OnHelp()
        {
            Process.Start(BasePath + "OBS.docx");
        }
        protected override void OnTest()
        {
            string sbStr = GetConnectionString();
            IFileStorage storage = FSCreater.Create("OBS", sbStr);
            try
            {
                storage.Open();
                storage.GetFiles("/", System.IO.SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("测试成功", "测试成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        protected override void OnConnOut()
        {
            string sbStr = GetConnectionString();
            OutText.Text = sbStr;

            StringBuilder sbCode = new StringBuilder();
            sbCode.Append("string sconnStr=\"");
            sbCode.Append(sbStr);
            sbCode.Append("\";");
            sbCode.AppendLine("");
            sbCode.Append("IFileStorage storage = FSCreater.Create(\"OBS\", sconnStr);");
            sbCode.AppendLine("");
            sbCode.Append("storage.Open();");
            CodeText.Text = sbCode.ToString();

        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            StringBuilder sbStr = new StringBuilder();
            sbStr.Append("Server=");
            sbStr.Append(HttpUtility.UrlEncode(txtServer.Text));
            sbStr.Append(";");
            sbStr.Append("SecretId=");
            sbStr.Append(HttpUtility.UrlEncode(txtAccessKey.Text));
            sbStr.Append(";");
            sbStr.Append("SecretKey=");
            sbStr.Append(HttpUtility.UrlEncode(txtSecretKey.Text));
            sbStr.Append(";");
            sbStr.Append("BucketName=");
            sbStr.Append(HttpUtility.UrlEncode(txtBucketName.Text));
            sbStr.Append(";");
            sbStr.Append("InternetUrl=");
            sbStr.Append(HttpUtility.UrlEncode(txtInternetUrl.Text));
            sbStr.Append(";");
            sbStr.Append("NeedHash=");
            sbStr.Append(chkHash.Checked ? "1" : "0");
            sbStr.Append(";");
            sbStr.Append("Timeout=");
            sbStr.Append(txtTimeout.Value.ToString());
            sbStr.Append(";");
            FillProxyInfo(sbStr);
            return sbStr.ToString();
        }
    }
}
