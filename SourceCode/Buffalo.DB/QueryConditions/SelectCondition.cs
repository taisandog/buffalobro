using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// 分页数据暂存类
    /// </summary>
    public class SelectCondition : AbsCondition
    {
        public SelectCondition(DBInfo db) :base(db)
        {
            
            _itemName = "Select";
        }
        /// <summary>
        /// Having
        /// </summary>
        private StringBuilder _sqlHaving = new StringBuilder();

        public override StringBuilder Having
        {
            get
            {
                return _sqlHaving;
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            _sqlHaving = null;
            _sqlParams = null;
            _tables = null;
            _condition = null;
            _groupBy = null;
            _orders = null;
            _pageContente = null;
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
        /// <summary>
        /// 表
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
        /// 查询的表(不加Lock条件)
        /// </summary>
        private StringBuilder _tablesNoLock = new StringBuilder();
        /// <summary>
        /// 查询的表(不加Lock条件)
        /// </summary>
        public StringBuilder TablesNoLock
        {
            get { return _tablesNoLock; }
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
        /// <summary>
        /// 分组
        /// </summary>
        private StringBuilder _groupBy = new StringBuilder();
        /// <summary>
        /// 主键
        /// </summary>
        public override StringBuilder GroupBy
        {
            get { return _groupBy; }
        }
        /// <summary>
        /// 排序条件
        /// </summary>
        private StringBuilder _orders = new StringBuilder();
        /// <summary>
        /// 主键
        /// </summary>
        public override StringBuilder Orders
        {
            get { return _orders; }
        }
        /// <summary>
        /// 锁条件
        /// </summary>
        private StringBuilder _lockUpdate = new StringBuilder();
        /// <summary>
        /// 锁条件
        /// </summary>
        public override StringBuilder LockUpdate
        {
            get { return _lockUpdate; }
        }

        private bool _hasGroup = false;

        /// <summary>
        /// 是否有聚合语句
        /// </summary>
        public bool HasGroup 
        {
            get 
            {
                return _hasGroup;
            }
            set 
            {
                _hasGroup = value;
            }
        }
        
        PageContent _pageContente =null;

        /// <summary>
        /// 分页对象
        /// </summary>
        public override  PageContent PageContent
        {
            get { return _pageContente; }
            set { _pageContente = value; }
        }

        

        #region ICondition 成员



        public override string GetSql(bool useCache)
        {
            string ret=null;
            DataAccessCommon.TrimHead(_condition);
            if (_pageContente != null && _pageContente.PageSize > 0)
            {
                ret = _dbInfo.CurrentDbAdapter.CreatePageSql(_paramList, _oper, this, _pageContente, useCache);
            }
            else 
            {
                ret = GetSelect();
            }

            return ret;
        }

        
        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <param name="hasOrder">是否有排序</param>
        /// <returns></returns>
        public string GetSelect(bool hasOrder=true,bool fillLock=true) 
        {
            StringBuilder sql = new StringBuilder(5000);
            FillSelect(sql,hasOrder);
            if(hasOrder && fillLock) 
            {
                FillLock(sql);
            }
            return sql.ToString();
        }
        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <param name="hasOrder">是否有排序</param>
        /// <returns></returns>
        public void FillSelect(StringBuilder sql,bool hasOrder)
        {
            
            sql.Append("select ");
            sql.Append(_sqlParams.ToString());
            sql.Append(" from ");
            sql.Append(_tables);
            if (_condition.Length > 0)
            {
                sql.Append(" where ");
                sql.Append(_condition.ToString());
            }

            if (_groupBy.Length > 0)
            {
                sql.Append(" group by ");
                sql.Append(_groupBy.ToString());
            }

            if (hasOrder)
            {
                if (_orders.Length > 0)
                {
                    sql.Append(" order by ");
                    sql.Append(_orders.ToString());
                }
            }

            if (_sqlHaving.Length > 0)
            {
                sql.Append(" having ");
                sql.Append(_sqlHaving.ToString());
            }
            
        }
        /// <summary>
        /// 填充锁
        /// </summary>
        /// <param name="sql"></param>
        public void FillLock(StringBuilder sql) 
        {
            if (_lockUpdate.Length > 0)
            {
                sql.Append(_lockUpdate.ToString());
            }
        }
        #endregion
    }
}
