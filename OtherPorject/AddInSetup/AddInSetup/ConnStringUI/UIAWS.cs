using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using Buffalo.Storage;
using System.Diagnostics;
using Buffalo.Storage.AWS.S3;

namespace AddInSetup.ConnStringUI
{
    public partial class UIAWS : UIConnBase
    {
        public UIAWS()
        {
            InitializeComponent();
        }
        protected override void OnHelp()
        {
            Process.Start(BasePath + "aws.docx");
        }
        protected override void OnTest()
        {
            string sbStr = GetConnectionString();
            IFileStorage storage = FSCreater.Create("AWSS3", sbStr);
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
            sbCode.Append("IFileStorage storage = FSCreater.Create(\"AWSS3\", sconnStr);");
            sbCode.AppendLine("");
            sbCode.Append("storage.Open();");
            CodeText.Text = sbCode.ToString();

        }
        protected override void OnLoad(EventArgs e)
        {
            FillACL();
            FillServer();
            base.OnLoad(e);
        }
        private void FillACL()
        {
            List<KeyValuePair<string, string>> lstItems = new List<KeyValuePair<string, string>>();
            lstItems.Add(new KeyValuePair<string, string>("只读", "read"));
            lstItems.Add(new KeyValuePair<string, string>("读写", "write"));
            lstItems.Add(new KeyValuePair<string, string>("私有", ""));
            
            cmbACL.DisplayMember = "Key";
            cmbACL.ValueMember = "Value";
            cmbACL.DataSource = lstItems;
        }
        private void FillServer()
        {
            txtServer.Items.Clear();
            List<KeyValuePair<string, string>> lstItems = new List<KeyValuePair<string, string>>();
            List<string> lstServer = AWSS3Adapter.GetAllRegionEndpoint();
            txtServer.Items.Add("http://s3.amazonaws.com");
            foreach (string server in lstServer)
            {
                txtServer.Items.Add(server);
            }
            txtServer.SelectedIndex = 0;
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
            sbStr.Append("acl=");
            sbStr.Append(cmbACL.SelectedValue.ToString());
            sbStr.Append(";");
            FillProxyInfo(sbStr);
            return sbStr.ToString();
        }
    }
}
