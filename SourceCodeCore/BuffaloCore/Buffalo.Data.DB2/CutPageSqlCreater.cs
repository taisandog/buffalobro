using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using System.Data.Common;
using System.Threading.Tasks;

namespace Buffalo.Data.DB2
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
        public static async Task<string> CreatePageSqlAsync(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage, bool useCache)
        {

            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//初始化页数
            {
                return "";
            }
            //string sql = objCondition.GetSelect();
            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = await GetTotalRecordAsync(list, oper, objCondition.GetSelect(false, false),
                    objPage.MaxSelectRecords, (useCache ? objCondition.CacheTables : null));//获取总记录数
                //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
                //objPage.TotalPage = totalPage;
                if (objPage.CurrentPage >= objPage.TotalPage - 1)
                {
                    objPage.CurrentPage = objPage.TotalPage - 1;

                }
            }
            return GetCutPageSql(objCondition, objPage);
        }

        /// <summary>
        /// 生成SQL语句
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="oper">连接对象</param>
        /// <param name="objCondition">条件对象</param>
        /// <param name="objPage">分页记录类</param>
        /// <returns></returns>
        public static string CreatePageSql(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage, bool useCache)
        {

            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//初始化页数
            {
                return "";
            }
            //string sql = objCondition.GetSelect();
            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = GetTotalRecord(list, oper, objCondition.GetSelect(false, false),
                    objPage.MaxSelectRecords, (useCache ? objCondition.CacheTables : null));//获取总记录数
                //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
                //objPage.TotalPage = totalPage;
                if (objPage.CurrentPage >= objPage.TotalPage - 1)
                {
                    objPage.CurrentPage = objPage.TotalPage - 1;

                }
            }
            return GetCutPageSql(objCondition, objPage);
        }
        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="part">SQL条件</param>
        /// <returns></returns>
        private static string GetCutPageSql(SelectCondition objCondition, PageContent objPage)
        {
            string orderBy = null;
            if (objCondition.Orders.Length > 0)//如果有排序就用本身排序
            {
                orderBy = objCondition.Orders.ToString();
            }
            else//如果没有就用主键排序
            {
                StringBuilder sbOrder = new StringBuilder();
                foreach (string pkName in objCondition.PrimaryKey)
                {
                    sbOrder.Append(pkName + ",");
                }
                if (sbOrder.Length > 0)
                {
                    sbOrder.Remove(sbOrder.Length - 1, 1);
                    orderBy = sbOrder.ToString();
                }
            }
            long starIndex = objPage.GetStarIndex() + 1;
            string rowNumberName = "\"__cur_rowNumber" + objPage.PagerIndex + "\"";
            long endIndex = objPage.PageSize * (objPage.CurrentPage + 1);
            StringBuilder sql = new StringBuilder(5000);
            sql.Append("select * from (");
            sql.Append("select ROW_NUMBER() over(order by " + orderBy + ") as " + rowNumberName + ",");

            if (!objCondition.HasGroup)
            {
                sql.Append(objCondition.SqlParams.ToString() + "  from " + objCondition.Tables.ToString());
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where " + objCondition.Condition.ToString());
                }
                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by " + objCondition.GroupBy.ToString());
                }
                if (objCondition.Having.Length > 0)
                {
                    sql.Append(" having ");
                    sql.Append(objCondition.Having.ToString());
                }
            }
            else
            {
                sql.Append("\"_tmpInnerTable\".*");
                sql.Append("  from (");
                Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.GetGroupPart(objCondition, sql);
                sql.Append(") \"_tmpInnerTable\"");
            }
            sql.Append(") tmp where " + rowNumberName + " >=" + starIndex + " and " + rowNumberName + " <=" + endIndex);
            objCondition.FillLock(sql);
            return sql.ToString();
        }
        private static string GetTotalRecordSQL(string sql, long maxRecords)
        {
            StringBuilder tmpsql = new StringBuilder(5000);
            if (maxRecords > 0)
            {
                tmpsql.Append("select count(*) from (");
                tmpsql.Append(sql);
                if (maxRecords > 0)
                {
                    tmpsql.Append(" fetch " + maxRecords + " rows only");
                }
                tmpsql.Append(")");

            }
            else
            {
                tmpsql.Append("select count(*) from (");
                tmpsql.Append(sql);
                tmpsql.Append(")");
            }
            return tmpsql.ToString();
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="part">查询条件</param>
        /// <param name="list">变量列表</param>
        /// <param name="oper">通用类</param>
        public static long GetTotalRecord(ParamList list, DataBaseOperate oper, string sql,
            long maxRecords, Dictionary<string, bool> cacheTables)
        {
            long totalRecords = 0;
            string tmpSql = GetTotalRecordSQL(sql, maxRecords);
            DbDataReader reader = oper.Query(tmpSql, list, cacheTables);
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
        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="part">查询条件</param>
        /// <param name="list">变量列表</param>
        /// <param name="oper">通用类</param>
        public static async Task<long> GetTotalRecordAsync(ParamList list, DataBaseOperate oper, string sql,
            long maxRecords, Dictionary<string, bool> cacheTables)
        {
            long totalRecords = 0;
            string tmpSql = GetTotalRecordSQL(sql, maxRecords);
            DbDataReader reader = await oper.QueryAsync(tmpSql, list, CommandType.Text, cacheTables);
            try
            {
                if (await reader.ReadAsync())
                {
                    if (!(await reader.IsDBNullAsync(0)))
                    {
                        object obj =await reader.GetFieldValueAsync<object>(0);
                        totalRecords = Convert.ToInt64(obj);
                    }
                }
            }
            finally
            {
                await reader.CloseAsync();
            }
            return totalRecords;
        }
    }
}