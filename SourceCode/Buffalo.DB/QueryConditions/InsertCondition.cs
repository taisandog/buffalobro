using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.QueryConditions
{
    public class InsertCondition:AbsCondition
    {

        public InsertCondition(DBInfo db)
            : base(db)
        {
            _itemName = "Insert";
        }

        #region ICondition 成员

        private List<DbType> _paramTypes=new List<DbType>();

        /// <summary>
        /// 插入字段的数值类型
        /// </summary>
        public override List<DbType> ParamTypes
        {
            get { return _paramTypes; }
        }

        private StringBuilder _sqlParams = new StringBuilder();
        /// <summary>
        /// 字段
        /// </summary>
        public override StringBuilder SqlParams
        {
            get { return _sqlParams; }
        }

        private StringBuilder _sqlValues = new StringBuilder();
        /// <summary>
        /// 值
        /// </summary>
        public override StringBuilder SqlValues
        {
            get { return _sqlValues; }
        }
        /// <summary>
        /// 插入的表
        /// </summary>
        private StringBuilder _tables = new StringBuilder();
        /// <summary>
        /// 表
        /// </summary>
        public override StringBuilder Tables
        {
            get { return _tables; }
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        private StringBuilder _condition = new StringBuilder();
        /// <summary>
        /// 主键
        /// </summary>
        public override StringBuilder Condition
        {
            get { return _condition; }
        }

        public override string GetSql(bool useCache)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into ");
            sql.Append(_tables.ToString());

            if (_sqlParams.Length > 0)
            {
                sql.Append(" (");
                sql.Append(_sqlParams.ToString());
                sql.Append(") ");
            }
            if (_sqlValues.Length > 0)
            {
                sql.Append("values (");
                sql.Append(_sqlValues.ToString());
                sql.Append(")");
            }

            if (_condition.Length > 0) 
            {
                sql.Append(_condition);
            }
            
            return sql.ToString();
        }
        #endregion
    }
}
