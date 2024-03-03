using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.QueryConditions;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// 显示记录的范围
    /// </summary>
    public class KeyWordLimitItem : BQLQuery
    {
        uint star = 0;
        uint totalRecords = 0;
        /// <summary>
        /// 显示记录的范围
        /// </summary>
        /// <param name="star">开始记录</param>
        /// <param name="totalRecords">要显示多少条记录</param>
        /// <param name="previous">上一个语句</param>
        public KeyWordLimitItem(uint star, uint totalRecords, BQLQuery previous)
            : base(previous) 
        {
            this.star = star;
            this.totalRecords = totalRecords;
        }
        /// <summary>
        /// 锁定行
        /// </summary>
        /// <param name="noWait">如果冲突是否不等待</param>
        /// <returns></returns>
        public KeyWorkLockUpdateItem LockUpdate(bool noWait)
        {
            KeyWorkLockUpdateItem item = new KeyWorkLockUpdateItem(noWait, this);
            return item;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            //info.IsPage = true;
        }

        internal override void Tran(KeyWordInfomation info)
        {
            PageContent objPage = new PageContent();
            objPage.StarIndex = star;
            info.Infos.PagerCount++;
            objPage.PagerIndex = info.Infos.PagerCount;
            
            objPage.PageSize = totalRecords;
            objPage.IsFillTotalRecords = false;
            info.Condition.PageContent = objPage;
        }
    }
}
