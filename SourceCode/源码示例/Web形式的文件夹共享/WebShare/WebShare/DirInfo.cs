using System;
using System.Collections.Generic;
using System.Text;

namespace WebShare
{
    /// <summary>
    /// 地址信息
    /// </summary>
    public class DirInfo
    {
        private string _text;
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private string _url;
        /// <summary>
        /// 连接
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        private long _length;
        /// <summary>
        /// 大小
        /// </summary>
        public long Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// 计算机记数法的文件大小
        /// </summary>
        public string LenString 
        {
            get 
            {
                decimal dlen = _length;
                int cur = 0;
                while (dlen > 1024) 
                {
                    dlen /= 1024;
                    cur++;
                }
                return dlen.ToString("0.00") + FileSize[cur];
            }
        }

        private static string[] FileSize ={ "B","KB","MB","GB","TB" };
    }
}
