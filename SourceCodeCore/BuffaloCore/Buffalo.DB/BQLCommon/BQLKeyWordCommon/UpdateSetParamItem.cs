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
        /// Set关键字更新的项
        /// </summary>
        /// <param name="tables">表集合</param>
        /// <param name="previous">上一个关键字</param>
        internal UpdateSetParamItem(BQLParamHandle parameter, BQLValueItem valueItem)
        {
            this.parameter = parameter;
            this.valueItem = valueItem;
        }
        /// <summary>
        /// 被设置的字段
        /// </summary>
        internal BQLParamHandle Parameter
        {
            get
            {
                return parameter;
            }
        }

        /// <summary>
        /// 要设置的值
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
