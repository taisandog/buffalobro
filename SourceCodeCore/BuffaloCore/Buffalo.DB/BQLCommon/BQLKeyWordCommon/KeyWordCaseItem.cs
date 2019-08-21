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
        /// Case�ؼ�����
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordCaseItem(BQLValueItem value, KeyWordLinkValueItemBase previous)
            :base(previous)
        {
            this.itemValue = value;
        }

        ///// <summary>
        ///// Ҫ��ѯ���ֶ�
        ///// </summary>
        //internal BQLValueItem ItemValue
        //{
        //    get 
        //    {
        //        return itemValue;
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
