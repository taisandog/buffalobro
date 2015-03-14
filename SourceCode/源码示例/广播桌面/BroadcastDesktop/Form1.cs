using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.HttpServerModel;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Kernel;
using System.IO;
using System.Threading;
using BroadcastDesktop.Properties;
using System.Net;
using Buffalo.Win32Kernel;
using System.Net.Sockets;
using System.Management;
using System.Net.NetworkInformation;

namespace BroadcastDesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        ServerModel sm ;
        DesktopCache _cache;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            Listening(false);
            BindIP();
            BindQty();
        }

        private void BindIP() 
        {
            IPAddress[] ips= Dns.GetHostAddresses(Dns.GetHostName());
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
                    string[] ips=mo["IPAddress"] as string[];
                    if(ips!=null)
                    {
                        return ips[0];
                    }
                    string ip=mo["IPAddress"] as string;
                    if (ip != null)
                    {
                        return ip;
                    }
                }

            }
            return "";

        }
        

        void sm_OnRequestProcessing(RequestInfo request, ResponseInfo response)
        {
            string url = request.Path;
            if (url.Equals("/desktop", StringComparison.CurrentCultureIgnoreCase))
            {
                
                //response.MimeType = "image/jpeg";
                string content = Resources.model.Replace("<%=url%>", request.Host.Trim());
                content=content.Replace("<%=timeout%>", (1000 / ((int)nupFPS.Value)).ToString());
                response.Write(content);
                return;
            }
            if (url.Equals("/getdesktop", StringComparison.CurrentCultureIgnoreCase))
            {
                response.MimeType = "image/jpeg";
                if (_cache.CurrentDesktop != null)
                {
                    response.Write(_cache.CurrentDesktop);
                }
                return;
            }
            response.Write("没有数据");
        }

        private void Listening(bool isListen)
        {
            btnListen.Enabled = !isListen;
            gpSetting.Enabled = !isListen;
            gpIP.Enabled = isListen;

            btnStop.Enabled = isListen;
        }

        private void BindQty() 
        {
            cmbQty.Items.Clear();
            ComboBoxItem item = new ComboBoxItem();
            item.Text = "高质量(250K)";
            item.Tag = -1L;
            cmbQty.Items.Add(item);
            

            item = new ComboBoxItem();
            item.Text = "中高质量(150K)";
            item.Tag = 80L;
            cmbQty.Items.Add(item);

            item = new ComboBoxItem();
            item.Text = "中质量(100K)";
            item.Tag = 50L;
            cmbQty.Items.Add(item);
            item = new ComboBoxItem();
            item.Text = "中低质量(75K)";
            item.Tag = 25L;
            cmbQty.Items.Add(item);
            item = new ComboBoxItem();
            item.Text = "低质量(45K)";
            item.Tag = 10L;
            cmbQty.Items.Add(item);
            cmbQty.SelectedIndex = 0;
        }

        

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (sm != null)
            {
                try
                {
                    sm.StopServer();
                    Thread.Sleep(100);
                    _cache.StopUpdate();
                    _cache = null;
                    Listening(false);
                }
                catch { }
                Thread.Sleep(1000);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnStop_Click(btnStop, new EventArgs());
            
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            sm = new ServerModel(cmbIP.Text, (int)nupPort.Value);
            txturl.Text = "http://" + cmbIP.Text + ":" + (int)nupPort.Value + "/desktop";
            sm.OnRequestProcessing += new RequestProcessingHandle(sm_OnRequestProcessing);
            sm.StarServer();
            _cache = new DesktopCache(1000 / ((int)nupFPS.Value), chkIsMouse.Checked);
            ComboBoxItem selQry=cmbQty.SelectedItem as ComboBoxItem;
            if (selQry!=null) 
            {
                _cache.Qty = (long)selQry.Tag;
            }
            _cache.StarUpdate();
            Listening(true);
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
                txtInternetIP.Text = "http://" + ip+":" + ((int)nupPort.Value).ToString() + "/desktop";
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
    }
}