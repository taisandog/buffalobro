using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.DBTools
{

    public class FileEncodingInfo
    {
        /// <summary>
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="valNotBOM">是否验证没BOM的</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetEncodingType(string fileName,bool valNotBOM)
        {
            Encoding ret = null;
            if (!File.Exists(fileName)) 
            {
                
                return ret;
            }
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                ret = GetEncodingType(fs,valNotBOM);
            }
            return ret;
        }

        private static List<EncodingHead> _lstEncodingHead = InitEncodingHead();

        /// <summary>
        /// 初始化信息
        /// </summary>
        /// <returns></returns>
        private static List<EncodingHead> InitEncodingHead() 
        {
            List<EncodingHead> lst=new List<EncodingHead>();
            EncodingHead head = null;

            head = new EncodingHead(new byte[] { 0xFF, 0xFE, 0x00, 0x00 }, Encoding.UTF32);
            lst.Add(head);
            head = new EncodingHead(new byte[] { 0x00, 0x00, 0xFE, 0xFF }, Encoding.UTF32);
            lst.Add(head);

            head = new EncodingHead(new byte[] { 0xFF, 0xFE, 0x41 }, Encoding.UTF8);
            lst.Add(head);
            head = new EncodingHead(new byte[] { 0xEF, 0xBB, 0xBF }, Encoding.UTF8);
            lst.Add(head);

            head = new EncodingHead(new byte[] { 0xFE, 0xFF, 0x00 }, Encoding.BigEndianUnicode);
            lst.Add(head);
            head = new EncodingHead(new byte[] { 0xFF,0xFE, 0x00 }, Encoding.Unicode);
            lst.Add(head);
            return lst;

        }
        /// <summary>
        /// 通过给定的文件流，判断文件的编码类型
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetEncodingType(Stream fs,bool valNotBOM)
        {


            byte[] buffer = new byte[8];

            int read = fs.Read(buffer, 0, buffer.Length);
            if (valNotBOM)
            {
                byte[] bufferNotBOM = new byte[fs.Length];
                int readNotBOM = fs.Read(bufferNotBOM, 0, bufferNotBOM.Length);
                if (IsUTF8Bytes(bufferNotBOM)) 
                {
                    return Encoding.UTF8;
                }
            }
            

            return GetEncoding(buffer);

        }

        /// <summary>
        /// 是否此编码
        /// </summary>
        /// <param name="buffer">内容</param>
        /// <param name="headContent">文件头内容</param>
        /// <returns></returns>
        private static Encoding GetEncoding(byte[] filehead) 
        {
            if (filehead==null || filehead.Length<4) 
            {
                return null;
            }
            foreach (EncodingHead head in _lstEncodingHead) 
            {
                if (IsHeadEqual(filehead, head.Head)) 
                {
                    return head.Encoding;
                }
            }
            return null;
        }

        /// <summary>
        /// 判断是否标识头相等
        /// </summary>
        /// <param name="fileHead">文件的标识头</param>
        /// <param name="encodingHead">编码的标识头</param>
        /// <returns></returns>
        private static bool IsHeadEqual(byte[] fileHead, byte[] encodingHead) 
        {
            for (int i = 0; i < encodingHead.Length; i++)
            {
                if (fileHead[i] != encodingHead[i]) 
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;　 //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                return false;
            }
            return true;
        }
    }
        /// <summary>
    /// 编码头信息
    /// </summary>
    public class EncodingHead
    {
        private byte[] _head;

        public byte[] Head
        {
          get { return _head; }
          set { _head = value; }
        }
        private Encoding _encoding;

        public Encoding Encoding
        {
          get { return _encoding; }
          set { _encoding = value; }
        }

        /// <summary>
        /// 编码头跟编码的对应信息
        /// </summary>
        /// <param name="head">编码头</param>
        /// <param name="encoding">编码</param>
        public EncodingHead(byte[] head,Encoding encoding)
        {
             _head=head;
            _encoding=encoding;
        }
    }

}
