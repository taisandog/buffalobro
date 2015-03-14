using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// 数值类型信息
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
        /// 类型名
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
        }

        private IList<DbType> _dbTypes;

        /// <summary>
        /// 对应的数据库类型
        /// </summary>
        public IList<DbType> DbTypes
        {
            get { return _dbTypes; }
        }

        private string _defaultValue;

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
        }

        private string _defaultNullValue;

        /// <summary>
        /// 默认带空的值
        /// </summary>
        public string DefaultNullValue
        {
            get { return _defaultNullValue; }
        }

        private int _dbLength;

        /// <summary>
        /// 默认数据长度
        /// </summary>
        public int DbLength
        {
            get { return _dbLength; }
        }

    }
}
