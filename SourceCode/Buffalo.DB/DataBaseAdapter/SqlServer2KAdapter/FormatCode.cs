using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
{
    internal class FormatCode
    {
        private int formatCode;
        private int varcharLength;

        internal FormatCode(int formatCode, int varcharLength) 
        {
            this.formatCode = formatCode;
            this.varcharLength = varcharLength;
        }

        /// <summary>
        /// 格式代码
        /// </summary>
        public int Code
        {
            get 
            {
                return formatCode;
            }
        }

        /// <summary>
        /// 字符串长度
        /// </summary>
        public int VarcharLength 
        {
            get 
            {
                return varcharLength;
            }
        }
    }
}
