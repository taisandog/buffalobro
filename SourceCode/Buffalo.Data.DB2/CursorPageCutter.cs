using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

using Buffalo.DB.CommBase;
using Buffalo.DB.DataFillers;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.Data.DB2
{
    /// <summary>
    /// 游标分页
    /// </summary>
    public class CursorPageCutter
    {
        /// <summary>
        /// 查询并且返回集合(游标分页)
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="lstParam">参数集合</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="oper">数据库对象</param>
        /// <returns></returns>
        public static IDataReader Query(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper,Dictionary<string,bool> cacheTables)
        {

            objPage.TotalRecords = CutPageSqlCreater.GetTotalRecord(lstParam, oper, sql, objPage.MaxSelectRecords,cacheTables);
            //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
            //objPage.TotalPage = totalPage;
            if (objPage.CurrentPage >= objPage.TotalPage - 1)
            {
                objPage.CurrentPage = objPage.TotalPage - 1;
            }
            IDataReader reader = null;

            string qsql = GetCursorPageSql(sql, objPage);
            reader = oper.Query(qsql, lstParam,cacheTables);

            return reader;
        }

        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="part">SQL条件</param>
        /// <returns></returns>
        private static string GetCursorPageSql(string sql, PageContent objPage)
        {
            long starIndex = objPage.GetStarIndex() + 1;
            string rowNumberName =  "\"__cur_rowNumber" + objPage.PagerIndex+"\"";
            long endIndex = objPage.PageSize * (objPage.CurrentPage + 1);
            StringBuilder sb = new StringBuilder(5000);
            sb.Append("select * from (");
            sb.Append("select ROW_NUMBER() over() as " + rowNumberName + ",tmp.*  from (");
            sb.Append(sql);
            sb.Append(") tmp) tmp1 where " + rowNumberName + " >=" + starIndex + " and " + rowNumberName + " <=" + endIndex);
            return sb.ToString();
        }
        /// <summary>
        /// 查询并且返回DataSet(游标分页)
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="lstParam">参数集合</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="oper">数据库对象</param>
        /// <param name="curType">映射的实体类型(如果用回数据库的原列名，则此为null)</param>
        /// <returns></returns>
        public static DataTable QueryDataTable(string sql, ParamList lstParam, PageContent objPage,
            DataBaseOperate oper, Type curType,Dictionary<string,bool> cacheTables)
        {
            objPage.TotalRecords = CutPageSqlCreater.GetTotalRecord(lstParam, oper, sql,
                objPage.MaxSelectRecords,cacheTables);
            //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
            //objPage.TotalPage = totalPage;
            if (objPage.CurrentPage >= objPage.TotalPage - 1)
            {
                objPage.CurrentPage = objPage.TotalPage - 1;
            }
            if (objPage.CurrentPage >= objPage.TotalPage - 1)
            {
                objPage.CurrentPage = objPage.TotalPage - 1;
            }

            DataTable ret = new DataTable();
            IDataReader reader = null;
            try
            {
                string qsql = GetCursorPageSql(sql, objPage);
                reader = oper.Query(qsql, lstParam,cacheTables);
                
                if (curType == null)
                {
                    ret = CacheReader.GenerateDataTable(reader, "newDt", false);
                }
                else 
                {
                    ret = CacheReader.GenerateDataTable(reader, "newDt",curType, false);
                }
            }
            finally
            {
                reader.Close();
                //oper.CloseDataBase();
            }
            return ret;
        }
    }
}
