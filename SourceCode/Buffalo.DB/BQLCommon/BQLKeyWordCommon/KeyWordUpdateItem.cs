using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordUpdateItem : BQLQuery
    {
        private BQLTableHandle table;
        
        /// <summary>
        /// Update关键字项
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordUpdateItem(BQLTableHandle table, BQLQuery previous)
            : base(previous) 
        {
            this.table = table;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            //if (CommonMethods.IsNull(info.FromTable))
            //{
            //    info.FromTable = table;
            //}
            BQLValueItem.DoFillInfo(table, info);
        }
        /// <summary>
        /// 要查询的字段
        /// </summary>
        internal BQLTableHandle Table
        {
            get 
            {
                return table;
            }
        }
        /// <summary>
        /// 添加一个set项
        /// </summary>
        /// <param name="parameter">字段</param>
        /// <param name="valueItem">值</param>
        /// <returns></returns>
        public KeyWordUpdateSetItem Set(BQLParamHandle parameter, BQLValueItem valueItem)
        {
            KeyWordUpdateSetItem item = new KeyWordUpdateSetItem(parameter, valueItem, this);
            return item;
        }
        /// <summary>
        /// 添加一个set项
        /// </summary>
        /// <param name="parameter">字段</param>
        /// <param name="valueItem">值</param>
        /// <returns></returns>
        public KeyWordUpdateSetItem Set(UpdateSetParamItemList lstSetItem)
        {
            KeyWordUpdateSetItem item = new KeyWordUpdateSetItem(lstSetItem, this);
            return item;
        }
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new Buffalo.DB.QueryConditions.UpdateCondition(info.DBInfo);
                //info.ParamList = newBuffalo.DB.DbCommon.ParamList();
            }
            if (info.ParamList == null)
            {
                info.ParamList = new Buffalo.DB.DbCommon.ParamList();
            }
            string tableName = table.DisplayValue(info);
            
            info.Condition.Tables.Append(tableName);
            //return "update " + table.DisplayValue(info);
        }
    }
}
