using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Data.Oracle
{
    /// <summary>
    /// �ۺϺ�������
    /// </summary>
    public class AggregateFunctions :Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.AggregateFunctions
    {
        public override string DoStdDev(string paramName)
        {
            return "stddev(" + paramName + ")";
        }
    }
}
