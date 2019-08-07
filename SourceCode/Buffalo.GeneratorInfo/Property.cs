using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// 生成属性的项
    /// </summary>
    public class Property
    {
        /// <summary>
        /// 生成属性的项
        /// </summary>
        /// <param name="dicCheckItem">选中的项</param>
        /// <param name="typeFullName">对应的字段类型全名</param>
        /// <param name="summary">注释</param>
        /// <param name="typeName">类型名</param>
        /// <param name="propertyName">对应的属性名</param>
        /// <param name="dbInfo">属性的关联数据库信息</param>
        /// <param name="relInfo">关联信息</param>
        public Property(Dictionary<string, object> dicCheckItem, string typeFullName,
            string summary, string typeName, string propertyName,
            TableInfo tableInfo, RelationInfo relInfo) 
        {
            _dicCheckItem = dicCheckItem;
            _propertyName = propertyName;
            _typeFullName = typeFullName;
            _summary = summary;
            _typeName=typeName;
            _tableInfo = tableInfo;
            _relInfo = relInfo;
        }
        private TableInfo _tableInfo;
        /// <summary>
        /// 属性的关联数据库信息
        /// </summary>
        public TableInfo TableInfo 
        {
            get 
            {
                return _tableInfo;
            }
        }

        private RelationInfo _relInfo;
        /// <summary>
        /// 关联信息
        /// </summary>
        public RelationInfo RelInfo
        {
            get { return _relInfo; }
        }

        private Dictionary<string, object> _dicCheckItem = null;

        /// <summary>
        /// 选中的项
        /// </summary>
        internal Dictionary<string, object> CheckItem
        {
            get
            {
                return _dicCheckItem;
            }
        }

        public int ItemCount 
        {
            get 
            {
                return _dicCheckItem.Count;
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetValue(string itemName)
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret))
            {
                return ret.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取是否选中项
        /// </summary>
        /// <param name="itemName">项名称</param>
        /// <returns></returns>
        public bool HasItem(string itemName)
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret))
            {
                if (ret is bool)
                {
                    return (bool)ret;
                }
            }
            return false;
        }
        private string _typeFullName;

        /// <summary>
        /// 对应的字段类型
        /// </summary>
        public string TypeFullName
        {
            get { return _typeFullName; }
            
        }

        private string _summary;

        /// <summary>
        /// 注释
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }

        private string _typeName;

        /// <summary>
        /// 类型名
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
        }

        private string _propertyName;
        /// <summary>
        /// 对应的属性名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
    }
}
