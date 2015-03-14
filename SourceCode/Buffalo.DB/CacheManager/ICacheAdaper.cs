using System;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
namespace Buffalo.DB.CacheManager
{
    public interface ICacheAdaper
    {
        System.Data.DataSet GetData(IDictionary<string,bool> tableNames,string sql,DataBaseOperate oper);
        void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper);
        void RemoveByTableName(string tableName, DataBaseOperate oper);
        bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds, DataBaseOperate oper);
        DBInfo Info{get;}

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        IDictionary<string, object> GetValues(string[] keys, DataBaseOperate oper);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        void SetValue<E>(string key, E value, DataBaseOperate oper);
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        void DeleteValue(string key, DataBaseOperate oper);
        /// <summary>
        /// 自增1
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        void DoIncrement(string key, DataBaseOperate oper);
    }
}
