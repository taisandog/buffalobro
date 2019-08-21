using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// �ӱ��ʶ
    /// </summary>
    public class TableRelationAttribute : System.Attribute
    {
        private string _name;
        private string _propertyName;
        private EntityPropertyInfo _sourceProperty;
        private EntityPropertyInfo _targetProperty;
        private bool _isParent;

        private string _sourceName;
        private string _targetName;
        private Type _sourceType;
        private Type _targetType;
        private string _sourceTable;
        private string _targetTable;
        private string _fieldName;
        private bool _isToDB=false;
         /// <summary>
        /// ����ӳ����Ϣ
        /// </summary>
        public TableRelationAttribute() { }
        /// <summary>
        /// ����ӳ����Ϣ
        /// </summary>
        /// <param name="fieldName">��Ӧ�ֶ�</param>
        /// <param name="propertyName">����</param>
        /// <param name="sourceProperty">Դ��������</param>
        /// <param name="targetProperty">Ŀ���������</param>
        /// <param name="sourceTableType">ԭ��������</param>
        /// <param name="targetTableType">Ŀ���������</param>
        /// <param name="isParent">�Ƿ���������</param>
        public TableRelationAttribute(string fieldName,string name, string sourceTable, string targetTable,
            string sourceName, string targetName, string propertyName, bool isParent) 
        {
            _fieldName = fieldName;
            _propertyName = propertyName;
            _sourceTable = sourceTable;
            _sourceName = sourceName;
            _targetTable = targetTable;
            _targetName = targetName;

            _isParent = isParent;
        }

        ///// <summary>
        /////  ����ӳ����Ϣ
        ///// </summary>
        ///// <param name="fieldName">��Ӧ�ֶ�</param>
        ///// <param name="propertyName">����</param>
        ///// <param name="sourcePropertyName">Դ��������</param>
        ///// <param name="targetPropertyName">Ŀ������</param>
        ///// <param name="isParent">�Ƿ���������</param>
        ///// <param name="?"></param>
        //public TableRelationAttribute(string fieldName,
        //    string propertyName, string sourcePropertyName, 
        //    string targetPropertyName, bool isParent,string description)
        //{
        //    _fieldName = fieldName;
        //    _propertyName = propertyName;
        //    _sourceName = sourcePropertyName;
        //    _targetName = targetPropertyName;
        //    _isParent = isParent;
        //    _isToDB = false;
        //    _description = description;
        //}
        public override string ToString()
        {
            return Name;
        }
        private string _description;

        /// <summary>
        /// ע��
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// ��Ӧ���ֶ���
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        static int nameIndex=0;
        /// <summary>
        /// �����������
        /// </summary>
        public void CreateName()
        {

            Name = "FK" + DateTime.Now.ToString("yyyyMMddHHmmss" + nameIndex);
            nameIndex++;
        }
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="sourceEntity"></param>
        /// <param name="targetEniity"></param>
        internal void SetEntity(Type sourceType,Type targetType) 
        {
            _sourceType = sourceType;
            _targetType = targetType;
        }

        private string _fieldTypeName;

        /// <summary>
        /// �ֶ�������
        /// </summary>
        public string FieldTypeName
        {
            get { return _fieldTypeName; }
            set { _fieldTypeName = value; }
        }
        /// <summary>
        /// �Ƿ����ɵ����ݿ�
        /// </summary>
        public bool IsToDB 
        {
            get { return _isToDB; }
            set { _isToDB = value; }
        }
        /// <summary>
        /// Լ����
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// ��Ӧ��������
        /// </summary>
        public string PropertyName 
        {
            get 
            {
                return _propertyName;
            }
            set 
            {
                _propertyName=value;
            }
        }
        /// <summary>
        /// Դ����
        /// </summary>
        public string SourceTable
        {
            get 
            {
                if (_sourceTable == null && _sourceType != null)
                {
                    _sourceTable = GetTableName(_sourceType);
                }
                return _sourceTable;
            }
            set { _sourceTable = value; }
        }
        /// <summary>
        /// Ŀ�����
        /// </summary>
        public string TargetTable
        {
            get 
            {
                if (_targetTable == null && _targetType!=null) 
                {
                    _targetTable = GetTableName(_targetType);
                }
                return _targetTable; 
            }
            set { _targetTable = value; }
        }

        /// <summary>
        /// Ŀ��������
        /// </summary>
        public string TargetName
        {
            get { return _targetName; }
            set { _targetName = value; }
        }

        
        /// <summary>
        /// ��ȡ���Ͷ�Ӧ�ı���
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTableName(Type type) 
        {
            EntityInfoHandle eInfo = EntityInfoManager.GetEntityHandle(type);
            if (eInfo != null) 
            {
                return eInfo.TableName;
            }
            return "";
        }
        /// <summary>
        /// Դ������
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
            set { _sourceName = value; }
        }
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsParent
        {
            get { return _isParent; }
            set { _isParent = value; }
        }

        /// <summary>
        /// Դ����
        /// </summary>
        public EntityPropertyInfo SourceProperty 
        {
            get 
            {
                if (_sourceProperty == null)
                {
                    if(_sourceType==null)
                    {
                        return null;
                    }
                    _sourceProperty = EntityInfoManager.GetEntityHandle(_sourceType).PropertyInfo[_sourceName];
                }
                return _sourceProperty;
            }
        }
        /// <summary>
        /// Ŀ������
        /// </summary>
        public EntityPropertyInfo TargetProperty 
        {
            get
            {
                if (_targetProperty == null)
                {
                    if (_targetType == null)
                    {
                        return null;
                    }

                    _targetProperty = EntityInfoManager.GetEntityHandle(_targetType).PropertyInfo[_targetName];
                    
                }
                return _targetProperty;
            }
        }

    }
}
