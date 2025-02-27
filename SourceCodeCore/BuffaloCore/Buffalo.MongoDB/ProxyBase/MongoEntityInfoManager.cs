using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB.ProxyBase
{
    /// <summary>
    ///  Mongo实体信息管理器
    /// </summary>
    public class MongoEntityInfoManager
    {
        private static ConcurrentDictionary<Type, MongoEntityInfo> _dicClass = new ConcurrentDictionary<Type, MongoEntityInfo>();//记录已经初始化过的类型

        /// <summary>
        /// 设置实体信息
        /// </summary>
        /// <param name="info"></param>
        internal static MongoEntityInfo SetEntityHandle(Type type,  string collectionName, MongoDBInfo dbInfo)
        {
            MongoEntityInfo info = new MongoEntityInfo(type, collectionName, dbInfo);
            _dicClass[type] = info;
            return info;
        }

        /// <summary>
        /// 获取实体类里边得属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static MongoEntityInfo GetEntityHandle(Type type)
        {
            MongoEntityInfo classHandle = null;

            //if (!DataAccessLoader.HasInit)
            //{
            //    DataAccessLoader.InitConfig();
            //}

            if (_dicClass.TryGetValue(type, out classHandle))
            {
                return classHandle;
            }
            return null;
            
                //if (_dicClass.TryGetValue(type, out classHandle))//再次验证不是并发再生成类
                //{
                //    return classHandle;
                //}
                //classHandle = new MongoEntityInfo(type);
                //_dicClass[type] = classHandle;
                //return classHandle;
            
        }

    }
}
