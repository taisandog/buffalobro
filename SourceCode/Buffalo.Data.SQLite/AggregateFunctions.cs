using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Data.SQLite
{
    /// <summary>
    /// 聚合函数处理
    /// </summary>
    public class AggregateFunctions :Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.AggregateFunctions
    {
        public override string DoStdDev(string paramName)
        {
            throw new Exception("SQLite不支持StdDev函数");
        }
    }
}
 