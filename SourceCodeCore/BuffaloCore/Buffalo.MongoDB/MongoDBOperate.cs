using Buffalo.Kernel.FastReflection;
using Buffalo.MongoDB.ProxyBase;
using Buffalo.MongoDB.QueryCondition;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public partial class MongoDBOperate<TDocument> where TDocument : MongoEntityBase
    {
        /// <summary>
        /// 数据库信息
        /// </summary>
        private MongoDBInfo _dbInfo;
        /// <summary>
        /// 关联实体
        /// </summary>
        private MongoEntityInfo _entityInfo;
        /// <summary>
        /// 关联数据库
        /// </summary>
        private IMongoDatabase _db = null;
        /// <summary>
        /// 关联连接
        /// </summary>
        private MongoConnection _connection = null;


        /// <summary>
        /// 数据库操作类
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbName">数据库名</param>
        public MongoDBOperate(MongoDBInfo dbInfo, MongoEntityInfo entityInfo)
        {
            _dbInfo = dbInfo;
            _entityInfo = entityInfo;
            _connection = MongoDBManager.GetMongoClient(_dbInfo.DBKey);
            _db = _connection.DB;
        }
        /// <summary>
        /// 所属数据库
        /// </summary>
        public IMongoDatabase DB 
        {
            get 
            {
                return _db;
            }
        }
        /// <summary>
        /// 数据库连接
        /// </summary>
        public MongoConnection Connection
        {
            get
            {
                return _connection;
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
        /// 关联实体
        /// </summary>
        private MongoEntityInfo EntityInfo 
        {
            get 
            {
                return _entityInfo;
            }
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <param name="entity">实体</param>
        /// <param name="hasIdentity">是否有自增长</param>
        public void Insert(string collectionName, TDocument entity,bool hasIdentity=true,
            InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            //entity.ToBsonDocument();
            IMongoCollection<TDocument> col = _db.GetCollection<TDocument>(collectionName);
            MongoEntityInfo info = MongoEntityInfoManager.GetEntityHandle(entity.GetType());
            MongoLiquidUnit liquid = info.DBInfo.Liquid;
            if (hasIdentity && liquid != null)
            {
                foreach (MongoPropertyInfo pinfo in info.IdentityPropertyInfo)
                {
                    long curId = liquid.Increment(_db, pinfo.Key);
                    pinfo.PropertyHandle.SetValue(entity,curId);
                }
            }
            //插入
            col.InsertOne(entity,options,cancellationToken);
            
        }
        /// <summary>
        /// 查找所有集合信息
        /// </summary>
        /// <returns></returns>
        public List<BsonDocument> ListAllCollection() 
        {
            List<BsonDocument> lstDB = _db.ListCollections().ToList();
            return lstDB;
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public MongoTransaction StartTransaction()
        {
            MongoConnection conn = null;
            bool isStart = _connection.StartTransaction();//开启成功
            
            if (isStart)
            {
                conn = _connection;
            }
            return new MongoTransaction(conn);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="hasIdentity">是否有自增长</param>
        public void Insert(TDocument entity, bool hasIdentity = true,
            InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            Insert(_entityInfo.CollectionName, entity,hasIdentity, options, cancellationToken);
        }

       
       



        /// <summary>
        /// 插入批量数据
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <param name="lstEntity">实体</param>
        /// <param name="hasIdentity">是否有自增长</param>

        public void InsertList(string collectionName, IEnumerable<TDocument> lstEntity, bool hasIdentity = true,
            InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            IMongoCollection<TDocument> col = _db.GetCollection<TDocument>(collectionName);
            List<TDocument> lst = new List<TDocument>();
            
            MongoEntityInfo info = MongoEntityInfoManager.GetEntityHandle(_entityInfo.EntityType);
            MongoLiquidUnit liquid = info.DBInfo.Liquid;
            
            foreach (TDocument obj in lstEntity)
            {
                if (obj == null)
                {
                    continue;
                }
                if (hasIdentity && liquid != null)
                {
                    foreach (MongoPropertyInfo pinfo in info.IdentityPropertyInfo)
                    {
                        long curId = liquid.Increment(_db, pinfo.Key);
                        pinfo.PropertyHandle.SetValue(obj, curId);
                    }
                }

                lst.Add(obj);
            }
            //插入
            col.InsertMany(lst, options, cancellationToken);
        }


        /// <summary>
        /// 插入批量数据
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="hasIdentity">是否有自增长</param>
        public void InsertList(IEnumerable<TDocument> lstEntity, bool hasIdentity = true, 
            InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }


            InsertList(_entityInfo.CollectionName, lstEntity,hasIdentity,options,cancellationToken);
        }
        /// <summary>
        /// 移除指定的数据
        /// </summary>
        /// <param name="query">移除的数据条件</param>
        public void Delete(ConditionList<TDocument> query)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            Delete(query, _entityInfo.CollectionName);
        }
        /// <summary>
        /// 移除指定的数据
        /// </summary>
        /// <typeparam name="T">移除的数据类型</typeparam>
        /// <param name="query">移除的数据条件</param>
        /// <param name="collectionName">指定的集合名词</param>
        public void Delete(ConditionList<TDocument> query, string collectionName)
        {
            IMongoCollection<TDocument> mc = _db.GetCollection<TDocument>(collectionName);
            FilterDefinition<TDocument> qu = ConditionList<TDocument>.Where.And(query);

            //根据指定查询移除数据
            mc.DeleteMany(qu);
        }
        /// <summary>
        /// 移除指定的对象
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="collectionName">指定的集合名词</param>
        public void Delete(ObjectId id, string collectionName)
        {
            IMongoCollection<TDocument> mc = _db.GetCollection<TDocument>(collectionName);
            FilterDefinition<TDocument> qu = ConditionList<TDocument>.Where.Eq<ObjectId>("_id", id);

            //根据指定查询移除数据
            mc.DeleteOne(qu);
        }
        /// <summary>
        /// 移除指定的对象
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="collectionName">指定的集合名词</param>
        public void Delete(ObjectId id)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            Delete(id, _entityInfo.CollectionName);
        }
        /// <summary>
        /// 批量更新指定的数据
        /// </summary>
        /// <typeparam name="T">移除的数据类型</typeparam>
        /// <param name="query">移除的数据条件</param>
        /// <param name="collectionName">指定的集合名词</param>
        public void Update(ConditionList<TDocument> query, UpdateList<TDocument> updateList, string collectionName,
            UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            IMongoCollection<TDocument> mc = _db.GetCollection<TDocument>(collectionName);
            FilterDefinition<TDocument> qu = ConditionList<TDocument>.Where.And(query);
            UpdateDefinition<TDocument> update = UpdateList<TDocument>.Update.Combine(updateList);
            //根据指定查询移除数据
            mc.UpdateMany(qu, update, options, cancellationToken);
        }
        /// <summary>
        /// 更新单个指定的数据
        /// </summary>
        /// <typeparam name="T">移除的数据类型</typeparam>
        /// <param name="query">移除的数据条件</param>
        /// <param name="collectionName">指定的集合名词</param>
        public void UpdateOne(ConditionList<TDocument> query, UpdateList<TDocument> updateList, string collectionName,
            UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            IMongoCollection<TDocument> mc = _db.GetCollection<TDocument>(collectionName);
            FilterDefinition<TDocument> qu = ConditionList<TDocument>.Where.And(query);
            UpdateDefinition<TDocument> update = UpdateList<TDocument>.Update.Combine(updateList);
            //根据指定查询移除数据
            mc.UpdateOne(qu, update, options, cancellationToken);
        }
        /// <summary>
        /// 更新指定的数据
        /// </summary>
        /// <typeparam name="T">移除的数据类型</typeparam>
        /// <param name="query">移除的数据条件</param>
        /// <param name="collectionName">指定的集合名词</param>
        public void Update(ConditionList<TDocument> query, UpdateList<TDocument> updateList,
            UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            Update(query, updateList, _entityInfo.CollectionName, options, cancellationToken);
        }

        /// <summary>
        /// 更新指定的数据
        /// </summary>
        /// <typeparam name="T">移除的数据类型</typeparam>
        /// <param name="query">移除的数据条件</param>
        /// <param name="collectionName">指定的集合名词</param>
        public void Update(TDocument entity, ConditionList<TDocument> query=null, string collectionName=null,
            UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                query = new ConditionList<TDocument>();
            }
            if (collectionName == null)
            {
                collectionName = _entityInfo.CollectionName;
            }
            UpdateList<TDocument> updateList = new UpdateList<TDocument>();
            List<string> changes = entity.GetEntityBaseInfo().GetChangedPropertyName();
            foreach(string name in changes)
            {
                MongoPropertyInfo pinfo = _entityInfo.PropertyInfo[name];
                if (pinfo == null)
                {
                    continue;
                }
                object value = pinfo.PropertyHandle.GetValue(entity);
                if (string.Equals("_id", name))//主键
                {
                    query.AddEqual<ObjectId>("_id", (ObjectId)value);
                    continue;
                }
                updateList.Add(UpdateList<TDocument>.Update.Set(name, value));
            }
            
            UpdateOne(query, updateList, collectionName,options,cancellationToken);
        }
    }
}
