using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;

namespace Buffalo.Data.MySQL
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
            StringBuilder sb = new StringBuilder(2000);
            objCondition.FillSelect(sb, true);


            FillCutPageSql(sb, objPage);
            objCondition.FillLock(sb);
            return sb.ToString();

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
            StringBuilder sb = new StringBuilder(2000);
            objCondition.FillSelect(sb, true);


            FillCutPageSql(sb, objPage);
            objCondition.FillLock(sb);
            return sb.ToString();

        }
        /// <summary>
        /// ��ȡ��ҳ���
        /// </summary>
        /// <param name="sql">Ҫ����ҳ��SQL</param>
        /// <param name="objCondition">��ҳ��</param>
        /// <returns></returns>
        public static void FillCutPageSql(StringBuilder sbSQL, PageContent objPage)
        {
            long starIndex = objPage.GetStarIndex();

            sbSQL.Append(" limit ");
            sbSQL.Append(starIndex.ToString());
            sbSQL.Append(",");
            sbSQL.Append(objPage.PageSize.ToString());


        }

        private static string GetTotalRecordSQL(string sql, long maxRecords)
        {
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
            return tmpsql.ToString();
        }
        /// <summary>
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <param name="part">��ѯ����</param>
        /// <param name="list">�����б�</param>
        /// <param name="oper">ͨ����</param>
        public static long GetTotalRecord(ParamList list, DataBaseOperate oper,
            string sql, long maxRecords, Dictionary<string, bool> cacheTables)
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
        /// <summary>
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <param name="part">��ѯ����</param>
        /// <param name="list">�����б�</param>
        /// <param name="oper">ͨ����</param>
        public static async Task<long> GetTotalRecordAsync(ParamList list, DataBaseOperate oper,
            string sql, long maxRecords, Dictionary<string, bool> cacheTables)
        {
            long totalRecords = 0;
            string tmpsql = GetTotalRecordSQL(sql, maxRecords);
            DbDataReader reader = await oper.QueryAsync(tmpsql, list, CommandType.Text, cacheTables);
            try
            {
                if (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(0))
                    {
                        totalRecords = reader.GetInt64(0);
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