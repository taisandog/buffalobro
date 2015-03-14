using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel;
using System.Data;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 分布式缓存基类
    /// </summary>
    public abstract class NetCacheBase<T>: ICacheAdaper where T:IDisposable
    {
        /// <summary>
        /// 过期时间(分钟)
        /// </summary>
        protected TimeSpan _expiration;
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

        protected bool _throwExcertion=false;
        /// <summary>
        /// 抛出异常
        /// </summary>
        public bool ThrowExcertion
        {
            get { return _throwExcertion; }
        }

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
        /// 创建客户端
        /// </summary>
        /// <returns></returns>
        protected abstract T CreateClient(bool realOnly,string cmd);

        public System.Data.DataSet GetData(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(true, QueryCache.CommandGetDataSet))
                {
                    //client.PrimitiveAsString = true;
                    string sqlMD5 = GetSQLMD5(sql);
                    bool isVersion = ComparVersion(tableNames, sqlMD5, client);//判断版本号
                    if (!isVersion)
                    {
                        return null;
                    }

                    DataSet dsRet = DoGetDataSet(sqlMD5, client);

                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCache.CommandGetDataSet, sql, oper);
                    }

                    return dsRet;
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
        /// 获取表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetTableName(string tableName)
        {
            StringBuilder sbInfo = new StringBuilder(tableName.Length + 10);
            sbInfo.Append(_info.Name);
            sbInfo.Append(".");
            sbInfo.Append(tableName);
            return PasswordHash.ToMD5String(sbInfo.ToString());
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
            sbSqlInfo.Append(_info.Name);
            sbSqlInfo.Append(":");
            sbSqlInfo.Append(sql);
            sbSql.Append(PasswordHash.ToMD5String(sbSqlInfo.ToString()));
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
        private bool ComparVersion(IDictionary<string, bool> tableNames, string md5, T client)
        {
            string sqlKey = FormatVersionKey(md5);
            string[] keys = GetKeys(tableNames, sqlKey);
            IDictionary<string, object> tableVars = GetValues(keys, client);

            Dictionary<string, string> dicTableVers = GetTablesVersion(tableNames, tableVars, client, false);
            if (dicTableVers == null)
            {
                return false;
            }
            object data = null;
            if (!tableVars.TryGetValue(sqlKey, out data)) 
            {
                return false;
            }
            Dictionary<string, string> dicDataVers = GetDataVersion(data.ToString(), client);
            if (dicDataVers == null)
            {
                return false;
            }
            string tmp = null;
            foreach (KeyValuePair<string, string> kvp in dicTableVers)
            {
                if (!dicDataVers.TryGetValue(kvp.Key, out tmp))
                {
                    return false;
                }
                if (tmp != kvp.Value)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract E GetValue<E>(string key, T client);
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract IDictionary<string, object> GetValues(string[] keys, T client);
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract void SetValue<E>(string key, E value, T client);

        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract DataSet DoGetDataSet(string key,  T client);
        /// <summary>
        /// 设置DataSet
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">DataSet</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract bool DoSetDataSet(string key, DataSet value, T client);
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected abstract void DeleteValue(string key, T client);
        /// <summary>
        /// 自增1
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected abstract void DoIncrement(string key, T client);
        /// <summary>
        /// 设置版本号
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected abstract void DoNewVer(string key, T client);
        /// <summary>
        /// 缓存服务器类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected abstract string GetCacheName();

        /// <summary>
        /// 获取要申请的键集合
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="tableMD5"></param>
        /// <returns></returns>
        private string[] GetKeys(IDictionary<string, bool> tableNames, string tableMD5) 
        {
            int len = tableNames.Count;
            if (!string.IsNullOrEmpty(tableMD5)) 
            {
                len++;
            }
            string[] keys = new string[len];
            int index = 0;
            foreach (KeyValuePair<string, bool> kvp in tableNames)
            {
                string key = GetTableName(kvp.Key);
                keys[index] = key;
                index++;
            }
            if (!string.IsNullOrEmpty(tableMD5))
            {
                keys[len - 1] = tableMD5;
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
        private Dictionary<string, string> GetTablesVersion(IDictionary<string, bool> tableNames, IDictionary<string, object> tableVars, T client, bool needCreateTableVer)
        {
            Dictionary<string, string> dicTableVers = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            
            
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
                        DoNewVer(key, client);
                        objVer = "1";
                    }
                }
                dicTableVers[kvp.Key] = objVer.ToString();
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
        private string GetTablesVerString(IDictionary<string, bool> tableNames, T client, bool needCreateTableVer)
        {
            string[] keys = GetKeys(tableNames, null);
            IDictionary<string, object> tableVars = GetValues(keys, client);
            Dictionary<string, string> dicTableVers = GetTablesVersion(tableNames, tableVars, client, needCreateTableVer);
            StringBuilder sbTables = new StringBuilder(dicTableVers.Count * 10);
            foreach (KeyValuePair<string, string> kvp in dicTableVers)
            {
                sbTables.Append(kvp.Key);
                sbTables.Append("=");
                sbTables.Append(kvp.Value);
                sbTables.Append("\n");
            }
            if (sbTables.Length > 0)
            {
                sbTables.Remove(sbTables.Length - 1, 1);
            }
            return sbTables.ToString();
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
                using (T client = CreateClient(false, QueryCache.CommandDeleteSQL))
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
                        OutPutMessage(QueryCache.CommandDeleteSQL, sql, oper);
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
                using (T client = CreateClient(false, QueryCache.CommandDeleteTable))
                {
                    //client.PrimitiveAsString = true;
                    int val = GetValue<int>(key, client);

                    if (val <= 0 || val >= MaxVersion)
                    {
                        DoNewVer(key,  client);
                        //client.Set(key, 1, _expiration);
                    }
                    else
                    {
                        DoIncrement(key, client);
                    }
                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCache.CommandDeleteTable, tableName, oper);
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
        public bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(false, QueryCache.CommandSetDataSet))
                {
                    //client.PrimitiveAsString = true;
                    string md5 = GetSQLMD5(sql);
                    string verKey = FormatVersionKey(md5);
                    string verValue = GetTablesVerString(tableNames, client, true);

                    if (_info.SqlOutputer.HasOutput)
                    {
                        OutPutMessage(QueryCache.CommandSetDataSet, sql, oper);
                    }
                    SetValue<string>(verKey, verValue, client);
                    return DoSetDataSet(md5, ds, client);
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

        private void OutPutMessage(string type, string message, DataBaseOperate oper)
        {
            oper.OutMessage(MessageType.QueryCache, GetCacheName(), type, message);

        }
        private void OutExceptionMessage(Exception ex, DataBaseOperate oper)
        {
            MessageInfo info = new MessageInfo();
            info.Value = ex;
            info.Type = GetCacheName();
            oper.OutMessage(MessageType.CacheException, info);

        }
        #endregion

        #region ICacheAdaper 成员


        public IDictionary<string, object> GetValues(string[] keys, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(false, QueryCache.CommandDeleteSQL))
                {
                    return GetValues(keys, client);
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

        public void SetValue<E>(string key, E value, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(false, QueryCache.CommandDeleteSQL))
                {
                    SetValue<E>(key, value, client);
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

        public void DeleteValue(string key, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(false, QueryCache.CommandDeleteSQL))
                {
                    DeleteValue(key, client);
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

        public void DoIncrement(string key, DataBaseOperate oper)
        {
            try
            {
                using (T client = CreateClient(false, QueryCache.CommandDeleteSQL))
                {
                    DoIncrement(key, client);
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

        #endregion
    }
}
