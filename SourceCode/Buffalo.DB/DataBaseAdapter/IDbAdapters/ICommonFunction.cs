using System;
using System.Data;
namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    public interface ICommonFunction
    {
        string IsNull(string[] values);
        string Len(string[] values);
        string Distinct(string[] values);

    }
}
