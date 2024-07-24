using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.DataFillers;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.Common;
using System.Threading.Tasks;

namespace Buffalo.Data.SQLite
{
    /// <summary>
    /// �α��ҳ
    /// </summary>
    public class CursorPageCutter
    {
        /// <summary>
        /// ��ѯ���ҷ��ؼ���(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <returns></returns>
        public static DbDataReader Query(string sql, ParamList lstParam, PageContent objPage,
            DataBaseOperate oper,Dictionary<string,bool> cacheTables)
        {

            objPage.TotalRecords = CutPageSqlCreater.GetTotalRecord(lstParam, oper, sql,
                objPage.MaxSelectRecords, cacheTables);
            //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
            //objPage.TotalPage = totalPage;
            if (objPage.CurrentPage >= objPage.TotalPage - 1)
            {
                objPage.CurrentPage = objPage.TotalPage - 1;
            }

            DbDataReader reader = null;
            StringBuilder tmpsql = new StringBuilder();
            tmpsql.Append(sql);
            CutPageSqlCreater.FillCutPageSql(tmpsql, objPage);
            string qsql = tmpsql.ToString();
            reader = oper.Query(qsql, lstParam,cacheTables);


            return reader;
        }
        /// <summary>
        /// ��ѯ���ҷ��ؼ���(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <returns></returns>
        public static async Task<DbDataReader> QueryAsync(string sql, ParamList lstParam, PageContent objPage,
            DataBaseOperate oper, Dictionary<string, bool> cacheTables)
        {

            objPage.TotalRecords = await CutPageSqlCreater.GetTotalRecordAsync(lstParam, oper, sql,
                objPage.MaxSelectRecords, cacheTables);

            if (objPage.CurrentPage >= objPage.TotalPage - 1)
            {
                objPage.CurrentPage = objPage.TotalPage - 1;
            }

            DbDataReader reader = null;
            StringBuilder tmpsql = new StringBuilder();
            tmpsql.Append(sql);
            CutPageSqlCreater.FillCutPageSql(tmpsql, objPage);
            string qsql = tmpsql.ToString();
            reader =await oper.QueryAsync(qsql, lstParam,CommandType.Text, cacheTables);


            return reader;
        }
        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public static DataTable QueryDataTable(string sql, ParamList lstParam, PageContent objPage,
            DataBaseOperate oper, Type curType,Dictionary<string,bool> cacheTables)
        {
            objPage.TotalRecords = CutPageSqlCreater.GetTotalRecord(lstParam, oper,
                sql,objPage.MaxSelectRecords,cacheTables);
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
                StringBuilder tmpsql = new StringBuilder();
                tmpsql.Append(sql);
                CutPageSqlCreater.FillCutPageSql(tmpsql, objPage);
                string qsql = tmpsql.ToString();
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

        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public static async Task<DataTable> QueryDataTableAsync(string sql, ParamList lstParam, PageContent objPage,
            DataBaseOperate oper, Type curType, Dictionary<string, bool> cacheTables)
        {
            objPage.TotalRecords =await CutPageSqlCreater.GetTotalRecordAsync(lstParam, oper,
                sql, objPage.MaxSelectRecords, cacheTables);

            if (objPage.CurrentPage >= objPage.TotalPage - 1)
            {
                objPage.CurrentPage = objPage.TotalPage - 1;
            }
            if (objPage.CurrentPage >= objPage.TotalPage - 1)
            {
                objPage.CurrentPage = objPage.TotalPage - 1;
            }

            DataTable ret = new DataTable();
            DbDataReader reader = null;
            try
            {
                StringBuilder tmpsql = new StringBuilder();
                tmpsql.Append(sql);
                CutPageSqlCreater.FillCutPageSql(tmpsql, objPage);
                string qsql = tmpsql.ToString();
                reader = await oper.QueryAsync(qsql, lstParam,CommandType.Text, cacheTables);

                if (curType == null)
                {
                    ret = await CacheReader.GenerateDataTableAsync(reader, "newDt", false);
                }
                else
                {
                    ret =await CacheReader.GenerateDataTableAsync(reader, "newDt", curType, false);
                }
            }
            finally
            {
                reader.CloseAsync();
                //oper.CloseDataBase();
            }
            return ret;
        }
    }
}
