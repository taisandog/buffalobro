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
    /// �ֶ���Ϣ
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
        /// ʵ���ֶα�ʶ
        /// </summary>
        public EntityParam() 
        {
            _readonly = false;

        }
        /// <summary>
        /// ʵ���ֶα�ʶ
        /// </summary>
        /// <param name="fieldName">�ֶ���</param>
        /// <param name="paramName">���ݿ��ֶ���</param>
        /// <param name="propertyName">������</param>
        /// <param name="sqlType">Sql����</param>
        /// <param name="propertyType">��������</param>
        /// <param name="description">��ע</param>
        /// <param name="length">����</param>
        /// <param name="allowNull">�����</param>
        /// <param name="isReadOnly">ֻ��</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        public EntityParam(string fieldName, string paramName, string propertyName, DbType sqlType,
            EntityPropertyType propertyType, string description, string defaultValue)
            : this(fieldName,paramName, propertyName, sqlType, propertyType,description, 0, true,false, defaultValue)
        {
            
        }
        /// <summary>
        /// ʵ���ֶα�ʶ
        /// </summary>
        /// <param name="fieldName">�ֶ���</param>
        /// <param name="paramName">���ݿ��ֶ���</param>
        /// <param name="propertyName">������</param>
        /// <param name="sqlType">Sql����</param>
        /// <param name="propertyType">��������</param>
        /// <param name="description">��ע</param>
        /// <param name="length">����</param>
        /// <param name="allowNull">�����</param>
        /// <param name="isReadOnly">ֻ��</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
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
        /// ��Ӧ���ֶ���
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        /// <summary>
        /// �Ƿ�ֻ��
        /// </summary>
        public bool ReadOnly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }
        private string _description;

        /// <summary>
        /// �ֶ�ע��
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// Ĭ��ֵ
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string SequenceName
        {
            get { return _sequenceName; }
            internal set { _sequenceName = value; }
        }
        /// <summary>
        /// ��ȡ��Ӧ���ֶ���
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
        /// ��ȡ��Ӧ��������
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
        /// ��ȡ��Ӧ�����ݿ�����
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
        /// ��������
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
        /// �����
        /// </summary>
        public bool AllowNull
        {
            get { return _allowNull; }
            set { _allowNull = value; }
        }
        /// <summary>
        /// ����
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
        /// ��ȡ��Ӧ���ֶ��Ƿ�����
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.PrimaryKey);
            }
        }

        /// <summary>
        /// �Ƿ��Զ������ֶ�
        /// </summary>
        public bool Identity
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Identity);
            }
        }

        /// <summary>
        /// �Ƿ���ͨ�ֶ�
        /// </summary>
        public bool IsNormal
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Normal);
            }
        }

        /// <summary>
        /// �Ƿ�汾��Ϣ�ֶ�
        /// </summary>
        public bool IsVersion
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Version);
            }
        }
        /// <summary>
        /// ��鳤��
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
        /// �����Ϣ
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
            
            
            //����
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
            else if (!string.IsNullOrEmpty(_defaultValue)) //Ĭ��ֵ
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
        /// ��ȡĬ��ֵ
        /// </summary>
        /// <param name="idba">���ݿ�������</param>
        /// <param name="type">��������</param>
        /// <param name="value">ֵ</param>
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
