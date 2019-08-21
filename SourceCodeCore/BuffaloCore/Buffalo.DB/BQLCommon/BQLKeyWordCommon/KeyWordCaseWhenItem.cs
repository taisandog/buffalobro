using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordCaseWhenItem
    {
        private BQLValueItem value;
        private KeyWordLinkValueItemBase previous;

        /// <summary>
        /// Case��
        /// </summary>
        /// <param name="value">������ֵ</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordCaseWhenItem(BQLValueItem value, KeyWordLinkValueItemBase previous)
        {
            this.value = value;
            this.previous = previous;
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="thenValue"></param>
        /// <returns></returns>
        public KeyWordCaseThenItem Then(BQLValueItem thenValue) 
        {
            KeyWordCaseThenItem item = new KeyWordCaseThenItem(value, thenValue, previous);
            return item;
        }
        ///// <summary>
        ///// From��Щ��
        ///// </summary>
        ///// <param name="tables">��</param>
        ///// <returns></returns>
        //public KeyWordFromItem From(params BQLTableHandle[] tables)
        //{
        //    KeyWordFromItem fromItem = new KeyWordFromItem(tables,this);
        //    return fromItem;
        //}

        
    }
}
