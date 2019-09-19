using Buffalo.Kernel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB.QueryCondition
{
    /// <summary>
    /// 条件集合
    /// </summary>
    public class ConditionList<TDocument> : List<FilterDefinition<TDocument>>
    {
        private List<BsonElement> _showProperty = new List<BsonElement>();
        private List<BsonElement> _showGroupProterty = new List<BsonElement>();
        /// <summary>
        /// 显示的属性
        /// </summary>
        /// <param name="name">别名</param>
        /// <param name="propertyName">属性</param>
        public void AddShowProperty(string name, string propertyName)
        {
            //BsonElement item = FormatShow(null, MGAggregateType.None, propertyName);
            BsonElement item = new BsonElement(name, propertyName);
            _showGroupProterty.Add(item);
        }

        /// <summary>
        /// 显示的属性
        /// </summary>
        /// <param name="name">别名</param>
        /// <param name="type">类型</param>
        /// <param name="propertyName">属性</param>
        public void AddShowProperty(string name, MGAggregateType type, string propertyName)
        {
            BsonElement item = FormatShow(name, type, propertyName);
            _showProperty.Add(item);
        }
        /// <summary>
        /// 显示的属性
        /// </summary>
        /// <param name="element">属性信息</param>
        public void AddShowProperty(BsonElement element)
        {
            _showProperty.Add(element);
        }
        /// <summary>
        /// 显示的统计
        /// </summary>
        public List<BsonElement> ShowPropertyElement
        {
            get
            {
                return _showProperty;
            }
        }
        /// <summary>
        /// 显示属性
        /// </summary>
        public List<BsonElement> ShowProperty
        {
            get
            {
                return _showGroupProterty;
            }
        }
        /// <summary>
        /// 格式化显示的属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private BsonElement FormatShow(string name, MGAggregateType type, string propertyName)
        {
           
            if (type == MGAggregateType.None)
            {
                return new BsonElement("_id","$"+propertyName);
            }
            string fun = EnumUnit.GetEnumDescription(type);
            BsonDocument value = new BsonDocument(fun, "$" + propertyName);
            return new BsonElement(name, value);
        }
        #region 函数
        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddBetween(string propertyName, object minValue, object maxValue)
        {
            FilterDefinition<TDocument>  con=Where.And(Where.Gte(propertyName, minValue), Where.Lte(propertyName, maxValue));
            this.Add(con);
            return true;
        }
        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddBetween<T>(string propertyName, T minValue, T maxValue)
        {
            FilterDefinition<TDocument> con = Where.And(Where.Gte<T>(propertyName, minValue), Where.Lte<T>(propertyName, maxValue));
            this.Add(con);
            return true;
        }

        /// <summary>
        /// 添加新的大于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMore(string propertyName, object value)
        {

            this.Add(Where.Gt(propertyName, value));
            return true;
        }
        /// <summary>
        /// 添加新的大于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMore<T>(string propertyName, T value)
        {

            this.Add(Where.Gt<T>(propertyName, value));
            return true;
        }
        /// <summary>
        /// 添加新的大于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMoreThen(string propertyName, object value)
        {
            this.Add(Where.Gte(propertyName, value));
            return true;
        }

        /// <summary>
        /// 添加新的大于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMoreThen<T>(string propertyName, T value)
        {
            this.Add(Where.Gte<T>(propertyName, value));
            return true;
        }

        /// <summary>
        /// 添加新的小于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLessThen(string propertyName, object value)
        {
            this.Add(Where.Lte(propertyName, value));
            return true;
        }
        /// <summary>
        /// 添加新的小于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLessThen<T>(string propertyName, T value)
        {
            this.Add(Where.Lte<T>(propertyName, value));
            return true;
        }
        /// <summary>
        /// 添加新的小于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLess(string propertyName, object value)
        {
            this.Add(Where.Lt(propertyName, value));
            return true;
        }

        /// <summary>
        /// 添加新的小于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLess<T>(string propertyName, T value)
        {
            this.Add(Where.Lt<T>(propertyName, value));
            return true;
        }

        /// <summary>
        /// 添加新的In条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddIn<T>(string propertyName, IEnumerable<T> valuesCollection)
        {
            this.Add(Where.In<T>(propertyName, valuesCollection));
            return true;
        }
        /// <summary>
        /// 添加新的In条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotIn<T>(string propertyName, IEnumerable<T> valuesCollection)
        {
            this.Add(Where.Nin<T>(propertyName, valuesCollection));
            return true;
        }
        /// <summary>
        /// 添加新的不等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotEqual<T>(string propertyName, T value)
        {
            this.Add(Where.Ne<T>(propertyName, value));
            return true;
        }

        /// <summary>
        /// 添加新的等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddEqual<T>(string propertyName, T value)
        {
            this.Add(Where.Eq<T>(propertyName, value));
            return true;
        }
        #endregion


        private MGSortList _order = new MGSortList();

        public MGSortList OrderBy
        {
            get
            {
                return _order;
            }
        }

        private MGPageContent _pageContext = new MGPageContent();
        /// <summary>
        /// 分页
        /// </summary>
        public MGPageContent PageContext
        {
            get
            {
                return _pageContext;
            }
            set
            {
                _pageContext = value;
            }
        }

        /// <summary>
        /// 条件生成器
        /// </summary>
        public static FilterDefinitionBuilder<TDocument> Where
        {
            get
            {
                return Builders<TDocument>.Filter;
            }
        }
        /// <summary>
        /// 索引管理器
        /// </summary>
        public static IndexKeysDefinitionBuilder<TDocument> Index
        {
            get
            {
                return Builders<TDocument>.IndexKeys;
            }
        }
    }
    public class ConditionList : ConditionList<BsonDocument>
    {
    }
}
