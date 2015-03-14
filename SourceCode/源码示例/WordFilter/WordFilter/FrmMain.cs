using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Kernel.Win32;


namespace WordFilter
{
    public partial class FrmMain : Form
    {
        HotKey _hotKey;
        bool _visable = false;
        WordPicture _wp;
        QRCodeUnit _qrcode;
        private ToolStripMenuItem[] _toolItems;
        internal bool _isSys = false;//是否系统复制
        ConfigSave _config;
        ClipboardListener _listener;
        public FrmMain()
        {
            

            
            _wp = new WordPicture();
            _wp.Fcolor = Color.Black;
            _wp.LineAlpha = 200;
            _wp.Font = new Font("宋体", 12, FontStyle.Bold);
            _qrcode = new QRCodeUnit();
            
            InitializeComponent();
            _config = ConfigSave.ReadConfig();

            _toolItems = new ToolStripMenuItem[] { itemFont, itemQRCode, itemQRCodeEncry };
            _listener = new ClipboardListener(this.Handle);
            
            _listener.OnClipboardWrite += new DelOnWndProc(_listener_OnClipboardWrite);
            if (_config.ListenClipboard)
            {
                _listener.Listen();
            }
            InitSelectItem();

            ReSetConfig();
            
        }

        void _listener_OnClipboardWrite(Message msg)
        {
            if (!_isSys)
            {
                string str = _qrcode.GetQRCodeString();
                if (!string.IsNullOrEmpty(str))
                {
                    FrmQRResault.ShowBox(str);
                }
                
            }
        }

        
       
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 配置
        /// </summary>
        public ConfigSave Config
        {
            get { return _config; }
        }
        protected override void WndProc(ref Message m)
        {
            if (_hotKey != null) 
            {
                _hotKey.DoWndProc(m);
            }
            if (_listener != null)
            {
                _listener.DoWndProc(m);
            }
            base.WndProc(ref m);
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(_visable);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <returns></returns>
        private Bitmap GetPicture(string str) 
        {
            if (itemFont.Checked) 
            {
                return  _wp.DrawWordPicture(str);
            }
            else if (itemQRCode.Checked) 
            {
                return _qrcode.GetQRCode(str);
            }
            else if (itemQRCodeEncry.Checked) 
            {
                return _qrcode.GetEncryQRCode(str);
            }
            return _wp.DrawWordPicture(str);
        }
        

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //kbListener.StopListener();
            SaveSelectItem();
            FreeConfig();
            Application.Exit();
        }

        ~FrmMain() 
        {
            FreeConfig();
        }

        /// <summary>
        /// 释放钩子
        /// </summary>
        private void FreeConfig() 
        {
            if (_listener != null)
            {
                _listener.StopListen();
            }
            if (_hotKey!=null&&_hotKey.IsRegistered)
            {
                _hotKey.UnRegister();
            }
        }

        /// <summary>
        /// 重新启动配置
        /// </summary>
        public void ReSetConfig() 
        {
            if (_hotKey!=null && _hotKey.IsRegistered)
            {
                _hotKey.UnRegister();
            }
            _hotKey = new HotKey(1, _config.Modifiers, _config.HotKey, this);
            _hotKey.OnHotKeyPress += new DelOnWndProc(_hotKey_OnHotKeyPress);
            _hotKey.Register();
            _qrcode.Options.Width = _config.Side;
            _qrcode.Options.Height = _config.Side;
            if (_config.ListenClipboard)
            {
                if (!_listener.IsListen)
                {
                    _listener.Listen();
                }
            }
            else 
            {
                if (_listener.IsListen)
                {
                    _listener.StopListen();
                }
            }
        }

        void _hotKey_OnHotKeyPress(Message msg)
        {
            try
            {
                _isSys = true;
                Clipboard.Clear();

                SendKeys.SendWait("^A");
                SendKeys.SendWait("^C");


                if (Clipboard.ContainsText())
                {
                    string str = (String)Clipboard.GetData(DataFormats.Text);
                    if (str != null)
                    {
                        try
                        {

                            Image img = GetPicture(str);
                            if (img != null)
                            {
                                Clipboard.SetImage(img);
                                SendKeys.SendWait("^V");

                                if (_config.ShowTime > 0)
                                {
                                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                                    notifyIcon1.BalloonTipText = str;
                                    notifyIcon1.BalloonTipTitle = "已经转换文字";
                                    notifyIcon1.ShowBalloonTip(_config.ShowTime);
                                }
                                Clipboard.Clear();
                            }
                        }
                        catch
                        {
                        }
                    }
                }

            }
            finally
            {
                _isSys = false;
            }
        }

        /// <summary>
        /// 清空选中的项
        /// </summary>
        private void ClearSelectItem() 
        {
            foreach (ToolStripMenuItem item in _toolItems)
            {
                item.Checked = false;
            }
        }

        private void itemType_Click(object sender, EventArgs e)
        {
            ClearSelectItem();
            ToolStripMenuItem cur = sender as ToolStripMenuItem;
            if (cur != null) 
            {
                cur.Checked = true;
                
            }
        }

        /// <summary>
        /// 保存选中的项
        /// </summary>
        private void SaveSelectItem() 
        {
            for (int i = 0; i < _toolItems.Length; i++)
            {
                if (_toolItems[i].Checked)
                {
                    try
                    {
                        _config.OutItem = i;
                    }
                    catch { }
                    break;
                }
            }
            _config.SaveConfig();
        }

        /// <summary>
        /// 初始化选中的输出类型
        /// </summary>
        private void InitSelectItem() 
        {
            
            try
            {
                
                int index = _config.OutItem;
                ClearSelectItem();

                _toolItems[index].Checked = true; ;
            }
            catch { }
        }

        private void toolKey_Click(object sender, EventArgs e)
        {
            FrmKeys.ShowBox();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}