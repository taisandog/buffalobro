using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// 属性的关联数据库信息
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// 属性的关联数据库信息
        /// </summary>
        /// <param name="isPrimary">是否主键</param>
        /// <param name="paramName">对应的字段名</param>
        /// <param name="length">长度</param>
        /// <param name="isReadonly">是否只读</param>
        /// <param name="sqlType">数据库类型</param>
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
        /// 数据库长度
        /// </summary>
        public int Length
        {
            get { return _length; }
        }
        private bool _readonly;
        /// <summary>
        /// 是否只读属性
        /// </summary>
        public bool Readonly
        {
            get { return _readonly; }
        }


        private DbType _sqlType;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType SqlType
        {
            get { return _sqlType; }
        }

        private bool _isPrimary;
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimary
        {
            get { return _isPrimary; }
        }

        private string _paramName;
        /// <summary>
        /// 字段名
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
        }
        
    }
}
