using System;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CommBase;
using System.Collections;
using Buffalo.DB.CacheManager.CacheCollection;

namespace Buffalo.DB.CacheManager
{
    public interface ICacheAdaper
    {
        System.Data.DataSet GetData(IDictionary<string,bool> tableNames,string sql,DataBaseOperate oper);
        void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper);
        void RemoveByTableName(string tableName, DataBaseOperate oper);
        bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds, int expirSeconds, DataBaseOperate oper);
        DBInfo Info{get;}

        IList GetEntityList(string key, Type entityType, DataBaseOperate oper);


        bool SetEntityList(string key, IList lstEntity, int expirSeconds, DataBaseOperate oper);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="oper">连接</param>
        /// <returns></returns>
        E GetValue<E>(string key,E defaultValue, DataBaseOperate oper);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="oper">连接</param>
        /// <returns></returns>
        object GetValue(string key, DataBaseOperate oper);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key2">键</param>
        /// <param name="oper">连接</param>
        /// <returns></returns>
        IDictionary<string, object> GetValues(string[] keys, DataBaseOperate oper);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="type">设置值方式</param>
        /// <param name="oper">连接</param>
        /// <param name="expirSeconds">过期时间</param>
        /// <returns></returns>
        bool SetValue<E>(string key, E value, SetValueType type, int expirSeconds, DataBaseOperate oper);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="type">设置值方式</param>
        /// <param name="oper">连接</param>
        /// <param name="expirSeconds">过期时间</param>
        /// <returns></returns>
        bool SetValue(string key, object value, SetValueType type, int expirSeconds, DataBaseOperate oper);
        /// <summary>
        /// Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        bool ExistsKey(string key, DataBaseOperate oper);
        /// <summary>
        /// 设置键过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expirSeconds"></param>
        /// <returns></returns>
        bool SetKeyExpire(string key, int expirSeconds, DataBaseOperate oper);
        /// <summary>
        /// 清空缓存
        /// </summary>
        void ClearAll();
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="oper">连接</param>
        /// <returns></returns>
        bool DeleteValue(string key, DataBaseOperate oper);
        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inc"></param>
        /// <param name="oper">连接</param>
        long DoIncrement(string key, ulong inc, DataBaseOperate oper);

        /// <summary>
        /// 自减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        long DoDecrement(string key, ulong dec, DataBaseOperate oper);

        /// <summary>
        /// 获取缓存操作器
        /// </summary>
        /// <returns></returns>
        object GetClient();

        IEnumerable<string> GetAllKeys(string pattern);

        /// <summary>
        /// 获取哈希表的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        ICacheHash GetHashMap(string key, DataBaseOperate oper);

        /// <summary>
        /// 获取线性表的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        ICacheList GetList(string key, DataBaseOperate oper);

        /// <summary>
        /// 获取锁的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        ICacheLock GetCacheLock(string key, DataBaseOperate oper);

        /// <summary>
        /// 获取排序表的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        ICacheSortedSet GetSortedSet(string key, DataBaseOperate oper);

        /// <summary>
        /// 获取当前缓存连接的配置项
        /// </summary>
        /// <returns></returns>
        object ConnectConfiguration
        {
            get;
            set;
        }
        /// <summary>
        /// 获取当前连接
        /// </summary>
        /// <returns></returns>
        object ConnectClient
        {
            get;
        }
        /// <summary>
        /// 重新连接
        /// </summary>
        void ReconnectClient();
    }
}
