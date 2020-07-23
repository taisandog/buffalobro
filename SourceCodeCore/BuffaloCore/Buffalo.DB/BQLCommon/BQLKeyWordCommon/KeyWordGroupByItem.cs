using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordGroupByItem:BQLQuery
    {
        private IList<BQLParamHandle> paramhandles;

        /// <summary>
        /// Where关键字项
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordGroupByItem(IList<BQLParamHandle> paramhandles, BQLQuery previous)
            : base(previous) 
        {
            this.paramhandles = paramhandles;
        }
       
        internal override void LoadInfo(KeyWordInfomation info)
        {
            if (paramhandles != null && paramhandles.Count > 0) 
            {
                foreach (BQLParamHandle handle in paramhandles) 
                {
                    BQLValueItem.DoFillInfo(handle, info);
                }
            }
            info.HasGroup = true;
        }
        ///// <summary>
        ///// 字段集合
        ///// </summary>
        //internal BQLParamHandle[] Paramhandles 
        //{
        //    get 
        //    {
        //        return paramhandles;
        //    }
        //}

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
            string ret = "";
            SelectCondition con = info.Condition as SelectCondition;

            for (int i = 0; i < paramhandles.Count; i++)
            {
                BQLParamHandle prm = paramhandles[i];
                string prmStr=prm.DisplayValue(info);
                ret +=prmStr ;
                if (i < paramhandles.Count - 1)
                {
                    ret += ",";
                }
                if (con != null)
                {
                    info.Condition.PrimaryKey.Add(prmStr);

                }
            }
            info.Condition.GroupBy.Append(ret);
            info.IsWhere = false;
            //return " group by " + ret;
        
        }
    }
}
