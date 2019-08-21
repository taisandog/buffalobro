using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.ContantSearchs
{
    /// <summary>
    /// ���Ը��ֶε�ӳ��
    /// </summary>
    public class PropertyParamMapping
    {
        private string propertyName;
        private string paramName;
        private DbType dataType;

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
        /// �ֶ���
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
        /// �ֶ���������
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
