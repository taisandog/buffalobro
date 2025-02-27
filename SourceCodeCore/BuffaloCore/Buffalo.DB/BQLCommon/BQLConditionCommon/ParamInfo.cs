using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class ParamInfo
    {
        private string propertyName;
        private string paramName;
        private Type dataValueType;
        public ParamInfo(string propertyName, string paramName, Type dataValueType) 
        {
            this.propertyName = propertyName;
            this.paramName = paramName;
            this.dataValueType = dataValueType;
        }
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
        /// 属性类型
        /// </summary>
        public Type DataValueType
        {
            get
            {
                return dataValueType;
            }
            set
            {
                dataValueType = value;
            }
        }
        /// <summary>
        /// 对应的字段名
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
    }
}
