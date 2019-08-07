using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Buffalo.Win32Kernel.Win32
{
    public delegate void DelOnWndProc(Message msg);
    /// <summary>
    /// �����������
    /// </summary>
    public class ClipboardListener
    {
        private IntPtr _hNextClipboardViewer=IntPtr.Zero;//��һ�����ӵĴ���

        /// <summary>
        /// �������屻д��ʱ�򴥷�
        /// </summary>
        public event DelOnWndProc OnClipboardWrite;
        /// <summary>
        /// ��һ�����ӵĴ���
        /// </summary>
        public IntPtr HNextClipboardViewer
        {
            get { return _hNextClipboardViewer; }
        }

        private bool _isListen=false;
        /// <summary>
        /// �Ƿ��ڼ���
        /// </summary>
        public bool IsListen
        {
            get { return _isListen; }
        }

        private static readonly bool IsWinVistaOrLater = GetIsWinVistaOrLater();

        private IntPtr _handle;
        /// <summary>
        /// �����Ŀؼ����
        /// </summary>
        public IntPtr Handle
        {
            get { return _handle; }
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="handle">�����Ŀؼ����</param>
        public ClipboardListener(IntPtr handle) 
        {
            _handle = handle;
        }
        /// <summary>
        /// �Ƿ�Vista����
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
        /// ����������
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
        /// ֹͣ����
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
        /// ֪ͨ�������ؼ�������WndProc�¼�(����WndProc)�����)
        /// </summary>
        /// <param name="m">WndProc����Ϣ</param>
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
                    {//����Ҫ������Ϣ����һ�����ڵľ��
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

/*=============����================
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
            //��������д��
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }
*/