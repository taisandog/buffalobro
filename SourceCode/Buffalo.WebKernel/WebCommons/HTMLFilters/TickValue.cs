using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    public class TickValue
    {
        /// <summary>
        /// 标签属性
        /// </summary>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">属性值</param>
        public TickValue(string attributeName, string attributeValue) 
        {
            this.attributeName = attributeName;
            this.attributeValue = attributeValue;
        }

        

        private string attributeName=null;

        /// <summary>
        /// 获取标签名
        /// </summary>
        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }
        
        private string attributeValue=null;

        /// <summary>
        /// 获取标签值
        /// </summary>
        public string AttributeValue
        {
            get { return attributeValue; }
            set { attributeValue = value; }
        }
        
        
    }
}
