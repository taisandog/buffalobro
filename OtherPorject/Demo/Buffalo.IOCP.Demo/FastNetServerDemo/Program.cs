using Buffalo.IOCP;
using Buffalo.IOCP.DataProtocol;
using Newtonsoft.Json;
using System.Net.Security;
using System.Net.WebSockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace FastNetServerDemo
{
    internal class Program
    {
        private static ServerSocket _serverFast = null;
        private static ServerSocket _serverWebSocket = null;
        private static ServerSocket _serverWebSocketTLS = null;
        private static HeartManager _heart = null;
        private static ServerMessageLog _log = new ServerMessageLog();

        /// <summary>
        /// 默认协议
        /// </summary>
        private static FastNetAdapter _defaultNetAdapter = new FastNetAdapter();

        /// <summary>
        /// WebSocket协议
        /// </summary>
        private static WebSocketAdapter _wsNetAdapter = null;
        static void Main(string[] args)
        {
            _log.ShowWarning = true;
            _log.ShowError=true;
            _log.ShowLog = true;
            _heart = new HeartManager(20000, 5000, 1000,0, _log);
            _heart.NeedSendheart = false;
            _heart.StartHeart(500,10);
            _serverFast = ConnectFast(8586);
            _serverWebSocket = ConnectWebSocket(8588);
            _serverWebSocketTLS = ConnectWebSocketTLS(8589);
            Console.WriteLine("服务开启");
            string line = null;
            while (true)
            {
                Console.WriteLine("请输入命令");
                line = Console.ReadLine();
                if (string.Equals(line, "exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("正在停止");
                    break;
                }

            }
            _heart.StopHeart();
            _serverFast.Stop();
            _serverWebSocket.Stop();
            _serverWebSocketTLS.Stop();

        }

        /// <summary>
        /// 简易协议
        /// </summary>
        /// <returns></returns>
        private static ServerSocket ConnectFast(int port) 
        {
            ServerSocket server = new ServerSocket("0.0.0.0", port, _heart, _defaultNetAdapter, _log);

            server.OnAccept += Server_OnAccept;
            server.OnClose += Server_OnClose;
            server.OnReceiveData += server_OnReceiveData;
            server.OnMessage += Server_OnMessage;
            server.OnError += Server_OnError;
            server.Start();
            Console.WriteLine("Fast:0.0.0.0:" + port);
            return server;
        }
        /// <summary>
        /// 简易协议
        /// </summary>
        /// <returns></returns>
        private static ServerSocket ConnectWebSocket(int port)
        {
            _wsNetAdapter = new WebSocketAdapter();
            _wsNetAdapter.OnSendPacket += _wsNetAdapter_OnSendPacket;
            
            ServerSocket server = new ServerSocket("0.0.0.0", port, _heart, _wsNetAdapter, _log);


            server.OnAccept += Server_OnAccept;
            server.OnClose += Server_OnClose;
            server.OnReceiveData += server_OnReceiveData2;
            server.OnMessage += Server_OnMessage;
            server.OnError += Server_OnError;
            server.Start();
            Console.WriteLine("Websocket:ws://0.0.0.0:" + port);
            return server;
        }
        /// <summary>
        /// 简易协议
        /// </summary>
        /// <returns></returns>
        private static ServerSocket ConnectWebSocketTLS(int port)
        {
            _wsNetAdapter = new WebSocketAdapter();
            _wsNetAdapter.OnSendPacket += _wsNetAdapter_OnSendPacket;

            ServerSocket server = new ServerSocket("0.0.0.0", port, _heart, _wsNetAdapter, _log);

            X509Certificate2 cert = LoadCert();
             //string password = "";
             SslProtocols sslProtocols = WebSocketAdapter.DefaultProtocols;
           
            server.CertConfig = SocketCertConfig.CreateServerConfig(cert, false, sslProtocols, false, true,
                DoRemoteCertificateValidation, null, EncryptionPolicy.AllowNoEncryption);

            server.OnAccept += Server_OnAccept;
            server.OnClose += Server_OnClose;
            server.OnReceiveData += server_OnReceiveData2;
            server.OnMessage += Server_OnMessage;
            server.OnError += Server_OnError;
            server.Start();
            Console.WriteLine("Websocket:wss://0.0.0.0:" + port);
            return server;
        }

        private static X509Certificate2 LoadCert() 
        {
            string fileName = "App_Data/111.pfx";
            string password = "111111";
            X509Certificate2 cert = null;
            if (!string.IsNullOrWhiteSpace(fileName))
            {

                if (!string.IsNullOrWhiteSpace(password))
                {
                    cert = new X509Certificate2(fileName, password, X509KeyStorageFlags.Exportable);
                }
                else
                {
                    cert = new X509Certificate2(fileName);
                }

            }
            return cert;
        }
       

        private static void _wsNetAdapter_OnSendPacket(DataPacketBase packet)
        {
            WebSocketDataPacket dp = packet as WebSocketDataPacket;
            if (dp != null)
            {
                dp.WebSocketMessageType = OperType.Text;//强行把send(byte[])的类型改成Text类型
            }
        }
        static void server_OnReceiveData(ClientSocketBase socket, DataPacketBase data)
        {
            try
            {
                string error = null;
                byte[] bdata = data.Data;

                string mess=System.Text.Encoding.UTF8.GetString(bdata);
                if (_log.ShowLog)
                {
                    _log.Log("收到:" + mess);
                }
                FastClientSocket fSock = socket as FastClientSocket;
                fSock.Send(data.PacketID.ConvertTo<int>(),"服务器已收到:" +mess,null);
            }
            catch (Exception e)
            {
                _log.LogError(e.ToString());
            }
        }
        static void server_OnReceiveData2(ClientSocketBase socket, DataPacketBase data)
        {
            try
            {
                string error = null;
                byte[] bdata = data.Data;

                string mess = System.Text.Encoding.UTF8.GetString(bdata);
                if (_log.ShowLog)
                {
                    _log.Log("收到:" + mess);
                }
                WebSocketClientSocket fSock = socket as WebSocketClientSocket;
                fSock.Send("服务器已收到:" + mess);
            }
            catch (Exception e)
            {
                _log.LogError(e.ToString());
            }
        }
        static void Server_OnClose(ClientSocketBase clientSocket)
        {
            try
            {
                
                if (_log.ShowLog)
                {
                    _log.LogWarning("用户断开" );
                }
                
            }
            catch (Exception e)
            {
                _log.LogError(e.ToString());
            }

        }
        private static bool Server_OnMessage(ClientSocketBase clientSocket, int type, object message)
        {
            WebSocketClientSocket socket = clientSocket as WebSocketClientSocket;
            WebSocketHandshake handshake = message as WebSocketHandshake;
            if( handshake != null ) 
            {
                Console.WriteLine("握手地址:"+handshake.Url+"参数:"+JsonConvert.SerializeObject(handshake.Param));
                
            }
            
            return true;
        }
        private static void Server_OnError(ClientSocketBase clientSocket, Exception ex)
        {
            try
            {
                _log.LogError(ex.ToString());
            }
            catch (Exception ex1) { }
        }
        static void Server_OnAccept(ClientSocketBase clientSocket)
        {
            //if (FCRUnit.IsDebug) 
            //{
            //    Console.WriteLine("当前连接数:" + _heart.Clients.Count);
            //}
        }
        public static bool DoRemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {

            return true;//强行跳过验证合法性,测试时候没有域名和证书，需要此方法

        }
    }
}