using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Threading;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordJoinItem : KeyWordFromItem
    {
        //private BQLTableHandle joinTable;
        private BQLCondition _condition;
        private string _keyWord;
        /// <summary>
        /// LeftJoin关键字项
        /// </summary>
        /// <param name="prmsHandle">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordJoinItem(BQLTableHandle joinTable, BQLCondition condition, string keyWord, BQLQuery previous)
            : base(new BQLTableHandle[]{joinTable}, previous) 
        {
            //this.joinTable = joinTable;
            this._condition = condition;
            this._keyWord = keyWord;
        }
        
        /// <summary>
        /// 加载表的别名信息
        /// </summary>
        /// <param name="info"></param>
        //internal void LoadTableInfo(KeyWordInfomation info)
        //{
        //    joinTable.FillInfo(info);
        //}

        internal override void Tran(KeyWordInfomation info)
        {
            string table0 = _tables[0].DisplayValue(info);
            string table1 = _condition.DisplayValue(info);
            string ret = " " + _keyWord + " join " + table0 + " on " + table1;
            info.Condition.Tables.Append(ret);
        }
    }
}
