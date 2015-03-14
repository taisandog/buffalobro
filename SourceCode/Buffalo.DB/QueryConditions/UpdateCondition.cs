using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;

namespace Buffalo.DB.QueryConditions
{
    public class UpdateCondition : AbsCondition
    {

        public UpdateCondition(DBInfo db)
            : base(db) 
        {
            _itemName = "Update";
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

        private StringBuilder _updateSetValue = new StringBuilder();

        public override StringBuilder UpdateSetValue
        {
            get { return _updateSetValue; }
        }

        public override string GetSql(bool useCache)
        {
            DataAccessCommon.TrimHead(_condition);
            StringBuilder sbRet = new StringBuilder(2000);
            sbRet.Append("update  ");
            sbRet.Append(_tables.ToString());
            sbRet.Append(" set ");
            sbRet.Append(_updateSetValue.ToString());

            if (_condition.Length > 0)
            {
                sbRet.Append(" where ");
                sbRet.Append(_condition.ToString());
            }

            return sbRet.ToString();
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
    }
}
