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
using System.Web;

namespace AddInSetup.ConnStringUI
{
    public partial class UIMemcached : UIConnBase
    {
        public UIMemcached()
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
            string sbStr = GetConnectionString();
           
            try
            {
                QueryCache cache = CacheUnit.CreateCache(BuffaloCacheTypes.Memcached, sbStr);
                Random rnd=new Random();
                int num=rnd.Next(1,99999999);
                cache.SetValue<int>("BuffaloTest", num);
                int val = cache.GetValue<int>("BuffaloTest");
                if (val != num) 
                {
                    MessageBox.Show("测试失败:[写入值=" + num + "]，[输出值=" + val+"]，请检查配置", "测试失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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
            sbCode.Append("string connString=\"");
            sbCode.Append(sbStr);
            sbCode.Append("\";");
            sbCode.AppendLine("");
            sbCode.Append(" QueryCache cache = CacheUnit.CreateCache(BuffaloCacheTypes.Memcached, connString);");
            
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
            if (txtPoolsize.Value > 0)
            {
                sbStr.Append("poolsize=");
                sbStr.Append(((int)txtPoolsize.Value).ToString());
                sbStr.Append(";");
            }
            sbStr.Append("throw=");
            sbStr.Append(chkThrow.Checked?"1":"0");
            sbStr.Append(";");

            if (!string.IsNullOrWhiteSpace(txtUid.Text))
            {
                sbStr.Append("uid=");
                sbStr.Append(CacheUnit.EncodeString(txtUid.Text));
                sbStr.Append(";");
            }

            if (!string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                sbStr.Append("pwd=");
                sbStr.Append(CacheUnit.EncodeString(txtPwd.Text));
                sbStr.Append(";");
            }

            

            return sbStr.ToString();
        }
    }
}
