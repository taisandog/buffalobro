using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Threading;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordJoinItem : KeyWordFromItem
    {
        //private BQLTableHandle joinTable;
        private BQLCondition _condition;
        private string _keyWord;
        /// <summary>
        /// LeftJoin�ؼ�����
        /// </summary>
        /// <param name="prmsHandle">�ֶμ���</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordJoinItem(BQLTableHandle joinTable, BQLCondition condition, string keyWord, BQLQuery previous)
            : base(new BQLTableHandle[]{joinTable}, previous) 
        {
            //this.joinTable = joinTable;
            this._condition = condition;
            this._keyWord = keyWord;
        }
        
        /// <summary>
        /// ���ر�ı�����Ϣ
        /// </summary>
        /// <param name="info"></param>
        //internal void LoadTableInfo(KeyWordInfomation info)
        //{
        //    joinTable.FillInfo(info);
        //}

        internal override void Tran(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;

            string table0 = _tables[0].DisplayValue(info);

            string table1 = _condition.DisplayValue(info);

            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" ");
            sbSQL.Append(_keyWord);
            sbSQL.Append(" join");
            sbSQL.Append(" ");
            sbSQL.Append(table0);

            if (info.LockType != BQLLockType.None) //����
            {
                string lockSql = idba.ShowFromLockUpdate(info.LockType, info.DBInfo);
                if (!string.IsNullOrWhiteSpace(lockSql))
                {
                    sbSQL.Append(" ");
                    sbSQL.Append(lockSql);
                }
            }
            sbSQL.Append(" on ");
            sbSQL.Append(table1);
            info.Condition.Tables.Append(sbSQL.ToString());
        }
    }
}
