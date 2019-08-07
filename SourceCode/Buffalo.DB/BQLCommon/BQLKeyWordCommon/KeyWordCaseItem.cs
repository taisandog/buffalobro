using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordCaseItem : KeyWordLinkValueItemBase
    {
        protected BQLValueItem itemValue;

       

        /// <summary>
        /// Case关键字项
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordCaseItem(BQLValueItem value, KeyWordLinkValueItemBase previous)
            :base(previous)
        {
            this.itemValue = value;
        }

        ///// <summary>
        ///// 要查询的字段
        ///// </summary>
        //internal BQLValueItem ItemValue
        //{
        //    get 
        //    {
        //        return itemValue;
        //    }
        //}
        /// <summary>
        /// When关键字
        /// </summary>
        /// <param name="whenValue">when条件</param>
        /// <returns></returns>
        public KeyWordCaseWhenItem When(BQLValueItem whenValue)
        {
            KeyWordCaseWhenItem item = new KeyWordCaseWhenItem(whenValue, this);
            return item;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(itemValue, info);
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            StringBuilder sb = new StringBuilder();
            if (!Buffalo.Kernel.CommonMethods.IsNull(Previous))
            {
                sb.Append(Previous.DisplayValue(info));
            }
            sb.Append("case ");
            if (!CommonMethods.IsNull(itemValue))
            {
                sb.Append(itemValue.DisplayValue(info) + " ");
            }
            //return sb.ToString();
            return sb.ToString();
        }

        
    }
}
