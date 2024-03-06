using Buffalo.Kernel.TreadPoolManager;
using Buffalo.IOCP;
using Buffalo.IOCP.DataProtocol;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetClientDemo
{
    public partial class UCFastNet : UserControl
    {
        public UCFastNet()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 信息线程
        /// </summary>
        private BlockThread _thdCheck;
        private bool _isRunning = false;

        private FastNetAdapter _defaultNetAdapter;
        /// <summary>
        /// 连接
        /// </summary>
        private FastClientSocket _conn = null;

        private HeartManager _heart = null;
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsConnect
        {
            get
            {
                return _conn != null && _conn.Connected;
            }
        }
        private IConnectMessage _messbox;
        public IConnectMessage Messbox
        {
            get
            {
                return _messbox;
            }
            set
            {
                _messbox = value;
            }
        }
        private const int Interval = 300;

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                return;
            }
            EnableEdit(false);
            _isRunning = true;
            _thdCheck = BlockThread.Create(CheckConnect);
            _thdCheck.StartThread();
        }
        /// <summary>
        /// 检查连接的线程
        /// </summary>
        /// <param name="args"></param>
        private void CheckConnect()
        {
            DateTime lastRun = DateTime.MinValue;
            DateTime nowDate = DateTime.MinValue;
            while (_isRunning)
            {
                nowDate = DateTime.Now;
                if (nowDate.Subtract(lastRun).TotalSeconds >= 5)
                {
                    if (_conn == null || !_conn.Connected)
                    {


                        if (_messbox != null && _messbox.ShowWarning)
                        {
                            _messbox.LogWarning(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "已断开消息服务，正在重连");
                        }

                        try
                        {

                            ConnectPush();
                        }
                        catch (Exception ex)
                        {
                            if (_messbox != null && _messbox.ShowError)
                            {
                                _messbox.LogError(ex.ToString());
                            }
                        }
                    }
                    lastRun = nowDate;

                }
                Thread.Sleep(Interval);
            }

        }


        private void ConnectPush()
        {
            DisConnect();
            lock (this)
            {

                Socket socket = null;
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(txtFastIP.Text, (int)nupFastPort.Value);
                }
                catch (Exception ex)
                {
                    if (_messbox != null && _messbox.ShowError)
                    {
                        _messbox.LogError(ex.ToString());
                    }
                    return;
                }

                if (_heart == null)
                {
                    _heart = new HeartManager(20000, 5000, 1000,0, _messbox);
                }
                if (_defaultNetAdapter == null)
                {
                    _defaultNetAdapter = new FastNetAdapter();
                }

                _conn = new FastClientSocket(socket, _heart, false, _defaultNetAdapter);
                _conn.OnClose += _conn_OnClose; ;
                _conn.AddReceiveDataHandle(_conn_OnReceiveData);
                _conn.OnError += _conn_OnError;
                _conn.Messager = _messbox;
                if (_messbox != null && _messbox.ShowLog)
                {
                    String str = String.Format("{0},已经连上服务器:{1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), txtFastIP.Text, (int)nupFastPort.Value);
                    _messbox.Log(str);
                }
            }
        }
        private void _conn_OnClose(ClientSocketBase clientSocket)
        {
            if (!_isRunning)
            {
                return;
            }
            if (_messbox != null && _messbox.ShowWarning)
            {
                String str = String.Format("{0},已经断开服务器:{1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), txtFastIP.Text, (int)nupFastPort.Value);
                _messbox.Log(str);
            }

        }
        private void _conn_OnError(ClientSocketBase clientSocket, Exception ex)
        {

            if (_messbox != null && _messbox.ShowError)
            {
                _messbox.LogError(ex.ToString());
            }

        }
        private void _conn_OnReceiveData(ClientSocketBase socket, DataPacketBase data)
        {
            if (_conn != socket)
            {
                return;
            }
            try
            {

                FastDataPacket fdp = data as FastDataPacket;
                if (fdp == null)
                {
                    return;
                }
                string content = System.Text.Encoding.UTF8.GetString(fdp.Data);
                if (_messbox != null && _messbox.ShowLog)
                {
                    _messbox.Log(content);
                }
            }
            catch (Exception ex)
            {
                if (_messbox != null && _messbox.ShowError)
                {
                    _messbox.LogError(ex.ToString());
                }
            }
        }
        /// <summary>
        /// 断开
        /// </summary>
        private void DisConnect()
        {
            lock (this)
            {
                if (_conn != null)
                {
                    _conn.Close();
                }
                _conn = null;
            }


        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnect()
        {
            _isRunning = false;
            if (_thdCheck != null)
            {
                try
                {
                    _thdCheck.StopThread();
                }
                catch (Exception ex)
                {
                    if (_messbox != null && _messbox.ShowError)
                    {
                        _messbox.LogError(ex.ToString());
                    }
                }
            }
            _thdCheck = null;
            if (_heart != null)
            {
                try
                {
                    _heart.StopHeart();

                }
                catch (Exception ex)
                {
                    if (_messbox != null && _messbox.ShowError)
                    {
                        _messbox.LogError(ex.ToString());
                    }
                }
                _heart = null;
            }
            try
            {
                DisConnect();
            }
            catch (Exception ex)
            {
                if (_messbox != null && _messbox.ShowError)
                {
                    _messbox.LogError(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 启用编辑控件
        /// </summary>
        /// <param name="enable"></param>
        private void EnableEdit(bool enable)
        {
            txtContent.Enabled = !enable;
            txtFastIP.Enabled = enable;
            nupFastPort.Enabled = enable;
            btnSend.Enabled = !enable;
            btnStart.Enabled = enable;
            btnStop.Enabled = !enable;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            CloseConnect();
            EnableEdit(true);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            _conn.Send(txtContent.Text);
        }

        private void UCFastNet_Load(object sender, EventArgs e)
        {
            EnableEdit(true);
        }
    }
}
