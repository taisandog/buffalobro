using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buffalo.DB.CacheManager;

namespace AddInSetup.ConnStringUI
{
    public partial class UIRedis : UIConnBase
    {
        public UIRedis()
        {
            InitializeComponent();
            ShowHelp = false;
        }
        protected override void OnHelp()
        {
            MessageBox.Show("请填入信息并按生成","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        protected override void OnTest()
        {

            MessageBox.Show("本程序只支持.NET 4.5或以上的Redis测试","不支持此测试", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        protected override void OnConnOut()
        {
            string sbStr = GetConnectionString();
            OutText.Text = sbStr;

            StringBuilder sbCode = new StringBuilder();
            sbCode.Append("string connString=\"");
            sbCode.Append(sbStr);
            sbCode.Append("\";");
            sbCode.AppendLine("");
            sbCode.Append(" QueryCache cache = CacheUnit.CreateCache(BuffaloCacheTypes.Redis, connString);");
            
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
            sbStr.Append(CacheUnit.EncodeString(txtServer.Text));
            sbStr.Append(";");
            if (!string.IsNullOrWhiteSpace(txtRoServer.Text))
            {
                sbStr.Append("readserver=");
                sbStr.Append(CacheUnit.EncodeString(txtRoServer.Text));
                sbStr.Append(";");
            }
            if (txtExpir.Value > 0)
            {
                sbStr.Append("expir=");
                sbStr.Append(((int)txtExpir.Value).ToString());
                sbStr.Append(";");
            }
            if (txtPoolsize.Value > 0)
            {
                sbStr.Append("poolsize=");
                sbStr.Append(((int)txtPoolsize.Value).ToString());
                sbStr.Append(";");
            }
            sbStr.Append("throw=");
            sbStr.Append(chkThrow.Checked?"1":"0");
            sbStr.Append(";");

            
            

            return sbStr.ToString();
        }
    }
}
