using Buffalo.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace AddInSetup.ConnStringUI
{
    public partial class UILocationStorage : AddInSetup.ConnStringUI.UIConnBase
    {
        public UILocationStorage()
        {
            InitializeComponent();
        }

        protected override void OnHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("路径为：盘符路径，可以是局域网路径");
            sb.AppendLine("用户名为：登陆用户名，默认用户则为空");
            sb.AppendLine("密码为：登陆用户密码，默认则为空");
            MessageBox.Show(sb.ToString(), "提示");
        }
        protected override void OnTest()
        {
            string sbStr = GetConnectionString();
            IFileStorage storage = FSCreater.Create("local", sbStr);
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
            sbCode.Append("IFileStorage storage = FSCreater.Create(\"local\", sconnStr);");
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
            sbStr.Append("path=");
            sbStr.Append(HttpUtility.UrlEncode(txtPath.Text));
            sbStr.Append(";");
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                sbStr.Append("user=");
                sbStr.Append(HttpUtility.UrlEncode(txtName.Text));
                sbStr.Append(";");
                if (!string.IsNullOrWhiteSpace(txtpwd.Text))
                {
                    sbStr.Append("pwd=");
                    sbStr.Append(HttpUtility.UrlEncode(txtpwd.Text));
                    sbStr.Append(";");
                }
            }
            return sbStr.ToString();
        }
    }
}
