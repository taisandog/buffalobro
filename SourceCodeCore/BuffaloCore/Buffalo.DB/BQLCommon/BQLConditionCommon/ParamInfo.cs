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
        /// ������
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
        /// ��������
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
        /// ��Ӧ���ֶ���
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
