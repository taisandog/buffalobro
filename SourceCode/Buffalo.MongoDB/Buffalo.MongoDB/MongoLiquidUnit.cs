using Buffalo.DB.CacheManager;
using Buffalo.MongoDB.ProxyBase;
using Buffalo.MongoDB.QueryCondition;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// mongo自增长ID管理器
    /// </summary>
    public class MongoLiquidUnit
    {
        private string _liquidcollName;

        /// <summary>
        /// mongo自增长ID管理器
        /// </summary>
        /// <param name="cache"></param>
        public MongoLiquidUnit()
        {
           
        }
        /// <summary>
        /// mongo自增长ID管理器
        /// </summary>
        /// <param name="liquidcollName">自增长的表名</param>
        public MongoLiquidUnit(string liquidcollName)
        {
            _liquidcollName = liquidcollName;
        }
        /// <summary>
        /// 初始化自增长
        /// </summary>
        /// <param name="db"></param>
        /// <param name="liquidcollName"></param>
        /// <param name="info"></param>
        public void InitLiquid(IMongoDatabase db,MongoEntityInfo info)
        {
            IMongoCollection<BsonDocument> col = db.GetCollection<BsonDocument>(_liquidcollName);
            List<string> lst = new List<string>();

            foreach (MongoPropertyInfo pinfo in info.IdentityPropertyInfo)
            {
                lst.Add(pinfo.Key);
            }
            FilterDefinition<BsonDocument> where = ConditionList<BsonDocument>.Where.In<string>("Key", lst);
            IFindFluent<BsonDocument, BsonDocument> cus = col.Find(where);
            using (IAsyncCursor<BsonDocument> reader = cus.ToCursor())
            {
                string curKey = null;
                while (reader.MoveNext())
                {
                    foreach (BsonDocument item in reader.Current)
                    {
                        curKey = item["Key"].AsString;
                        if (!string.IsNullOrWhiteSpace(curKey))
                        {
                            lst.Remove(curKey);
                        }
                    }
                }
            }

            foreach (string key in lst)
            {
                BsonDocument doc = new BsonDocument();
                doc["_id"] = ObjectId.GenerateNewId();
                doc["Key"] = key;
                doc["Value"] = 0L;
                col.InsertOne(doc);
            }
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="db"></param>
        /// <param name="liquidcollName"></param>
        public void InitLiquidIndex(IMongoDatabase db)
        {
            IMongoCollection<BsonDocument> col = db.GetCollection<BsonDocument>(_liquidcollName);
            //创建索引
            IndexKeysDefinitionBuilder<BsonDocument> index = ConditionList<BsonDocument>.Index;
            IndexKeysDefinition<BsonDocument> definition = index.Ascending("Key");
            CreateIndexModel<BsonDocument> model = new CreateIndexModel<BsonDocument>(definition);
            col.Indexes.CreateOne(model);
        }
        /// <summary>
        /// 获取当前值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetCurrent(IMongoDatabase db,string key)
        {
            IMongoCollection<BsonDocument> col = db.GetCollection<BsonDocument>(_liquidcollName);
            FilterDefinition<BsonDocument> where = ConditionList<BsonDocument>.Where.Eq<string>("Key", key);
            IFindFluent<BsonDocument, BsonDocument> cus = col.Find(where).Limit(1);
            using (IAsyncCursor<BsonDocument> reader = cus.ToCursor())
            {
                
                while (reader.MoveNext())
                {
                    foreach (BsonDocument item in reader.Current)
                    {
                        long ret = item["Value"].AsInt64;
                        return ret;
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// 设置当前值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void SetCurrent(IMongoDatabase db,string key, long value)
        {

            IMongoCollection<BsonDocument> mc = db.GetCollection<BsonDocument>(_liquidcollName);
            FilterDefinition<BsonDocument> where = ConditionList<BsonDocument>.Where.Eq<string>("Key", key);

            UpdateList<BsonDocument> updateList = new UpdateList<BsonDocument>();
            updateList.Add(UpdateList<BsonDocument>.Update.Set("Value", value));

            UpdateDefinition<BsonDocument> update = UpdateList<BsonDocument>.Update.Combine(updateList);
            
            //根据指定查询移除数据
            mc.UpdateMany(where, update);
        }
        /// <summary>
        /// 自增值
        /// </summary>
        public long Increment(IMongoDatabase db, string key)
        {
            FilterDefinition<BsonDocument>  query =ConditionList<BsonDocument>.Where.Eq<string>("Key", key);

            UpdateList<BsonDocument> updateList = new UpdateList<BsonDocument>();
            updateList.Add(UpdateList<BsonDocument>.Update.Inc<long>("Value",1L));
            UpdateDefinition<BsonDocument> update = UpdateList<BsonDocument>.Update.Combine(updateList);
            FindOneAndUpdateOptions<BsonDocument, BsonDocument> options = new FindOneAndUpdateOptions<BsonDocument, BsonDocument>();
            options.IsUpsert = true;
            options.ReturnDocument = ReturnDocument.After;
            IMongoCollection<BsonDocument> mc = db.GetCollection<BsonDocument>(_liquidcollName);
            BsonDocument retObject= mc.FindOneAndUpdate(query, update, options);
            if (retObject == null)
            {
                return 0L;
            }
            return retObject["Value"].AsInt64;
        }

        /// <summary>
        /// 自减值
        /// </summary>
        public long Decrement(IMongoDatabase db, string key)
        {
            FilterDefinition<BsonDocument> query = ConditionList<BsonDocument>.Where.Eq<string>("Key", key);

            UpdateList<BsonDocument> updateList = new UpdateList<BsonDocument>();
            updateList.Add(UpdateList<BsonDocument>.Update.Inc<long>("Value", -1L));
            UpdateDefinition<BsonDocument> update = UpdateList<BsonDocument>.Update.Combine(updateList);
            FindOneAndUpdateOptions<BsonDocument, BsonDocument> options = new FindOneAndUpdateOptions<BsonDocument, BsonDocument>();
            options.IsUpsert = true;
            IMongoCollection<BsonDocument> mc = db.GetCollection<BsonDocument>(_liquidcollName);
            BsonDocument retObject = mc.FindOneAndUpdate(query, update, options);
            if (retObject == null)
            {
                return 0L;
            }
            return retObject["Value"].AsInt64;
        }
    }
}
