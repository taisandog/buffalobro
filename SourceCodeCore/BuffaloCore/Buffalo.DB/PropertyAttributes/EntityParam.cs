using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.Kernel.Commons;
using Buffalo.Kernel;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DBCheckers;
namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// 字段信息
    /// </summary>
    public class EntityParam:System.Attribute
    {
        private string _paramName;
        private string _propertyName;
        private DbType _sqlType;
        private EntityPropertyType _propertyType;
        private long _length;
        private bool _allowNull;
        private string _fieldName;
        private bool _readonly;
        private string _sequenceName;
        private string _defaultValue;

        

        /// <summary>
        /// 实体字段标识
        /// </summary>
        public EntityParam() 
        {
            _readonly = false;

        }
        /// <summary>
        /// 实体字段标识
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="paramName">数据库字段名</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="sqlType">Sql类型</param>
        /// <param name="propertyType">属性类型</param>
        /// <param name="description">备注</param>
        /// <param name="length">长度</param>
        /// <param name="allowNull">允许空</param>
        /// <param name="isReadOnly">只读</param>
        /// <param name="defaultValue">默认值</param>
        public EntityParam(string fieldName, string paramName, string propertyName, DbType sqlType,
            EntityPropertyType propertyType, string description, string defaultValue)
            : this(fieldName,paramName, propertyName, sqlType, propertyType,description, 0, true,false, defaultValue)
        {
            
        }
        /// <summary>
        /// 实体字段标识
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="paramName">数据库字段名</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="sqlType">Sql类型</param>
        /// <param name="propertyType">属性类型</param>
        /// <param name="description">备注</param>
        /// <param name="length">长度</param>
        /// <param name="allowNull">允许空</param>
        /// <param name="isReadOnly">只读</param>
        /// <param name="defaultValue">默认值</param>
        public EntityParam(string fieldName, string paramName, string propertyName, DbType sqlType,
            EntityPropertyType propertyType, string description, int length, bool allowNull, bool isReadOnly,string defaultValue)
        {
            this._fieldName = fieldName;
            this._paramName = paramName;
            this._propertyName = propertyName;
            this._sqlType = sqlType;
            this._propertyType = propertyType;
            this._length = length;
            this._allowNull = allowNull;
            this._readonly = isReadOnly;
            this._description = description;
            this._defaultValue = defaultValue;
        }
        /// <summary>
        /// 对应的字段名
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }
        private string _description;

        /// <summary>
        /// 字段注释
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
        /// <summary>
        /// 序列名
        /// </summary>
        public string SequenceName
        {
            get { return _sequenceName; }
            internal set { _sequenceName = value; }
        }
        /// <summary>
        /// 获取对应的字段名
        /// </summary>
        public string ParamName
        {
            get
            {
                return _paramName;
            }
            set 
            {
                _paramName = value;
            }
        }
        /// <summary>
        /// 获取对应的属性名
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
            set
            {
                _propertyName = value;
            }
        }
        /// <summary>
        /// 获取对应的数据库类型
        /// </summary>
        public DbType SqlType 
        {
            get
            {
                return _sqlType;
            }
            set
            {
                _sqlType = value;
                CheckLength();
            }
        }

        /// <summary>
        /// 属性类型
        /// </summary>
        public EntityPropertyType PropertyType
        {
            get
            {
                return _propertyType;
            }
            set 
            {
                _propertyType = value;
            }
        }
        /// <summary>
        /// 允许空
        /// </summary>
        public bool AllowNull
        {
            get { return _allowNull; }
            set { _allowNull = value; }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public long Length
        {
            get { return _length; }
            set 
            { 
                _length = value;
                CheckLength();
            }
        }
        /// <summary>
        /// 获取对应的字段是否主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.PrimaryKey);
            }
        }

        /// <summary>
        /// 是否自动增长字段
        /// </summary>
        public bool Identity
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Identity);
            }
        }

        /// <summary>
        /// 是否普通字段
        /// </summary>
        public bool IsNormal
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Normal);
            }
        }

        /// <summary>
        /// 是否版本信息字段
        /// </summary>
        public bool IsVersion
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Version);
            }
        }
        /// <summary>
        /// 检查长度
        /// </summary>
        private void CheckLength() 
        {
            switch (_sqlType) 
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.Binary:
                case DbType.String:
                case DbType.StringFixedLength:
                    if (_length <= 0) 
                    {
                        _length = 99999;
                    }
                    break;
                default:
                    break;
            }
            
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string DisplayInfo(KeyWordInfomation info, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            bool isPrimary = EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.PrimaryKey);
            bool isAutoIdentity = EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Identity);

            
            bool needIdentity = false;
            bool putType = true;
            sb.Append(idba.FormatParam(ParamName) + " ");

            if (isAutoIdentity && TableChecker.IsIdentityType(SqlType))
            {
                if (idba.IdentityIsType)
                {
                    sb.Append(idba.DBIdentity(tableName, _paramName));
                    sb.Append(" ");
                    putType = false;
                }
                else
                {
                    needIdentity = true;
                }
            }

            bool allowNULL = _allowNull & (!isPrimary);
            if (putType)
            {
                CheckLength();
                sb.Append(idba.DBTypeToSQL(SqlType, Length, allowNULL) + " ");
            }
            
            
            //主键
            if (isPrimary && info.PrimaryKeys==1)
            {
                sb.Append(" PRIMARY KEY ");
            }
            else
            {
                if (allowNULL)
                {
                    sb.Append("NULL ");
                }
                else
                {
                    sb.Append("NOT NULL ");
                }
            }

           
            
            
            if (needIdentity&&isAutoIdentity && TableChecker.IsIdentityType(SqlType))
            {

                sb.Append(idba.DBIdentity(tableName, _paramName));
            }
            else if (!string.IsNullOrEmpty(_defaultValue)) //默认值
            {
                string defValue = GetDefaultValue(idba, SqlType, _defaultValue);
                sb.Append(" DEFAULT ");
                sb.Append(defValue);
                sb.Append(" ");
            }
            string comment=idba.GetColumnDescriptionSQL(this,info.DBInfo);
            if (!string.IsNullOrEmpty(comment)) 
            {
                sb.Append(" ");
                sb.Append(comment);
            }

            return sb.ToString();
        }
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="idba">数据库适配器</param>
        /// <param name="type">数据类型</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private static string GetDefaultValue(IDBAdapter idba,DbType type,string value)
        {
            if (string.Equals(value, "db_now()",StringComparison.CurrentCultureIgnoreCase))
            {
                return idba.GetNowDate(type);
            }
            if (string.Equals(value, "db_utcnow()", StringComparison.CurrentCultureIgnoreCase))
            {
                return idba.GetUTCDate(type);
            }
            if (string.Equals(value, "db_timestamp()", StringComparison.CurrentCultureIgnoreCase))
            {
                return idba.GetTimeStamp(type);
            }
            return value;
        }

    }
}
