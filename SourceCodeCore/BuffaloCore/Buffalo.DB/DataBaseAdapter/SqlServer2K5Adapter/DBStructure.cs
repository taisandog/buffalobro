using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;


namespace Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure
    {

        protected override string GetTableParamsSQL()
        {
            return Resource.tableParam2005;
        }
    }
}
