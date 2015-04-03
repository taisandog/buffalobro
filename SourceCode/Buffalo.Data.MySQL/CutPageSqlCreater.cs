using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.Data.MySQL
{
    /// <summary>
    /// 生成分页语句的类
    /// </summary>
    public class CutPageSqlCreater
    {
       
        /// <summary>
        /// 生成SQL语句
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="oper">连接对象</param>
        /// <param name="objCondition">条件对象</param>
        /// <param name="objPage">分页记录类</param>
        /// <returns></returns>
        public static string CreatePageSql(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage,bool useCache)
        {

            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//初始化页数
            {
                return "";
            }
            //StringBuilder sb = new StringBuilder(2000);
            //sb.Append("select ");
            //sb.Append(objCondition.SqlParams.ToString());
            //sb.Append(" from ");
            //sb.Append(DbAdapterLoader.CurrentDbAdapter.FormatTableName(objCondition.Tables.ToString()));
            //if (objCondition.Condition.Length > 0)
            //{
            //    sb.Append(" where ");
            //    sb.Append(objCondition.Condition.ToString());
            //}
            //if (objCondition.GroupBy.Length > 0)
            //{
            //    sb.Append(" group by ");
            //    sb.Append(objCondition.GroupBy.ToString());
            //}
            //if (objCondition.Orders.Length>0) 
            //{
            //    sb.Append(" order by ");
            //    sb.Append(objCondition.Orders.ToString());
            //}
            //if (objCondition.Having.Length > 0)
            //{
            //    sb.Append(" having ");
            //    sb.Append(objCondition.Having.ToString());
            //}
            string sql = objCondition.GetSelect();
            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = GetTotalRecord(list, oper, objCondition.GetSelect(false), objPage.MaxSelectRecords,
                    (useCache?objCondition.CacheTables:null));//获取总记录数
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
        /// 获取分页语句
        /// </summary>
        /// <param name="sql">要被分页的SQL</param>
        /// <param name="objCondition">分页类</param>
        /// <returns></returns>
        public static string GetCutPageSql(string sql, PageContent objPage) 
        {
            long starIndex = objPage.GetStarIndex();
            string tmpsql = sql;
            tmpsql += " limit " + starIndex + "," + objPage.PageSize;
            return tmpsql;
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="part">查询条件</param>
        /// <param name="list">变量列表</param>
        /// <param name="oper">通用类</param>
        public static long GetTotalRecord(ParamList list, DataBaseOperate oper,
            string sql,long maxRecords,Dictionary<string,bool> cacheTables)
        {
            long totalRecords = 0;
            //string tmpsql = "select count(*) from (" + sql + ")tmp";
            StringBuilder tmpsql = new StringBuilder(2000);
            if (maxRecords > 0)
            {
                tmpsql.Append("select count(*) from (select * from (");
                tmpsql.Append(sql);
                tmpsql.Append(")tmp1 limit 0,");
                tmpsql.Append(maxRecords.ToString());
                tmpsql.Append(")tmp");
            }
            else
            {
                tmpsql.Append("select count(*) from (");
                tmpsql.Append(sql);
                tmpsql.Append(")tmp");
            }
            IDataReader reader = oper.Query(tmpsql.ToString(), list, cacheTables);
            try
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        totalRecords = reader.GetInt64(0);
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
