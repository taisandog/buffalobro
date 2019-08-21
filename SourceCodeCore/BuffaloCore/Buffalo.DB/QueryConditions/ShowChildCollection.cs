using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.QueryConditions
{
    public class ShowChildCollection : List<ShowChildItem>
    {
        
        public ShowChildCollection() 
        {
            
        }
        /// <summary>
        /// 添加一个项
        /// </summary>
        /// <param name="prm">查询子项</param>
        /// <param name="filter">筛选条件</param>
        public void Add(BQLEntityTableHandle prm,ScopeList filter)
        {
            
            ShowChildItem item = new ShowChildItem();
            item.ChildItem = prm;
            item.FilterScope = filter;
            base.Add(item);
        }
        /// <summary>
        /// 添加一个项
        /// </summary>
        /// <param name="prm">查询子项</param>
        /// <param name="filter">筛选条件</param>
        public void Add(BQLEntityTableHandle prm, BQLCondition filterCondition)
        {
            ScopeList filter = new ScopeList();
            filter.Add(filterCondition);
            ShowChildItem item = new ShowChildItem();
            item.ChildItem = prm;
            item.FilterScope = filter;
            base.Add(item);
        }
        /// <summary>
        /// 添加一个项
        /// </summary>
        /// <param name="prm">查询子项</param>
        public new void Add(BQLEntityTableHandle prm)
        {
            
            ShowChildItem item = new ShowChildItem();
            item.ChildItem = prm;
            item.FilterScope = new ScopeList();
            base.Add(item);
        }
    }
}
