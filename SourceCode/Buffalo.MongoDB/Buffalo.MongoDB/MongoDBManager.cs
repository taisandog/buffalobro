﻿using Buffalo.DB.CacheManager;
using Buffalo.Kernel;
using Buffalo.MongoDB.ProxyBase;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// Mongo连接管理
    /// </summary>
    public class MongoDBManager
    {
        private static ConcurrentDictionary<string, MongoDBInfo> _dicConn = new ConcurrentDictionary<string, MongoDBInfo>();
        private static readonly string MongoDBKey = "_?MDB.Conn.Key.";

        private static readonly string LiquidName = "Buffalo_Liquid_Sequence";
        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="key">此连接的标识</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="cache">关联自增长缓存</param>
        /// <param name="assembly">程序集</param>
        public static void AddConfig(string key, string connectionString, string dbName, Assembly assembly)
        {
            MongoLiquidUnit liquid = null;



            liquid = new MongoLiquidUnit(LiquidName);

            MongoDBInfo conn = new MongoDBInfo(key,connectionString, dbName, liquid);
            _dicConn[key] = conn;
            InitAssembly(assembly, conn);
            
        }
       

        /// <summary>
        /// 通过标识获取Mongo连接
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IMongoDatabase GetMongoClient(string key)
        {
            string skey = MongoDBKey + key;
            IMongoDatabase db = ContextValue.Current[skey] as IMongoDatabase;
            if (db == null)
            {
                MongoDBInfo conn = null;
                if (!_dicConn.TryGetValue(key, out conn))
                {
                    throw new KeyNotFoundException("找不到标识为：" + key + "的配置");
                }

                db = conn.CreateConnection();
                ContextValue.Current[skey] = db;
            }
            return db;
        }

        /// <summary>
        /// 初始化程序集
        /// </summary>
        /// <param name="assembly"></param>
        public static void InitAssembly(Assembly assembly, MongoDBInfo dbInfo)
        {
            Type baseType = typeof(MongoEntityBase);
            string dbKey = null;
            string collectionName = null;
            IMongoDatabase dbconnection = GetMongoClient(dbInfo.DBKey);
            foreach (Type entityType in assembly.GetTypes())
            {
                if (!entityType.IsSubclassOf(baseType))
                {
                    continue;
                }

                object[] arr = entityType.GetCustomAttributes(typeof(MGCollection), false);
                if (arr == null || arr.Length <= 0)
                {
                    continue;
                }

                MGCollection coll = arr[0] as MGCollection;
                if (coll == null)
                {
                    continue;
                }
                collectionName = coll.CollectionName;
                dbKey = coll.DBKey;
                if (!string.Equals(dbKey, dbInfo.DBKey))
                {
                    continue;
                }
                MongoEntityInfo info = MongoEntityInfoManager.SetEntityHandle(entityType, collectionName, dbInfo);
                dbInfo.Liquid.InitLiquid(dbconnection, info);

            }
            dbInfo.Liquid.InitLiquidIndex(dbconnection);
        }

        

        /// <summary>
        /// 创建数据库操作类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MongoDBOperate<T> CreateOperate<T>() where T:MongoEntityBase
        {
            Type t = typeof(T);
            MongoEntityInfo info = MongoEntityInfoManager.GetEntityHandle(t);
            if (info == null)
            {
                throw new NotSupportedException("实体" + t.FullName + "并非MongoEntityBase的子类");
            }
            
            if (info.CollectionName==null)
            {
                throw new KeyNotFoundException("实体" + t.FullName + "没有标记MGCollection");
            }
            return CreateOperate<T>(info);
        }

        /// <summary>
        /// 创建数据库操作类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info">数据信息</param>
        /// <returns></returns>
        public static MongoDBOperate<T> CreateOperate<T>(MongoEntityInfo info) where T : MongoEntityBase
        {
            
            return new MongoDBOperate<T>(info.DBInfo, info);
        }
    }
}