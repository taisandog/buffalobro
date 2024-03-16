using Buffalo.Kernel;
using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.IOCP.DataProtocol
{
    /// <summary>
    /// 连接事件
    /// </summary>
    /// <param name="clientSocket">客户端连接</param>
    public delegate void SocketEvent(ClientSocketBase clientSocket);
    /// <summary>
    /// 数据事件
    /// </summary>
    /// <param name="data">数据</param>
    public delegate void DataEvent(ClientSocketBase socket, DataPacketBase data);

    /// <summary>
    /// 通知事件
    /// </summary>
    public delegate void MessageEvent(ClientSocketBase clientSocket);
    /// <summary>
    /// 一般通知事件
    /// </summary>
    public delegate bool NormalMessageHandle(ClientSocketBase clientSocket,int type, object message);
    /// <summary>
    /// 连接处理错误
    /// </summary>
    public delegate void ClientSocketError(ClientSocketBase clientSocket, Exception ex);


    /// <summary>
    /// 数据通讯处理
    /// </summary>
    public abstract partial class ClientSocketBase : IDisposable
    {
        #region 事件

        /// <summary>
        /// 收到数据
        /// </summary>
        private event DataEvent OnReceiveData;
        private SocketCertConfig _certConfig;
        /// <summary>
        /// 加密证书配置
        /// </summary>
        public SocketCertConfig CertConfig
        {
            get { return _certConfig; }
            
        }

        /// <summary>
        /// 重建字节流
        /// </summary>
        private void RebuildTlsStream()
        {
            
            if (_certConfig == null)
            {
                return;
            }
            CloseTlsStream();

            _netStream = new NetworkStream(_bindSocket);
            
            _tlsStream = _certConfig.CreateStream(_netStream, _isServerSocket);


        }

        private void CloseTlsStream() 
        {
            SslStream sslStream = _tlsStream;
            if (sslStream != null)
            {
                try
                {
                    sslStream.Close();
                    sslStream.Dispose();
                }
                catch { }
            }
            sslStream = null;
            _tlsStream = null;

            NetworkStream netStream = _netStream;
            if (netStream != null)
            {
                netStream.Close();
                netStream.Dispose();
            }
            _netStream = null;
            netStream = null;
        }

        public void AddReceiveDataHandle(DataEvent receiveData)
        {
            OnReceiveData += receiveData;
            Receive();
        }
        /// <summary>
        /// 通讯已经关闭
        /// </summary>
        public event MessageEvent OnClose;

        /// <summary>
        /// 通讯错误
        /// </summary>
        public event ClientSocketError OnError;
        /// <summary>
        /// 需要锁的对象
        /// </summary>
        internal object _lokRootObject = new object();
        /// <summary>
        /// 一般通知事件
        /// </summary>
        public event NormalMessageHandle OnMessage;
        /// <summary>
        /// 当前的加密Stream
        /// </summary>
        private SslStream _tlsStream;

        /// <summary>
        /// 网络流
        /// </summary>
        private NetworkStream _netStream;

        //private BlockThreadPool _thdPool = null;
        #endregion
        public void Dispose()
        {
            Close();
        }

        #region 属性
        protected Socket _bindSocket;

        /// <summary>
        /// 绑定的Socket连接
        /// </summary>
        public Socket BindSocket
        {
            get
            {
                return _bindSocket;
            }
        }
        protected DataManager _dataManager;
        /// <summary>
        /// 数据包管理
        /// </summary>
        public DataManager DataManager
        {

            get
            {
                return _dataManager;
            }
        }
        protected INetProtocol _netProtocol;
        /// <summary>
        /// 协议
        /// </summary>
        public INetProtocol NetProtocol
        {
            get
            {
                return _netProtocol;
            }
        }
        /// <summary>
        /// 是否连接
        /// </summary>
        public bool Connected
        {

            get
            {
                return _bindSocket != null && _bindSocket.Connected;
            }
        }
        /// <summary>
        /// 收到的数据缓存区
        /// </summary>
        protected NetByteBuffer _bufferData = new NetByteBuffer(256);
        /// <summary>
        /// 是否连接
        /// </summary>
        public NetByteBuffer BufferData
        {

            get
            {
                return _bufferData;
            }
        }

        /// <summary>
        /// 连接超时时间
        /// </summary>
        protected int _outTime;
        /// <summary>
        /// 最后收到数据时间
        /// </summary>
        public DateTime LastReceiveTime
        {
            set;
            get;
        }

        /// <summary>
        /// 最后发送数据时间
        /// </summary>
        public DateTime LastSendTime
        {
            set;
            get;
        }

        protected IPEndPoint _remoteIP;
        /// <summary>
        /// 远程IP
        /// </summary>
        public IPEndPoint RemoteIP
        {
            get
            {
                if (_remoteIP == null && _bindSocket != null)
                {
                    _remoteIP = (IPEndPoint)_bindSocket.RemoteEndPoint;
                }
                return _remoteIP;
            }
        }


        /// <summary>
        /// 远程主机IP
        /// </summary>
        protected String _HostIP;
        /// <summary>
        /// 远程主机IP
        /// </summary>
        public String HostIP
        {
            get
            {
                if (_HostIP == null && RemoteIP != null)
                {

                    _HostIP = RemoteIP.Address.ToString();
                }
                return _HostIP;
            }
        }

        protected int _port = -1;
        /// <summary>
        /// 远程主机IP
        /// </summary>
        public int Port
        {
            get
            {
                if (_port < 0 && RemoteIP != null)
                {

                    _port = RemoteIP.Port;
                }
                return _port;
            }
        }
        /// <summary>
        /// 异步接收事件
        /// </summary>
        protected SocketAsyncEventArgs _recevieSocketAsync;
        /// <summary>
        /// 异步发送
        /// </summary>
        protected SocketAsyncEventArgs _sendSocketAsync;

        /// <summary>
        /// 是否正在发送
        /// </summary>
        protected bool IsSend;
        /// <summary>
        /// 心跳管理
        /// </summary>
        protected HeartManager _heartmanager;

        public HeartManager HeartManager
        {
            get { return _heartmanager; }
        }

        /// <summary>
        /// 本连接ID
        /// </summary>
        protected long _id = 0;
        /// <summary>
        /// 本连接ID
        /// </summary>
        public long Id
        {
            get
            {
                return _id;
            }
        }
        #endregion

        #region 方法
        protected static long _autoincrementId = 0;
        protected static object autoLock = new object();

        private static INetProtocol _defaultAdapter = new FastNetAdapter();

        protected virtual INetProtocol CreateDefaultAdapter()
        {

            return _defaultAdapter;
        }
        private bool _isServerSocket = false;
        /// <summary>
        /// 是否服务器连接
        /// </summary>
        public bool IsServerSocket
        {
            get { return _isServerSocket; }
        }




        /// <summary>
        /// 创建Socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="maxSendPool"></param>
        /// <param name="maxLostPool"></param>
        /// <param name="isServerSocket"></param>
        /// <param name="heartManager"></param>
        /// <param name="netProtocol"></param>
        protected ClientSocketBase(Socket socket, int maxSendPool, int maxLostPool, bool isServerSocket,
            HeartManager heartManager, INetProtocol netProtocol, SocketCertConfig certConfig)
        {
            _netProtocol = netProtocol;
            if (_netProtocol == null)
            {
                _netProtocol = CreateDefaultAdapter();
            }
            _certConfig = certConfig;
            _isServerSocket = isServerSocket;
            //_thdPool = new BlockThreadPool(2000);
            _dataManager = new DataManager(maxSendPool, maxLostPool,this, _netProtocol);
            _bindSocket = socket;
            lock (autoLock)
            {
                _id = _autoincrementId;
                _autoincrementId++;

                if (_netProtocol.ShowLog)
                {
                    _netProtocol.Log("SocketCreate:id=" + _id);
                }

            }
            _recevieSocketAsync = new SocketAsyncEventArgs();
            
            int buffLen = _netProtocol.BufferLength;
           
            if (_certConfig == null)
            {
                _recevieSocketAsync.AcceptSocket = _bindSocket;
                _recevieSocketAsync.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecCompleted);
            }
            _recevieSocketAsync.SetBuffer(new byte[buffLen], 0, buffLen);

            _sendSocketAsync = new SocketAsyncEventArgs();
            _sendSocketAsync.AcceptSocket = BindSocket;
            _sendSocketAsync.Completed += new EventHandler<SocketAsyncEventArgs>(OnCompleted);
            
            LastSendTime = DateTime.Now;
            LastReceiveTime = DateTime.Now;
            //SocketCount++;
            if (_certConfig != null)
            {
                RebuildTlsStream();
            }
            if (heartManager != null)
            {
                heartManager.AddClient(this);
                _heartmanager = heartManager;
            }

        }


        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="dataPacket"></param>
        public virtual void SendPacket(DataPacketBase dataPacket)
        {
            lock (_lokRootObject)
            {
                if (Connected)
                {
                    string err = _dataManager.AddData(dataPacket);
                    if (err != null )
                    {
                        PutMessageEvent(1, err);
                    }
                }
            }
            Send(_sendSocketAsync);
        }
        /// <summary>
        /// 发送字节数组
        /// </summary>
        /// <param name="data"></param>
        public abstract DataPacketBase Send(byte[] data);
        /// <summary>
        /// 发送字符串
        /// </summary>
        /// <param name="data"></param>
        public abstract DataPacketBase Send(string data);


        private object _lokSend = new object();
        /// <summary>
        /// 发送原始数据
        /// </summary>
        /// <param name="data"></param>
        public virtual DataPacketBase SendRaw(byte[] data)
        {
            DataPacketBase dp = new DataPacketBase();
            dp.IsRaw = true;
            dp.Data = data;
            SendPacket(dp);
            return dp;
        }
        /// <summary>
        /// 执行发送方法
        /// </summary>
        internal void RunSend() 
        {
            Send(_sendSocketAsync);
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="isAsync">是否异步发送</param>
        protected void Send(SocketAsyncEventArgs sendSocketAsync)
        {
            lock (_lokSend)
            {
                if (IsSend)
                {
                    return;
                }
                IsSend = true;
            }
            bool isSync = true;
            DataPacketBase dataPacket = null;
            
            try
            {

                if (!Connected)
                {
                    return;
                }



                lock (_lokRootObject)
                {
                    dataPacket = _dataManager.GetData();
                }
                if (dataPacket == null)
                {
                    return;
                }
                Socket socket = null;

                try
                {
                    byte[] data = null;
                    if (dataPacket.IsRaw)
                    {
                        data = dataPacket.Data;
                    }
                    else
                    {
                        data = _netProtocol.ToArray(dataPacket);
                    }
                    
                    lock (_lokRootObject)
                    {
                        socket = _bindSocket;
                    }
                    sendSocketAsync.AcceptSocket = socket;
                    
                    sendSocketAsync.SetBuffer(data, 0, data.Length);
                    sendSocketAsync.UserToken = null;
                    sendSocketAsync.UserToken = dataPacket;
                    if (socket != null && Connected)
                    {
                        isSync = SendAsync(socket, sendSocketAsync);
                    }
                    LastSendTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    DoSendFault(dataPacket);
                    if (_netProtocol.ShowError)
                    {
                        _netProtocol.LogError("Send Error:" + ex.ToString());
                    }
                }
                finally
                {
                    dataPacket = null;
                    
                    if (!isSync) 
                    {
                        OnCompleted(this, sendSocketAsync);
                    }
                    socket = null;
                }


            }
            catch (Exception ex)
            {
                if (_netProtocol.ShowError)
                {
                    _netProtocol.Log("Send Error:" + ex.ToString());
                }
            }
            finally
            {
                lock (_lokSend)
                {
                    IsSend = false;
                }

                if (!isSync)
                {
                    DoSocketSend(sendSocketAsync);
                }
               
                sendSocketAsync = null;
            }
        }

        private bool SendAsync(Socket socket, SocketAsyncEventArgs sendSocketAsync)
        {
            if (_tlsStream == null)
            {
                return socket.SendAsync(sendSocketAsync);
            }
            _tlsStream.BeginWrite(sendSocketAsync.Buffer, 0, sendSocketAsync.Buffer.Length, SendCallback, sendSocketAsync);
            //_tlsStream.WriteAsync(sendSocketAsync.Buffer, 0, sendSocketAsync.Buffer.Length, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false); ;
            return true;
        }
        public void SendCallback(IAsyncResult ar)
        {
            SocketAsyncEventArgs sendSocketAsync = ar.AsyncState as SocketAsyncEventArgs;
            _tlsStream.EndWrite(ar);
            //_tlsStream.Flush();
            DoSocketSend(sendSocketAsync);
        }


       

        /// <summary>
        /// 发送失败
        /// </summary>
        /// <param name="dataPacket"></param>
        private void DoSendFault(DataPacketBase dataPacket)
        {
            //if (dataPacket.IsHeart || (!dataPacket.IsLost))
            //{
            //    return;
            //}
            //lock (_lokRootObject)
            //{
            //    if (Connected && !_dataManager.IsSendPacketFull)
            //    {
            //        string err = _dataManager.AddData(dataPacket);
            //        if (err != null )
            //        {
            //            PutMessageEvent(1, err);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 触发消息事件
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int PutMessageEvent(int type, object message) 
        {
            if ( OnMessage != null)
            {
                return OnMessage(this, type, message)?1:0;
            }
            return -1;
        }

        /// <summary>
        /// 接收
        /// </summary>
        protected void Receive()
        {
            Socket socket = null;

            SocketAsyncEventArgs eventArgs = null;

            if (!Connected)
            {
                return;
            }
            socket = _bindSocket;
            eventArgs = _recevieSocketAsync;

            try
            {
                if (!ReceiveAsync(socket,eventArgs))
                {
                    DoSocketReceive(eventArgs,-1);
                }
            }
           
            catch (Exception ex)
            {
                if (ShowError)
                {
                    _message.LogError(ex.ToString());
                }
            }
            eventArgs = null;
            socket = null;
        }

        private bool ReceiveAsync(Socket socket,SocketAsyncEventArgs eventArgs) 
        {
            if(_tlsStream == null) 
            {
                return socket.ReceiveAsync(eventArgs);
            }
            try
            {
                _tlsStream.BeginRead(eventArgs.Buffer, 0, eventArgs.Buffer.Length, ReadCallback, eventArgs);
            }catch(Exception ex) 
            {
                Close(true);
                HandleClose("Client Close");
                return true;
            }
            return true;
        }

        public void ReadCallback(IAsyncResult ar) 
        {
            SocketAsyncEventArgs eventArgs = ar.AsyncState as SocketAsyncEventArgs;
            try
            {
                if (_tlsStream == null) 
                {
                    return;
                }
                int readCount = _tlsStream.EndRead(ar);
                //if (readCount <= 0)
                //{
                //    Receive();
                //    return;
                //}
                DoSocketReceive(eventArgs, readCount);
            }
            catch (Exception ex)
            {
                Close(true);
                HandleClose("Client Close");
              
            }
        }



        /// <summary>
        /// 检查数据重发
        /// </summary>
        internal void CheckResend(int timeResend)
        {

        }
        protected void OnCompleted(object sender, SocketAsyncEventArgs e)
        {

            //using (SocketAsyncEventArgs ae = e)
            //{
            //    using (DataPacketBase packet = ae.UserToken as DataPacketBase)
            //    {
            //        if (packet != null)
            //        {
            //            packet.SetEvent();
            //        }

                    DoCompleted(sender, e);
                //}
                //EventHandleClean.ClearAllEvents(ae);
            //}

        }
        object _lokBuffdata = new object();
        protected void DoCompleted(object sender, SocketAsyncEventArgs e)
        {

            if (e.SocketError == SocketError.Success)
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        DoSocketReceive(e);
                        break;
                    case SocketAsyncOperation.Send:
                        DoSocketSend(e);
                        break;
                    case SocketAsyncOperation.Disconnect:
                        DoSocketDisconnect(e);
                        break;
                    default:
                        break;
                }
            }
            else
            {

                HandleClose("Client error:" + e.SocketError);
                Close(true);
                return;
            }

        }
        /// <summary>
        /// 处理断开
        /// </summary>
        /// <param name="e"></param>
        private void DoSocketDisconnect(SocketAsyncEventArgs e)
        {
            HandleClose("Client Disconnect");
            Close(true);
            return;
        }
        /// <summary>
        /// 处理发送
        /// </summary>
        /// <param name="e"></param>
        private void DoSocketSend(SocketAsyncEventArgs e)
        {

            //LastSendTime = DateTime.Now;
            lock (_lokSend)
            {
                IsSend = false;
            }
            Send(e);
        }

        //protected bool _isWebsocketHandShanked = false;

        /// <summary>
        /// 处理接收
        /// </summary>
        /// <param name="e"></param>
        private void DoSocketReceive(SocketAsyncEventArgs e,int count=-1)
        {

            if (count < 0) 
            {
                count = e.BytesTransferred;
            }


            if (count <= 0 || e.Buffer == null)
            {
                Close(true);
                HandleClose("Client Close:"+ e.SocketError);
                return;
            }
            if (!Connected)
            {
                return;
            }

            DateTime recDate = DateTime.Now;
            Socket socket = _bindSocket;
            try
            {


                lock (_lokBuffdata)
                {
                    NetByteBuffer bufferData = _bufferData;
                    if (bufferData == null)
                    {
                        return;
                    }
                    AppendToBuffer(bufferData,e.Buffer, e.Offset, count);
                    //byte[] content = new byte[bufferData.Count];
                    //bufferData.ReadBytes(0, content, 0, content.Length);
                    LastReceiveTime = DateTime.Now;
                    if (socket.Available == 0 || _certConfig!=null)
                    {

                        DataPacketBase dataPacket = null;
                        while (_netProtocol.IsDataLegal(out dataPacket, this))
                        {
                            if (dataPacket != null)
                            {
                                DoDataPacket(dataPacket, recDate);
                            }
                        }
                    }

                }
                
                Receive();
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    OnError(this, ex);
                }
                HandleClose("Client error:" + e.SocketError);
                Close(true);
            }
        }





        /// <summary>
        /// 读入到缓冲区
        /// </summary>
        /// <param name="bufferData">缓冲数组</param>
        /// <param name="socketBuffer">socket缓冲</param>
        /// <param name="offest">位置</param>
        /// <param name="count">数值</param>
        protected virtual void AppendToBuffer(NetByteBuffer bufferData, byte[] socketBuffer, int offest, int count)
        {
            bufferData.AppendBytes(socketBuffer, offest, count);
        }


        public virtual void DoDataPacket(DataPacketBase dataPacket, DateTime recDate)
        {
            if ( OnReceiveData != null)
            {
                OnReceiveData(this, dataPacket);
            }
        }


        /// <summary>
        /// 处理接收数据
        /// </summary>
        /// <param name="args"></param>
        private void DoReceiveData(DataPacketBase dataPacket)
        {

            if (dataPacket == null)
            {
                return;
            }

            lock (dataPacket)
            {
                OnReceiveData(this, dataPacket);
            }

        }


        /// <summary>
        /// 是否有接收信息的触发
        /// </summary>
        public bool HasReceiveDataHandle
        {
            get
            {
                return OnReceiveData != null;
            }
        }
        protected void OnRecCompleted(object sender, SocketAsyncEventArgs e)
        {
            DoSocketReceive(e);
            //DoCompleted(sender, e);

        }

        /// <summary>
        ///  关闭
        /// </summary>
        /// <param name="isHandleMessage">是否已经处理过消息通知</param>
        public virtual void Close(bool isHandleMessage=false)
        {
            //Connected = false;
            try
            {
                if (_heartmanager != null)
                {
                    _heartmanager.RemoveSocket(this);
                }
            }
            catch { }
            Socket socket = null;
            SocketAsyncEventArgs eventArgs = null;
            SocketAsyncEventArgs eventSendArgs = null;
            lock (_lokRootObject)
            {

                if (_bindSocket != null)
                {
                    socket = _bindSocket;
                    
                }
                _bindSocket = null;
                if (_dataManager != null)
                {
                    try
                    {
                        _dataManager.Close();

                    }
                    catch (Exception)
                    {
                    }


                }
                _dataManager = null;
                if (_recevieSocketAsync != null)
                {
                    eventArgs = _recevieSocketAsync;
                   
                    _recevieSocketAsync = null;
                }
                if (_sendSocketAsync != null)
                {
                    eventSendArgs = _sendSocketAsync;

                    _sendSocketAsync = null;
                }
            }
            if (socket != null)
            {
                try
                {

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Dispose();

                }
                catch { }
            }
            socket = null;
            if (eventArgs != null)
            {
                try
                {

                    eventArgs.Dispose();

                }
                catch (Exception)
                {
                }  
                
            }
            eventArgs = null;

            if (eventSendArgs != null)
            {
                try
                {

                    eventSendArgs.Dispose();

                }
                catch (Exception)
                {
                }

            }
            eventSendArgs = null;


            lock (_lokBuffdata)
            {
                NetByteBuffer bufferData = _bufferData;
                if (bufferData != null)
                {
                    try
                    {
                        bufferData.Dispose();
                    }
                    catch { }
                }
                bufferData = null;
                _bufferData = null;

                CloseTlsStream();
            }
            _message = null;
            
            _remoteIP = null;
            try
            {
                EventHandleClean.ClearAllEvents(this);
            }
            catch (Exception)
            {
            }
            if(!isHandleMessage) 
            {
                HandleClose("Auto Close");
            }
        }

        internal void HandleClose(String str)
        {
            if (ShowWarning)
            {
                _message.LogWarning(String.Format("{0}:{1}",HostIP , str));
            }
            if (OnClose != null)
            {
                OnClose(this);
            }


        }
        /// <summary>
        /// 发送跳包
        /// </summary>
        public virtual void SendHeard()
        {
            DataPacketBase data = _netProtocol.CreateDataPacket(0, false, null,false);
            data.IsHeart = true;
            SendPacket(data);
        }
       

        public void Log(string message)
        {
            if (_message != null)
            {
                _message.Log(message);
            }
        }

        public void LogError(string message)
        {
            if (_message != null)
            {
                _message.LogError(message);
            }
        }

        public void LogWarning(string message)
        {
            if (_message != null)
            {
                _message.LogWarning(message);
            }
        }

        public bool ShowLog
        {
            get
            {
                if (_message == null)
                {
                    return false;
                }
                return _message.ShowLog;
            }

        }
        public bool ShowError
        {
            get
            {
                if (_message == null)
                {
                    return false;
                }
                return _message.ShowError;
            }

        }
        public bool ShowWarning
        {
            get
            {
                if (_message == null)
                {
                    return false;
                }
                return _message.ShowWarning;
            }

        }
        private IConnectMessage _message;

        public IConnectMessage Messager
        {
            get
            {
                return _message;
            }
            set { _message = value; }
        }
        #endregion
    }
}
