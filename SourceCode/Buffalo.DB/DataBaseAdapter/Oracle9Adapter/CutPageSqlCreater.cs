using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.Oracle9Adapter
{
    /// <summary>
    /// ���ɷ�ҳ������
    /// </summary>
    public class CutPageSqlCreater
    {
       
        /// <summary>
        /// ����SQL���
        /// </summary>
        /// <param name="list">�����б�</param>
        /// <param name="oper">���Ӷ���</param>
        /// <param name="objCondition">��������</param>
        /// <param name="objPage">��ҳ��¼��</param>
        /// <returns></returns>
        public static string CreatePageSql(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage,bool useCache)
        {

            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//��ʼ��ҳ��
            {
                return "";
            }
            //string sql = "select " + objCondition.SqlParams + " from " + DbAdapterLoader.CurrentDbAdapter.FormatTableName(objCondition.Tables) + " where " + objCondition.Condition + " order by " + objCondition.Orders;
            //StringBuilder sql = new StringBuilder(5000);
            //sql.Append("select ");
            //sql.Append(objCondition.SqlParams.ToString());
            //sql.Append(" from ");
            //sql.Append(DbAdapterLoader.CurrentDbAdapter.FormatTableName(objCondition.Tables.ToString()));
            //if (objCondition.Condition.Length > 0)
            //{
            //    sql.Append(" where ");
            //    sql.Append(objCondition.Condition.ToString());
            //}
            //if (objCondition.GroupBy.Length > 0)
            //{
            //    sql.Append(" group by ");
            //    sql.Append(objCondition.GroupBy.ToString());
            //}
            //if (objCondition.Orders.Length>0)
            //{
            //    sql.Append(" order by ");
            //    sql.Append(objCondition.Orders.ToString());
            //}
            //if (objCondition.Having.Length > 0)
            //{
            //    sql.Append(" having ");
            //    sql.Append(objCondition.Having.ToString());
            //}
            string sql = objCondition.GetSelect();
            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = GetTotalRecord(list, oper, objCondition.GetSelect(false), objPage.MaxSelectRecords,
                    (useCache?objCondition.CacheTables:null));//��ȡ�ܼ�¼��
                //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
                //objPage.TotalPage = totalPage;
                if (objPage.CurrentPage >= objPage.TotalPage - 1)
                {
                    objPage.CurrentPage = objPage.TotalPage - 1;

                }
            }
            return GetCutPageSql(sql, objPage);
        }
        /// <summary>
        /// ��ȡ��ҳ���
        /// </summary>
        /// <param name="sql">Ҫ����ҳ��SQL</param>
        /// <param name="objCondition">��ҳ��</param>
        /// <returns></returns>
        public static string GetCutPageSql(string sql, PageContent objPage) 
        {
            long starIndex = objPage.GetStarIndex() + 1;
            long endIndex = objPage.PageSize + starIndex-1;
            StringBuilder tmpsql = new StringBuilder(5000);
            string rowNumberName = "\"cur_rowNumber" + objPage.PagerIndex+"\"" ;
            tmpsql.Append("SELECT * FROM (SELECT tmp.*, ROWNUM as " + rowNumberName + " FROM (");
            tmpsql.Append(sql);
            tmpsql.Append(") tmp WHERE ROWNUM <= ");
            tmpsql.Append(endIndex.ToString());
            tmpsql.Append(") WHERE " + rowNumberName + " >=");
            tmpsql.Append(starIndex.ToString());
            return tmpsql.ToString();
        }

        /// <summary>
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <param name="part">��ѯ����</param>
        /// <param name="list">�����б�</param>
        /// <param name="oper">ͨ����</param>
        public static long GetTotalRecord(ParamList list, DataBaseOperate oper,string sql
            , long maxRecords,Dictionary<string,bool> cacheTables)
        {
            long totalRecords = 0;
            StringBuilder tmpsql = new StringBuilder(5000);
            if (maxRecords > 0)
            {
                tmpsql.Append("select count(*) from (select ROWNUM,tmpTable.* from (");
                tmpsql.Append(sql);
                tmpsql.Append(") tmpTable where ROWNUM<=");
                tmpsql.Append(maxRecords.ToString());
                tmpsql.Append(")");
            }
            else
            {
                tmpsql.Append("select count(*) from (");
                tmpsql.Append(sql);
                tmpsql.Append(")");
            }
            IDataReader reader = oper.Query(tmpsql.ToString(), list, cacheTables);
            try
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        totalRecords = Convert.ToInt64(reader[0]);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
            return totalRecords;
        }
    
    }
}
