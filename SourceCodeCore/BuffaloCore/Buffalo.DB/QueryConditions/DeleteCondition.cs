using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;

namespace Buffalo.DB.QueryConditions
{
    public class DeleteCondition : AbsCondition
    {

        public DeleteCondition(DBInfo db)
            : base(db)
        {
            _itemName = "Delete";
        }

        #region ICondition 成员

        /// <summary>
        /// 查询条件
        /// </summary>
        private StringBuilder _condition = new StringBuilder();
        /// <summary>
        /// 条件
        /// </summary>
        public override StringBuilder Condition
        {
            get { return _condition; }
        }

        public override string GetSql(bool useCache)
        {
            DataAccessCommon.TrimHead(_condition);
            StringBuilder sql = new StringBuilder(500);
            sql.Append("Delete from ");
            sql.Append(_tables.ToString());
            if (_condition.Length > 0)
            {
                sql.Append(" where ");
                sql.Append(_condition.ToString());
            }
            return sql.ToString();
        }

        


        /// <summary>
        /// 查询的表
        /// </summary>
        private StringBuilder _tables = new StringBuilder();
        /// <summary>
        /// 表
        /// </summary>
        public override StringBuilder Tables
        {
            get { return _tables; }
        }


        #endregion
    }
}
