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
    /// <summary>
    /// WebSocket工具
    /// </summary>
    public class WebSocketUnit
    {
        private const string WSGUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private readonly static Regex _reg = new Regex(@"Sec\-WebSocket\-Key:(.*?)\r\n");

        /// <summary>
        /// WebSocket工具
        /// </summary>
        private WebSocketUnit()
        {

        }

        /// <summary>
        /// 是否WebSocket握手
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool IsWebSocketHandShake(byte[] content,int start,int count)
        {
            if (content.Length < 150)
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



        ///// <summary>
        ///// 回应websocket握手
        ///// </summary>
        ///// <param name="inputData">握手内容</param>
        ///// <param name="socket">socket</param>
        //public static void ResponseWebSocketHandShake(byte[] inputByteData, Socket socket)
        //{
        //    string inputData = System.Text.Encoding.UTF8.GetString(inputByteData);
        //    string inputKey = null;
        //    Match m = _reg.Match(inputData);
        //    if (m.Value != "")
        //    {
        //        inputKey = Regex.Replace(m.Value, @"Sec\-WebSocket\-Key:(.*?)\r\n", "$1").Trim();
        //    }
        //    SHA1 sha1 = SHA1.Create();

        //    string aKey = inputKey + WSGUID;
        //    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(aKey);
        //    string accessKey = Convert.ToBase64String(sha1.ComputeHash(bytes));

        //    StringBuilder sbSend = new StringBuilder();
        //    sbSend.AppendLine("HTTP/1.1 101 Switching Protocols");
        //    sbSend.AppendLine("Upgrade: websocket");
        //    sbSend.AppendLine("Connection: Upgrade");

        //    sbSend.AppendLine("Sec-WebSocket-Accept: " + accessKey);
        //    sbSend.AppendLine();
        //    byte[] data = System.Text.Encoding.ASCII.GetBytes(sbSend.ToString());

        //    //socket.Send(data);
        //    SocketAsyncEventArgs sendSocketAsync = new SocketAsyncEventArgs();
        //    sendSocketAsync.AcceptSocket = socket;
        //    sendSocketAsync.SetBuffer(data, 0, data.Length);
        //    socket.SendAsync(sendSocketAsync);
        //}

        #region 处理接收的数据
        /// <summary>
        /// 处理接收的数据
        /// </summary>
        /// <param name="recBytes"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] AnalyzeClientData(byte[] recBytes,int start, int length)
        {
            // 如果有数据则至少包括3位
            if (length < 2)
            {
                return null;
            }

            // 判断是否为结束针
            bool IsEof = (recBytes[start] >> 7) > 0;
            // 暂不处理超过一帧的数据
            if (!IsEof)
            {
                return null;
            }
            start++;
            // 是否包含掩码
            bool hasMask = (recBytes[start] >> 7) > 0;
            // 不包含掩码的暂不处理
            if (!hasMask)
            {
                return null;
            }
            // 获取数据长度
            UInt64 mPackageLength = (UInt64)recBytes[start] & 0x7F;
            start++;
            // 存储4位掩码值
            byte[] Masking_key = new byte[4];
            // 存储数据
            byte[] mDataPackage;
            if (mPackageLength == 126)
            {
                // 等于126 随后的两个字节16位表示数据长度
                mPackageLength = (UInt64)(recBytes[start] << 8 | recBytes[start + 1]);
                start += 2;
            }
            if (mPackageLength == 127)
            {
                // 等于127 随后的八个字节64位表示数据长度
                mPackageLength = (UInt64)(recBytes[start] << (8 * 7) | recBytes[start] << (8 * 6) | recBytes[start] << (8 * 5) | recBytes[start] << (8 * 4) | recBytes[start] << (8 * 3) | recBytes[start] << (8 * 2) | recBytes[start] << 8 | recBytes[start + 1]);
                start += 8;
            }
            mDataPackage = new byte[mPackageLength];
            for (UInt64 i = 0; i < mPackageLength; i++)
            {
                mDataPackage[i] = recBytes[i + (UInt64)start + 4];
            }
            Buffer.BlockCopy(recBytes, start, Masking_key, 0, 4);
            for (UInt64 i = 0; i < mPackageLength; i++)
            {
                mDataPackage[i] = (byte)(mDataPackage[i] ^ Masking_key[i % 4]);
            }
            return mDataPackage;
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 把发送给客户端消息打包处理（拼接上谁什么时候发的什么消息）
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="message">Message.</param>
        public static byte[] PackageServerData(byte[] sendData)
        {
            byte[] content = null;
           //sendData = System.Text.Encoding.UTF8.GetBytes(Convert.ToBase64String(sendData));
            if (sendData.Length < 126)
            {
                content = new byte[sendData.Length + 2];
                content[0] = 0x81;
                content[1] = (byte)sendData.Length;
                Buffer.BlockCopy(sendData, 0, content, 2, sendData.Length);
            }
            else if (sendData.Length < 0xFFFF)
            {
                content = new byte[sendData.Length + 4];
                content[0] = 0x81;
                content[1] = 126;
                content[2] = (byte)(sendData.Length & 0xFF);
                content[3] = (byte)(sendData.Length >> 8 & 0xFF);
                Buffer.BlockCopy(sendData, 0, content, 4, sendData.Length);
            }
            return content;
        }
        #endregion

    }
}
