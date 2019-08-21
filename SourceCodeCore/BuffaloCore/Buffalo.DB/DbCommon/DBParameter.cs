using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.DbCommon
{
    public class DBParameter
    {
        public DBParameter() { }
        public DBParameter(string paramName, DbType type, object paramValue, ParameterDirection paramDir) 
        {
            _parameterName = paramName;
            _dbType = type;
            _value = paramValue;
            _direction = paramDir;
        }

        private string _valueName;

        /// <summary>
        /// 在SQL语句中的值名称
        /// </summary>
        internal string ValueName 
        {
            get 
            {
                return _valueName;
            }
            set 
            {
                _valueName = value;
            }
        }

        private DbType _dbType;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType DbType 
        { 
            get
            {
                return _dbType;
            }
            set 
            {
                _dbType = value;
            }
        }

        private ParameterDirection _direction = ParameterDirection.Input;
        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterDirection Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        private string _parameterName;
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParameterName
        {
            get
            {
                return _parameterName;
            }
            set
            {
                _parameterName = value;
            }
        }


        private object _value;

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
