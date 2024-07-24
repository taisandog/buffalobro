using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter
{
    public class DBAdapter : SqlServer2KAdapter.DBAdapter
    {
        public override string CreatePageSql(ParamList list, DataBaseOperate oper, 
            SelectCondition objCondition, PageContent objPage,bool useCache)
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage, useCache);
        }
        public override Task<string> CreatePageSqlAsync(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage, bool useCache)
        {
            return CutPageSqlCreater.CreatePageSqlAsync(list, oper, objCondition, objPage, useCache);
        }
        /// <summary>
        /// 全文搜索时候排序字段是否显示表达式
        /// </summary>
        public override bool IsShowExpression
        {
            get
            {
                return true;
            }
        }
        ///// <summary>
        ///// 获取变量列表
        ///// </summary>
        //public override ParamList BQLSelectParamList
        //{
        //    get
        //    {
        //        return new ParamList();
        //    }
        //}
        
    }
}
