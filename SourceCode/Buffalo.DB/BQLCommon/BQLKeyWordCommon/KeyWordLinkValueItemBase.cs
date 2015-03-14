using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// 链式值类型基类
    /// </summary>
    public abstract class KeyWordLinkValueItemBase : BQLValueItem
    {
        /// <summary>
        /// 链式值类型基类
        /// </summary>
        /// <param name="previous">上一个值</param>
        public KeyWordLinkValueItemBase(KeyWordLinkValueItemBase previous)
        {
            _previous = previous;
        }
        private KeyWordLinkValueItemBase _previous;

        /// <summary>
        /// 上一个值
        /// </summary>
        public KeyWordLinkValueItemBase Previous
        {
            get { return _previous; }
        }
    }
}
