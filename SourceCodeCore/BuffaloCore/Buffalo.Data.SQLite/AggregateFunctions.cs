using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Data.SQLite
{
    /// <summary>
    /// �ۺϺ�������
    /// </summary>
    public class AggregateFunctions :Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.AggregateFunctions
    {
        public override string DoStdDev(string paramName)
        {
            throw new Exception("SQLite��֧��StdDev����");
        }
    }
}
 