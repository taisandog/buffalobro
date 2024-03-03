using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
{
    /// <summary>
    /// 生成分页语句的类
    /// </summary>
    public class CutPageSqlCreater
    {
        /// <summary>
        /// 获取分组分页的片段
        /// </summary>
        /// <param name="objCondition"></param>
        /// <returns></returns>
        public static void GetGroupPart(SelectCondition objCondition, StringBuilder sql)
        {
            sql.Append("select ");
            sql.Append(objCondition.SqlParams.ToString());
            sql.Append(" from ");
            sql.Append(objCondition.Tables.ToString());
            if (objCondition.Condition.Length > 0)
            {
                sql.Append(" where ");
                sql.Append(objCondition.Condition.ToString());
            }

            if (objCondition.GroupBy.Length > 0)
            {
                sql.Append(" group by ");
                sql.Append(objCondition.GroupBy.ToString());
            }
            if (objCondition.Having.Length > 0)
            {
                sql.Append(" having ");
                sql.Append(objCondition.Having.ToString());
            }
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
            SelectCondition objCondition, PageContent objPage,Dictionary<string,bool> cacheTables)
        {
            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//初始化页数
            {
                return "";
            }
            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = GetTotalRecord(list, oper, objCondition, objPage,cacheTables);//获取总记录数
                //long totalPage = (long)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
                //objPage.TotalPage = totalPage;
                if (objPage.CurrentPage >= objPage.TotalPage - 1)
                {
                    objPage.CurrentPage = objPage.TotalPage - 1;
                }
            }
            return CreateCutPageSql(objCondition, objPage);
        }

        /// <summary>
        /// 获取第一页的SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="objCondition"></param>
        /// <param name="objPage"></param>
        /// <returns></returns>
        protected internal static string GetFristPageSql(SelectCondition objCondition, PageContent objPage) 
        {
            StringBuilder sql = new StringBuilder(5000);
            sql.Append("select top ");
            sql.Append(objPage.PageSize.ToString());
            sql.Append(" ");
            
            if (!objCondition.HasGroup)
            {
                sql.Append(objCondition.SqlParams);
                sql.Append("  from ");

                sql.Append(objCondition.Tables);

                sql.Append(" ");
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where ");
                    sql.Append(objCondition.Condition.ToString());
                }
                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by ");
                    sql.Append(objCondition.GroupBy.ToString());
                }
                if (objCondition.Orders.Length > 0)
                {
                    sql.Append(" order by ");
                    sql.Append(objCondition.Orders.ToString());
                }
                if (objCondition.Having.Length > 0)
                {
                    sql.Append(" having ");
                    sql.Append(objCondition.Having.ToString());
                }
            }
            else 
            {
                sql.Append("*  from ");
                sql.Append("(");
                sql.Append("select ");
                sql.Append(objCondition.SqlParams.ToString());
                sql.Append(" from ");
                sql.Append(objCondition.Tables.ToString());
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where ");
                    sql.Append(objCondition.Condition.ToString());
                }

                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by ");
                    sql.Append(objCondition.GroupBy.ToString());
                }
                
                if (objCondition.Having.Length > 0)
                {
                    sql.Append(" having ");
                    sql.Append(objCondition.Having.ToString());
                }
                sql.Append(") tmp");
                if (objCondition.Orders.Length > 0)
                {
                    sql.Append(" order by ");
                    sql.Append(FilterGroupOrderBy(objCondition.Orders.ToString(),"tmp"));
                }
            }
            if (objCondition.LockUpdate.Length > 0)
            {
                sql.Append(objCondition.LockUpdate.ToString());
            }
            return sql.ToString();
        }

        /// <summary>
        /// 处理groupBy
        /// </summary>
        /// <param name="group">groupBy</param>
        /// <param name="newAlias">新别名</param>
        /// <returns></returns>
        protected internal static string FilterGroupOrderBy(string group, string newAlias) 
        {
            string[] prms = group.Split(',');
            StringBuilder sbItem = new StringBuilder();
            foreach (string prm in prms) 
            {
                string tmp = prm;
                int index=tmp.IndexOf('.');
                if (index >= 0) 
                {
                    tmp = newAlias+tmp.Substring(index, tmp.Length - index);
                }
                sbItem.Append(tmp+",");
            }
            if (sbItem.Length > 0) 
            {
                sbItem.Remove(sbItem.Length - 1, 1);
            }
            return sbItem.ToString();
        }

        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="part">SQL条件</param>
        /// <returns></returns>
        private static string CreateCutPageSql(SelectCondition objCondition, PageContent objPage)
        {

            string totalLine = objPage.PageSize.ToString();

            long starIndex = objPage.GetStarIndex();

            if (starIndex == 0)
            {
                //sql.Append("select top " + totalLine + " " + objCondition.SqlParams + "  from [" + objCondition.Tables + "] " + " where " + objCondition.Condition);
                return GetFristPageSql(objCondition, objPage);
            }

            string pkKey = null;
            if (objCondition.PrimaryKey.Count> 1)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string pk in objCondition.PrimaryKey) 
                {
                    sb.Append("convert(varchar," + pk + ",21)+'|'+");
                }
                if (sb.Length > 0) 
                {
                    sb.Remove(sb.Length - 5, 5);
                }
                pkKey = sb.ToString();
            }
            else if (objCondition.PrimaryKey.Count == 1)
            {
                pkKey = objCondition.PrimaryKey[0];
            }

            StringBuilder sql = new StringBuilder(5000);
            //sql.Append("select top " + totalLine + " " + objCondition.SqlParams.ToString() + " from [" + objCondition.Tables.ToString() + "] where(not exists (select * from (select top " + starIndex.ToString() + " " + objCondition.SqlParams + " from [" + objCondition.Tables + "] where " + objCondition.Condition);
            sql.Append("select top ");
            sql.Append(totalLine);
            sql.Append(" ");
            sql.Append(objCondition.SqlParams.ToString());
            sql.Append(" from ");
            sql.Append(objCondition.Tables.ToString());
            sql.Append(" where(" + pkKey + " not in (select top ");
            sql.Append(starIndex.ToString());
            sql.Append(" ");
            sql.Append(pkKey);
            sql.Append(" from ");
            sql.Append(objCondition.Tables.ToString());
            if (objCondition.Condition.Length > 0)
            {
                sql.Append(" where ");
                sql.Append(objCondition.Condition.ToString());
            }
            if (objCondition.GroupBy.Length > 0)
            {
                sql.Append(" group by ");
                sql.Append(objCondition.GroupBy.ToString());
            }
            if (objCondition.Orders.Length > 0)
            {
                sql.Append(" order by ");
                sql.Append(objCondition.Orders.ToString());
            }
            if (objCondition.Having.Length > 0)
            {
                sql.Append(" having ");
                sql.Append(objCondition.Having.ToString());
            }
            sql.Append(")");
            
            sql.Append(")");
            if (objCondition.Condition.Length > 0)
            {
                sql.Append(" and (");
                sql.Append(objCondition.Condition.ToString());
                sql.Append(")");
            }
            if (objCondition.GroupBy.Length > 0)
            {
                sql.Append(" group by ");
                sql.Append(objCondition.GroupBy.ToString());
            }
            if (objCondition.Orders.Length > 0)
            {
                sql.Append(" order by ");
                sql.Append(objCondition.Orders.ToString());
            }
            if (objCondition.Having.Length > 0)
            {
                sql.Append(" having ");
                sql.Append(objCondition.Having.ToString());
            }
            if (objCondition.LockUpdate.Length > 0)
            {
                sql.Append(objCondition.LockUpdate.ToString());
            }
            return sql.ToString();
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="part">查询条件</param>
        /// <param name="list">变量列表</param>
        /// <param name="oper">通用类</param>
        private static long GetTotalRecord(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition,PageContent objPage,Dictionary<string,bool> cacheTables)
        {
            long totalRecords = 0;
            StringBuilder sql = new StringBuilder(5000);

            if (objPage.MaxSelectRecords > 0)
            {
                sql.Append("select count(*) from (select top " );
                sql.Append(objPage.MaxSelectRecords.ToString());
                sql.Append(" * from " );
                sql.Append(objCondition.TablesNoLock);
                sql.Append("" );
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where ");
                    sql.Append(objCondition.Condition.ToString());
                }
                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by ");
                    sql.Append(objCondition.GroupBy.ToString());
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
                sql.Append("select count(*) from " );
                sql.Append(objCondition.TablesNoLock);
                sql.Append("" );
                
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where ");
                    sql.Append(objCondition.Condition.ToString());
                }
                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by ");
                    sql.Append(objCondition.GroupBy.ToString());
                }
                if (objCondition.Having.Length > 0)
                {
                    sql.Append(" having ");
                    sql.Append(objCondition.Having.ToString());
                }
            }


            IDataReader reader = oper.Query(sql.ToString(), list, cacheTables);
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
