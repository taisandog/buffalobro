using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public abstract class BQLCondition : BQLValueItem
    {
        /// <summary>
        /// 正确条件(1=1)
        /// </summary>
        public static BQLConditionValueItem TrueValue
        {
            get
            {
                BQLConditionValueItem fHandle = new BQLConditionValueItem(true);
                return fHandle;
            }
        }

        /// <summary>
        /// 错误条件(1=2)
        /// </summary>
        public static BQLConditionValueItem FalseValue
        {
            get
            {
                BQLConditionValueItem fHandle = new BQLConditionValueItem(false);
                return fHandle;
            }
        }
    }
}
