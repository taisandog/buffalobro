
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Buffalo.IOCP.DataProtocol
{
    public delegate void OnSendPacketHandle(DataPacketBase packet);
    /// <summary>
    /// 网络协议
    /// </summary>
    public abstract class INetProtocol : IConnectMessage
    {
        private IConnectMessage _message;

        public IConnectMessage Messager
        {
            get
            {
                return _message;
            }
            set { _message = value; }
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
        public event OnSendPacketHandle OnSendPacket;
        /// <summary>
        /// 进行发包前的事件
        /// </summary>
        /// <param name="packet"></param>
        internal void PutSendPacketEvent(DataPacketBase packet) 
        {
            if(OnSendPacket != null) 
            {
                OnSendPacket(packet);
            }
        }

        /// <summary>
        /// 数据包空包长度
        /// </summary>
        public abstract int PACKET_LENGHT
        {
            get;
        }

        /// <summary>
        /// 将数据包输出为数组
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToArray(DataPacketBase packet);


        /// <summary>
        /// 判断数据是否合法,并进行一次数据解析
        /// </summary>
        /// <returns>是否进行下一次判断</returns>
        public abstract bool IsDataLegal(out DataPacketBase recPacket, ClientSocketBase socket);
        /// <summary>
        /// 创建数据包
        /// </summary>
        /// <returns></returns>
        public abstract DataPacketBase CreateDataPacket(object packetId, bool lost, byte[] data, bool verify);

        /// <summary>
        /// 创建socket连接
        /// </summary>
        /// <param name="socket">连接</param>
        /// <param name="maxSendPool">最大发送池</param>
        /// <param name="maxLostPool">最大重发池</param>
        /// <param name="heartManager">心跳管理</param>
        /// <param name="isServerSocket">是否监听创建连接</param>
        /// <returns></returns>
        public abstract ClientSocketBase CreateClientSocket(Socket socket, int maxSendPool = 15, int maxLostPool = 15,
            HeartManager heartManager = null, bool isServerSocket = false, SocketCertConfig certConfig = null);

        
        /// <summary>
        /// 是否空包id
        /// </summary>
        /// <param name="packetId"></param>
        /// <returns></returns>
        public virtual bool IsEmptyPacketId(string packetId) 
        {
            return string.IsNullOrWhiteSpace(packetId);
        }
        /// <summary>
        /// 缓冲长度
        /// </summary>
        public abstract int BufferLength { get; }
        /// <summary>
        /// 默认最大重发次数
        /// </summary>
        public virtual int DefaultMaxResend 
        {
            get 
            {
                return 5;
            }
        }
    }
}
