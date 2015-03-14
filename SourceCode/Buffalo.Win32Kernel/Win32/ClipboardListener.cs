using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.Win32;
using System.Windows.Forms;

namespace Buffalo.Win32Kernel.Win32
{
    public delegate void DelOnWndProc(Message msg);
    /// <summary>
    /// 剪贴板监听器
    /// </summary>
    public class ClipboardListener
    {
        private IntPtr _hNextClipboardViewer=IntPtr.Zero;//下一个监视的窗口

        /// <summary>
        /// 当剪贴板被写入时候触发
        /// </summary>
        public event DelOnWndProc OnClipboardWrite;
        /// <summary>
        /// 下一个监视的窗口
        /// </summary>
        public IntPtr HNextClipboardViewer
        {
            get { return _hNextClipboardViewer; }
        }

        private bool _isListen=false;
        /// <summary>
        /// 是否在监听
        /// </summary>
        public bool IsListen
        {
            get { return _isListen; }
        }

        private static readonly bool IsWinVistaOrLater = GetIsWinVistaOrLater();

        private IntPtr _handle;
        /// <summary>
        /// 关联的控件句柄
        /// </summary>
        public IntPtr Handle
        {
            get { return _handle; }
        }

        /// <summary>
        /// 剪贴板监听器
        /// </summary>
        /// <param name="handle">关联的控件句柄</param>
        public ClipboardListener(IntPtr handle) 
        {
            _handle = handle;
        }
        /// <summary>
        /// 是否Vista以上
        /// </summary>
        /// <returns></returns>
        private static bool GetIsWinVistaOrLater()
        {
            System.OperatingSystem osInfo = System.Environment.OSVersion;
            if (osInfo.Version.Major >= 6)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 监听剪贴板
        /// </summary>
        public void Listen()
        {
            if (IsWinVistaOrLater)
            {
                WindowsAPI.AddClipboardFormatListener(_handle);
            }
            else
            {
                _hNextClipboardViewer = WindowsAPI.SetClipboardViewer(_handle);
            }
            _isListen = true;
        }
        /// <summary>
        /// 停止监听
        /// </summary>
        public void StopListen() 
        {
            if (IsWinVistaOrLater)
            {
                WindowsAPI.RemoveClipboardFormatListener(_handle);
            }
            else if (_hNextClipboardViewer != IntPtr.Zero)
            {
                WindowsAPI.ChangeClipboardChain(_handle, _hNextClipboardViewer);
                _hNextClipboardViewer = IntPtr.Zero;
            }
            _isListen = false;
        }

        /// <summary>
        /// 通知监听器控件触发了WndProc事件(请在WndProc)里调用)
        /// </summary>
        /// <param name="m">WndProc的消息</param>
        /// <returns></returns>
        public void DoWndProc(Message m) 
        {
            Msg objMsg = (Msg)m.Msg;
            if (objMsg == Msg.WM_DRAWCLIPBOARD || objMsg == Msg.WM_CLIPBOARDUPDATE)
            {
                if (OnClipboardWrite != null) 
                {
                    OnClipboardWrite(m);
                    if (_hNextClipboardViewer != IntPtr.Zero)
                    {
                        WindowsAPI.SendMessage(_hNextClipboardViewer, (Msg)m.Msg, m.WParam, m.LParam);
                    }
                }
                
            }
            else if (objMsg == Msg.WM_CHANGECBCHAIN)
            {
                if (_hNextClipboardViewer != IntPtr.Zero)
                {
                    if (_hNextClipboardViewer == m.WParam)
                    {//更新要发送消息的下一个窗口的句柄
                        _hNextClipboardViewer = m.LParam;
                    }
                    else
                    {
                        WindowsAPI.SendMessage(_hNextClipboardViewer, (Msg)m.Msg, m.WParam, m.LParam);
                    }
                }
            }
        }
    }
}

/*=============例子================
public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ClipboardListener _listener;
        private void Form1_Load(object sender, EventArgs e)
        {
            _listener = new ClipboardListener(this.Handle);
            _listener.Listen();
            _listener.OnClipboardWrite += new DelOnWndProc(_listener_OnClipboardWrite);
            
        }
        
        
        protected override void WndProc(ref Message m)
        {
            if (_listener != null)
            {
                _listener.DoWndProc(m);
            }
            base.WndProc(ref m);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_listener != null)
            {
                _listener.StopListen();
            }
        }
        void _listener_OnClipboardWrite(Message msg)
        {
            //剪贴板有写入
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }
*/