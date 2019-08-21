using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// ��ҳ�����ݴ���
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
        /// <summary>
        /// ��ѯ�ı�
        /// </summary>
        private StringBuilder _tables = new StringBuilder();
        /// <summary>
        /// ����
        /// </summary>
        public override StringBuilder Tables
        {
            get { return _tables; }
        }
        /// <summary>
        /// ��ѯ����
        /// </summary>
        private StringBuilder _condition = new StringBuilder();
        /// <summary>
        /// ����
        /// </summary>
        public override StringBuilder Condition
        {
            get { return _condition; }
        }
        /// <summary>
        /// ����
        /// </summary>
        private StringBuilder _groupBy = new StringBuilder();
        /// <summary>
        /// ����
        /// </summary>
        public override StringBuilder GroupBy
        {
            get { return _groupBy; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        private StringBuilder _orders = new StringBuilder();
        /// <summary>
        /// ����
        /// </summary>
        public override StringBuilder Orders
        {
            get { return _orders; }
        }

        private bool _hasGroup = false;

        /// <summary>
        /// �Ƿ��оۺ����
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
        /// ��ҳ����
        /// </summary>
        public override  PageContent PageContent
        {
            get { return _pageContente; }
            set { _pageContente = value; }
        }

        

        #region ICondition ��Ա



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
        /// ��ȡ��ѯ���
        /// </summary>
        /// <param name="hasOrder">�Ƿ�������</param>
        /// <returns></returns>
        public string GetSelect()
        {
            return GetSelect(true);
        }
        /// <summary>
        /// ��ȡ��ѯ���
        /// </summary>
        /// <param name="hasOrder">�Ƿ�������</param>
        /// <returns></returns>
        public string GetSelect(bool hasOrder) 
        {
            StringBuilder sql = new StringBuilder(5000);
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
            return sql.ToString();
        }

        #endregion
    }
}
