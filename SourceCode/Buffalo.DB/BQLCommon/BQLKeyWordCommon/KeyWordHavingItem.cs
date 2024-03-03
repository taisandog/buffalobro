using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordHavingItem : BQLQuery
    {
        private BQLCondition condition;

        /// <summary>
        /// Where关键字项
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordHavingItem(BQLCondition condition, BQLQuery previous)
            : base(previous) 
        {
            this.condition = condition;
        }
        
        internal override void LoadInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(condition, info);
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
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordOrderByItem OrderBy(params BQLParamHandle[] paramhandles)
        {
            KeyWordOrderByItem item = new KeyWordOrderByItem(paramhandles, this);
            return item;
        }
        ///// <summary>
        ///// 条件
        ///// </summary>
        //internal BQLCondition Condition 
        //{
        //    get
        //    {
        //        return condition;
        //    }
        //}

        internal override void Tran(KeyWordInfomation info)
        {
            info.IsWhere = true;
            info.Condition.Having.Append(condition.DisplayValue(info));
            info.IsWhere = false;
        }
    }
}
