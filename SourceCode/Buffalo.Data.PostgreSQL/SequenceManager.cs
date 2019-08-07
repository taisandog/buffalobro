using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Data;
using Buffalo.DB.DbCommon;
using Buffalo.DB.PropertyAttributes;

namespace Buffalo.Data.PostgreSQL
{
    /// <summary>
    /// 主键序列管理
    /// </summary>
    public class SequenceManager
    {
        //private static Dictionary<string, bool> dicSequence = new Dictionary<string, bool>();//序列已经初始化的集合

        /// <summary>
        /// 获取该属性的序列
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetSequenceName(EntityPropertyInfo info) 
        {

            return Buffalo.DB.DataBaseAdapter.Oracle9Adapter.SequenceManager.GetSequenceName(info);
        }


        /// <summary>
        /// 获取默认的序列名
        /// </summary>
        /// <param name="tableName">表</param>
        /// <param name="paramName">字段</param>
        /// <returns></returns>
        internal static string GetDefaultName(string tableName, string paramName)
        {


            return Buffalo.DB.DataBaseAdapter.Oracle9Adapter.SequenceManager.GetDefaultName(tableName,paramName);
        }

        /// <summary>
        /// 初始化序列
        /// </summary>
        /// <param name="seqName"></param>
        public static string GetInitSequence(string seqName,EntityParam prm, DataBaseOperate oper)
        {
            //string dbType = oper.DBInfo.CurrentDbAdapter.DBTypeToSQL(prm.SqlType,4,prm.AllowNull);

            if (!IsSequenceExists(seqName, oper)) //判断是否已经存在序列
            {
                string sql = "CREATE SEQUENCE \"" + seqName + "\" INCREMENT 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1";//创建序列
                return sql;
            }
            return null;


        }

        /// <summary>
        /// 获取序列是否存在
        /// </summary>
        /// <param name="seqName">序列名</param>
        /// <param name="oper">数据库链接</param>
        /// <returns></returns>
        private static bool IsSequenceExists(string seqName, DataBaseOperate oper)
        {
            string sql = "select count(*) from information_schema.sequences where SEQUENCE_NAME='" + seqName + "'";

            IDataReader reader = null;
            int count = 0;
            try
            {

                reader = oper.Query(sql, null,null);
                if (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询序列时候出现错误:" + ex.Message);
            }
            finally
            {
                reader.Close();
            }
            return count > 0;
        }

    }
}
