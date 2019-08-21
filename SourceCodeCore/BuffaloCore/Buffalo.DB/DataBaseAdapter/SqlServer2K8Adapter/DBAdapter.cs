using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.SqlClient;
using System.Data;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2K8Adapter
{
    public class DBAdapter : SqlServer2K5Adapter.DBAdapter
    {

        /// <summary>
        /// 把DBType类型转成对应的SQLType
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public override string DBTypeToSQL(DbType dbType, long length, bool canNull)
        {
            int type = ToRealDbType(dbType, 10);
            SqlDbType stype = (SqlDbType)type;
            switch (stype)
            {
                case SqlDbType.VarChar:
                    if (length > 8000) 
                    {
                        return "varchar(max)";
                    }
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.Char:
                    if (length > 8000)
                    {
                        return "varchar(max)";
                    }
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.Binary:
                    if (length > 8000)
                    {
                        return "varbinary(max)";
                    }
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.Decimal:
                    long len = length;
                    if (len <= 0) 
                    {
                        len = 19;
                    }
                    return stype.ToString() + "(" + len + ",5)";
                case SqlDbType.NVarChar:
                    if (length > 8000)
                    {
                        return "nvarchar(max)";
                    }
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.NChar:
                    if (length > 8000)
                    {
                        return "nvarchar(max)";
                    }
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.UniqueIdentifier:
                    return "varchar(64)";
                default:
                    return stype.ToString();
            }


        }
    }
}
