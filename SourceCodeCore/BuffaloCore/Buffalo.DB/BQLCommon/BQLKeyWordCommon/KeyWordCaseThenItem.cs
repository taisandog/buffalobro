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
        /// Case��
        /// </summary>
        /// <param name="whenValue">when��ֵ</param>
        /// <param name="thenValue">then��ֵ</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordCaseThenItem(BQLValueItem whenValue, BQLValueItem thenValue, KeyWordLinkValueItemBase previous)
        :base(previous)
        {
            this.whenValue = whenValue;
            this.thenValue = thenValue;
        }
        ///// <summary>
        ///// then��ֵ
        ///// </summary>
        //internal BQLValueItem ThenValue 
        //{
        //    get 
        //    {
        //        return thenValue;
        //    }
        //}
        ///// <summary>
        ///// when��ֵ
        ///// </summary>
        //internal BQLValueItem WhenValue
        //{
        //    get
        //    {
        //        return whenValue;
        //    }
        //}

        /// <summary>
        /// When�ؼ���
        /// </summary>
        /// <param name="whenValue">when����</param>
        /// <returns></returns>
        public KeyWordCaseWhenItem When(BQLValueItem whenValue) 
        {
            KeyWordCaseWhenItem item = new KeyWordCaseWhenItem(whenValue, this);
            return item;
        }

        /// <summary>
        /// else�ؼ���
        /// </summary>
        /// <param name="elseValue">else����</param>
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
