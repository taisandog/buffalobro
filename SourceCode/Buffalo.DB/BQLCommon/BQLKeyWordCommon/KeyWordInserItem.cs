using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordInserItem : BQLQuery
    {
        private BQLTableHandle tableHandle;
        /// <summary>
        /// Insert关键字项
        /// </summary>
        /// <param name="tableHandle">要插入的表</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordInserItem(BQLTableHandle tableHandle, BQLQuery previous)
            : base(previous) 
        {
            this.tableHandle = tableHandle;
        }
        
        ///// <summary>
        ///// 要插入的表
        ///// </summary>
        //internal BQLTableHandle TableHandle 
        //{
        //    get 
        //    {
        //        return tableHandle;
        //    }
        //}
        /// <summary>
        /// 字段
        /// </summary>
        /// <param name="paramhandles">字段</param>
        /// <returns></returns>
        public KeyWordInsertFieldItem Fields(params BQLParamHandle[] paramhandles)
        {
            KeyWordInsertFieldItem fItem = new KeyWordInsertFieldItem(paramhandles, this);
            return fItem;
        }

        /// <summary>
        /// 插入一个查询集合
        /// </summary>
        /// <param name="query">查询</param>
        /// <returns></returns>
        public KeyWordInsertQueryItem ByQuery(BQLQuery query)
        {
            return new KeyWordInsertQueryItem(query,this);
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(tableHandle, info);
        }
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new InsertCondition(info.DBInfo);
                //info.ParamList = new ParamList();
            }
            if (info.ParamList == null)
            {
                info.ParamList = new Buffalo.DB.DbCommon.ParamList();
            }
            string tableName=tableHandle.DisplayValue(info);
            
            info.Condition.Tables.Append(tableName);
            
        
        }
    }
}
