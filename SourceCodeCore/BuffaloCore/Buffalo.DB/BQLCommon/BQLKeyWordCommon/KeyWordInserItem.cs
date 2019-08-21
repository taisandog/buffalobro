using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordInserItem : BQLQuery
    {
        private BQLTableHandle tableHandle;
        /// <summary>
        /// Insert�ؼ�����
        /// </summary>
        /// <param name="tableHandle">Ҫ����ı�</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordInserItem(BQLTableHandle tableHandle, BQLQuery previous)
            : base(previous) 
        {
            this.tableHandle = tableHandle;
        }

        ///// <summary>
        ///// Ҫ����ı�
        ///// </summary>
        //internal BQLTableHandle TableHandle 
        //{
        //    get 
        //    {
        //        return tableHandle;
        //    }
        //}
        /// <summary>
        /// �ֶ�
        /// </summary>
        /// <param name="paramhandles">�ֶ�</param>
        /// <returns></returns>
        public KeyWordInsertFieldItem Fields(params BQLParamHandle[] paramhandles)
        {
            KeyWordInsertFieldItem fItem = new KeyWordInsertFieldItem(paramhandles, this);
            return fItem;
        }

        /// <summary>
        /// ����һ����ѯ����
        /// </summary>
        /// <param name="query">��ѯ</param>
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
