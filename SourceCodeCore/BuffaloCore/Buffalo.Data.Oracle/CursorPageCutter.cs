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

namespace Buffalo.Data.Oracle
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
        public static DbDataReader Query(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {

            objPage.TotalRecords = CutPageSqlCreater.GetTotalRecord(lstParam, oper, sql, objPage.MaxSelectRecords, null);
            //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
            //objPage.TotalPage = totalPage;
            if (objPage.CurrentPage >= objPage.TotalPage - 1)
            {
                objPage.CurrentPage = objPage.TotalPage - 1;
            }
            DbDataReader reader = null;
            StringBuilder sbTmp = new StringBuilder();
            CutPageSqlCreater.FillCutPageSql(sbTmp, sql, objPage);
            string qsql = sbTmp.ToString();
            reader = oper.Query(qsql, lstParam, null);

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
        public static async Task<DbDataReader> QueryAsync(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {

            objPage.TotalRecords = await CutPageSqlCreater.GetTotalRecordAsync(lstParam, oper, sql, objPage.MaxSelectRecords, null);
            //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
            //objPage.TotalPage = totalPage;
            if (objPage.CurrentPage >= objPage.TotalPage - 1)
            {
                objPage.CurrentPage = objPage.TotalPage - 1;
            }
            DbDataReader reader = null;
            StringBuilder sbTmp = new StringBuilder();
            CutPageSqlCreater.FillCutPageSql(sbTmp, sql, objPage);
            string qsql = sbTmp.ToString();
            reader = await oper.QueryAsync(qsql, lstParam, CommandType.Text, null);

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
        public static DataTable QueryDataTable(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            objPage.TotalRecords = CutPageSqlCreater.GetTotalRecord(lstParam, oper, sql, objPage.MaxSelectRecords, null);
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
                StringBuilder sbTmp = new StringBuilder();

                CutPageSqlCreater.FillCutPageSql(sbTmp, sql, objPage);
                string qsql = sbTmp.ToString();
                reader = oper.Query(qsql, lstParam, null);

                if (curType == null)
                {
                    ret = CacheReader.GenerateDataTable(reader, "newDt", false);
                }
                else
                {
                    ret = CacheReader.GenerateDataTable(reader, "newDt", curType, false);
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
        public static async Task<DataTable> QueryDataTableAsync(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            objPage.TotalRecords = await CutPageSqlCreater.GetTotalRecordAsync(lstParam, oper, sql, objPage.MaxSelectRecords, null);
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
            DbDataReader reader = null;
            try
            {
                StringBuilder sbTmp = new StringBuilder();

                CutPageSqlCreater.FillCutPageSql(sbTmp, sql, objPage);
                string qsql = sbTmp.ToString();
                reader = await oper.QueryAsync(qsql, lstParam, CommandType.Text, null);

                if (curType == null)
                {
                    ret = await CacheReader.GenerateDataTableAsync(reader, "newDt", false);
                }
                else
                {
                    ret = await CacheReader.GenerateDataTableAsync(reader, "newDt", curType, false);
                }
            }
            finally
            {
                await reader.CloseAsync();
                //oper.CloseDataBase();
            }
            return ret;
        }
    }
}