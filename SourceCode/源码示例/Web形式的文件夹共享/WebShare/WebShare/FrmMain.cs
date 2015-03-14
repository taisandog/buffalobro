using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Management;
using System.Threading;
using Buffalo.Win32Kernel;

namespace WebShare
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        ConfigManager _curConfig = Program.Config;
        WebServer _server = null;
        private void FrmMain_Load(object sender, EventArgs e)
        {
            BindIP();
            gvPath.AutoGenerateColumns = false;
            BindShareInfo();
            Listening(false);
            FillInfo();
            _server = new WebServer(_curConfig);
            chkAutoStart.Checked = RegConfig.IsAutoRun;
            LockSize();
            
        }

        /// <summary>
        /// 锁定大小
        /// </summary>
        private void LockSize() 
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private bool _isAutoStart;
        /// <summary>
        /// 是否自启动
        /// </summary>
        public bool AutoStart
        {
            get { return _isAutoStart; }
            set { _isAutoStart = value; }
        }

        /// <summary>
        /// 开启
        /// </summary>
        private void StartListen() 
        {
            txturl.Text = GetLocalString(cmbIP.Text, (int)nupPort.Value);

            Listening(true);
            Save();
            _server.Start();
        }

        private void FillInfo() 
        {
            cmbIP.Text = _curConfig.BindIP;
            nupPort.Value = _curConfig.BindPort;
        }

        private void BindShareInfo() 
        {
            gvPath.DataSource = null;
            if (Program.Config.ShareInfos != null && Program.Config.ShareInfos.Count > 0)
            {
                gvPath.DataSource = _curConfig.ShareInfos;
            }
        }
        private void BindIP()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in ips)
            {
                AddressFamily str = ip.AddressFamily;
                cmbIP.Items.Add(ip.ToString());
            }
            cmbIP.Text = GetCurrentIPAddress();
        }
        /// <summary>
        /// 获取当前IP
        /// </summary>
        /// <returns></returns>
        private string GetCurrentIPAddress()
        {

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    string[] ips = mo["IPAddress"] as string[];
                    if (ips != null)
                    {
                        return ips[0];
                    }
                    string ip = mo["IPAddress"] as string;
                    if (ip != null)
                    {
                        return ip;
                    }
                }

            }
            return "";

        }

        private void btnAddShare_Click(object sender, EventArgs e)
        {
            using (FrmAddShare frmAdd = new FrmAddShare()) 
            {
                if (frmAdd.ShowDialog() == DialogResult.OK) 
                {

                    _curConfig.ShareInfos.AddShareInfo(frmAdd.Name, frmAdd.Path);
                    BindShareInfo();
                }
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_server.Running) 
            {
                if(MessageBox.Show("共享正在运行,是否要退出？",this.Text,MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _server.Stop();
            Save();
            Thread.Sleep(300);

            
        }
        private void Listening(bool isListen)
        {
            btnStart.Enabled = !isListen;
            pnlSetting.Enabled = !isListen;
            gpIP.Enabled = isListen;
            cmbIP.Enabled = !isListen;
            nupPort.Enabled = !isListen;
            btnStop.Enabled = isListen;
            
            if (!isListen)
            {
                gvPath.BackgroundColor = Color.White;
            }
            else 
            {
                gvPath.BackgroundColor =Color.DarkGray;
            }
        }

        private void Save() 
        {
            _curConfig.BindIP = cmbIP.Text;
            _curConfig.BindPort = (int)nupPort.Value;
            _curConfig.SaveConfig();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartListen();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _server.Stop();
                Listening(false);
                Thread.Sleep(100);
            }
            catch { }
        }

        private void gvPath_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvPath.Columns[e.ColumnIndex].Name == "ColDelete") 
            {
                _curConfig.ShareInfos.RemoveAt(e.RowIndex);
                BindShareInfo();
            }
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        private string GetLocalString(string ip, int port) 
        {
            StringBuilder sbRet = new StringBuilder();
            sbRet.Append("http://");
            sbRet.Append(ip);
            if (port != 80) 
            {
                sbRet.Append(":");
                sbRet.Append(port);
            }
            sbRet.Append("/");
           
            return sbRet.ToString();
        }

        private void btnGetIP_Click(object sender, EventArgs e)
        {
            string ip = HttpHelper.GetInternetIP();
            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("获取外网IP失败");
            }
            else
            {
                txtInternetIP.Text = GetLocalString(ip,(int)nupPort.Value);
            }
        }

        private void btnCopyInternet_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtInternetIP.Text);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txturl.Text);
        }

        private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            RegConfig.IsAutoRun = chkAutoStart.Checked;
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            if (_isAutoStart)
            {
                StartListen();
                this.Hide();
            }
        }

        private void 显示窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void nfIco_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        
    }
}