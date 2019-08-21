using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using System.Data;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// �����ѯ��
    /// </summary>
    public class KeyWordInsertQueryItem : BQLQuery
    {
        private BQLAliasHandle _query;
        /// <summary>
        /// Insert���ֶιؼ�����
        /// </summary>
        /// <param name="valueHandles">ֵ�ļ���</param>
        /// <param name="previous">��һ���ؼ���</param>
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
