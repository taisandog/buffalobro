using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.DataFillers;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

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
        public static IDataReader Query(string sql, ParamList lstParam, PageContent objPage,
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

            IDataReader reader = null;

            string qsql = CutPageSqlCreater.GetCutPageSql(sql, objPage);
            reader = oper.Query(qsql, lstParam,cacheTables);


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
                string qsql = CutPageSqlCreater.GetCutPageSql(sql, objPage);
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
