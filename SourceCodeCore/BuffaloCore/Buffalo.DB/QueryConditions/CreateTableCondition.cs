using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// �����������
    /// </summary>
    public class CreateTableCondition:AbsCondition
    {

        public CreateTableCondition(DBInfo info):base(info) { }

        private StringBuilder _tables = new StringBuilder();
        public override StringBuilder Tables
        {
            get
            {
                return _tables;
            }
        }

        /// <summary>
        /// ����ֶ�
        /// </summary>
        private StringBuilder _sqlParams = new StringBuilder();
        /// <summary>
        /// �ֶ�
        /// </summary>
        public override StringBuilder SqlParams
        {
            get { return _sqlParams; }
        }

        public override string GetSql(bool useCache)
        {
            IDBAdapter ida=_dbInfo.CurrentDbAdapter;
            StringBuilder sbRet = new StringBuilder();
            sbRet.Append("create table " + Tables);
            sbRet.Append("(");
            sbRet.Append(SqlParams);
            sbRet.Append(")");
            string end = ida.CreateTableSQLEnd(_dbInfo);
            if (!string.IsNullOrEmpty(end)) 
            {
                sbRet.Append(end);
            }
            return sbRet.ToString();
        }
    }
}
