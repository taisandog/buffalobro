using Buffalo.Kernel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Buffalo.IOCP.DataProtocol
{
    /*----------------------------------------------------------------
     * 创建者：wx57ba58dea2328 
     * 地址：https://blog.51cto.com/u_11990719/3108209
     *----------------------------------------------------------------*/

    /// <summary>
    /// WebSocket包
    /// </summary>
    public class ProtocolDraft10
    {
        private const String WebSocketKeyPattern = @"Sec\-WebSocket\-Key:\s+(?<key>.*)\r\n";
        private const String MagicKey = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        //private const Char charOne = '1';
        //private const Char charZero = '0';

        private readonly static Regex _reg = new Regex(WebSocketKeyPattern);
        #region Handshake
        /// <summary>
        /// 是否WebSocket握手
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool IsWebSocketHandShake(byte[] content, int start, int count)
        {
            if (content.Length < 50)
            {
                return false;
            }
            using (MemoryStream ms = new MemoryStream(content, start, count))
            {
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        
                        if (line.IndexOf("Sec-WebSocket-Key:", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 获取握手失败信息
        /// </summary>
        /// <param name="server">服务器(Server:)</param>
        /// <param name="httpError">http标记错误，例如:HTTP/1.1 500 ServerError</param>
        /// <param name="messparams">其他参数</param>
        /// <param name="message">Ws_err_msg错误信息</param>
        /// <param name="postData">其他内容</param>
        /// <returns></returns>
        public static string GetWebSocketHandShakeFail(string server,string httpError, IDictionary<string, string> messparams=null, string message=null,string postData=null)
        {
            StringBuilder sbRet = new StringBuilder();
            if (string.IsNullOrWhiteSpace(httpError)) 
            {
                httpError = "HTTP/1.1 500 ServerError";
            }
            sbRet.AppendLine(httpError);
            if (string.IsNullOrWhiteSpace(server)) 
            {
                server = "Kestrel";
            }
            sbRet.AppendLine("Server: " + server);
            sbRet.AppendLine("Content-Type: text/plain; charset=utf-8");
            if (!string.IsNullOrWhiteSpace(postData))
            {
                sbRet.AppendLine("Content-Length: " + postData.Length.ToString());
            }
            sbRet.AppendLine("Date: "+DateTime.Now);
            
            sbRet.AppendLine("Connection: keep-alive");
            sbRet.AppendLine("Sec-Websocket-Version: 13");
            if (!string.IsNullOrWhiteSpace(message))
            {
                sbRet.AppendLine("Ws_err_msg: " + System.Web.HttpUtility.UrlEncode(message));
            }
            sbRet.AppendLine("X-Content-Type-Options: nosniff");
            if (messparams != null)
            {
                foreach (KeyValuePair<string, string> pair in messparams)
                {
                    sbRet.AppendLine(pair.Key + ":" + System.Web.HttpUtility.UrlEncode(pair.Value));
                }
            }
            sbRet.AppendLine("");//必须有一行空行作为结尾

            if (!string.IsNullOrWhiteSpace(postData))
            {
                sbRet.AppendLine(postData);
                sbRet.AppendLine("");//必须有一行空行作为结尾
            }

            return sbRet.ToString();
        }

        /// <summary>
        /// 获取WebSocket握手信息
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="webSocketKey">websocket key</param>
        /// <returns></returns>
        public static string GetWebSocketHandShake(string host, string getParam, string webSocketKey) 
        {
            StringBuilder sbRet=new StringBuilder();
            sbRet.AppendLine("GET "+ getParam + " HTTP/1.1");
            sbRet.AppendLine("Host: " + host);
            sbRet.AppendLine("Connection: Upgrade");
            //sbRet.AppendLine("Pragma: no-cache");
            //sbRet.AppendLine("Cache-Control: no-cache");
            //sbRet.AppendLine("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.67 Safari/537.36");
            sbRet.AppendLine("Upgrade: websocket");
            //sbRet.AppendLine("Origin: http://"+ host);
            sbRet.AppendLine("Sec-WebSocket-Version: 13");
            //sbRet.AppendLine("Accept-Encoding: gzip, deflate, br");
            //sbRet.AppendLine("Accept-Language: zh-CN,zh;q=0.9,en;q=0.8,ja;q=0.7,zh-TW;q=0.6");

            if (string.IsNullOrWhiteSpace(webSocketKey)) 
            {
                webSocketKey=Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            }
            sbRet.AppendLine("Sec-WebSocket-Key: "+ webSocketKey);
            //sbRet.AppendLine("Sec-WebSocket-Extensions: permessage-deflate; client_max_window_bits");
            sbRet.AppendLine("" );//必须有一行空行作为结尾
           
            return sbRet.ToString();
        }
        private static KeyValuePair<string, string> CreateSecKeyAndSecWebSocketAccept()
        {
            string text = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            SHA1 sHA = SHA1.Create();
            return new KeyValuePair<string, string>(text, Convert.ToBase64String(sHA.ComputeHash(Encoding.ASCII.GetBytes(text + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"))));
        }

        /// <summary>
        /// 获取截取的值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetRegexValue(string request) 
        {
            string ret = null;
            Match m = _reg.Match(request);
            if (m.Value != "")
            {
                ret = Regex.Replace(m.Value, @"Sec\-WebSocket\-Key:(.*?)\r\n", "$1").Trim();
            }
            return ret;
        }
        
        /// <summary>
        /// 回应websocket握手
        /// </summary>
        /// <param name="inputData">握手内容</param>
        /// <param name="socket">socket</param>
        public static void ResponseWebSocketHandShake(WebSocketHandshake hanshakeInfo, ClientSocketBase socket)
        {
            //string inputData = System.Text.Encoding.UTF8.GetString(inputByteData);
            //string inputKey = null;
            //Match m = _reg.Match(inputData);
            //if (m.Value != "")
            //{
            //    inputKey = Regex.Replace(m.Value, @"Sec\-WebSocket\-Key:(.*?)\r\n", "$1").Trim();
            //}
            string inputKey = hanshakeInfo.HandshakeContent["Sec-WebSocket-Key"];
            SHA1 sha1 = SHA1.Create();

            string aKey = inputKey + MagicKey;
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(aKey);
            string accessKey = Convert.ToBase64String(sha1.ComputeHash(bytes));

            StringBuilder sbSend = new StringBuilder();
            sbSend.AppendLine("HTTP/1.1 101 Switching Protocols");
            sbSend.AppendLine("Upgrade: websocket");
            sbSend.AppendLine("Connection: Upgrade");

            sbSend.AppendLine("Sec-WebSocket-Accept: " + accessKey);
            sbSend.AppendLine();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(sbSend.ToString());

            socket.SendRaw(data);
            
        }


        #endregion

        #region Decode
        /// <summary>
        /// 数据包空包长度
        /// </summary>
        public const int EmptyPacketLength = 4;

        /// <summary>
        /// 对客户端发来的数据进行解析
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static Message AnalyzeClientData(NetByteBuffer data)
        {
            if (data.Count < EmptyPacketLength) 
            {
                return null;
            }
            byte[] buffer = new byte[14];
            if (data.Count >= buffer.Length)
            {
                data.ReadBytes(0, buffer, 0, buffer.Length);
            }
            else 
            {
                data.ReadBytes(0, buffer, 0, data.Count);
            }


            MessageHeader header = AnalyseHead(buffer);
            Message msg = new Message();
            msg.Header = header;
            if ((header.Payloadlen + (ulong)header.PayloadDataStartIndex) > (ulong)data.Count) 
            {
                return null;
            }
            
            byte[] payload=null;
            if (header != null)
            {
                payload = new byte[(int)header.Payloadlen];
                //Buffer.BlockCopy(data, start+header.PayloadDataStartIndex, payload, 0, payload.Length);
                data.ReadBytes(header.PayloadDataStartIndex, payload, 0, payload.Length);
                if (header.MASK)
                {
                    for (int i = 0; i < payload.Length; i++)
                    {
                        payload[i] = (byte)(payload[i] ^ header.Maskey[i % 4]);
                    }
                }
                msg.Payload = payload;
            }
            data.RemoveHeadBytes(header.PayloadDataStartIndex + payload.Length);

            return msg;
        }
        /// <summary>
        /// 解包包头
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static MessageHeader AnalyseHead(byte[] buffer)
        {
            MessageHeader header = new MessageHeader();
            header.FIN = (buffer[0] & 0x80) == 0x80 ;
            header.RSV1 = (buffer[0] & 0x40) == 0x40 ;
            header.RSV2 = (buffer[0] & 0x20) == 0x20 ;
            header.RSV3 = (buffer[0] & 0x10) == 0x10;

            if ((buffer[0] & 0xA) == 0xA)
                header.Opcode = OperType.Pong;
            else if ((buffer[0] & 0x9) == 0x9)
                header.Opcode = OperType.Ping;
            else if ((buffer[0] & 0x8) == 0x8)
                header.Opcode = OperType.Close;
            else if ((buffer[0] & 0x2) == 0x2)
                header.Opcode = OperType.Binary;
            else if ((buffer[0] & 0x1) == 0x1)
                header.Opcode = OperType.Text;
            else if ((buffer[0] & 0x0) == 0x0)
                header.Opcode = OperType.Row;

            header.MASK = (buffer[1] & 0x80) == 0x80 ;
            Int32 len = buffer[1] & 0x7F;
            if (len == 126)
            {
                header.Payloadlen = (UInt16)(buffer[2] << 8 | buffer[3]);
                if (header.MASK )
                {
                    header.Maskey = new byte[4];
                    Buffer.BlockCopy(buffer, 4, header.Maskey, 0, 4);
                    header.PayloadDataStartIndex = 8;
                }
                else
                    header.PayloadDataStartIndex = 4;
            }
            else if (len == 127)
            {
                //byte[] byteLen = new byte[8];
                //Buffer.BlockCopy(buffer, 4, byteLen, 0, 8);
                const int startIndex127 = 2 + 8-1;
                ulong clen = 0;
                int curNum = 0;
                for (int i = 0; i < 8; i++)
                {
                    clen += (ulong)buffer[startIndex127 - i] << curNum;
                    curNum += 8;
                }

                header.Payloadlen = clen;
                if (header.MASK)
                {
                    header.Maskey = new byte[4];
                    Buffer.BlockCopy(buffer, 10, header.Maskey, 0, 4);
                    header.PayloadDataStartIndex = 14;
                }
                else
                    header.PayloadDataStartIndex = 10;
            }
            else
            {
                if (header.MASK)
                {
                    header.Maskey = new byte[4];
                    Buffer.BlockCopy(buffer, 2, header.Maskey, 0, 4);
                    header.PayloadDataStartIndex = 6;
                }
                else
                {
                    header.PayloadDataStartIndex = 2;
                }
                header.Payloadlen = (ulong)len;
            }
            return header;
        }

        #endregion

        #region Encode
        /// <summary>
        ///  对要发送的数据进行编码一符合草案10的规则
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="hasMask"></param>
        /// <returns></returns>
        public static byte[] PackageServerData(byte[] data, OperType type, byte[] maskKey)
        {
            int dataLength = 0;
            if (data != null) 
            {
                dataLength = data.Length;
            }
            byte[] head = PacketHeader(type, dataLength, maskKey);
            if (maskKey!=null)
            {
                if (dataLength > 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = (byte)(data[i] ^ maskKey[i % 4]);
                    }
                }
            }
            byte[] result = new byte[head.Length + dataLength];
            Buffer.BlockCopy(head, 0, result, 0, head.Length);
            if (dataLength > 0)
            {
                Buffer.BlockCopy(data, 0, result, head.Length, dataLength);
            }
            return result;
        }
        private const byte byte80 = 0x80;
        public static byte[] DefaultMaskKey = new byte[] { 113, 105, 97, 110 };
        /// <summary>
        /// 打包头数据
        /// </summary>
        /// <param name="operType">类型</param>
        /// <param name="length">长度</param>
        /// <param name="maskKey">掩码(不使用则为null)</param>
        /// <returns></returns>
        private static byte[] PacketHeader(OperType operType, long length, byte[] maskKey)
        {
            byte byteHead = (byte)(byte80 | (byte)operType);
            byte[] byteLen=null;
            byte value = 0;
            if (length < 126)
            {
                byteLen = new byte[1];
                value = (byte)length;
               
                if (maskKey!=null)
                {
                    value = (byte)(byte80 | value);
                }
                byteLen[0] = value;
            }
            else if (length < 65535)
            {
                byteLen = new byte[3];
                //byteLen[0] = (byte)(byte80 | (byte)126);
                value = (byte)126;

                if (maskKey != null)
                {
                    value = (byte)(byte80 | value);
                }
                byteLen[0] = value;
                for (int i = 1; i < 3; i++)
                {
                    byteLen[i] = (byte)(length >> (8 * (2 - i)));
                }
            }
            else
            {
                byteLen = new byte[9];
                //byteLen[0] = (byte)(byte80 | (byte)127);
                value = (byte)127;

                if (maskKey != null)
                {
                    value = (byte)(byte80 | value);
                }
                byteLen[0] = value;
                for (int i = 1; i < 9; i++)
                {

                    byteLen[i] = (byte)(length >> (8 * (8 - i)));
                }
            }
            byte[] packet = null;
            if (maskKey != null)
            {
                packet = new byte[1 + byteLen.Length + maskKey.Length];
            }
            else 
            {
                packet = new byte[1 + byteLen.Length ];
            }
            packet[0] = byteHead;
            Buffer.BlockCopy(byteLen, 0, packet, 1, byteLen.Length);
            if (maskKey != null)
            {
                Buffer.BlockCopy(maskKey, 0, packet, 1 + byteLen.Length, maskKey.Length);
            }
            return packet;
        }

        #endregion
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperType:int
    {
        Pong= 0xA,
        Ping= 0x9,
        Close= 0x8,
        Binary= 0x2,
        Text= 0x1,
        Row = 0x0,
        None=-1
    }
    /// <summary>
    /// 消息包头
    /// </summary>
    public class MessageHeader 
    {
        public int PayloadDataStartIndex;
        public ulong Payloadlen;
        public bool MASK;
        public byte[] Maskey;
        public OperType Opcode;
        public bool FIN;
        public bool RSV1;
        public bool RSV2;
        public bool RSV3;
    }
    /// <summary>
    /// 消息
    /// </summary>
    public class Message 
    {
        /// <summary>
        /// 包头
        /// </summary>
        public MessageHeader Header;
        /// <summary>
        /// 内容
        /// </summary>
        public byte[] Payload;

        
    }
}
