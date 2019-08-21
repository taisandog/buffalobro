using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordCaseThenItem : KeyWordLinkValueItemBase
    {
        private BQLValueItem thenValue;
        private BQLValueItem whenValue;

        /// <summary>
        /// Case项
        /// </summary>
        /// <param name="whenValue">when的值</param>
        /// <param name="thenValue">then的值</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordCaseThenItem(BQLValueItem whenValue, BQLValueItem thenValue, KeyWordLinkValueItemBase previous)
        :base(previous)
        {
            this.whenValue = whenValue;
            this.thenValue = thenValue;
        }
        ///// <summary>
        ///// then的值
        ///// </summary>
        //internal BQLValueItem ThenValue 
        //{
        //    get 
        //    {
        //        return thenValue;
        //    }
        //}
        ///// <summary>
        ///// when的值
        ///// </summary>
        //internal BQLValueItem WhenValue
        //{
        //    get
        //    {
        //        return whenValue;
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

        /// <summary>
        /// else关键字
        /// </summary>
        /// <param name="elseValue">else条件</param>
        /// <returns></returns>
        public KeyWordCaseElseItem Else(BQLValueItem elseValue)
        {
            KeyWordCaseElseItem item = new KeyWordCaseElseItem(elseValue, this);
            return item;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(thenValue, info);
            BQLValueItem.DoFillInfo(whenValue, info);
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            StringBuilder sb = new StringBuilder();
            if (!Buffalo.Kernel.CommonMethods.IsNull(Previous)) 
            {
                sb.Append(Previous.DisplayValue(info));
            }
            sb.Append(" when ");
            sb.Append(whenValue.DisplayValue(info));
            sb.Append(" then ");
            sb.Append(thenValue.DisplayValue(info));
            return sb.ToString();
        }


    }
}
