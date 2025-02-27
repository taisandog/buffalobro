using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// 显示子项的集合
    /// </summary>
    public class ShowChildItem
    {
        private BQLEntityTableHandle _childItem;
        /// <summary>
        /// 子项
        /// </summary>
        public BQLEntityTableHandle ChildItem
        {
            get { return _childItem; }
            set { _childItem = value; }
        }

        private ScopeList _filterScope;
        /// <summary>
        /// 筛选条件
        /// </summary>
        public ScopeList FilterScope
        {
            get { return _filterScope; }
            set { _filterScope = value; }
        }
    }
}
