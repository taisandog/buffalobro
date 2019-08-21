using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter
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
            //if (objCondition.Condition == null || objCondition.Condition == "")//��ʼ����ѯ����
            //{
            //    objCondition.Condition = "1=1";
            //}
            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//��ʼ��ҳ��
            {
                return "";
            }
            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = GetTotalRecord(list, oper, objCondition,objPage,
                    (useCache?objCondition.CacheTables:null));//��ȡ�ܼ�¼��
                //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
                //objPage.TotalPage = totalPage;
                if (objPage.CurrentPage >= objPage.TotalPage - 1)
                {
                    objPage.CurrentPage = objPage.TotalPage - 1;
                    //objCondition.CurrentPage = objPage.CurrentPage;
                }
            }
            return CreateCutPageSql(objCondition, objPage);
        }

        /// <summary>
        /// ���ɷ�ҳSQL���
        /// </summary>
        /// <param name="part">SQL����</param>
        /// <returns></returns>
        private static string CreateCutPageSql(SelectCondition objCondition, PageContent objPage)
        {
            string orderBy = null;
            if (objCondition.Orders.Length>0)//�����������ñ�������
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
            StringBuilder sql = new StringBuilder(5000);

            if (objPage.GetStarIndex() == 0)
            {
                return Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.GetFristPageSql(objCondition, objPage);
            }

            long starIndex = objPage.GetStarIndex() + 1;
            string rowNumberName = "[cur_rowNumber" + objPage.PagerIndex+"]";
            long topRec = objPage.GetStarIndex() + objPage.PageSize;

            //sql.Append("select top " + objPage.PageSize + " * from(");
            sql.Append("select ");
            string newOrderBy = null;
            if (objCondition.HasGroup)
            {
                sql.Append("*");
                newOrderBy = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.FilterGroupOrderBy(orderBy, "[_tmpInnerTable]");
            }
            else 
            {
                sql.Append("*");
                newOrderBy = orderBy;
            }
            sql.Append(" from (");
            //sql.Append("select row_number() over(order by " + newOrderBy + ") as " +
            //  rowNumberName + ",");
            sql.Append("select row_number() over(order by " + newOrderBy + ") as " +
                rowNumberName + ",");

            if (!objCondition.HasGroup)
            {
                sql.Append(objCondition.SqlParams.ToString());
                sql.Append("  from ");
                sql.Append(objCondition.Tables.ToString());
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
                sql.Append("[_tmpInnerTable].*");
                sql.Append("  from (");
               Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.GetGroupPart(objCondition, sql);
                sql.Append(") [_tmpInnerTable]");
            }
            sql.Append(") tmp where " + rowNumberName + " between " + starIndex + " and " + topRec);
            sql.Append(" order by ");
            sql.Append(rowNumberName);
            return sql.ToString();
        }

        

        /// <summary>
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <param name="part">��ѯ����</param>
        /// <param name="list">�����б�</param>
        /// <param name="oper">ͨ����</param>
        private static long GetTotalRecord(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage,Dictionary<string,bool> cacheTables)
        {
            long totalRecords = 0;
            StringBuilder sql = new StringBuilder();
            if (objPage.MaxSelectRecords > 0)
            {

                sql.Append("select count(*) from (select top " + objPage.MaxSelectRecords + " * from " );

                if (!objCondition.HasGroup)
                {
                    sql.Append(objCondition.Tables).ToString();
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
                    sql.Append(") tab");
                }
                else 
                {
                   Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.GetGroupPart(objCondition, sql);
                    sql.Append(") tab");
                }
            }
            else
            {
                sql.Append("select count(*) from ");
                if (!objCondition.HasGroup)
                {
                    sql.Append(objCondition.Tables.ToString());
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
                    sql.Append("(");
                   Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.GetGroupPart(objCondition, sql);
                    sql.Append(") tmp");

                }
            }
            IDataReader reader = oper.Query(sql.ToString(), list,cacheTables);
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
