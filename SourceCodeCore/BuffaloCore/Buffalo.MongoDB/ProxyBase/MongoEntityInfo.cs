using Buffalo.Kernel.FastReflection;
using Buffalo.MongoDB.QueryCondition;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB.ProxyBase
{
    /// <summary>
    /// Mongo实体信息
    /// </summary>
    public class MongoEntityInfo
    {
        ///// <summary>
        ///// 类型建造器
        ///// </summary>
        //internal static MongoEntityProxyBuilder _builder = new MongoEntityProxyBuilder();
        /// <summary>
        /// 属性类型
        /// </summary>
        private PropertyInfoHandleCollection _propertyInfoHandles;
        /// <summary>
        /// 自增长属性类型
        /// </summary>
        private PropertyInfoHandleCollection _autoIncrementHandles;
        /// <summary>
        /// 实体类型
        /// </summary>
        private Type _entityType;
        /// <summary>
        /// 代理类型
        /// </summary>
        private Type _proxyType;
        /// <summary>
        /// 关联的集合
        /// </summary>
        private string _collectionName;

        /// <summary>
        /// 数据库键
        /// </summary>
        private string _dbKey;
        /// <summary>
        /// 数据库信息
        /// </summary>
        private MongoDBInfo _dbInfo;
        /// <summary>
        /// 类的信息
        /// </summary>
        /// <param name="entityType">类的类型</param>
        internal MongoEntityInfo(Type entityType,string collectionName, MongoDBInfo dbInfo)
        {
            _entityType = entityType;
            _dbInfo = dbInfo;

            _collectionName = collectionName;
            _dbKey = dbInfo.DBKey;

            _proxyType = _entityType;
            
            CreatePropertyInfo(_entityType);
        }

        /// <summary>
        /// 创建属性类型
        /// </summary>
        /// <param name="entityType"></param>
        private void CreatePropertyInfo(Type entityType)
        {
            PropertyInfo[] infos = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Dictionary<string, MongoPropertyInfo> propertyInfoHandles = new Dictionary<string, MongoPropertyInfo>();
            Dictionary<string, MongoPropertyInfo> autoIncrementHandles = new Dictionary<string, MongoPropertyInfo>();
            string key = null;
            foreach (PropertyInfo pInfo in infos)
            {
                key = null;
                if (pInfo.GetMethod == null || pInfo.SetMethod == null)
                {
                    continue;
                }
                BsonIgnoreAttribute ignore = pInfo.GetCustomAttribute(typeof(BsonIgnoreAttribute)) as BsonIgnoreAttribute;
                if (ignore != null)//忽略属性
                {
                    continue;
                }
                MongoAutoIncrement autoIncrement = pInfo.GetCustomAttribute(typeof(MongoAutoIncrement)) as MongoAutoIncrement;

                if (autoIncrement != null)//忽略属性
                {
                    key = autoIncrement.Name;
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        StringBuilder sbKey = new StringBuilder();
                        sbKey.Append("Liquid.");
                        sbKey.Append(_dbKey);
                        sbKey.Append(".");
                        sbKey.Append(_collectionName);
                        sbKey.Append(".");
                        sbKey.Append(pInfo.Name);
                        key = sbKey.ToString();
                    }
                }
                PropertyInfoHandle handle = FastValueGetSet.GetPropertyInfoHandleWithOutCache(pInfo);
                MongoPropertyInfo info= new MongoPropertyInfo(pInfo.Name, key, handle);
                propertyInfoHandles[pInfo.Name] = info;
                if (autoIncrement != null)//忽略属性
                {
                    autoIncrementHandles[pInfo.Name] = info;
                }
            }
            _propertyInfoHandles = new PropertyInfoHandleCollection(propertyInfoHandles);
            _autoIncrementHandles = new PropertyInfoHandleCollection(autoIncrementHandles);
        }

        /// <summary>
        /// 获取属性的信息
        /// </summary>
        public PropertyInfoHandleCollection PropertyInfo
        {
            get
            {
                return _propertyInfoHandles;
            }
        }
        /// <summary>
        /// 获取自增长属性的信息
        /// </summary>
        public PropertyInfoHandleCollection IdentityPropertyInfo
        {
            get
            {
                return _autoIncrementHandles;
            }
        }
        /// <summary>
        /// 获取所属集合
        /// </summary>
        public string CollectionName
        {
            get
            {
                return _collectionName;
            }
        }
        /// <summary>
        /// 数据库信息
        /// </summary>
        public MongoDBInfo DBInfo
        {
            get
            {
                return _dbInfo;
            }
        }
        /// <summary>
        /// 代理类
        /// </summary>
        public Type ProxyType
        {
            get
            {

                return _proxyType;
            }
        }

        /// <summary>
        /// 代理类
        /// </summary>
        public Type EntityType
        {
            get
            {

                return _entityType;
            }
        }

        /// <summary>
        /// 返回此类型的实例
        /// </summary>
        /// <returns></returns>
        public object CreateInstance()
        {
            return Activator.CreateInstance(_entityType);
        }
        /// <summary>
        /// 返回此类型的代理类实例
        /// </summary>
        /// <returns></returns>
        public object CreateProxyInstance()
        {
            
            return Activator.CreateInstance(_proxyType);
        }
    }
}
