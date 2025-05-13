using System.Text;
using System.Net;
using System.Reflection;

namespace Buffalo.IOCP
{
    /// <summary>
    /// 工具辅助类
    /// </summary>
    public class Util
    {
        /// <summary>
        /// CRC值
        /// </summary>
        private readonly static byte[] CRC = new byte[]{0x00,0x91,0xe3,0x72,0x07,0x96,0xe4,0x75, 
                                                      0x0e,0x9f,0xed,0x7c,0x09,0x98,0xea,0x7b, 
                                                      0x1c,0x8d,0xff,0x6e,0x1b,0x8a,0xf8,0x69, 
                                                      0x12,0x83,0xf1,0x60,0x15,0x84,0xf6,0x67, 
                                                      0x38,0xa9,0xdb,0x4a,0x3f,0xae,0xdc,0x4d, 
                                                      0x36,0xa7,0xd5,0x44,0x31,0xa0,0xd2,0x43, 
                                                      0x24,0xb5,0xc7,0x56,0x23,0xb2,0xc0,0x51, 
                                                      0x2a,0xbb,0xc9,0x58,0x2d,0xbc,0xce,0x5f, 
                                                      0x70,0xe1,0x93,0x02,0x77,0xe6,0x94,0x05, 
                                                      0x7e,0xef,0x9d,0x0c,0x79,0xe8,0x9a,0x0b, 
                                                      0x6c,0xfd,0x8f,0x1e,0x6b,0xfa,0x88,0x19, 
                                                      0x62,0xf3,0x81,0x10,0x65,0xf4,0x86,0x17, 
                                                      0x48,0xd9,0xab,0x3a,0x4f,0xde,0xac,0x3d, 
                                                      0x46,0xd7,0xa5,0x34,0x41,0xd0,0xa2,0x33, 
                                                      0x54,0xc5,0xb7,0x26,0x53,0xc2,0xb0,0x21, 
                                                      0x5a,0xcb,0xb9,0x28,0x5d,0xcc,0xbe,0x2f, 
                                                      0xe0,0x71,0x03,0x92,0xe7,0x76,0x04,0x95, 
                                                      0xee,0x7f,0x0d,0x9c,0xe9,0x78,0x0a,0x9b, 
                                                      0xfc,0x6d,0x1f,0x8e,0xfb,0x6a,0x18,0x89, 
                                                      0xf2,0x63,0x11,0x80,0xf5,0x64,0x16,0x87, 
                                                      0xd8,0x49,0x3b,0xaa,0xdf,0x4e,0x3c,0xad, 
                                                      0xd6,0x47,0x35,0xa4,0xd1,0x40,0x32,0xa3, 
                                                      0xc4,0x55,0x27,0xb6,0xc3,0x52,0x20,0xb1, 
                                                      0xca,0x5b,0x29,0xb8,0xcd,0x5c,0x2e,0xbf, 
                                                      0x90,0x01,0x73,0xe2,0x97,0x06,0x74,0xe5, 
                                                      0x9e,0x0f,0x7d,0xec,0x99,0x08,0x7a,0xeb, 
                                                      0x8c,0x1d,0x6f,0xfe,0x8b,0x1a,0x68,0xf9, 
                                                      0x82,0x13,0x61,0xf0,0x85,0x14,0x66,0xf7, 
                                                      0xa8,0x39,0x4b,0xda,0xaf,0x3e,0x4c,0xdd, 
                                                      0xa6,0x37,0x45,0xd4,0xa1,0x30,0x42,0xd3, 
                                                      0xb4,0x25,0x57,0xc6,0xb3,0x22,0x50,0xc1, 
                                                      0xba,0x2b,0x59,0xc8,0xbd,0x2c,0x5e,0xcf};

        

       
        /// <summary>
        /// 生成CRC
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static byte CreateCRC(byte[] data, int length)
        {
            byte result = 0;
            for (int i = 0; i < length; i++)
            {
                result = CRC[(byte)(result + data[i])];
            }
            return result;
        }

        /// <summary>
        /// 生成CRC
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static byte CreateCRC(params byte[] data)
        {
            byte result = 0;
            for (int i = 0; i < data.Length; i++)
            {
                result = CRC[(byte)(result + data[i])];
            }
            return result;
        }
        /// <summary>
        /// 是否需要显示日志
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool HasShowLog(IConnectMessage message)
        {
            return message != null && message.ShowLog;
        }
        /// <summary>
        /// 是否需要显示警告
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool HasShowWarning(IConnectMessage message)
        {
            return message != null && message.ShowWarning;
        }
        /// <summary>
        /// 是否需要显示警告
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool HasShowError(IConnectMessage message)
        {
            return message != null && message.ShowError;
        }
        /// <summary>
        /// 4字节转换为整型，高位在前
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ByteToInt4(byte[] b)
        {
            int result = 0;
            result += (b[3] & 0xFF);
            result += (b[2] & 0xFF) << 8;
            result += (b[1] & 0xFF) << 16;
            result += (b[0] & 0xFF) << 24;
            return result;
        }

        /// <summary>
        /// 整型转换为4字节，高位在前
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] IntToByte4(int i)
        {
            byte[] result = new byte[4];
            result[3] = (byte)(i & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[0] = (byte)((i >> 24) & 0xFF);
            return result;
        }

        /// <summary>
        /// 输出比例字符 0xae00cf => "AE00CF "
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2") + " ");
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns></returns>
        public static string[] GetLocalIPv4()
        {            
            IPAddress[] addr = Dns.GetHostAddresses(Dns.GetHostName());
            string ip = "";
            for (int i = 0; i < addr.Length; i++)
            {
                if (addr[i].ToString().IndexOf(":") == -1)
                {
                    ip += addr[i].ToString() + "|";
                }              
            }
            return ip.Substring(0,ip.Length-1).Split('|');
        }
    }
}
