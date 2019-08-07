using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.NetClients
{
    

    /// <summary>
    /// 通知事件
    /// </summary>
    public delegate void MessageEvent(NetConnection conn);

    public partial class NetConnection
    {
        /// <summary>
        /// 通讯已经关闭
        /// </summary>
        public event MessageEvent OnClose;

        protected DataSteam _data = new DataSteam(512);
        /// <summary>
        /// 打开异步收发
        /// </summary>
        public void OpenAsync()
        {
            _recevieSocketAsync = new SocketAsyncEventArgs();
            _recevieSocketAsync.AcceptSocket = _client;
            _recevieSocketAsync.SetBuffer(new byte[128], 0, 128);
            _sendSocketAsync = new SocketAsyncEventArgs();
            _sendSocketAsync.AcceptSocket = _client;
            _lastReceiveTime = DateTime.Now;
            _lastSendTime = DateTime.Now;
            _recevieSocketAsync.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecevieCompleted);
            _sendSocketAsync.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
            ReceiveAsync();
        }
        private void OnRecevieCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                if (e.BytesTransferred > 0)
                {
                    _data.AddRange(e.Buffer, e.Offset, e.BytesTransferred);
                }
                PackageRecevie();
                ReceiveAsync();
                _lastReceiveTime = DateTime.Now;
            }
            else
            {
                CloseConnection();
            }
        }
        

        protected virtual bool PackageRecevie() 
        {
            
            return true;
        }
        


        /// <summary>
        /// 异步接收
        /// </summary>
        private void ReceiveAsync()
        {
            if (_client != null && _client.Connected)
            {
                _client.ReceiveAsync(_recevieSocketAsync);
            }

        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="data"></param>
        public void SendAsync(byte[] data) 
        {
            _sendSocketAsync.SetBuffer(data, 0, data.Length);
            Client.SendAsync(_sendSocketAsync);
            _lastSendTime = DateTime.Now;
        }

        private DateTime _lastReceiveTime;

        /// <summary>
        /// 最后接收时间
        /// </summary>
        public DateTime LastReceiveTime
        {
            get { return _lastReceiveTime; }
        }

        private DateTime _lastSendTime;

        /// <summary>
        /// 最后接收时间
        /// </summary>
        public DateTime LastSendTime
        {
            get { return _lastSendTime; }
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        public virtual void SendHeard() 
        {

        }
    }
}
