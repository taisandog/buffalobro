using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using System.Data;

namespace Buffalo.DB.DBFunction
{
    public class DBCommonFunction
    {
        public static string IsNull(string[] values,DBInfo info) 
        {
            return info.Common.IsNull(values);
        }
        public static string Len(string[] values, DBInfo info) 
        {
            return info.Common.Len(values);
        }
        public static string Distinct(string[] values, DBInfo info)
        {
            return info.Common.Distinct(values);
        }

    }
}
