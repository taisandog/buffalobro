using Buffalo.MongoDB.QueryCondition;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    public partial class MongoDBOperate<TDocument>
    {
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public BsonDocument GetUniqueDocument(ConditionList<BsonDocument> query, string collectionName)
        {
            //IMongoDatabase db = MongoDBManager.GetMongoClient(_dbInfo.DBKey);
            IMongoCollection<BsonDocument> mc = _db.GetCollection<BsonDocument>(collectionName);
            FilterDefinition<BsonDocument> qu = null;
            if (query.Count > 0)
            {
                qu = ConditionList<BsonDocument>.Where.And(query);
            }
            else
            {
                qu = ConditionList<BsonDocument>.Where.Empty;
            }

            IFindFluent<BsonDocument, BsonDocument> cus = mc.Find(qu);

            return cus.FirstOrDefault<BsonDocument>();
        }
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public BsonDocument GetUniqueDocument(ConditionList<BsonDocument> query)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            return GetUniqueDocument(query, _entityInfo.CollectionName);
        }
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<BsonDocument> SelectDocumentList(ConditionList<BsonDocument> query)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            return SelectDocumentList(query, _entityInfo.CollectionName);
        }

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public List<BsonDocument> SelectDocumentList(ConditionList<BsonDocument> query, string collectionName)
        {
            //IMongoDatabase db = MongoDBManager.GetMongoClient(_dbInfo.DBKey);
            IMongoCollection<BsonDocument> mc = _db.GetCollection<BsonDocument>(collectionName);
            FilterDefinition<BsonDocument> qu = null;
            if (query.Count > 0)
            {
                qu = ConditionList<BsonDocument>.Where.And(query);
            }
            else
            {
                qu = ConditionList<BsonDocument>.Where.Empty;
            }

            IFindFluent<BsonDocument, BsonDocument> cus = mc.Find(qu);
            cus = FillSort(cus, query.OrderBy);//填充排序
            cus = FillPage(cus, query.PageContext);//填充分页
            List<BsonDocument> lst = new List<BsonDocument>();
            using (IAsyncCursor<object> reader = cus.ToCursor())
            {
                while (reader.MoveNext())
                {
                    foreach (BsonDocument doc in reader.Current)
                    {
                        lst.Add(doc);

                    }
                }
            }
            return lst;
        }

        /// <summary>
        /// 插入Document数据
        /// </summary>
        /// <param name="document">Bson数据</param>
        public void InsertDocument(BsonDocument document,
            InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            InsertDocument(document, _entityInfo.CollectionName, options, cancellationToken);
        }
        /// <summary>
        /// 插入Document数据
        /// </summary>
        /// <param name="document">Bson数据</param>
        public void InsertDocument(BsonDocument document, string collectionName,
            InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            IMongoCollection<BsonDocument> col = _db.GetCollection<BsonDocument>(collectionName);
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            col.InsertOne(document, options, cancellationToken);
        }
        /// <summary>
        /// 插入批量数据
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <param name="lstEntity">实体</param>
        /// <param name="hasIdentity">是否有自增长</param>

        public void InsertDocumentList(string collectionName, IEnumerable<BsonDocument> lstEntity,
            InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            IMongoCollection<BsonDocument> col = _db.GetCollection<BsonDocument>(collectionName);

            //插入
            col.InsertMany(lstEntity, options, cancellationToken);
        }


        /// <summary>
        /// 插入批量数据
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="hasIdentity">是否有自增长</param>
        public void InsertDocumentList(IEnumerable<BsonDocument> lstEntity, InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }


            InsertDocumentList(_entityInfo.CollectionName, lstEntity, options, cancellationToken);
        }



    }
}
