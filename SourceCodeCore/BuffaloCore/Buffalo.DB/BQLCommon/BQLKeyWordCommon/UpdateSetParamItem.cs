using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class UpdateSetParamItem 
    {
        protected BQLParamHandle parameter;
        protected BQLValueItem valueItem;
        /// <summary>
        /// Set�ؼ��ָ��µ���
        /// </summary>
        /// <param name="tables">����</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal UpdateSetParamItem(BQLParamHandle parameter, BQLValueItem valueItem)
        {
            this.parameter = parameter;
            this.valueItem = valueItem;
        }
        /// <summary>
        /// �����õ��ֶ�
        /// </summary>
        internal BQLParamHandle Parameter
        {
            get
            {
                return parameter;
            }
        }

        /// <summary>
        /// Ҫ���õ�ֵ
        /// </summary>
        internal BQLValueItem ValueItem
        {
            get
            {
                return valueItem;
            }
        }
        
    }

    
}
