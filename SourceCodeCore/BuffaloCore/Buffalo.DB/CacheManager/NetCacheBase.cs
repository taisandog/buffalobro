﻿using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel;
using System.Data;
using System.Collections;
using Buffalo.DB.CacheManager.CacheCollection;
using System.Threading.Tasks;
using Buffalo.Kernel.Defaults;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 分布式缓存基类
    /// </summary>
    public abstract class NetCacheBase<T> : ICacheAdaper where T : IDisposable
    {
        /// <summary>
        /// 查询缓存
        /// </summary>
        private static Dictionary<string, DataSetCacheItem> _dicQueryCache = new Dictionary<string, DataSetCacheItem>();
        /// <summary>
        /// 过期时间(分钟)
        /// </summary>
        protected TimeSpan _expiration=TimeSpan.MinValue;
        /// <summary>
        /// 过期时间(分钟)
        /// </summary>
        public TimeSpan Expiration
        {
            get { return _expiration; }
        }
        protected DBInfo _info;
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBInfo Info
        {
            get { return _info; }
        }
        protected bool _throwExcertion = false;
        /// <summary>
        /// 抛出异常
        /// </summary>
        public bool ThrowExcertion
        {
            get { return _throwExcertion; }
        }

        public abstract object ConnectConfiguration { get ; set; }
        public abstract object ConnectClient { get ;  }
        /// <summary>
        /// 重新连接
        /// </summary>
        public abstract void ReconnectClient();

        #region ICacheAdaper 成员

        ///// <summary>
        ///// 把表名集合换成已排序的集合
        ///// </summary>
        ///// <param name="dicTables"></param>
        ///// <returns></returns>
        //internal static List<string> GetSortTables(IDictionary<string, bool> dicTables)
        //{
        //    List<string> ret = new List<string>(dicTables.Count);
        //    foreach (KeyValuePair<string, bool> kvp in dicTables)
        //    {
        //        ret.Add(kvp.Key);
        //    }
        //    ret.Sort();
        //    return ret;
        //}
        /// <summary>
        /// 设置键过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expirSeconds"></param>
        /// <returns></returns>
        public bool SetKeyExpire(string key, TimeSpan expir, DataBaseOperate oper) 
        {
            using (T client = CreateClient(false, QueryCacheCommand.CommandSetValues))
            {
                return DoSetKeyExpire(key, expir, client);
            }
        }

        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <returns></returns>
        protected abstract T CreateClient(bool realOnly,string cmd);
        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <returns></returns>
        protected abstract Task<T> CreateClientAsync(bool realOnly, string cmd);

        public System.Data.DataSet GetData(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(true, QueryCacheCommand.CommandGetDataSet))
                {
                    //client.PrimitiveAsString = true;
                    string sqlMD5 = GetSQLMD5(sql);
                    DataSetCacheItem dataItem=ValueConvertExtend.GetMapDataValue<DataSetCacheItem>(_dicQueryCache,sqlMD5);
                    if(dataItem==null)
                    {
                        return null;
                    }
                    bool isVersion = ComparVersion(tableNames, sqlMD5, dataItem.TablesVersion, client);//判断版本号
                    if (!isVersion)
                    {
                        return null;
                    }

                    

                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandGetDataSet, sql, oper);
                    }

                    return dataItem.Data;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    return null;
                }
            }
        }

        public async Task<System.Data.DataSet> GetDataAsync(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            try
            {
                using (T client =await CreateClientAsync(true, QueryCacheCommand.CommandGetDataSet))
                {
                    //client.PrimitiveAsString = true;
                    string sqlMD5 = GetSQLMD5(sql);
                    DataSetCacheItem dataItem = ValueConvertExtend.GetMapDataValue<DataSetCacheItem>(_dicQueryCache, sqlMD5);
                    if (dataItem == null)
                    {
                        return null;
                    }
                    bool isVersion = await ComparVersionAsync(tableNames, sqlMD5, dataItem.TablesVersion, client);//判断版本号
                    if (!isVersion)
                    {
                        return null;
                    }



                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandGetDataSet, sql, oper);
                    }

                    return dataItem.Data;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetTableName(string tableName)
        {
            StringBuilder sbInfo = new StringBuilder(tableName.Length + 10);
            sbInfo.Append(_info.FullName);
            sbInfo.Append(".");
            sbInfo.Append(tableName);
            return PasswordHash.ToSHA1String(sbInfo.ToString());
        }

        /// <summary>
        /// 获取SQL语句的键
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="client">创建器</param>
        /// <returns></returns>
        private string GetSQLMD5(string sql)
        {
            StringBuilder sbSql = new StringBuilder(256);
            StringBuilder sbSqlInfo = new StringBuilder();
            sbSqlInfo.Append(_info.FullName);
            sbSqlInfo.Append(":");
            sbSqlInfo.Append(sql);
            sbSql.Append(PasswordHash.ToSHA1String(sbSqlInfo.ToString()));
            return sbSql.ToString();
        }
        /// <summary>
        /// 获取版本号的键
        /// </summary>
        /// <param name="md5">哈希值</param>
        /// <returns></returns>
        private string FormatVersionKey(string md5)
        {
            return "v." + md5;
        }
        /// <summary>
        /// 对比版本
        /// </summary>
        /// <param name="tableNames">表名集合</param>
        /// <param name="md5">sql语句的MD5</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        private bool ComparVersion(IDictionary<string, bool> tableNames, string md5, Dictionary<string, int> dicTableVers, T client)
        {
            //string sqlKey = FormatVersionKey(md5);
            string[] keys = GetKeys(tableNames);
            IDictionary<string, object> tableVars = GetValues(keys, client);

            
            object tmp = null;
            foreach (KeyValuePair<string, int> kvp in dicTableVers)
            {
                if (!tableVars.TryGetValue(kvp.Key, out tmp))
                {
                    return false;
                }
                if (ValueConvertExtend.ConvertValue<int>(tmp) != kvp.Value)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 对比版本
        /// </summary>
        /// <param name="tableNames">表名集合</param>
        /// <param name="md5">sql语句的MD5</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        private async Task<bool> ComparVersionAsync(IDictionary<string, bool> tableNames, string md5, Dictionary<string, int> dicTableVers, T client)
        {
            //string sqlKey = FormatVersionKey(md5);
            string[] keys = GetKeys(tableNames);
            IDictionary<string, object> tableVars = await GetValuesAsync(keys, client);


            object tmp = null;
            foreach (KeyValuePair<string, int> kvp in dicTableVers)
            {
                if (!tableVars.TryGetValue(kvp.Key, out tmp))
                {
                    return false;
                }
                if (ValueConvertExtend.ConvertValue<int>(tmp) != kvp.Value)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 设置键过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expirSeconds"></param>
        /// <returns></returns>
        public abstract bool DoSetKeyExpire(string key, TimeSpan expir, T client);
        /// <summary>
        /// 设置键过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expirSeconds"></param>
        /// <returns></returns>
        public abstract Task<bool> DoSetKeyExpireAsync(string key, TimeSpan expir, T client);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract E GetValue<E>(string key, E defaultValue, T client);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract Task<E> GetValueAsync<E>(string key, E defaultValue, T client);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract object GetValue(string key, T client);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract Task<object> GetValueAsync(string key, T client);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract bool DoExistsKey(string key, T client);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract Task<bool> DoExistsKeyAsync(string key, T client);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract IDictionary<string, object> GetValues(string[] keys, T client);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract Task<IDictionary<string, object>> GetValuesAsync(string[] keys, T client);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="type">设置方式</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract bool SetValue<E>(string key, E value, SetValueType type, TimeSpan expir, T client);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="type">设置方式</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract Task<bool> SetValueAsync<E>(string key, E value, SetValueType type, TimeSpan expir, T client);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="type">设置方式</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract bool SetValue(string key, object value, SetValueType type, TimeSpan expir, T client);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="type">设置方式</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract Task<bool> SetValueAsync(string key, object value, SetValueType type, TimeSpan expir, T client);
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract DataSet DoGetDataSet(string key,  T client);
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract Task<DataSet> DoGetDataSetAsync(string key, T client);
        /// <summary>
        /// 设置DataSet
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">DataSet</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract bool DoSetDataSet(string key, DataSet value, TimeSpan expir, T client);
        /// <summary>
        /// 设置DataSet
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">DataSet</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract Task<bool> DoSetDataSetAsync(string key, DataSet value, TimeSpan expir, T client);
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract bool DeleteValue(string key, T client);
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract Task<bool> DeleteValueAsync(string key, T client);
        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="inc">自增值</param>
        /// <param name="client"></param>
        protected abstract long DoIncrement(string key, ulong inc, T client);
        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="inc">自增值</param>
        /// <param name="client"></param>
        protected abstract Task<long> DoIncrementAsync(string key, ulong inc, T client);
        /// <summary>
        /// 自减
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dec">自减值</param>
        /// <param name="client"></param>
        protected abstract long DoDecrement(string key, ulong dec, T client);
        /// <summary>
        /// 自减
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dec">自减值</param>
        /// <param name="client"></param>
        protected abstract Task<long> DoDecrementAsync(string key, ulong dec, T client);

        /// <summary>
        /// 缓存服务器类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected abstract string GetCacheName();
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public abstract System.Collections.IList DoGetEntityList(string key,Type entityType, T client);
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public abstract Task<System.Collections.IList> DoGetEntityListAsync(string key, Type entityType, T client);

        /// <summary>
        /// 设置属性集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lstEntity"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public abstract bool DoSetEntityList(string key, System.Collections.IList lstEntity, TimeSpan expir, T client);
        /// <summary>
        /// 设置属性集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lstEntity"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public abstract Task<bool> DoSetEntityListAsync(string key, System.Collections.IList lstEntity, TimeSpan expir, T client);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public bool ExistsKey(string key, DataBaseOperate oper)
        {
            using (T client = CreateClient(false, QueryCacheCommand.CommandGetValues))
            {
                return DoExistsKey(key, client);
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public E GetValue<E>(string key, E defaultValue, DataBaseOperate oper)
        {
            using (T client = CreateClient(false, QueryCacheCommand.CommandGetValues))
            {
                return GetValue<E>(key, defaultValue, client);
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public object GetValue(string key, DataBaseOperate oper)
        {
            using (T client = CreateClient(false, QueryCacheCommand.CommandGetValues))
            {
                return GetValue(key, client);
            }
        }
        /// <summary>
        /// 获取要申请的键集合
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="tableMD5"></param>
        /// <returns></returns>
        private string[] GetKeys(IDictionary<string, bool> tableNames) 
        {
            int len = tableNames.Count;
            
            string[] keys = new string[len];
            int index = 0;
            foreach (KeyValuePair<string, bool> kvp in tableNames)
            {
                string key = GetTableName(kvp.Key);
                keys[index] = key;
                index++;
            }
            
            return keys;
        }

        /// <summary>
        /// 获取当前库中所有表的版本号
        /// </summary>
        /// <param name="tableNames">表名集合</param>
        /// <param name="client">Redis连接</param>
        /// <param name="needCreateTableVer">是否需要创建表的键</param>
        /// <returns></returns>
        private Dictionary<string, int> GetTablesVersion(IDictionary<string, bool> tableNames, 
            IDictionary<string, object> tableVars, T client, bool needCreateTableVer)
        {
            Dictionary<string, int> dicTableVers = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
            
            
            //IDictionary<string, object> tableVars = GetValues(keys, client);
            object objVer = null;
            foreach (KeyValuePair<string, bool> kvp in tableNames)
            {
                string key = GetTableName(kvp.Key);
                if (!tableVars.TryGetValue(key, out objVer))
                {
                    objVer = null;
                }
                if (objVer == null)
                {
                    if (!needCreateTableVer)
                    {
                        return null;
                    }
                    else
                    {
                        SetValue(key,1,SetValueType.Set,TimeSpan.MinValue, client);
                        objVer = "1";
                    }
                }
                dicTableVers[kvp.Key] = ValueConvertExtend.ConvertValue<int>(objVer);
            }


            return dicTableVers;
        }
        /// <summary>
        /// 获取当前库中所有表的版本号
        /// </summary>
        /// <param name="tableNames">表名集合</param>
        /// <param name="client">Redis连接</param>
        /// <param name="needCreateTableVer">是否需要创建表的键</param>
        /// <returns></returns>
        private async Task<Dictionary<string, int>> GetTablesVersionAsync(IDictionary<string, bool> tableNames,
            IDictionary<string, object> tableVars, T client, bool needCreateTableVer)
        {
            Dictionary<string, int> dicTableVers = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);


            //IDictionary<string, object> tableVars = GetValues(keys, client);
            object objVer = null;
            bool ret = false;
            foreach (KeyValuePair<string, bool> kvp in tableNames)
            {
                string key = GetTableName(kvp.Key);
                if (!tableVars.TryGetValue(key, out objVer))
                {
                    objVer = null;
                }
                if (objVer == null)
                {
                    if (!needCreateTableVer)
                    {
                        return null;
                    }
                    else
                    {
                        ret = await SetValueAsync(key, 1, SetValueType.Set, TimeSpan.MinValue, client);
                        objVer = "1";
                    }
                }
                dicTableVers[kvp.Key] = ValueConvertExtend.ConvertValue<int>(objVer);
            }


            return dicTableVers;
        }
        /// <summary>
        /// 获取当前库中表的版本号字符串
        /// </summary>
        /// <param name="tableNames">表名集合</param>
        /// <param name="client">Redis连接</param>
        /// <param name="needCreateTableVer">是否需要创建表的键</param>
        /// <returns></returns>
        private Dictionary<string, int> GetTablesVerString(IDictionary<string, bool> tableNames, T client, bool needCreateTableVer)
        {
            string[] keys = GetKeys(tableNames);
            IDictionary<string, object> tableVars = GetValues(keys, client);
            Dictionary<string, int> dicTableVers = GetTablesVersion(tableNames, tableVars, client, needCreateTableVer);
            Dictionary<string, int> ret = new Dictionary<string, int>(dicTableVers.Count, StringComparer.CurrentCultureIgnoreCase);
            foreach (KeyValuePair<string, int> kvp in dicTableVers)
            {
                ret[GetTableName(kvp.Key)] = kvp.Value;
            }

            return ret;
        }
        /// <summary>
        /// 获取当前库中表的版本号字符串
        /// </summary>
        /// <param name="tableNames">表名集合</param>
        /// <param name="client">Redis连接</param>
        /// <param name="needCreateTableVer">是否需要创建表的键</param>
        /// <returns></returns>
        private async Task<Dictionary<string, int>> GetTablesVerStringAsync(IDictionary<string, bool> tableNames, T client, bool needCreateTableVer)
        {
            string[] keys = GetKeys(tableNames);
            IDictionary<string, object> tableVars = await GetValuesAsync(keys, client);
            Dictionary<string, int> dicTableVers = await GetTablesVersionAsync(tableNames, tableVars, client, needCreateTableVer);
            Dictionary<string, int> ret = new Dictionary<string, int>(dicTableVers.Count, StringComparer.CurrentCultureIgnoreCase);
            foreach (KeyValuePair<string, int> kvp in dicTableVers)
            {
                ret[GetTableName(kvp.Key)] = kvp.Value;
            }

            return ret;
        }
        /// <summary>
        /// 获取当前查询的版本号
        /// </summary>
        /// <param name="md5">SQL的md5</param>
        /// <param name="client">Redis连接</param>
        /// <returns></returns>
        private Dictionary<string, string> GetDataVersion(string vers, T client)
        {
            //string md5 = GetSQLKey(sql);
            
            Dictionary<string, string> dicDataVers = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            if (CommonMethods.IsNullOrWhiteSpace(vers))
            {
                return null;
            }
            string[] verItems = vers.Split('\n');
            foreach (string verItem in verItems)
            {
                if (string.IsNullOrEmpty(verItem))
                {
                    continue;
                }
                string[] part = verItem.Split('=');
                if (part.Length < 2)
                {
                    continue;
                }

                dicDataVers[part[0]] = part[1];
            }
            return dicDataVers;
        }

        public void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(false, QueryCacheCommand.CommandDeleteSQL))
                {

                    //client.PrimitiveAsString = true;
                    string md5 = GetSQLMD5(sql);
                    string verKey = FormatVersionKey(md5);
                    if (!string.IsNullOrEmpty(md5))
                    {
                        DeleteValue(md5, client);
                        DeleteValue(verKey, client);
                    }
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandDeleteSQL, sql, oper);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                }
            }
        }
        /// <summary>
        /// 最大版本号
        /// </summary>
        protected const int MaxVersion = (int.MaxValue - 1000);
        /// <summary>
        /// 根据表名删除缓存
        /// </summary>
        /// <param name="tableName"></param>
        public void RemoveByTableName(string tableName, DataBaseOperate oper)
        {
            try
            {
                string key = GetTableName(tableName);
                using (T client = CreateClient(false, QueryCacheCommand.CommandDeleteTable))
                {
                    //client.PrimitiveAsString = true;
                    int val = ValueConvertExtend.ConvertValue<int>(GetValue<object>(key,-1, client));

                    if (val <= 0 || val >= MaxVersion)
                    {
                        SetValue(key, 1, SetValueType.Set, TimeSpan.MinValue, client);
                        //client.Set(key, 1, _expiration);
                    }
                    else
                    {
                        DoIncrement(key,1, client);
                    }
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandDeleteTable, tableName, oper);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                }
            }
        }
        

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="sql"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds, TimeSpan expir, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(false, QueryCacheCommand.CommandSetDataSet))
                {
                    string md5 = GetSQLMD5(sql);
                    Dictionary<string,int> verValue = GetTablesVerString(tableNames, client, true);

                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandSetDataSet, sql, oper);
                    }
                    //SetValue<string>(verKey, verValue, expirSeconds, client);
                    //return DoSetDataSet(md5, ds, expirSeconds, client);
                    DataSetCacheItem item = new DataSetCacheItem();
                    item.Data = ds;
                    item.MD5 = md5;
                    item.TablesVersion = verValue;
                    _dicQueryCache[md5] = item;
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    return false;
                }
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="sql"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public async Task<bool> SetDataAsync(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds, TimeSpan expir, DataBaseOperate oper)
        {
            try
            {
                using (T client = await CreateClientAsync(false, QueryCacheCommand.CommandSetDataSet))
                {
                    string md5 = GetSQLMD5(sql);
                    Dictionary<string, int> verValue = await GetTablesVerStringAsync(tableNames, client, true);

                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandSetDataSet, sql, oper);
                    }
                    
                    DataSetCacheItem item = new DataSetCacheItem();
                    item.Data = ds;
                    item.MD5 = md5;
                    item.TablesVersion = verValue;
                    _dicQueryCache[md5] = item;
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return false;
                }
            }
        }
        private void OutPutMessage(string type, string message, DataBaseOperate oper)
        {
            oper.OutMessage(MessageType.QueryCache, GetCacheName(), type, message);

        }
        private async Task OutPutMessageAsync(string type, string message, DataBaseOperate oper)
        {
            await oper.OutMessageAsync(MessageType.QueryCache, GetCacheName(), type, message);

        }
        private void OutExceptionMessage(Exception ex, DataBaseOperate oper)
        {
            MessageInfo info = new MessageInfo();
            info.Value = ex;
            info.Type = GetCacheName();
            oper.OutMessage(MessageType.CacheException, info);

        }
        private async Task OutExceptionMessageAsync(Exception ex, DataBaseOperate oper)
        {
            MessageInfo info = new MessageInfo();
            info.Value = ex;
            info.Type = GetCacheName();
            await oper.OutMessageAsync(MessageType.CacheException, info);

        }
        #endregion

        #region ICacheAdaper 成员


        public IDictionary<string, object> GetValues(string[] keys, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(false, QueryCacheCommand.CommandGetValues))
                {
                    IDictionary<string, object> ret=GetValues(keys, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        StringBuilder sbKeys = new StringBuilder();
                        sbKeys.Append("keys={");
                        foreach (string key in keys) 
                        {
                            sbKeys.Append(key);
                            sbKeys.Append(',');
                        }
                        if (sbKeys[sbKeys.Length - 1] == ',') 
                        {
                            sbKeys.Remove(sbKeys.Length-1, 1);
                        }
                        sbKeys.Append("}");
                        OutPutMessage(QueryCacheCommand.CommandGetValues, sbKeys.ToString(), oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    return null;
                }
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="type">设置值方式</param>
        /// <param name="expirSeconds">过期时间(-1为默认)</param>
        /// <param name="oper">连接</param>
        public bool SetValue<E>(string key, E value,SetValueType type, TimeSpan expir, DataBaseOperate oper)
        {
            bool ret = false;
            try
            {
                using (T client = CreateClient(false, QueryCacheCommand.CommandSetValues))
                {
                    ret = SetValue<E>(key, value,type, expir, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandSetValues, "key="+key, oper);
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    
                }
            }
            return ret;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expirSeconds">过期时间(-1为默认)</param>
        /// <param name="oper">连接</param>
        public bool SetValue(string key, object value,SetValueType type, TimeSpan expir, DataBaseOperate oper)
        {
            bool ret = false;
            try
            {
                using (T client = CreateClient(false, QueryCacheCommand.CommandSetValues))
                {
                    ret = SetValue(key, value,type, expir, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandSetValues, "key=" + key, oper);
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);

                }
            }
            return ret;
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="oper"></param>
        public bool DeleteValue(string key, DataBaseOperate oper)
        {
            bool ret = false;
            try
            {
                using (T client = CreateClient(false, QueryCacheCommand.CommandDeleteSQL))
                {
                    ret = DeleteValue(key, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandDeleteValues, "key="+key, oper);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);

                }
            }
            return ret;
        }
        /// <summary>
        /// 增加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inc"></param>
        /// <param name="oper"></param>
        public long DoIncrement(string key, ulong inc, DataBaseOperate oper)
        {
            long ret = 0;
            try
            {
                using (T client = CreateClient(false, QueryCacheCommand.CommandIncrement))
                {
                    ret = DoIncrement(key, inc, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandIncrement, "key=" + key + ";inc=" + inc, oper);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);

                }
            }
            return ret;
        }
        /// <summary>
        /// 增减值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inc"></param>
        /// <param name="oper"></param>
        public long DoDecrement(string key, ulong dec, DataBaseOperate oper)
        {
            long ret = 0;
            try
            {
                using (T client = CreateClient(false, QueryCacheCommand.CommandIncrement))
                {
                    ret=DoDecrement(key, dec, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandIncrement, "key=" + key + ";dec=" + dec, oper);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);

                }
            }
            return ret;
        }
        #endregion

        
       

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="sql"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool SetEntityList(string key, IList lst, TimeSpan expir, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(false, QueryCacheCommand.CommandSetList))
                {

                    bool ret = DoSetEntityList(key, lst, expir, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandSetList, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public IList GetEntityList(string key, Type entityType, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(true, QueryCacheCommand.CommandGetList))
                {
                    IList ret=DoGetEntityList(key, entityType, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandGetList, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    return null;
                }
            }
        }
        /// <summary>
        /// 清空所有内容
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public void ClearAll()
        {
            using (T client = CreateClient(false, QueryCacheCommand.CommandGetValues))
            {
                ClearAll(client);
            }
        }
        /// <summary>
        /// 获取存在的键键
        /// </summary>
        /// <param name="pattern"></param>
        public IEnumerable<string> GetAllKeys(string pattern)
        {
            using (T client = CreateClient(false, QueryCacheCommand.CommandGetValues))
            {
                return GetAllKeys(pattern, client);
            }
        }
        /// <summary>
        /// 获取哈希集合
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheHash GetHashMap(string key,  DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(true, QueryCacheCommand.CommandGetHash))
                {
                    ICacheHash ret = GetHashMap(key, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandGetHash, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取List集合
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheList GetList(string key, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(true, QueryCacheCommand.CommandGetList))
                {
                    ICacheList ret = GetList(key, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandGetList, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取SortedSet集合
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheSortedSet GetSortedSet(string key,  DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(true, QueryCacheCommand.CommandSortedSet))
                {
                    ICacheSortedSet ret = GetSortedSet(key, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandSortedSet, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    return null;
                }
            }
        }
        /// <summary>
        /// 获取SortedSet集合
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheLock GetCacheLock(string key, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(true, QueryCacheCommand.CommandLock))
                {
                    ICacheLock ret = GetCacheLock(key, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCacheCommand.CommandLock, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    OutExceptionMessage(ex, oper);
                    return null;
                }
            }
        }
        public abstract void ClearAll(T client);
        public abstract Task ClearAllAsync(T client);
        public abstract object GetClient();
        public abstract IEnumerable<string> GetAllKeys(string pattern, T client);

        public abstract Task<IEnumerable<string>> GetAllKeysAsync(string pattern, T client);

        /// <summary>
        /// 获取hash操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public abstract ICacheHash GetHashMap(string key, T connection);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public abstract ICacheList GetList(string key, T connection);

        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public abstract ICacheLock GetCacheLock(string key, T connection);





        /// <summary>
        /// 获取顺序集
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public abstract ICacheSortedSet GetSortedSet(string key, T connection);







        public async Task<E> GetValueAsync<E>(string key, E defaultValue, DataBaseOperate oper)
        {
            try
            {
                using (T client = await CreateClientAsync(true, QueryCacheCommand.CommandGetValues))
                {
                    E ret = await GetValueAsync<E>(key, defaultValue,client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandGetValues, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return default(E);
                }
            }
        }

        public async Task<object> GetValueAsync(string key,  DataBaseOperate oper) 
        {
            try
            {
                using (T client =await CreateClientAsync(true, QueryCacheCommand.CommandGetValues))
                {
                    object ret = await GetValueAsync(key, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandGetValues, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return null;
                }
            }
        }

        public async Task<IDictionary<string, object>> GetValuesAsync(string[] keys, DataBaseOperate oper)
        {

            try
            {
                using (T client =await CreateClientAsync(true, QueryCacheCommand.CommandGetValues))
                {
                    IDictionary<string, object> ret = await GetValuesAsync(keys, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandGetValues, "values", oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return null;
                }
            }

        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="expir"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public async Task<bool> SetValueAsync<E>(string key, E value, SetValueType type, TimeSpan expir, DataBaseOperate oper) 
        {
            try
            {
                using (T client = await CreateClientAsync(true, QueryCacheCommand.CommandSetValues))
                {
                    bool ret = await SetValueAsync(key, value,type,expir, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandSetValues, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return false;
                }
            }
        }

        public async Task<bool> SetValueAsync(string key, object value, SetValueType type, TimeSpan expir, DataBaseOperate oper) 
        {
            try
            {
                using (T client =await CreateClientAsync(true, QueryCacheCommand.CommandSetValues))
                {
                    bool ret = await SetValueAsync(key, value, type, expir, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandSetValues, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return false;
                }
            }
        }

        public async Task<bool> ExistsKeyAsync(string key, DataBaseOperate oper)
        {
            try
            {
                using (T client =await CreateClientAsync(true, QueryCacheCommand.CommandGetValues))
                {
                    bool ret = await DoExistsKeyAsync(key, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandGetValues, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return false;
                }
            }
        }

        public async Task<bool> SetKeyExpireAsync(string key, TimeSpan expir, DataBaseOperate oper)
        {
            try
            {
                using (T client = await CreateClientAsync(true, QueryCacheCommand.CommandSetValues))
                {
                    bool ret = await DoSetKeyExpireAsync(key, expir, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandSetValues, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return false;
                }
            }
        }

        public async Task ClearAllAsync()
        {
            try
            {
                using (T client = await CreateClientAsync(true, QueryCacheCommand.CommandSetValues))
                {
                    await ClearAllAsync(client);
                    
                    return;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    
                    return;
                }
            }
        }

        public async Task<bool> DeleteValueAsync(string key, DataBaseOperate oper) 
        {
            try
            {
                using (T client = await CreateClientAsync(true, QueryCacheCommand.CommandSetValues))
                {
                    bool ret = await DeleteValueAsync(key, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandSetValues, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return false;
                }
            }
        }

        public async Task<long> DoIncrementAsync(string key, ulong inc, DataBaseOperate oper)
        {
            try
            {
                using (T client =await CreateClientAsync(true, QueryCacheCommand.CommandSetValues))
                {
                    long ret = await DoIncrementAsync(key, inc,client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandSetValues, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return -1;
                }
            }
        }

        public async Task<long> DoDecrementAsync(string key, ulong dec, DataBaseOperate oper)
        {
            try
            {
                using (T client =await CreateClientAsync(true, QueryCacheCommand.CommandSetValues))
                {
                    long ret = await DoDecrementAsync(key, dec, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandSetValues, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return -1;
                }
            }
        }

        public async Task<IEnumerable<string>> GetAllKeysAsync(string pattern)
        {
            using (T client = await CreateClientAsync(false, QueryCacheCommand.CommandGetValues))
            {
                return await GetAllKeysAsync(pattern, client);
            }
        }

       

        public async Task RemoveBySQLAsync(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            try
            {
                using (T client =await CreateClientAsync(false, QueryCacheCommand.CommandDeleteSQL))
                {

                    //client.PrimitiveAsString = true;
                    string md5 = GetSQLMD5(sql);
                    string verKey = FormatVersionKey(md5);
                    if (!string.IsNullOrEmpty(md5))
                    {
                        bool ret= await DeleteValueAsync(md5, client);
                        ret = await DeleteValueAsync(verKey, client);
                    }
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandDeleteSQL, sql, oper);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                }
            }
        }

        public async Task RemoveByTableNameAsync(string tableName, DataBaseOperate oper)
        {
            try
            {
                string key = GetTableName(tableName);
                using (T client =await CreateClientAsync(false, QueryCacheCommand.CommandDeleteTable))
                {
                    //client.PrimitiveAsString = true;
                    int val = ValueConvertExtend.ConvertValue<int>(GetValue<object>(key, -1, client));

                    if (val <= 0 || val >= MaxVersion)
                    { 
                        long ret= await DoIncrementAsync(key, 1, client);
                    }
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandDeleteTable, tableName, oper);
                    }
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                }
            }
        }

       

        public async Task<IList> GetEntityListAsync(string key, Type entityType, DataBaseOperate oper)
        {
            try
            {
                using (T client =await CreateClientAsync(true, QueryCacheCommand.CommandGetList))
                {
                    IList ret = await DoGetEntityListAsync(key, entityType, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandGetList, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return null;
                }
            }
        }

        public async Task<bool> SetEntityListAsync(string key, IList lstEntity, TimeSpan expir, DataBaseOperate oper)
        {
            try
            {
                using (T client = await CreateClientAsync(false, QueryCacheCommand.CommandSetList))
                {

                    bool ret = await DoSetEntityListAsync(key, lstEntity, expir, client);
                    if (_info.SqlOutputer.HasOutput)
                    {
                        await OutPutMessageAsync(QueryCacheCommand.CommandSetList, "key=" + key, oper);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (_throwExcertion)
                {
                    throw ex;
                }
                else
                {
                    await OutExceptionMessageAsync(ex, oper);
                    return false;
                }
            }
        }
    }
}
