using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    public class EnumInfo
    {
        private object value;
        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        
        private string fieldName;
        /// <summary>
        /// 常量名
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        private string description;
        /// <summary>
        /// 注释[Description("内容")]的内容
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string _displayName;
        /// <summary>
        /// 获取[DisplayName("显示名")]的内容
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        private List<Attribute> _customerAttributes=new List<Attribute>();
        /// <summary>
        /// 其他标签
        /// </summary>
        public List<Attribute> CustomerAttributes
        {
            get { return _customerAttributes; }
        }

        /// <summary>
        /// 查找其他标签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindCustomerAttribute<T>() where T:Attribute
        {
            T ret = null;
            foreach (Attribute att in _customerAttributes) 
            {
                ret = att as T;
                if (ret != null) 
                {
                    break;
                }
            }
            return ret;
        }
    }
}
