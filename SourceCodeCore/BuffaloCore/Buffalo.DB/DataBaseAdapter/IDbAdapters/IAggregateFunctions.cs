using System;
namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    public interface IAggregateFunctions
    {
        string DoAvg(string paramName);
        string DoCount(string paramName);
        string DoMax(string paramName);
        string DoMin(string paramName);
        string DoStdDev(string paramName);
        string DoSum(string paramName);
    }
}
