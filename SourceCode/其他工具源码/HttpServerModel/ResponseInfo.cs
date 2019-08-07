using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.Kernel.HttpServerModel
{
    /// <summary>
    /// 回传信息
    /// </summary>
    public class ResponseInfo:IDisposable
    {
        private MemoryStream _stm;
        private Encoding _encoding;
        private Dictionary<string, string> _dicHeader = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 返回的头
        /// </summary>
        public Dictionary<string, string> Header
        {
            get { return _dicHeader; }
        }
        public ResponseInfo(Encoding encoding) 
        {
            _mimeType="text/html";
            _stm = new MemoryStream();
            _encoding = encoding;
        }

        private string _mimeType = null;

        /// <summary>
        /// 传送类型
        /// </summary>
        public string MimeType
        {
            get { return _mimeType; }
            set { _mimeType = value; }
        }

        private int _statusCode=200;
        /// <summary>
        /// 返回的状态码
        /// </summary>
        public int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        /// <summary>
        /// 返回的内容
        /// </summary>
        public byte[] ResponseContent 
        {
            get 
            {
                return _stm.ToArray();
            }
        }

        private long _length=0;

        /// <summary>
        /// 返回的内容
        /// </summary>
        public long Length
        {
            get
            {
                return _length;
            }
            set 
            {
                _length = value;
            }
        }
        private long _rangeLength = 0;
        /// <summary>
        /// 分段文件总长度
        /// </summary>
        public long RangeLength
        {
            get
            {
                return _rangeLength;
            }
            set
            {
                _rangeLength = value;
            }
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="content"></param>
        public void Write(byte[] content) 
        {
            _stm.Write(content, 0, content.Length);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="content"></param>
        public void Write(string content)
        {
            byte[] byteStr=_encoding.GetBytes(content);
            Write(byteStr);
        }
        private object _userTag;

        /// <summary>
        /// 用户标记
        /// </summary>
        public object UserTag
        {
            get { return _userTag; }
            set { _userTag = value; }
        }

        /// <summary>
        /// 计算文件读取范围
        /// </summary>
        /// <param name="rangeString"></param>
        /// <param name="fileLen"></param>
        /// <returns></returns>
        public static List<RangeInfo> GetRange(string rangeString, long fileLen) 
        {
            List<RangeInfo> lst = new List<RangeInfo>();
            
            string[] bytesPart = rangeString.Split('=');
            if (bytesPart.Length >= 2)
            {
                string strAllValue = bytesPart[1];
                string[] values = strAllValue.Split(',');
                long start=0;
                long end = 0;
                foreach (string val in values) 
                {
                    string[] valItems = val.Split('-');
                    
                    RangeInfo info = new RangeInfo();

                    if (valItems.Length > 0) 
                    {
                        if (!long.TryParse(valItems[0], out start)) 
                        {
                            start = 0;
                        }
                    }
                    if (valItems.Length > 1)
                    {
                        if (!long.TryParse(valItems[1], out end))
                        {
                            end = fileLen - 1;
                        }
                    }
                    info.Start = start;
                    info.End = end;
                    lst.Add(info);
                }
            }
            if (lst.Count == 0) 
            {
                RangeInfo info = new RangeInfo();
                info.End = fileLen - 1;
                lst.Add(info);
            }
            return lst;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (_stm != null) 
            {
                _stm.Close();
            }
        }

        #endregion
    }
}
