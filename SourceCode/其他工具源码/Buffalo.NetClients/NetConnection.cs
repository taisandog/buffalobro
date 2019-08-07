using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Buffalo.NetClients
{
    public partial class NetConnection : IDisposable
    {
        /// <summary>
        /// �첽�����¼�
        /// </summary>
        protected SocketAsyncEventArgs _recevieSocketAsync;
        /// <summary>
        /// �첽����
        /// </summary>
        protected SocketAsyncEventArgs _sendSocketAsync;

        protected Socket _client;


        protected IPEndPoint _endPoint;
        
        
        public NetConnection(string ip,int port,ProtocolType type) 
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, type);
            _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            
        }
        
        public NetConnection(string ip, int port)
            :this(ip,port,ProtocolType.Tcp)
        {

        }

        public NetConnection(int port)
            : this("127.0.0.1", port, ProtocolType.Tcp)
        {

        }

        public NetConnection(Socket client)
        {
            this._client = client;
            
            _endPoint = (IPEndPoint)client.RemoteEndPoint;
        }
        public Socket Client
        {
            get { return _client; }

        }
        /// <summary>
        /// IP״̬
        /// </summary>
        public IPEndPoint EndPoint
        {
            get { return _endPoint; }

        }
        /// <summary>
        /// ������Ϣ��ָ��λ��
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int SendTo(byte[] content)
        {
            OpenConnection();
            int totalbytes = content.Length;
            //client.Send(BitConverter.GetBytes(totalbytes));
            int sbytes = 0;
            while (sbytes < totalbytes)
            {
                sbytes += _client.Send(content, sbytes, totalbytes - sbytes, 0);
            }
            _lastSendTime = DateTime.Now;
            return 0;
        }
        

        /// <summary>
        /// ���Դ�NAT�˿�
        /// </summary>
        /// <param name="ip">ip��ַ</param>
        /// <param name="data">����</param>
        /// <param name="type">��������</param>
        /// <returns></returns>
        public static NetConnection TryOpenNATPort(string ip, int port, byte[] data, ProtocolType type)
        {

            NetConnection conn = new NetConnection(ip, port, type);
            conn.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            conn.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            conn.SendTo(data);
            return conn;
        }

        /// <summary>
        /// ������Ϣ��ָ��λ��
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int SendTo(string message)
        {
            return SendTo(System.Text.Encoding.Default.GetBytes(message));
        }

        

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="buffer">����</param>
        /// <param name="len">����</param>
        /// <returns></returns>
        public int Receive(byte[] buffer, int size)
        {
            _lastReceiveTime = DateTime.Now;
            return _client.Receive(buffer, size, 0);
        }

        /// <summary>
        /// ��ȡ Socket ����Ϣ��
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static byte[] GetSocketMsg(Socket socket)
        {
            byte[] buffer = new byte[1024 * 1024];
            int b = socket.Receive(buffer, 0);
            if (b > 0)
                return buffer;
            else
                return null;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        public static byte[] GetSocketData(Socket socket)
        {
            MemoryStream stm=new MemoryStream(2048);
            byte[] buffer=new byte[512];
            int bytes=0;
            do
            {
                bytes = socket.Receive(buffer, bytes == 0 ? 0 : bytes, buffer.Length, 0);
                if (bytes > 0)
                {
                    stm.Write(buffer, 0, bytes);
                }
            } while (bytes > 0);

            return stm.ToArray();
        }

        private void OpenConnection() 
        {
            if (!_client.Connected) 
            {
                _client.SendTimeout = 3000;
                _client.Connect(_endPoint);
                _endPoint = _client.LocalEndPoint as IPEndPoint;
            }
        }

        /// <summary>
        /// �ر�����
        /// </summary>
        public void CloseConnection() 
        {
            if (_client!=null && _client.Connected)
            {
                
                _client.Close();
                _client = null;
                _endPoint = null;
            }
            if (_recevieSocketAsync != null) 
            {
                _recevieSocketAsync.Dispose();
            }
            if (_sendSocketAsync != null)
            {
                _sendSocketAsync.Dispose();
            }
            if (OnClose != null) 
            {
                OnClose(this);
            }
        }

        #region IDisposable ��Ա

        public void Dispose()
        {
            CloseConnection();
        }

        #endregion
        ~NetConnection()
        {
            CloseConnection();
        }

       

    }
}
