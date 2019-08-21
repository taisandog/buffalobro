using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// ��ʽֵ���ͻ���
    /// </summary>
    public abstract class KeyWordLinkValueItemBase : BQLValueItem
    {
        /// <summary>
        /// ��ʽֵ���ͻ���
        /// </summary>
        /// <param name="previous">��һ��ֵ</param>
        public KeyWordLinkValueItemBase(KeyWordLinkValueItemBase previous)
        {
            _previous = previous;
        }
        private KeyWordLinkValueItemBase _previous;

        /// <summary>
        /// ��һ��ֵ
        /// </summary>
        public KeyWordLinkValueItemBase Previous
        {
            get { return _previous; }
        }
    }
}
