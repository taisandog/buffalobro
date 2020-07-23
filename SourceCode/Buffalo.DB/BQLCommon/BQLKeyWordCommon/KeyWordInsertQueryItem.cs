using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using System.Data;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// 插入查询项
    /// </summary>
    public class KeyWordInsertQueryItem : BQLQuery
    {
        private BQLAliasHandle _query;
        /// <summary>
        /// Insert的字段关键字项
        /// </summary>
        /// <param name="valueHandles">值的集合</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordInsertQueryItem(BQLQuery query, BQLQuery previous)
            : base(previous) 
        {
            this._query = new BQLAliasHandle(query,null);
        }
        
        internal override void LoadInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(_query, info);
        }
        
        
        internal override void Tran(KeyWordInfomation info)
        {
            info.Condition.Condition.Append(_query.DisplayValue(info));
        }
    }
}
