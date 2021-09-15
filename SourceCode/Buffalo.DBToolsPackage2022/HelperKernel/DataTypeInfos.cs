using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// ��ֵ������Ϣ
    /// </summary>
    public class DataTypeInfos
    {

        public DataTypeInfos(string typeName, IList<DbType> dbTypes,
            string defaultValue, string defaultNullValue, int dbLength) 
        {
            _typeName = typeName;
            _dbTypes = dbTypes;
            _defaultValue = defaultValue;
            _dbLength = dbLength;
            _defaultNullValue = defaultNullValue;
        }



        private string _typeName;

        /// <summary>
        /// ������
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
        }

        private IList<DbType> _dbTypes;

        /// <summary>
        /// ��Ӧ�����ݿ�����
        /// </summary>
        public IList<DbType> DbTypes
        {
            get { return _dbTypes; }
        }

        private string _defaultValue;

        /// <summary>
        /// Ĭ��ֵ
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
        }

        private string _defaultNullValue;

        /// <summary>
        /// Ĭ�ϴ��յ�ֵ
        /// </summary>
        public string DefaultNullValue
        {
            get { return _defaultNullValue; }
        }

        private int _dbLength;

        /// <summary>
        /// Ĭ�����ݳ���
        /// </summary>
        public int DbLength
        {
            get { return _dbLength; }
        }

    }
}
