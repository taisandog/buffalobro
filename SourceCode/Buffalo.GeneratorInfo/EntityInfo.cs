using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// 要生成实体信息
    /// </summary>
    public class EntityInfo
    {
        /// <summary>
        /// 要生成实体信息
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <param name="fileName">类文件名</param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <param name="summary">注释</param>
        /// <param name="baseTypeName">基类名</param>
        /// <param name="dicGenericInfo">泛型信息</param>
        /// <param name="lstAllProperty">所有属性</param>
        /// <param name="model">可选项信息</param>
        public EntityInfo(string dbName, string fileName, string nameSpace, string className, 
            string summary, string baseTypeName,List<Property> lstAllProperty,
            Dictionary<string, List<string>> dicGenericInfo, Property model) 
        {
            _dbName = dbName;
            _fileName = fileName; 
            _baseTypeName = baseTypeName;
            _className = className;
            _dicGenericInfo = dicGenericInfo;
            _fileName = fileName;
            _model = model;
            _namespace = nameSpace;
            _summary = summary;
            _allProperty = lstAllProperty;

            foreach (Property pro in _allProperty)
            {
                if (pro.TableInfo == null) 
                {
                    continue;
                }
                if (pro.TableInfo.IsPrimary)
                {
                    _primaryProperty = pro;
                    break;
                }
            }
        }

        private string _dbName;
        /// <summary>
        /// 类图名
        /// </summary>
        public string DBName
        {
            get { return _dbName; }
        }

        private string _fileName;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }
        private string _namespace;
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
        }

        private string _className;
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }
        /// <summary>
        /// 类全名
        /// </summary>
        public string FullName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(_namespace))
                {
                    sb.Append(_namespace);
                    sb.Append(".");
                }
                sb.Append(_className);
                return sb.ToString();
            }
        }

        private string _summary;

        /// <summary>
        /// 注释
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }
        private string _baseTypeName;
        /// <summary>
        /// 基名称
        /// </summary>
        public string BaseTypeName
        {
            get
            {

                return _baseTypeName;
            }
        }

        private Dictionary<string, List<string>> _dicGenericInfo =null;
        /// <summary>
        /// 泛型信息
        /// </summary>
        public Dictionary<string, List<string>> GenericInfo
        {
            get { return _dicGenericInfo; }
        }
        private List<Property> _allProperty;
        /// <summary>
        /// 所有属性
        /// </summary>
        public List<Property> AllProperty
        {
            get { return _allProperty; }
        }
        private Property _primaryProperty;
        /// <summary>
        /// 主键对应的属性
        /// </summary>
        public Property PrimaryProperty
        {
            get { return _primaryProperty; }
        }


        private Property _model=null;


        /// <summary>
        /// 选中的项
        /// </summary>
        internal Dictionary<string, object> CheckItem
        {
            get
            {
                return _model.CheckItem;
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetValue(string itemName)
        {
            return _model.GetValue(itemName);
        }

        /// <summary>
        /// 获取是否选中项
        /// </summary>
        /// <param name="itemName">项名称</param>
        /// <returns></returns>
        public bool HasItem(string itemName)
        {

            return _model.HasItem(itemName);
        }

    }
}
