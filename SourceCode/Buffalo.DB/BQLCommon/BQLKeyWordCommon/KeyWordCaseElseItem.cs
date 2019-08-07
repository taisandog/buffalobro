using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordCaseElseItem : KeyWordCaseItem
    {
        /// <summary>
        /// Case的Else关键字项
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordCaseElseItem(BQLValueItem value, KeyWordLinkValueItemBase previous)
            : base(value,previous) 
        {
            
        }
        /// <summary>
        /// 结束case语句
        /// </summary>
        public BQLCaseHandle End
        {
            get
            {
                return new BQLCaseHandle(this);
            }
        }


        internal override void FillInfo(KeyWordInfomation info)
        {
        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            StringBuilder sb = new StringBuilder();
            if (!Buffalo.Kernel.CommonMethods.IsNull(Previous)) 
            {
                sb.Append(Previous.DisplayValue(info));
            }
            
            sb.Append(" else ");
            sb.Append(itemValue.DisplayValue(info));
            return sb.ToString();
        }

    }
}
