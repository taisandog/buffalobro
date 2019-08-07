using System;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using System.Data.Common;
namespace Buffalo.DB.DbCommon
{
    public class DBConn
    {
        //private static Dictionary<string, string> dicConnString = new Dictionary<string, string>();//链接字符串的暂存表
        

        /// <summary>
        /// 获取指定连接
        /// </summary>
        /// <param name="connectionKey">指定连接的连接字符串的键(如果要默认键则此为null)</param>
        /// <returns></returns>
        public static DbConnection GetConnection(DBInfo db)
        {
            string connectionString = db.ConnectionString;
            
            DbConnection conn = db.CurrentDbAdapter.GetConnection(db);
            conn.ConnectionString = connectionString;
            return conn;
        }
        /// <summary>
        /// 获取指定只读连接
        /// </summary>
        /// <param name="connectionKey">指定连接的连接字符串的键(如果要默认键则此为null)</param>
        /// <returns></returns>
        public static DbConnection GetReadConnection(DBInfo db)
        {
            string connectionString = db.ReadOnlyConnectionString;

            DbConnection conn = db.CurrentDbAdapter.GetConnection(db);
            conn.ConnectionString = connectionString;
            return conn;
        }
    }
}
