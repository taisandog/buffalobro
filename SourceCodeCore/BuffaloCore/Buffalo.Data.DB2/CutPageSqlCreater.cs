using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.Data.DB2
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
            //string sql = objCondition.GetSelect();
            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = GetTotalRecord(list, oper, objCondition.GetSelect(false), 
                    objPage.MaxSelectRecords,(useCache?objCondition.CacheTables:null));//��ȡ�ܼ�¼��
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
        /// ���ɷ�ҳSQL���
        /// </summary>
        /// <param name="part">SQL����</param>
        /// <returns></returns>
        private static string GetCutPageSql(SelectCondition objCondition, PageContent objPage)
        {
            string orderBy = null;
            if (objCondition.Orders.Length > 0)//�����������ñ�������
            {
                orderBy = objCondition.Orders.ToString();
            }
            else//���û�о�����������
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
            string rowNumberName = "\"__cur_rowNumber" + objPage.PagerIndex+"\"";
            long endIndex = objPage.PageSize * (objPage.CurrentPage + 1);
            StringBuilder sql = new StringBuilder(5000);
            sql.Append("select * from (");
            sql.Append("select ROW_NUMBER() over(order by " + orderBy + ") as " + rowNumberName + "," );

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
            return sql.ToString();
        }
        /// <summary>
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <param name="part">��ѯ����</param>
        /// <param name="list">�����б�</param>
        /// <param name="oper">ͨ����</param>
        public static long GetTotalRecord(ParamList list, DataBaseOperate oper,string sql,
            long maxRecords,Dictionary<string,bool> cacheTables)
        {
            long totalRecords = 0;
            StringBuilder tmpsql = new StringBuilder(5000);
            if (maxRecords > 0)
            {
                tmpsql.Append("select count(*) from (");
                tmpsql.Append(sql);
                if (maxRecords > 0) 
                {
                    tmpsql.Append(" fetch "+maxRecords+" rows only");
                }
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
