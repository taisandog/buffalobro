using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordDeleteItem: BQLQuery
    {
        private BQLTableHandle table;
        
        /// <summary>
        /// Delete关键字项
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordDeleteItem(BQLTableHandle table, BQLQuery previous)
            : base(previous) 
        {
            this.table = table;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(table, info);
        }
        ///// <summary>
        ///// 要查询的字段
        ///// </summary>
        //internal BQLTableHandle Table
        //{
        //    get 
        //    {
        //        return table;
        //    }
        //}
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public KeyWordWhereItem Where(BQLCondition condition)
        {
            KeyWordWhereItem item = new KeyWordWhereItem(condition, this);
            return item;
        }

        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new Buffalo.DB.QueryConditions.DeleteCondition(info.DBInfo);
                //info.ParamList = newBuffalo.DB.DbCommon.ParamList();
            }
            if (info.ParamList == null)
            {
                info.ParamList = new Buffalo.DB.DbCommon.ParamList();
            }

            string tableName = table.DisplayValue(info);
            
            info.Condition.Tables.Append(tableName);
        
        }
    }
}
