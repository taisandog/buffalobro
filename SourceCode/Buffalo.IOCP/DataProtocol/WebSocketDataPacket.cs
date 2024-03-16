using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.IOCP.DataProtocol
{
    public class WebSocketDataPacket : DataPacketBase
    {
        private OperType _webSocketMessageType = OperType.Binary;
        /// <summary>
        /// WebSocket类型(Websocket协议有效)
        /// </summary>
        public OperType WebSocketMessageType
        {
            get { return _webSocketMessageType; }
            set { _webSocketMessageType = value; }
        }
        private byte[] _webSocketMask = null;
        /// <summary>
        /// WebSocket用来打包的掩码(Websocket协议有效)
        /// </summary>
        public byte[] WebSocketMask
        {
            get { return _webSocketMask; }
            set { _webSocketMask = value; }
        }
        public override void Dispose()
        {
            _webSocketMask = null;
            base.Dispose();
        }
        /// <summary>
        /// 构造，生成一个数据包，用来发送
        /// </summary>
        /// <param name="packetID">包编号</param>
        /// <param name="lost">是否验证丢失</param>
        /// <param name="verify">是否验证数据体</param>
        /// <param name="data">数据体</param>
        public WebSocketDataPacket(int packetID, bool lost, byte[] data,  INetProtocol netProtocol)
        {
            _netProtocol = netProtocol;
            if (_netProtocol == null)
            {
                throw new NullReferenceException("netProtocol can't be null");
            }
            if (packetID > 0)
            {
                this.PacketID = packetID.ToString();
            }

            this.IsLost = lost;
            this.MaxResend = _netProtocol.DefaultMaxResend;

            if (data != null)
            {
                this.Data = data;
                this.Length = data.Length + _netProtocol.PACKET_LENGHT;
                
            }
            else //空包
            {
                this.Data = null;
                this.Length = _netProtocol.PACKET_LENGHT;
                
            }
            
            //if (hasResetEvent)
            //{
            //    _event = new ManualResetEvent(false);
            //    ResetEvent();
            //}
        }
    }
}
