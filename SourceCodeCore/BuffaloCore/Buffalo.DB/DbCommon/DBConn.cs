using System;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using System.Data.Common;
namespace Buffalo.DB.DbCommon
{
    public class DBConn
    {
        //private static Dictionary<string, string> dicConnString = new Dictionary<string, string>();//�����ַ������ݴ��
        

        /// <summary>
        /// ��ȡָ������
        /// </summary>
        /// <param name="connectionKey">ָ�����ӵ������ַ����ļ�(���ҪĬ�ϼ����Ϊnull)</param>
        /// <returns></returns>
        public static DbConnection GetConnection(DBInfo db)
        {
            string connectionString = db.SelectedConnectionString;
            
            DbConnection conn = db.CurrentDbAdapter.GetConnection(db);
            conn.ConnectionString = connectionString;
            return conn;
        }
        /// <summary>
        /// ��ȡָ��ֻ������
        /// </summary>
        /// <param name="connectionKey">ָ�����ӵ������ַ����ļ�(���ҪĬ�ϼ����Ϊnull)</param>
        /// <returns></returns>
        public static DbConnection GetReadConnection(DBInfo db)
        {
            string connectionString = db.SelectedReadOnlyConnectionString;

            DbConnection conn = db.CurrentDbAdapter.GetConnection(db);
            conn.ConnectionString = connectionString;
            return conn;
        }
    }
}
