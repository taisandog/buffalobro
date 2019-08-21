using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.ContantSearchs
{
    /// <summary>
    /// 属性跟字段的映射
    /// </summary>
    public class PropertyParamMapping
    {
        private string propertyName;
        private string paramName;
        private DbType dataType;

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName 
        {
            get 
            {
                return propertyName;
            }
            set 
            {
                propertyName = value;
            }
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string ParamName
        {
            get
            {
                return paramName;
            }
            set
            {
                paramName = value;
            }
        }

        /// <summary>
        /// 字段数据类型
        /// </summary>
        public DbType DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
            }
        }
    }
}
