using Buffalo.Kernel;
using Buffalo.IOCP;
using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.IOCP.DataProtocol
{
    /// <summary>
    /// WebSocket适配器
    /// </summary>
    public class WebSocketAdapter : INetProtocol
    {
        /// <summary>
        /// 默认支持协议
        /// </summary>
        public const SslProtocols DefaultProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;

        public override int BufferLength
        {
            get { return 2048; }
        }

       

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="mess"></param>
        /// <returns>返回是否被处理</returns>
        protected virtual bool DoWSMessage(Message mess, WebSocketClientSocket socket, out DataPacketBase recPacket)
        {
            recPacket = null;
            OperType otype = mess.Header.Opcode;
            switch (otype)
            {
                case OperType.Binary:
                case OperType.Text:
                    recPacket = Format( mess.Payload);
                    return true;

                case OperType.Ping:
                    socket.SendPong();
                    return true;
                case OperType.Pong:
                case OperType.Row:
                    return true;
                case OperType.Close:
                    socket.Close(true);
                    socket.HandleClose("websocket user closed");
                    return true;
                default:
                    break;
            }
            return true;
        }


        /// <summary>
        /// 数据包空包长度
        /// </summary>
        public override int PACKET_LENGHT
        {
            get
            {
                return ProtocolDraft10.EmptyPacketLength;
            }
        }


        /// <summary>
        /// 将数据包输出为数组
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray(DataPacketBase packetBase)
        {
            WebSocketDataPacket packet = packetBase as WebSocketDataPacket;
            byte[] result=null;
            if (packet.Data == null)
            {
                result = new byte[0];
            }
            else
            {
                result = new byte[packet.Data.Length];
            }
       
            
            if (packet.Data != null)
            {
                result = ProtocolDraft10.PackageServerData(packet.Data, packet.WebSocketMessageType, packet.WebSocketMask);
                //Array.Copy(packet.Data, 0, result, 0, packet.Data.Length);
            }
            return result;
        }



        /// <summary>
        /// 根据数据格式化一个包,如果数据长度小于最小返回null
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>失败返回null</returns>
        public WebSocketDataPacket Format(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            WebSocketDataPacket packet = CreateDataPacket(0, false, data,false) as WebSocketDataPacket;

            return packet;

        }
        public const int HandshakeType = 0x100;

        private object _lokRootObject = new object();
        /// <summary>
        /// 判断数据是否合法,并进行一次数据解析
        /// </summary>
        /// <returns>是否进行下一次判断</returns>
        public override bool IsDataLegal(out DataPacketBase recPacket, ClientSocketBase socket)
        {
            NetByteBuffer buffer = socket.BufferData;
            DataManager dataManager = socket.DataManager;
            WebSocketHandshake hanshakeInfo = null;
            recPacket = null;
            WebSocketClientSocket wsocket = socket as WebSocketClientSocket;
            if(wsocket == null || !wsocket.Connected)
            {
                return false;
            }
            if (wsocket.HasWebSocketFirstTransfer) 
            {
                byte[] allData=new byte[buffer.Count];
                buffer.ReadBytes(0, allData, 0, allData.Length);
                if (wsocket.IsServerSocket)
                {
                    hanshakeInfo = wsocket.ServerHandshake(allData, 0, allData.Length);
                    if (hanshakeInfo.IsSuccess)
                    {
                        int retInt = socket.PutMessageEvent(HandshakeType, hanshakeInfo);
                        if (retInt != 0)
                        {
                            ProtocolDraft10.ResponseWebSocketHandShake(wsocket.HanshakeInfo, wsocket);//握手
                        }
                    }
                    else 
                    {
                        wsocket.Close();
                        return false;
                    }
                }
                else
                {
                    hanshakeInfo = wsocket.ClientHandshake(allData, 0, allData.Length);
                    if (!hanshakeInfo.IsSuccess)
                    {
                        socket.PutMessageEvent(HandshakeType, hanshakeInfo);
                        wsocket.Close();
                        return false;
                    }
                    socket.PutMessageEvent(HandshakeType, hanshakeInfo);
                }
                buffer.RemoveHeadBytes(allData.Length);
                //wsocket.HasWebSocketFirstTransfer = true;
                return true;
            }

            if (dataManager == null || buffer == null)
            {
                return false;
            }
            //检查是否达到最小包长度
            if (buffer.Count < PACKET_LENGHT)
            {
                return false;
            }
            bool ret = false;
            Message mess = ProtocolDraft10.AnalyzeClientData(buffer);
            if (mess == null)
            {
                return false;
            }

            WebSocketClientSocket wssocket = socket as WebSocketClientSocket;
            if (mess.Header.FIN)
            {
                if (!wssocket.IsWSBufferEmpty)
                {
                    NetByteBuffer msMessage = wssocket.BufferMessage;
                    msMessage.AppendBytes(mess.Payload, 0, mess.Payload.Length);
                    mess.Payload = msMessage.ToByteArray();
                    msMessage.Clear();
                }

                ret = DoWSMessage(mess, wssocket, out recPacket);
            }
            else
            {
                NetByteBuffer msMessage = wssocket.BufferMessage;
                msMessage.AppendBytes(mess.Payload, 0, mess.Payload.Length);
                ret = true;
            }


            return ret;
        }



        /// <summary>
        /// 创建数据包
        /// </summary>
        /// <returns></returns>
        public override DataPacketBase CreateDataPacket(object packetId, bool lost, byte[] data, bool verify)
        {
            WebSocketDataPacket packet = new WebSocketDataPacket((int)packetId, lost, data, this);
            packet.WebSocketMessageType = OperType.Binary;
            return packet;
        }

        /// <summary>
        /// 创建socket连接
        /// </summary>
        /// <returns></returns>
        public override ClientSocketBase CreateClientSocket(Socket socket, int maxSendPool = 15, int maxLostPool = 15,
            HeartManager heartManager = null, bool isServerSocket = false, SocketCertConfig certConfig = null)
        {
            WebSocketClientSocket ret = new WebSocketClientSocket(socket, maxSendPool, maxLostPool, heartManager, isServerSocket, this,certConfig);
            return ret;
        }
    }
}
