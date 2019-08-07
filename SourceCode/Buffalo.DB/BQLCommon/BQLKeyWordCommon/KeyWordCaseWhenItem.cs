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
        /// Case项
        /// </summary>
        /// <param name="value">条件或值</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordCaseWhenItem(BQLValueItem value, KeyWordLinkValueItemBase previous)
        {
            this.value = value;
            this.previous = previous;
        }

        /// <summary>
        /// 结果
        /// </summary>
        /// <param name="thenValue"></param>
        /// <returns></returns>
        public KeyWordCaseThenItem Then(BQLValueItem thenValue) 
        {
            KeyWordCaseThenItem item = new KeyWordCaseThenItem(value, thenValue, previous);
            return item;
        }
        ///// <summary>
        ///// From哪些表
        ///// </summary>
        ///// <param name="tables">表</param>
        ///// <returns></returns>
        //public KeyWordFromItem From(params BQLTableHandle[] tables)
        //{
        //    KeyWordFromItem fromItem = new KeyWordFromItem(tables,this);
        //    return fromItem;
        //}

        
    }
}
