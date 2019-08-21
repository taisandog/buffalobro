using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// ��ʾ����ļ���
    /// </summary>
    public class ShowChildItem
    {
        private BQLEntityTableHandle _childItem;
        /// <summary>
        /// ����
        /// </summary>
        public BQLEntityTableHandle ChildItem
        {
            get { return _childItem; }
            set { _childItem = value; }
        }

        private ScopeList _filterScope;
        /// <summary>
        /// ɸѡ����
        /// </summary>
        public ScopeList FilterScope
        {
            get { return _filterScope; }
            set { _filterScope = value; }
        }
    }
}
