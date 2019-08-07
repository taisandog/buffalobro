using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.HttpServerModel
{
    /// <summary>
    /// �ļ���д��Χ��Ϣ
    /// </summary>
    public class RangeInfo
    {
        private long _start;
        /// <summary>
        /// ��ʼλ
        /// </summary>
        public long Start
        {
            get { return _start; }
            set { _start = value; }
        }

        private long _end;
        /// <summary>
        /// ����λ
        /// </summary>
        public long End
        {
            get { return _end; }
            set { _end = value; }
        }

        /// <summary>
        /// ����
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
