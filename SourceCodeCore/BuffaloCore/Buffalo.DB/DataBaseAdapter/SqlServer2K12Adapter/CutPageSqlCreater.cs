using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2K12Adapter
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
        public static string CreatePageSql(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage, bool useCache)
        {
            //if (objCondition.Condition == null || objCondition.Condition == "")//初始化查询条件
            //{
            //    objCondition.Condition = "1=1";
            //}
            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//初始化页数
            {
                return "";
            }
            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter.CutPageSqlCreater.GetTotalRecord(list, oper, objCondition, objPage,
                    (useCache ? objCondition.CacheTables : null));//获取总记录数
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
            //if (objCondition.Condition == null || objCondition.Condition == "")//初始化查询条件
            //{
            //    objCondition.Condition = "1=1";
            //}
            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//初始化页数
            {
                return "";
            }
            if (objPage.IsFillTotalRecords)
            {
                objPage.TotalRecords = await Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter.CutPageSqlCreater.GetTotalRecordAsync
                    (list, oper, objCondition, objPage,(useCache ? objCondition.CacheTables : null));//获取总记录数
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
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="part">SQL条件</param>
        /// <returns></returns>
        private static string CreateCutPageSql(SelectCondition objCondition, PageContent objPage)
        {
            string sql = objCondition.GetSelect();
            long starIndex = objPage.GetStarIndex();
            StringBuilder sbSQL = new StringBuilder(sql.Length + 50);
            sbSQL.Append(sql);
            sbSQL.Append(" OFFSET ");
            sbSQL.Append(starIndex.ToString());
            sbSQL.Append(" ROW FETCH NEXT ");
            sbSQL.Append(objPage.PageSize.ToString());
            sbSQL.Append(" rows only");
            //string tmpsql = sql;
            //tmpsql += " OFFSET "+starIndex+" ROW FETCH NEXT "+objPage.PageSize+" rows only";
            return sbSQL.ToString();
        }



        

    }
}
