using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    
    public class KeyWordOrderByItem : BQLQuery
    {
        private BQLParamHandle[] parameters;

        /// <summary>
        /// Select关键字项
        /// </summary>
        /// <param name="prmsHandle">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordOrderByItem(BQLParamHandle[] parameters, BQLQuery previous)
            : base(previous) 
        {
            this.parameters = parameters;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                BQLParamHandle prmHandle = parameters[i];
                BQLValueItem.DoFillInfo(prmHandle, info);
            }
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

        internal override void Tran(KeyWordInfomation info)
        {
            info.IsWhere = true;
            StringBuilder condition = new StringBuilder();
            for ( int i=0;i< parameters.Length;i++)
            {
                BQLParamHandle prmHandle = parameters[i];
                condition.Append(prmHandle.DisplayValue(info));
                if (i < parameters.Length - 1) 
                {
                    condition.Append(',');
                }
            }
            info.Condition.Orders.Append(condition.ToString());
            //return " order by " + condition.ToString();
            info.IsWhere = false;
        }
    }
}
