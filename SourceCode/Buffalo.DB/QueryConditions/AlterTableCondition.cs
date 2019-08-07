using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// 修改表的条件
    /// </summary>
    public class AlterTableCondition:AbsCondition
    {

        public AlterTableCondition(DBInfo info) : base(info) { }

        private StringBuilder _tables = new StringBuilder();
        public override StringBuilder Tables
        {
            get
            {
                return _tables;
            }
        }

        private StringBuilder _operator=new StringBuilder();

        /// <summary>
        /// 操作符
        /// </summary>
        public virtual StringBuilder Operator
        {
            get { return _operator; }
        }

        /// <summary>
        /// 输出字段
        /// </summary>
        private StringBuilder _sqlParams = new StringBuilder();
        /// <summary>
        /// 字段
        /// </summary>
        public override StringBuilder SqlParams
        {
            get { return _sqlParams; }
        }

        public override string GetSql(bool useCache)
        {
            IDBAdapter ida=_dbInfo.CurrentDbAdapter;
            StringBuilder sbRet = new StringBuilder();
            sbRet.Append("alter table " + Tables+" ");
            sbRet.Append(Operator + " ");
            if (SqlParams.Length > 0)
            {
                sbRet.Append(SqlParams);
            }
            return sbRet.ToString();
        }
    }
}
