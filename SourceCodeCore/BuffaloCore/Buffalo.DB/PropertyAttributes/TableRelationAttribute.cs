using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// 子表标识
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
        /// 关联映射信息
        /// </summary>
        public TableRelationAttribute() { }
        /// <summary>
        /// 关联映射信息
        /// </summary>
        /// <param name="fieldName">对应字段</param>
        /// <param name="propertyName">属性</param>
        /// <param name="sourceProperty">源对象属性</param>
        /// <param name="targetProperty">目标对象属性</param>
        /// <param name="sourceTableType">原对象类型</param>
        /// <param name="targetTableType">目标对象类型</param>
        /// <param name="isParent">是否主表属性</param>
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
        /////  关联映射信息
        ///// </summary>
        ///// <param name="fieldName">对应字段</param>
        ///// <param name="propertyName">属性</param>
        ///// <param name="sourcePropertyName">源对象属性</param>
        ///// <param name="targetPropertyName">目标属性</param>
        ///// <param name="isParent">是否主表属性</param>
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
        /// 注释
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// 对应的字段名
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        static int nameIndex=0;
        /// <summary>
        /// 创建外键名称
        /// </summary>
        public void CreateName()
        {

            Name = "FK" + DateTime.Now.ToString("yyyyMMddHHmmss" + nameIndex);
            nameIndex++;
        }
        /// <summary>
        /// 设置实体
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
        /// 字段类型名
        /// </summary>
        public string FieldTypeName
        {
            get { return _fieldTypeName; }
            set { _fieldTypeName = value; }
        }
        /// <summary>
        /// 是否生成到数据库
        /// </summary>
        public bool IsToDB 
        {
            get { return _isToDB; }
            set { _isToDB = value; }
        }
        /// <summary>
        /// 约束名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 对应的属性名
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
        /// 源表名
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
        /// 目标表名
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
        /// 目标属性名
        /// </summary>
        public string TargetName
        {
            get { return _targetName; }
            set { _targetName = value; }
        }

        
        /// <summary>
        /// 获取类型对应的表名
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
        /// 源属性名
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
            set { _sourceName = value; }
        }
        /// <summary>
        /// 是否主表属性
        /// </summary>
        public bool IsParent
        {
            get { return _isParent; }
            set { _isParent = value; }
        }

        /// <summary>
        /// 源属性
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
        /// 目标属性
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
