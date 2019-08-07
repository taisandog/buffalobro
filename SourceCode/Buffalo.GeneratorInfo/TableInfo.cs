using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// ���ԵĹ������ݿ���Ϣ
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// ���ԵĹ������ݿ���Ϣ
        /// </summary>
        /// <param name="isPrimary">�Ƿ�����</param>
        /// <param name="paramName">��Ӧ���ֶ���</param>
        /// <param name="length">����</param>
        /// <param name="isReadonly">�Ƿ�ֻ��</param>
        /// <param name="sqlType">���ݿ�����</param>
        public TableInfo(bool isPrimary, string paramName,
            int length, bool isReadonly, DbType sqlType) 
        {
            _isPrimary = isPrimary;
            _paramName = paramName;
            _length = length;
            _readonly = isReadonly;
            _sqlType = sqlType;
        }
        
        private int _length;
        /// <summary>
        /// ���ݿⳤ��
        /// </summary>
        public int Length
        {
            get { return _length; }
        }
        private bool _readonly;
        /// <summary>
        /// �Ƿ�ֻ������
        /// </summary>
        public bool Readonly
        {
            get { return _readonly; }
        }


        private DbType _sqlType;

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DbType SqlType
        {
            get { return _sqlType; }
        }

        private bool _isPrimary;
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsPrimary
        {
            get { return _isPrimary; }
        }

        private string _paramName;
        /// <summary>
        /// �ֶ���
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
        }
        
    }
}
