using System;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CommBase;
using System.Collections;
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
        /// 清空缓存
        /// </summary>
        void ClearAll();
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="oper">连接</param>
        /// <returns></returns>
        void DeleteValue(string key, DataBaseOperate oper);
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
        /// 增加到列表
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引(0为增加到头部，-1为增加到尾部)</param>
        /// <param name="value">值</param>
        /// <param name="setType">设置值方式</param>
        /// <returns></returns>
        long ListAddValue<E>(string key, long index, E value, SetValueType setType, DataBaseOperate oper);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="index">值位置</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        E ListGetValue<E>(string key, long index, E defaultValue, DataBaseOperate oper);

        /// <summary>
        /// 获取集合长度
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListGetLength(string key, DataBaseOperate oper);
        /// <summary>
        /// 移除并返回值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="isPopEnd">是否从尾部移除(true则从尾部移除，否则从头部移除)</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        E ListPopValue<E>(string key, bool isPopEnd, E defaultValue, DataBaseOperate oper);
        
        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="count">要移除几个，0则为全部移除</param>
        /// <returns></returns>
        long ListRemoveValue(string key, object value, long count, DataBaseOperate oper);
        

        /// <summary>
        /// 获取集合所有值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="start">起始位置(默认0)</param>
        /// <param name="end">结束位置(-1则为读到末尾)</param>
        /// <returns></returns>
        List<E> ListAllValues<E>(string key, long start, long end, DataBaseOperate oper);
    }
}
