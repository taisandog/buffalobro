using Buffalo.MongoDB.QueryCondition;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    public partial class MongoDBOperate<TDocument>
    {
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public List<TDocument> SelectList(ConditionList<TDocument> query, string collectionName)
        {
            //IMongoDatabase db = MongoDBManager.GetMongoClient(_dbInfo.DBKey);
            IMongoCollection<TDocument> mc = _db.GetCollection<TDocument>(collectionName);
            FilterDefinition<TDocument> qu = null;
            if (query.Count > 0)
            {
                qu = ConditionList<TDocument>.Where.And(query);
            }
            else
            {
                 qu = ConditionList<TDocument>.Where.Empty;
            }
            
            IFindFluent<TDocument, TDocument> cus = mc.Find(qu);
            cus = FillSort(cus, query.OrderBy);//填充排序
            cus = FillPage(cus, query.PageContext);//填充分页
            List<TDocument> lst = new List<TDocument>();
            using (IAsyncCursor<object> reader = cus.ToCursor())
            {
                while (reader.MoveNext())
                {
                    foreach (TDocument doc in reader.Current)
                    {
                        lst.Add(doc);
                        doc.GetEntityBaseInfo().CancelUpdateProperty(null);
                    }
                }
            }
            return lst;
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="definition">索引定义</param>
        /// <param name="collectionName">集合</param>

        public string CreateIndex(IndexKeysDefinition<TDocument> definition, string collectionName )
        {
            //IMongoDatabase db = MongoDBManager.GetMongoClient(_dbInfo.DBKey);
            IMongoCollection<TDocument> mc = _db.GetCollection<TDocument>(collectionName);
            CreateIndexModel<TDocument> model = new CreateIndexModel<TDocument>(definition);
            return mc.Indexes.CreateOne(model);
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="definition">索引定义</param>

        public string CreateIndex(IndexKeysDefinition<TDocument> definition)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            return CreateIndex(definition,_entityInfo.CollectionName);
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="definitions">索引定义</param>
        /// <param name="collectionName">集合</param>
        public IEnumerable<string> CreateManyIndex(IEnumerable<IndexKeysDefinition<TDocument>> definitions, string collectionName)
        {
            IMongoCollection<TDocument> mc = _db.GetCollection<TDocument>(collectionName);
            List<CreateIndexModel<TDocument>> lstModel = new List<CreateIndexModel<TDocument>>(20);
            foreach (IndexKeysDefinition<TDocument> definition in definitions) 
            {
                CreateIndexModel<TDocument> model = new CreateIndexModel<TDocument>(definition);
                lstModel.Add(model);
            }
            
            return mc.Indexes.CreateMany(lstModel);
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="definitions">索引定义</param>
        /// <param name="collectionName">集合</param>
        public IEnumerable<string> CreateManyIndex(IEnumerable<IndexKeysDefinition<TDocument>> definitions)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            return CreateManyIndex(definitions, _entityInfo.CollectionName);
        }

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<TDocument> SelectList(ConditionList<TDocument> query)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            return SelectList(query, _entityInfo.CollectionName);
        }

        /// <summary>
        /// 查询单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public TDocument GetUnique(ConditionList<TDocument> query, string collectionName)
        {
            //IMongoDatabase db = MongoDBManager.GetMongoClient(_dbInfo.DBKey);
            IMongoCollection<TDocument> mc = _db.GetCollection<TDocument>(collectionName);
            FilterDefinition<TDocument> qu = null;
            if (query.Count > 0)
            {
                qu = ConditionList<TDocument>.Where.And(query);
            }
            else
            {
                qu = ConditionList<TDocument>.Where.Empty;
            }

            IFindFluent<TDocument, TDocument> cus = mc.Find(qu);
            
            return cus.FirstOrDefault<TDocument>();
        }

        /// <summary>
        /// 查询单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public TDocument GetUnique(ConditionList<TDocument> query)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            return GetUnique(query, _entityInfo.CollectionName);
        }

        /// <summary>
        /// 填充排序
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="cus"></param>
        /// <param name="lstSort"></param>
        /// <returns></returns>
        private IFindFluent<TDocument, TProjection> FillSort<TProjection>(IFindFluent<TDocument, TProjection> cus, MGSortList lstSort)
        {

            SortDefinitionBuilder<TDocument> sortBuild = Builders<TDocument>.Sort;
            SortDefinition<TDocument> sortItem = null;
            foreach (MGSort sort in lstSort)
            {
                if (sort.SortType == MGSortType.ASC)
                {
                    if (sortItem == null)
                    {
                        sortItem = sortBuild.Ascending(sort.PropertyName);
                    }
                    sortItem = sortItem.Ascending(sort.PropertyName);
                    continue;

                }
                if (sortItem == null)
                {
                    sortItem = sortBuild.Descending(sort.PropertyName);
                }
                sortItem = sortItem.Descending(sort.PropertyName);
            }
            if (sortItem != null)
            {
                cus = cus.Sort(sortItem);
            }
            return cus;
        }


        /// <summary>
        /// 填充分页
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="cus"></param>
        /// <param name="lstSort"></param>
        /// <returns></returns>
        private IFindFluent<TDocument, TProjection> FillPage<TProjection>(IFindFluent<TDocument, TProjection> cus, MGPageContent page)
        {
            if (page.PageSize <= 0)
            {
                return cus;
            }
            if (page.IsFillTotalRecords)//填充总页数
            {
                page.TotalRecords = cus.CountDocuments();
            }
            int start = (int)page.GetStarIndex();
            return cus.Skip(start).Limit(page.PageSize);
        }

        /// <summary>
        /// 查询统计
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <param name="dbMatch">条件</param>
        /// <param name="dbGroup">统计</param>
        /// <returns></returns>
        private List<BsonDocument> SelectGroupBy(string collectionName, FilterDefinition<TDocument> dbMatch, ProjectionDefinition<TDocument, BsonDocument> dbGroup)
        {
            //IMongoDatabase db = MongoDBManager.GetMongoClient(_dbInfo.DBKey);
            IMongoCollection<TDocument> mc = _db.GetCollection<TDocument>(collectionName);
            IAggregateFluent<TDocument> query = mc.Aggregate();
            if (dbMatch != null)
            {
                query = query.Match(dbMatch);
            }
            
            return query.Group(dbGroup).ToList();
        }
        /// <summary>
        /// 查询统计
        /// </summary>
        /// <param name="dbMatch">条件</param>
        /// <param name="dbGroup">统计</param>
        /// <returns></returns>
        public List<BsonDocument> SelectGroupBy(BsonDocument dbMatch, BsonDocument dbGroup)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }

            return SelectGroupBy(_entityInfo.CollectionName, dbMatch, dbGroup);
        }
        /// <summary>
        /// 查询统计
        /// </summary>
        /// <param name="query">条件</param>
        /// <returns></returns>
        public List<BsonDocument> SelectGroupBy(ConditionList<TDocument> query)
        {
            if (string.IsNullOrWhiteSpace(_entityInfo.CollectionName))
            {
                throw new Exception("找不到实体对应的集合名");
            }
            FilterDefinition<TDocument> qu = null;
            if (query.Count > 0)
            {
                qu = ConditionList<TDocument>.Where.And(query);
            }

            List<BsonElement> element = query.ShowPropertyElement;
            List<BsonElement> showItem = query.ShowProperty;
            BsonElement id;
            if (element == null || element.Count == 0)
            {
                id = new BsonElement("_id", new BsonDocument(new BsonElement("None", "None")));
                element.Add(id);
            }
            else
            {
                id = new BsonElement("_id", new BsonDocument(showItem));
                element.Add(id);
            }
            BsonDocument dbGroup = new BsonDocument(element);
            return SelectGroupBy(_entityInfo.CollectionName, qu, dbGroup);
        }
    }
}
