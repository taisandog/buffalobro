
using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Buffalo.IOCP
{
    public class ServerSocket
    {
        #region 事件
        /// <summary>
        /// 有新的连接
        /// </summary>
        public event SocketEvent OnAccept;

        /// <summary>
        /// 有连接断开
        /// 如果给接入的ClientSocket单独设置监听,将不会触发这个事件
        /// </summary>
        public event SocketEvent OnClose;

        /// <summary>
        /// 收到数据
        /// 如果给接入的ClientSocket单独设置监听,将不会触发这个事件
        /// </summary>
        public event DataEvent OnReceiveData;

        /// <summary>
        /// 通讯错误
        /// </summary>
        public event ClientSocketError OnError;

        /// <summary>
        /// 一般通知事件
        /// </summary>
        public event NormalMessageHandle OnMessage;
        #endregion
        #region 属性

        private IPEndPoint _listenIPEndPoint;
        /// <summary>
        /// 监听的IP
        /// </summary>
        public IPEndPoint ListenIPEndPoint
        {
            set { _listenIPEndPoint = value; }
            get { return _listenIPEndPoint; }
        }
        

        private AddressFamily _listenAddressFamily= AddressFamily.InterNetwork;
        /// <summary>
        /// 监听的地址类型
        /// </summary>
        public AddressFamily ListerAddressFamily
        {
            set { _listenAddressFamily = value; }
            get { return _listenAddressFamily; }
        }

        private SocketType _listenSocketType= SocketType.Stream;
        /// <summary>
        /// 监听流类型
        /// </summary>
        public SocketType ListenSocketType
        {
            get
            {
                return _listenSocketType;
            }
            set
            {
                _listenSocketType = value;
            }
        }
        private ProtocolType _listenProtocolType= ProtocolType.Tcp;
        /// <summary>
        /// 监听协议类型
        /// </summary>
        public ProtocolType ListenProtocolType
        {
            get
            {
                return _listenProtocolType;
            }
            set
            {
                _listenProtocolType = value;
            }
        }


        /// <summary>
        /// 运行中
        /// </summary>
        public bool Running
        {
            get
            {
                return _listenSocket != null;
            }
        }
        
        /// <summary>
        /// 数据重发时间(毫秒)
        /// </summary>
        public int TimeResend
        {
            get
            {
                if (_heardmanager == null) 
                {
                    return 0;
                }
                return _heardmanager.TimeResend;
            }
        }
       
        /// <summary>
        /// 连接重发时间(毫秒)
        /// </summary>
        public int TimeOut
        {
            get 
            {
                if (_heardmanager == null)
                {
                    return 0;
                }
                return _heardmanager.TimeOut;
            }
        }
        /// <summary>
        /// 监听的Socket
        /// </summary>
        private Socket _listenSocket;
        /// <summary>
        /// 监听的Socket
        /// </summary>
        public Socket ListenSocket 
        {
            get { return _listenSocket; }
        }
        /// <summary>
        /// socket的异步操作
        /// </summary>
        private SocketAsyncEventArgs _socketAsyncEventArgs;
        /// <summary>
        /// 心跳管理
        /// </summary>
        private HeartManager _heardmanager;
        /// <summary>
        /// 心跳管理
        /// </summary>
        public HeartManager Heardmanager
        {
            get
            {
                return _heardmanager;
            }
        }
        /// <summary>
        /// 是否本类创建的心跳管理
        /// </summary>
        private bool _createHeardmanager=false;

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
        /// 消息类
        /// </summary>
        private IConnectMessage _message;
        /// <summary>
        /// 消息类
        /// </summary>
        public IConnectMessage Message
        {
            get{ return _message; }
        }
        #endregion
        private SocketCertConfig _certConfig;
        /// <summary>
        /// 加密证书配置
        /// </summary>
        public SocketCertConfig CertConfig
        { 
            get { return _certConfig; }
            set { _certConfig = value; }
        }


        #region 方法
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="ip">监听地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeOut">超时时间</param>
        /// <param name="timeHreat">心跳</param>
        /// <param name="timeResend">数据重发时间</param>
        public ServerSocket(string ip, int port, int timeOut, int timeHreat, int timeResend, int timeCheckDisconnect, INetProtocol netProtocol=null, IConnectMessage message=null)
            :this(ip,port,new HeartManager(timeOut, timeHreat, timeResend, timeCheckDisconnect, message),netProtocol,message)
        {
            _createHeardmanager=true;
        }

        /// <summary>
        /// 创建socket
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private Socket CreateSocket()
        {
            Socket socket = new Socket(ListerAddressFamily, ListenSocketType, ListenProtocolType);

            socket.Bind(_listenIPEndPoint);

            return socket;
        }

        private const string AnyIPV4 = "0.0.0.0";
        private const string AnyIPV6 = "::";

        /// <summary>
        /// 创建地址
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private static IPEndPoint CreateIPEndPoint(string ip,int port) 
        {
            IPEndPoint ret = null;
            
            if (string.IsNullOrWhiteSpace(ip) || string.Equals(AnyIPV4, ip))
            {
                ret = new IPEndPoint(IPAddress.Any, port);
            }
            else if ( string.Equals(AnyIPV6, ip)) 
            {
                ret = new IPEndPoint(IPAddress.IPv6Any, port);
            }
            else
            {
                ret = new IPEndPoint(IPAddress.Parse(ip), port);
                
            }
            return ret;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="ip">监听地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeOut">超时时间</param>
        /// <param name="timeHreat">心跳</param>
        /// <param name="timeResend">数据重发时间</param>
        public ServerSocket(String ip, int port, HeartManager heardmanager, INetProtocol netProtocol = null, IConnectMessage message = null)
            :this(CreateIPEndPoint(ip,port),heardmanager,netProtocol,message)
        {
            
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="ip">监听地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeOut">超时时间</param>
        /// <param name="timeHreat">心跳</param>
        /// <param name="timeResend">数据重发时间</param>
        public ServerSocket(IPEndPoint listenIPEndPoint, HeartManager heardmanager, INetProtocol netProtocol = null, IConnectMessage message = null)
        {
            _listenIPEndPoint = listenIPEndPoint;
            _message = message;

            _heardmanager = heardmanager;
            _netProtocol = netProtocol;
            if (_netProtocol == null)
            {
                throw new NullReferenceException("netProtocol can't be null");
            }
        }
        /// <summary>
        /// 开启监听服务
        /// </summary>
        /// <param name="backlog">新连接队列的长度限制</param>
        /// <param name="listenSocket">使用此Socket进行监听</param>
        public void Start(int backlog = 128,Socket listenSocket=null)
        {
            if (listenSocket != null)
            {
                _listenSocket = listenSocket;
            }
            else
            {
                _listenSocket = CreateSocket();
            }

            _listenSocket.Listen(backlog);
            
            _socketAsyncEventArgs = new SocketAsyncEventArgs();
            _socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AsyncAccept);
            if (!_listenSocket.AcceptAsync(_socketAsyncEventArgs))
            {
                AsyncAccept(listenSocket, _socketAsyncEventArgs);
            }
            if (Util.HasShowLog(_message))
            {
                _message.Log("Server start.");
            }
            if (_heardmanager != null && !_heardmanager.Running)
            {
                _heardmanager.StartHeart();
            }
        }

       
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (_socketAsyncEventArgs != null)
            {
                EventHandleClean.ClearAllEvents(_socketAsyncEventArgs);
                _socketAsyncEventArgs.Dispose();

            }
            _socketAsyncEventArgs = null;
            if (_listenSocket != null)
            {
                
                try
                {

                    _listenSocket.Shutdown(SocketShutdown.Both);

                }
                catch
                {

                }
                try
                {
                    _listenSocket.Close();
                    
                }
                catch
                {

                }
                EventHandleClean.ClearAllEvents(_listenSocket);
            }
            if(_createHeardmanager && _heardmanager != null) 
            {
                _heardmanager.StopHeart();
            }
            //_heardmanager = null;
            _listenSocket = null;
            EventHandleClean.ClearAllEvents(_socketAsyncEventArgs);
            
            

            EventHandleClean.ClearAllEvents(this);
            if (Util.HasShowLog(_message))
            {
                _message.Log("Server Stop.");
            }
            
        }

        /// <summary>
        /// 异步处理接入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AsyncAccept(object sender, SocketAsyncEventArgs e)
        {
            
            if (!Running)
            {
                try
                {
                    
                    e.AcceptSocket.Close();
                }
                catch { }
                return;
            }
            ClientSocketBase clientSocket = null;
            try
            {
               
                clientSocket = _netProtocol.CreateClientSocket(e.AcceptSocket, 15, 15, _heardmanager,true,_certConfig);
                

                
                clientSocket.OnClose += Client_OnClose;
                clientSocket.OnError += ClientSocket_OnError;
                if (OnMessage != null)
                {
                    clientSocket.OnMessage += OnMessage;
                }

                if (OnAccept != null)
                {
                    OnAccept(clientSocket);
                }
                clientSocket.AddReceiveDataHandle(Client_OnReceiveData);
                
            }
            catch(Exception ex) 
            {
                if (clientSocket != null)
                {
                    try
                    {
                        clientSocket.Close();
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        e.AcceptSocket.Close();
                    }
                    catch { }
                }
                if (Util.HasShowError(_message))
                {
                    _message.LogError(ex.ToString());
                }
            }
            finally
            {
                _socketAsyncEventArgs.AcceptSocket = null;
                if (!_listenSocket.AcceptAsync(_socketAsyncEventArgs)) 
                {
                    AsyncAccept(sender, _socketAsyncEventArgs);
                }
            }
        }

        //private bool ClientSocket_OnMessage( ClientSocketBase clientSocket, int type, object message)
        //{
        //    if (OnMessage != null)
        //    {
        //        return OnMessage(clientSocket, type, message);
        //    }
        //    return false;
        //}

        private void ClientSocket_OnError(ClientSocketBase clientSocket, Exception ex)
        {
            if (OnError != null)
            {
                OnError(clientSocket, ex);
            }
        }

        void Client_OnReceiveData(ClientSocketBase clientSocket,DataPacketBase data)
        {
            if (OnReceiveData != null)
            {
                OnReceiveData(clientSocket, data);
            }
        }

        void Client_OnClose(ClientSocketBase clientSocket)
        {
            if (OnClose != null)
            {
                OnClose(clientSocket);
            }
            
        }

    }
        #endregion
}
