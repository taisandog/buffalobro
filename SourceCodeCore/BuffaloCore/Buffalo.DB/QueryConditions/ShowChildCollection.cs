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
        /// ���һ����
        /// </summary>
        /// <param name="prm">��ѯ����</param>
        /// <param name="filter">ɸѡ����</param>
        public void Add(BQLEntityTableHandle prm,ScopeList filter)
        {
            
            ShowChildItem item = new ShowChildItem();
            item.ChildItem = prm;
            item.FilterScope = filter;
            base.Add(item);
        }
        /// <summary>
        /// ���һ����
        /// </summary>
        /// <param name="prm">��ѯ����</param>
        /// <param name="filter">ɸѡ����</param>
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
        /// ���һ����
        /// </summary>
        /// <param name="prm">��ѯ����</param>
        public new void Add(BQLEntityTableHandle prm)
        {
            
            ShowChildItem item = new ShowChildItem();
            item.ChildItem = prm;
            item.FilterScope = new ScopeList();
            base.Add(item);
        }
    }
}
