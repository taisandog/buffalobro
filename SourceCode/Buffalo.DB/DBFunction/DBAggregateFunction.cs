using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.DBFunction
{
    public class DBAggregateFunction
    {
        public static string DoAvg(string paramName,DBInfo info) 
        {
            return info.Aggregate.DoAvg(paramName);
        }
        public static string DoCount(string paramName, DBInfo info) 
        {
            return info.Aggregate.DoCount(paramName);
        }
        public static string DoMax(string paramName, DBInfo info) 
        {
            return info.Aggregate.DoMax(paramName);
        }
        public static string DoMin(string paramName, DBInfo info) 
        {
            return info.Aggregate.DoMin(paramName);
        }
        public static string DoStdDev(string paramName, DBInfo info) 
        {
            return info.Aggregate.DoStdDev(paramName);
        }
        public static string DoSum(string paramName, DBInfo info) 
        {
            return info.Aggregate.DoSum(paramName);
        }
    }
}
