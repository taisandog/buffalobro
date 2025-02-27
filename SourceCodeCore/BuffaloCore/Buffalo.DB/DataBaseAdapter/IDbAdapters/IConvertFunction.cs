using System;
namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    public interface IConvertFunction
    {
        string ConvetTo(string value, System.Data.DbType dbType);
        string DateTimeToString(string dateTime, string format);
        string StringToDateTime(string value, string format);
    }
}
