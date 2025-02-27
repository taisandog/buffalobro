using System;
namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    public interface IAdapterLoader
    {
        Buffalo.DB.DataBaseAdapter.IDbAdapters.IAggregateFunctions AggregateFunctions { get; }
        Buffalo.DB.DataBaseAdapter.IDbAdapters.ICommonFunction CommonFunctions { get; }
        Buffalo.DB.DataBaseAdapter.IDbAdapters.IConvertFunction ConvertFunctions { get; }
        Buffalo.DB.DataBaseAdapter.IDbAdapters.IDBAdapter DbAdapter { get; }
        Buffalo.DB.DataBaseAdapter.IDbAdapters.IDBStructure DBStructure { get; }
        Buffalo.DB.DataBaseAdapter.IDbAdapters.IMathFunctions MathFunctions { get; }

        /// <summary>
        /// 数据库版本
        /// </summary>
        /// <param name="version"></param>
        void SetDBVersion(string version);
    }
}
