using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Management;

namespace Buffalo.NetClients
{
    public delegate void ClientReceive(NetClientReceiveEvent arg);
    public class NetListener
    {
        public event ClientReceive OnClientReceive;
        IPEndPoint endPoint;

        
        Thread lisThd;
        Socket listener;
        
        /// <summary>
        /// 网络监听器
        /// </summary>
        /// <param name="port">监听端口</param>
        public NetListener(int port)
            : this(null, port)
        {
        }
        /// <summary>
        ///  网络监听器
        /// </summary>
        /// <param name="port">监听端口</param>
        public NetListener()
            : this(null, 0)
        {
        }
        public IPEndPoint EndPoint
        {
            get { return endPoint; }
            
        }
        
        /// <summary>
        ///  网络监听器
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">监听端口</param>
        /// <param name="synInvoker">需要线程同步的宿主(如：当前窗体)</param>
        public NetListener(string ipAddress, int port)
        {
            IPAddress ipAddr = null;
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddr = IPAddress.Parse(GetActiveIPAddress());
            }
            else
            {
                ipAddr = IPAddress.Parse(ipAddress);
            }
            endPoint = new IPEndPoint(ipAddr, port);
            
        }

        /// <summary>
        /// 获取活动IP
        /// </summary>
        /// <returns></returns>
        public static string GetActiveIPAddress()
        {
            
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string str = " ";
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    return (mo["IPAddress"] as string[])[0];
                }

            }
            return "";

        }
        /// <summary>
        /// 开始监听
        /// </summary>
        public void StarListen() 
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endPoint);
            endPoint = listener.LocalEndPoint as IPEndPoint;
            _running = true;
            lisThd = new Thread(new ThreadStart(MainListener));
            //lisThd.ApartmentState = ApartmentState.STA;
            lisThd.Start();
        }
        
        /// <summary>
        /// 关闭监听
        /// </summary>
        public void StopListener()
        {
            _running = false;
            if (listener != null)
            {
                listener.Close();
                listener = null;
                endPoint = null;
            }

            if (lisThd != null)
            {
                //lisThd.Abort();
                lisThd = null;
            }
            
        }

        private bool _running = false;

        /// <summary>
        /// 主监听线程
        /// </summary>
        private void MainListener() 
        {
            listener.Listen(0);
            Socket client = null;

            while (_running)
            {
                try
                {
                    client = listener.Accept();
                }
                catch (Exception ex) 
                {
                    if (_running)
                    {
                        throw ex;
                    }
                    else 
                    {
                        break;
                    }
                }
                
                ParameterizedThreadStart start = new ParameterizedThreadStart(DoAccept);
                Thread thdAccept = new Thread(start);
                thdAccept.Start(client);

            }
        }

        

        /// <summary>
        /// 当有信息进入时候
        /// </summary>
        private void DoAccept(object objClient) 
        {
            Socket client = objClient as Socket;

            if (client == null)
            {
                return;
            }
            
            //NetConnection conn = new NetConnection(client);
            //byte[] tmpByte = conn.Receive();
            NetClientReceiveEvent ent = new NetClientReceiveEvent(client);
            if (OnClientReceive != null)
            {
                
                    OnClientReceive(ent);
                
            }
            if (ent.AutoClose)
            {
                client.Close();
            }
        }
        ~NetListener() 
        {
            StopListener();
        }

        
    }

    /// <summary>
    /// 网络接收事件参数
    /// </summary>
    public class NetClientReceiveEvent 
    {

        public NetClientReceiveEvent(Socket connection) 
        {
            _connection = connection;
        }

        private Socket _connection;

        /// <summary>
        /// 连接
        /// </summary>
        public Socket Connection
        {
            get { return _connection; }
        }

        private bool _autoClose=true;

        /// <summary>
        /// 自动关闭连接
        /// </summary>
        public bool AutoClose
        {
            get { return _autoClose; }
            set { _autoClose = value; }
        }
        
    }
}

//**************示例*************

//***********需要控件同步(在窗体中)**********
//NetListener lis = new NetListener(9090, this);
//lis.StarListen();

//**********不需要同步*************
//NetListener lis = new NetListener(9090);