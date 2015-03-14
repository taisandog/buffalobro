using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.HttpServerModel
{
    /// <summary>
    /// 文件读写范围信息
    /// </summary>
    public class RangeInfo
    {
        private long _start;
        /// <summary>
        /// 开始位
        /// </summary>
        public long Start
        {
            get { return _start; }
            set { _start = value; }
        }

        private long _end;
        /// <summary>
        /// 结束位
        /// </summary>
        public long End
        {
            get { return _end; }
            set { _end = value; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public long Length 
        {
            get 
            {
                return _end - _start+1;
            }
        }
    }
}
