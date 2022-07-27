using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.Common;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.DataBaseAdapter;
using Oracle.ManagedDataAccess.Client;

namespace Buffalo.Data.Oracle
{
    public class DBAdapter : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.DBAdapter
    {
        
        /// <summary>
        /// 获取参数类
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="type">参数数据库类型</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="paramDir">参数进出类型</param>
        /// <returns></returns>
        public override IDataParameter GetDataParameter(string paramName, DbType type, object paramValue, ParameterDirection paramDir)
        {
            IDataParameter newParam = new OracleParameter();
            newParam.ParameterName = paramName;
            newParam.DbType = FormatDbType(type);
            
            newParam.Value = paramValue;
            newParam.Direction = paramDir;
            return newParam;
        }
        /// <summary>
        /// 格式化数据库类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override DbType FormatDbType(DbType type)
        {
            switch (type)
            {
                case DbType.UInt16:
                    return DbType.Int16;

                case DbType.UInt64:
                    return DbType.Int64;

                case DbType.UInt32:
                    return DbType.Int32;

                case DbType.DateTime2:
                    return DbType.DateTime;

                case DbType.DateTimeOffset:
                    return DbType.Int32;
                case DbType.Currency:
                case DbType.VarNumeric:
                    return DbType.Decimal;
                case DbType.Guid:
                case DbType.Xml:
                    return DbType.String;

                default:
                    return type;
            }
        }
       
        /// <summary>
        /// 获取SQL命令类
        /// </summary>
        /// <returns></returns>
        public override IDbCommand GetCommand()
        {
            IDbCommand comm = new OracleCommand();
            return comm;
        }
        /// <summary>
        /// 获取SQL连接
        /// </summary>
        /// <returns></returns>
        public override DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new OracleConnection();
            return conn;
        }
        /// <summary>
        /// 获取SQL适配器
        /// </summary>
        /// <returns></returns>
        public override IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new OracleDataAdapter();
            return adapter;
        }
        /// <summary>
        /// 把DBType类型转成对应的SQLType
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public override string DBTypeToSQL(DbType dbType, long length,bool canNull)
        {
            switch (dbType)
            {

                case DbType.AnsiString:
                    if (length < 2000)
                    {
                        return "VARCHAR2(" + length + ")";
                    }
                    else
                    {
                        return "CLOB";
                    }
                case DbType.AnsiStringFixedLength:
                    if (length < 8000)
                    {
                        return "Char(" + length + ")";
                    }
                    else
                    {
                        return "CLOB";
                    }
                case DbType.Binary:
                    if (length < 2000)
                    {
                        return "RAW(" + length + ")";
                    }
                    else
                    {
                        return "BLOB";
                    }
                case DbType.Boolean:
                    return "Number(1,0)";
                case DbType.Byte:
                    return "Number(3,0)";
                case DbType.Date:
                    return "DATE";
                case DbType.DateTimeOffset:
                    return "TIMESTAMP WITH TIME ZONE";
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.Time:
                    return "TIMESTAMP";
                case DbType.Decimal:
                    if (length <= 0)
                    {
                        return "Number(30,30)";

                    }
                    return DBInfo.GetNumberLengthType("Number", length); 
                case DbType.Double:
                    return "BINARY_DOUBLE";
                case DbType.Currency:
                case DbType.VarNumeric:
                    return "Number(30,30)";
                case DbType.Single:
                    return "BINARY_FLOAT";
                case DbType.Int64:
                case DbType.UInt64:
                case DbType.UInt32:
                    return "Number(*,0)";

                case DbType.Int16:
                    return "SMALLINT";
                case DbType.UInt16:
                    return "Number(6,0)";
                case DbType.Int32:
                    return "INTEGER";
                case DbType.SByte:
                    return "Number(4,0)";
                case DbType.Guid:
                    return "VARCHAR2(64)";
                case DbType.String:
                    if (length < 8000)
                    {
                        return "NVARCHAR2(" + length + ")";
                    }
                    else
                    {
                        return "NCLOB";
                    }
                case DbType.StringFixedLength:
                    if (length < 8000)
                    {
                        return "NChar(" + length + ")";
                    }
                    else
                    {
                        return "NCLOB";
                    }
                default:
                    return "BLOB";
            }



        }
    }
}
