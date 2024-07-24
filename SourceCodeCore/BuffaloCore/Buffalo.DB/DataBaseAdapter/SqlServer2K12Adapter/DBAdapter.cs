using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2K12Adapter
{
    public class DBAdapter : SqlServer2K8Adapter.DBAdapter
    {
        public override string CreatePageSql(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage, bool useCache)
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage, useCache);
        }
        public override Task<string> CreatePageSqlAsync(ParamList list, DataBaseOperate oper,
           SelectCondition objCondition, PageContent objPage, bool useCache)
        {
            return CutPageSqlCreater.CreatePageSqlAsync(list, oper, objCondition, objPage, useCache);
        }
    }
}
