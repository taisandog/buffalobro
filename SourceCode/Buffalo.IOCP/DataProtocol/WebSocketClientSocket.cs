using Buffalo.Kernel;
using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.IOCP.DataProtocol
{
    public class WebSocketClientSocket : ClientSocketBase
    {
        

        protected NetByteBuffer _msMessage = null;
        /// <summary>
        /// WebSocket分段数据的缓冲
        /// </summary>
        public NetByteBuffer BufferMessage
        {
            get
            {
                if (_msMessage == null)
                {
                    _msMessage = new NetByteBuffer(256);
                }
                return _msMessage;
            }
        }
        /// <summary>
        /// 判断缓冲区是否为空
        /// </summary>
        public bool IsWSBufferEmpty
        {
            get
            {
                return _msMessage == null || _msMessage.Count <= 0;
            }
        }

       

        private static WebSocketAdapter _defaultAdapter = new WebSocketAdapter();

        protected override INetProtocol CreateDefaultAdapter()
        {
            return _defaultAdapter;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="heartManager"></param>
        public WebSocketClientSocket(Socket socket, int maxSendPool, int maxLostPool, HeartManager heartManager, bool isServerSocket,
            INetProtocol netProtocol = null, SocketCertConfig certConfig = null)
        : base(socket, maxSendPool, maxLostPool, isServerSocket, heartManager, netProtocol,certConfig)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="heartManager"></param>
        public WebSocketClientSocket(Socket socket, HeartManager heartManager, bool isServerSocket=false, 
            INetProtocol netProtocol = null, SocketCertConfig certConfig = null)
            : this(socket, 15, 15, heartManager,isServerSocket, netProtocol, certConfig)
        {

        }

        private string _urihandshake=null;

        /// <summary>
        /// 创建websocket连接
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="heartManager">心跳管理</param>
        /// <param name="cert">证书</param>
        /// <param name="isServerSocket">是否服务器socket</param>
        /// <param name="maxSendPool">最大发池</param>
        /// <param name="maxLostPool">最大丢包管理池</param>
        /// <param name="netProtocol">协议</param>
        /// <param name="clientCertificates">客户端需要证书</param>
        /// <param name="enabledSslProtocols">SSL类型</param>
        /// <param name="checkCertificateRevocation">检查证书吊销</param>
        /// <param name="leaveInnerStreamOpen">保持流打开</param>
        /// <param name="userCertificateValidationCallback">用户证书验证回调</param>
        /// <param name="userCertificateSelectionCallback">用户证书选择回调</param>
        /// <param name="encryptionPolicy">加密策略</param>
        /// <returns></returns>
        public static WebSocketClientSocket CreateConnect(string url,  HeartManager heartManager,
           SocketCertConfig cert=null,bool isServerSocket=false, int maxSendPool=15, int maxLostPool=15,
            INetProtocol netProtocol = null,X509CertificateCollection clientCertificates=null,
            SslProtocols enabledSslProtocols= WebSocketAdapter.DefaultProtocols, bool checkCertificateRevocation = false,
             bool leaveInnerStreamOpen = false,
            RemoteCertificateValidationCallback userCertificateValidationCallback = null,
            LocalCertificateSelectionCallback userCertificateSelectionCallback = null,
            EncryptionPolicy encryptionPolicy = EncryptionPolicy.AllowNoEncryption) 
        {
            Uri uri = new Uri(url);
            if (string.Equals(uri.Scheme, "wss", StringComparison.CurrentCultureIgnoreCase)) 
            {
                if (cert == null)
                {
                    cert = SocketCertConfig.CreateClientConfig(uri.Host, clientCertificates, enabledSslProtocols,
                        checkCertificateRevocation, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback);
                }
            }
            else 
            {
                cert = null;
            }
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(uri.Host, uri.Port);
           
            WebSocketClientSocket _coon= new WebSocketClientSocket(socket, maxSendPool, maxLostPool, heartManager, isServerSocket, netProtocol, cert);
            _coon._urihandshake = uri.PathAndQuery;
            return _coon;
        }
        
        /// <summary>
        /// 创建证书
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="clientCertificates">证书</param>
        /// <param name="protocols">协议</param>
        /// <returns></returns>
        public static SocketCertConfig CreateCert(string url, 
            X509CertificateCollection clientCertificates=null, SslProtocols protocols= WebSocketAdapter.DefaultProtocols) 
        {
            Uri uri = new Uri(url);
            return SocketCertConfig.CreateClientConfig(uri.Host, null, protocols);
        }


        public override DataPacketBase Send(byte[] data)
        {
            WebSocketAdapter adp = _netProtocol as WebSocketAdapter;
             WebSocketDataPacket dp = _netProtocol.CreateDataPacket(0, false, data,false) as WebSocketDataPacket;
            _netProtocol.PutSendPacketEvent(dp);
            
            SendPacket(dp);
            return dp;
        }
        public override DataPacketBase Send(string data)
        {
            return SendText(data);
           
        }
        

        /// <summary>
        /// 是否Websocket进行了首次传输
        /// </summary>
        public bool HasWebSocketFirstTransfer
        {
            get { return _hanshakeInfo==null; }
        }
        /// <summary>
        /// 握手信息
        /// </summary>
        private WebSocketHandshake _hanshakeInfo;
        /// <summary>
        /// 握手信息
        /// </summary>
        public WebSocketHandshake HanshakeInfo 
        {
            get { return _hanshakeInfo; }
        }

        /// <summary>
        /// 服务器握手
        /// </summary>
        /// <param name="content"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public WebSocketHandshake ServerHandshake(byte[] content, int start, int count) 
        {
            WebSocketHandshake hanshakeInfo = new WebSocketHandshake(content, start, count);
            _hanshakeInfo = hanshakeInfo;
            hanshakeInfo.IsSuccess = hanshakeInfo.HandshakeContent.ContainsKey("Sec-WebSocket-Key");
            return hanshakeInfo;
            
        }
        /// <summary>
        /// 服务器握手
        /// </summary>
        /// <param name="content"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public WebSocketHandshake ClientHandshake(byte[] content, int start, int count)
        {
            WebSocketHandshake hanshakeInfo = new WebSocketHandshake(content, start, count);
            _hanshakeInfo = hanshakeInfo;
            hanshakeInfo.IsSuccess = hanshakeInfo.HandshakeContent.ContainsKey("Sec-WebSocket-Accept");
            return hanshakeInfo;
            
        }
        public override void SendHeard()
        {
            SendPing();
        }
        /// <summary>
        /// 回复心跳
        /// </summary>
        public void SendPong()
        {
            WebSocketDataPacket data =_netProtocol.CreateDataPacket(0, false, null, false) as WebSocketDataPacket;
            data.IsHeart = true;
            data.WebSocketMask = null;
            data.WebSocketMessageType = OperType.Pong;
            SendPacket(data);
        }
        /// <summary>
        /// 回复心跳
        /// </summary>
        public void SendPing()
        {
            WebSocketDataPacket data = _netProtocol.CreateDataPacket(0, false, null, false) as WebSocketDataPacket;
            data.IsHeart = true;
            data.WebSocketMask = null;
            data.WebSocketMessageType = OperType.Ping;
            SendPacket(data);
        }

        /// <summary>
        /// 发送握手
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="getParam">get内容参数</param>
        /// <param name="webSocketKey">指定的webSocketKey</param>
        public void SendHandShake(string host,string getParam=null, string webSocketKey=null)
        {
            if (string.IsNullOrWhiteSpace(webSocketKey))
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();//创建SHA1对象

                webSocketKey = Convert.ToBase64String(sha1.ComputeHash(Guid.NewGuid().ToByteArray()));
            }
            if (string.IsNullOrWhiteSpace(getParam)) 
            {
                getParam = _urihandshake;
            }
            if (string.IsNullOrWhiteSpace(getParam))
            {
                getParam = "/";
            }
            string handShakeStr = ProtocolDraft10.GetWebSocketHandShake(host, getParam, webSocketKey);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(handShakeStr);

            SendRaw(data);
        }
        /// <summary>
        /// 发送握手失败
        /// </summary>
        /// <param name="server">服务器(Server:)</param>
        /// <param name="httpError">http标记错误，例如:HTTP/1.1 500 ServerError</param>
        /// <param name="messparams">其他参数</param>
        /// <param name="message">Ws_err_msg错误信息</param>
        /// <param name="postData">其他内容</param>
        public void SendHandShakeFail(string server=null, string httpError=null, IDictionary<string, string> messparams = null,
            string message = null, string postData = null)
        {

            string handShakeStr = ProtocolDraft10.GetWebSocketHandShakeFail(server, httpError, messparams, message, postData);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(handShakeStr);

            SendRaw(data);
        }
        public DataPacketBase SendText(string text) 
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            WebSocketDataPacket dp = _netProtocol.CreateDataPacket(0, false, data,false) as WebSocketDataPacket;
            dp.WebSocketMessageType=OperType.Text;
            _netProtocol.PutSendPacketEvent(dp);
            SendPacket(dp);
            return dp;
        }

        public override void Close(bool isHandleMessage = false)
        {
            lock (_lokRootObject)
            {
                NetByteBuffer msbuff = _msMessage;
                
                try
                {
                    if (msbuff != null)
                    {
                        msbuff.Dispose();
                    }
                }
                catch { }
                _msMessage = null;

                if (_hanshakeInfo != null) 
                {
                    _hanshakeInfo.Dispose();
                }
                _hanshakeInfo = null;
            }
            base.Close(isHandleMessage);
        }
    }
}
