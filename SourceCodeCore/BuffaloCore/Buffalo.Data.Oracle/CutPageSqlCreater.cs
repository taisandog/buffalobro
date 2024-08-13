using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Threading.Tasks;
using System.Data.Common;

namespace Buffalo.Data.Oracle
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
            SelectCondition objCondition, PageContent objPage, bool useCache)
        {

            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//��ʼ��ҳ��
            {
                return "";
            }


            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = GetTotalRecord(list, oper, objCondition.GetSelect(false, false), objPage.MaxSelectRecords,
                    (useCache ? objCondition.CacheTables : null));//��ȡ�ܼ�¼��
                if (objPage.CurrentPage >= objPage.TotalPage - 1)
                {
                    objPage.CurrentPage = objPage.TotalPage - 1;
                }
            }
            string sql = objCondition.GetSelect(false, false);
            StringBuilder tmpsql = new StringBuilder(1024);

            FillCutPageSql(tmpsql, sql, objPage);
            objCondition.FillLock(tmpsql);
            return tmpsql.ToString();
        }
        /// <summary>
        /// ����SQL���
        /// </summary>
        /// <param name="list">�����б�</param>
        /// <param name="oper">���Ӷ���</param>
        /// <param name="objCondition">��������</param>
        /// <param name="objPage">��ҳ��¼��</param>
        /// <returns></returns>
        public static async Task<string> CreatePageSqlAsync(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage, bool useCache)
        {

            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//��ʼ��ҳ��
            {
                return "";
            }


            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = await GetTotalRecordAsync(list, oper, objCondition.GetSelect(false, false), objPage.MaxSelectRecords,
                    (useCache ? objCondition.CacheTables : null));//��ȡ�ܼ�¼��
                if (objPage.CurrentPage >= objPage.TotalPage - 1)
                {
                    objPage.CurrentPage = objPage.TotalPage - 1;
                }
            }
            string sql = objCondition.GetSelect(false, false);
            StringBuilder tmpsql = new StringBuilder(1024);

            FillCutPageSql(tmpsql, sql, objPage);
            objCondition.FillLock(tmpsql);
            return tmpsql.ToString();
        }
        /// <summary>
        /// ��ȡ��ҳ���
        /// </summary>
        /// <param name="sql">Ҫ����ҳ��SQL</param>
        /// <param name="objCondition">��ҳ��</param>
        /// <returns></returns>
        public static void FillCutPageSql(StringBuilder tmpsql, string sql, PageContent objPage)
        {
            long starIndex = objPage.GetStarIndex() + 1;
            long endIndex = objPage.PageSize + starIndex - 1;

            string rowNumberName = "\"cur_rowNumber" + objPage.PagerIndex + "\"";
            tmpsql.Append("SELECT * FROM (SELECT tmp.*, ROWNUM as " + rowNumberName + " FROM (");
            tmpsql.Append(sql);
            tmpsql.Append(") tmp WHERE ROWNUM <= ");
            tmpsql.Append(endIndex.ToString());
            tmpsql.Append(") WHERE " + rowNumberName + " >=");
            tmpsql.Append(starIndex.ToString());


        }
        private static string GetTotalRecordSQL(string sql, long maxRecords)
        {
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
            return tmpsql.ToString();
        }
        /// <summary>
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <param name="part">��ѯ����</param>
        /// <param name="list">�����б�</param>
        /// <param name="oper">ͨ����</param>
        public static long GetTotalRecord(ParamList list, DataBaseOperate oper, string sql
            , long maxRecords, Dictionary<string, bool> cacheTables)
        {
            long totalRecords = 0;
            string tmpsql = GetTotalRecordSQL(sql, maxRecords);
            IDataReader reader = oper.Query(tmpsql, list, cacheTables);
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
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <param name="part">��ѯ����</param>
        /// <param name="list">�����б�</param>
        /// <param name="oper">ͨ����</param>
        public static async Task<long> GetTotalRecordAsync(ParamList list, DataBaseOperate oper, string sql
            , long maxRecords, Dictionary<string, bool> cacheTables)
        {
            long totalRecords = 0;
            string tmpsql = GetTotalRecordSQL(sql, maxRecords);
            DbDataReader reader = await oper.QueryAsync(tmpsql, list, CommandType.Text, cacheTables);
            try
            {
                if (await reader.ReadAsync())
                {
                    if (!(await reader.IsDBNullAsync(0)))
                    {
                        object obj = await reader.GetFieldValueAsync<object>(0);
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