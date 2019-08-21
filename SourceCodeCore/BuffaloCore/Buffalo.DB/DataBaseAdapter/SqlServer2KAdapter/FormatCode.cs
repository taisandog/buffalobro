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
        /// ��ʽ����
        /// </summary>
        public int Code
        {
            get 
            {
                return formatCode;
            }
        }

        /// <summary>
        /// �ַ�������
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
