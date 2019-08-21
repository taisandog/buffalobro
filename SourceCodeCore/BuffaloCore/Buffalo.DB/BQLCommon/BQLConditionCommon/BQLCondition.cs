using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// ��ѯ����
    /// </summary>
    public abstract class BQLCondition : BQLValueItem
    {
        /// <summary>
        /// ��ȷ����(1=1)
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
        /// ��������(1=2)
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
