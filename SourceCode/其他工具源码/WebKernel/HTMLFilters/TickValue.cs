using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    public class TickValue
    {
        /// <summary>
        /// ��ǩ����
        /// </summary>
        /// <param name="attributeName">������</param>
        /// <param name="attributeValue">����ֵ</param>
        public TickValue(string attributeName, string attributeValue) 
        {
            this.attributeName = attributeName;
            this.attributeValue = attributeValue;
        }

        

        private string attributeName=null;

        /// <summary>
        /// ��ȡ��ǩ��
        /// </summary>
        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }
        
        private string attributeValue=null;

        /// <summary>
        /// ��ȡ��ǩֵ
        /// </summary>
        public string AttributeValue
        {
            get { return attributeValue; }
            set { attributeValue = value; }
        }
        
        
    }
}
