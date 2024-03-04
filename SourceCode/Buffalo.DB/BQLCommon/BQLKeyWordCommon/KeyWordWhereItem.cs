using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordWhereItem:BQLQuery
    {
        private BQLCondition _condition;

        /// <summary>
        /// Where关键字项
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordWhereItem(BQLCondition condition, BQLQuery previous)
            : base(previous) 
        {
            this._condition = condition;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(_condition, info);
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
        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordGroupByItem GroupBy(params BQLParamHandle[] paramhandles)
        {
            KeyWordGroupByItem item = new KeyWordGroupByItem(paramhandles, this);
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
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public KeyWordHavingItem Having(BQLCondition condition)
        {
            KeyWordHavingItem item = new KeyWordHavingItem(condition, this);
            return item;
        }
        /// <summary>
        /// 查询范围
        /// </summary>
        /// <param name="star">开始条数</param>
        /// <param name="totalRecord">显示条数</param>
        /// <returns></returns>
        public KeyWordLimitItem Limit(uint star, uint totalRecord)
        {
            KeyWordLimitItem item = new KeyWordLimitItem(star, totalRecord, this);
            return item;
        }
        /// <summary>
        /// 锁定行
        /// </summary>
        /// <param name="noWait">如果冲突是否不等待</param>
        /// <returns></returns>
        public KeyWorkLockUpdateItem LockUpdate(BQLLockType type)
        {
            KeyWorkLockUpdateItem item = new KeyWorkLockUpdateItem(type, this);
            return item;
        }
        ///// <summary>
        ///// 查询范围
        ///// </summary>
        ///// <param name="star">开始条数</param>
        ///// <param name="totalRecord">显示条数</param>
        ///// <returns></returns>
        //public KeyWordLimitItem Limit(uint star, uint totalRecord)
        //{
        //    KeyWordLimitItem item = new KeyWordLimitItem(star, totalRecord, this);
        //    return item;
        //}
        internal override void Tran(KeyWordInfomation info)
        {
            
            info.IsWhere = true;
            string strCondition = _condition.DisplayValue(info);
            info.Condition.Condition.Append(strCondition);
            info.IsWhere = false;
        }
    }
}
