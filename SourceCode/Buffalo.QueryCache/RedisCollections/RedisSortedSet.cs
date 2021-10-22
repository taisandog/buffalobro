using Buffalo.DB.CacheManager;
using Buffalo.DB.CacheManager.CacheCollection;
using Buffalo.DB.QueryConditions;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.QueryCache.RedisCollections
{
    public class RedisSortedSet : ICacheSortedSet
    {
        private IDatabase _client;

        private string _key;

        private CommandFlags _commanfFlags;

        private TimeSpan _expiration;
        /// <summary>
        ///  List方式的缓存操作
        /// </summary>
        /// <param name="lst"></param>
        public RedisSortedSet(IDatabase client, string key, 
            CommandFlags commanfFlags, TimeSpan expiration)
        {
            _client = client;
            _key = key;
            _commanfFlags = commanfFlags;
            _expiration = expiration;
        }
        /// <summary>
        /// 向排序表增加元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="source">排序键</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public long Add(object value, double sorce, SetValueType setType) 
        {
            TimeSpan ts = _expiration;

            RedisValue val = RedisConverter.ValueToRedisValue(value);
            When when = RedisAdaperByStackExchange.GetSetValueMode(setType);
            return _client.SortedSetAdd(_key, val, sorce,when, _commanfFlags)?1:0;
            
        }

        private SortedSetEntry LoadSortedSetEntry(SortedSetItem item) 
        {
            RedisValue val = RedisConverter.ValueToRedisValue(item.Value);
            SortedSetEntry ret = new SortedSetEntry(val, item.Key);
            return ret;
        }
        private SortedSetItem LoadSortedSetItem(SortedSetEntry entry)
        {
            SortedSetItem ret = new SortedSetItem(entry.Element, entry.Score);
            return ret;
        }

        /// <summary>
        /// 向排序表增加元素
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="source">排序键</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public long AddRang(IEnumerable<SortedSetItem> values, SetValueType setType)
        {
            TimeSpan ts = _expiration;
            List<SortedSetEntry> lstValues = new List<SortedSetEntry>();

            foreach (SortedSetItem item in values) 
            {
                lstValues.Add(LoadSortedSetEntry(item));

            }
            if (lstValues.Count <= 0) 
            {
                return 0;
            }
            When when = RedisAdaperByStackExchange.GetSetValueMode(setType);
            return _client.SortedSetAdd(_key, lstValues.ToArray(), when, _commanfFlags);
        }
        /// <summary>
        /// 减少排序的元素的排序分
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="source">排序分</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public double Decrement(object value, double score) 
        {
            RedisValue val = RedisConverter.ValueToRedisValue(value);
            return _client.SortedSetDecrement(_key, val, score,  _commanfFlags);
        }

        
        /// <summary>
        /// 增加排序的元素的排序分
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="score">排序分</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public double Increment(object value, double score) 
        {
            RedisValue val = RedisConverter.ValueToRedisValue(value);
            return _client.SortedSetIncrement(_key, val, score, _commanfFlags);
        }

       

        /// <summary>
        /// 获取集合长度
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public long GetLength(double? min , double? max) 
        {
            Exclude exclude = GetExclude<double>(min, max);
            return _client.SortedSetLength(_key, min.GetValueOrDefault(double.NegativeInfinity),
                max.GetValueOrDefault(double.PositiveInfinity), exclude, _commanfFlags);
        }

        /// <summary>
        /// 获取排除项
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private Exclude GetExclude<T>(Nullable<T> min, Nullable<T> max)
            where T : struct
        {
            if(min==null && max == null) 
            {
                return Exclude.Both;//全排除
            }
            
            if (min == null || !min.HasValue)
            {
                return Exclude.Start;
            }
            if (max == null || !max.HasValue)
            {
                return Exclude.Stop;
            }
            return Exclude.None;
        }
        /// <summary>
        /// 获取排除项
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private Exclude GetObjectExclude(object min, object max)
        {
            if (min == null && max == null)
            {
                return Exclude.Both;//全排除
            }

            if (min == null )
            {
                return Exclude.Start;
            }
            if (max == null )
            {
                return Exclude.Stop;
            }
            return Exclude.None;
        }
        /// <summary>
        /// 获取Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private Order GetOeder(DB.QueryConditions.SortType order) 
        {
            switch (order) 
            {
                case DB.QueryConditions.SortType.ASC:
                    return Order.Ascending;
                default:
                    return Order.Descending;
            }
        }
        /// <summary>
        /// 弹出值
        /// </summary>
        /// <param name="count">值的数量</param>
        /// <param name="order">弹出方向，SortType.ASC是从开头弹出，SortType.DESC是从尾部弹出</param>
        /// <returns></returns>
        public SortedSetItem Pop(DB.QueryConditions.SortType order = DB.QueryConditions.SortType.ASC)
        {
            Order corder = GetOeder(order);
            SortedSetEntry? ent=_client.SortedSetPop(_key, corder,_commanfFlags);
            if (ent != null) 
            {
                return LoadSortedSetItem(ent.Value);
            }
            return null;
        }
        /// <summary>
        /// 弹出值
        /// </summary>
        /// <param name="count">值的数量</param>
        /// <param name="order">弹出方向，SortType.ASC是从开头弹出，SortType.DESC是从尾部弹出</param>
        /// <returns></returns>
        public SortedSetItem[] Pop(long count, DB.QueryConditions.SortType order = DB.QueryConditions.SortType.ASC)
        {
            Order corder = GetOeder(order);
            SortedSetEntry[] ents = _client.SortedSetPop(_key, count, corder, _commanfFlags);
            return LoadSortedSetItemArray(ents);
        }

        private SortedSetItem[] LoadSortedSetItemArray(SortedSetEntry[] arr) 
        {
            if (arr.Length <= 0) 
            {
                return null;
            }
            SortedSetItem[] ret = new SortedSetItem[arr.Length];
            for(int i = 0; i < arr.Length; i++) 
            {
                ret[i]=LoadSortedSetItem(arr[i]);
            }
            return ret;
        }
        private T[] LoadValues<T>(RedisValue[] arr)
        {
            if (arr.Length <= 0)
            {
                return null;
            }
            T[] ret = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                ret[i] = RedisConverter.RedisValueToValue<T>(arr[i],default(T));
            }
            return ret;
        }
        private RedisValue[] LoadRedisValues(object[] arr)
        {
            if (arr.Length <= 0)
            {
                return null;
            }
            RedisValue[] ret = new RedisValue[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                ret[i] = RedisConverter.ValueToRedisValue(arr[i]);
            }
            return ret;
        }
        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public SortedSetItem[] GetRangeByRankWithScores(long start = 0, long stop = -1,
            DB.QueryConditions.SortType order = DB.QueryConditions.SortType.ASC) 
        {
            Order corder = GetOeder(order);
            SortedSetEntry[] ents = _client.SortedSetRangeByRankWithScores(_key, start, stop, corder,_commanfFlags);
            return LoadSortedSetItemArray(ents);
        }
        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public T[] GetRangeByRank<T>(long start = 0, long stop = -1,
            DB.QueryConditions.SortType order = DB.QueryConditions.SortType.ASC)
        {
            Order corder = GetOeder(order);
            RedisValue[] vals = _client.SortedSetRangeByRank(_key, start, stop, corder, _commanfFlags);
            return LoadValues<T>(vals);
        }

        
        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public T[] GetRangeByScore<T>(double? start, double? stop,
            DB.QueryConditions.SortType order = DB.QueryConditions.SortType.ASC,
            long skip = 0, long take = -1)
        {
            Order corder = GetOeder(order);
            Exclude exclude = GetExclude<double>(start, stop);
            RedisValue[] vals = _client.SortedSetRangeByScore(_key, start.GetValueOrDefault(double.NegativeInfinity),
                stop.GetValueOrDefault(double.PositiveInfinity), exclude, corder,skip,take, _commanfFlags);
            return LoadValues<T>(vals);
        }
        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public SortedSetItem[] GetRangeByScoreWithScores(double? start, double? stop,
        DB.QueryConditions.SortType order = DB.QueryConditions.SortType.ASC, long skip = 0, long take = -1)
        {

            Order corder = GetOeder(order);
            Exclude exclude = GetExclude<double>(start, stop);
            SortedSetEntry[] vals = _client.SortedSetRangeByScoreWithScores(_key, start.GetValueOrDefault(double.NegativeInfinity),
                stop.GetValueOrDefault(double.PositiveInfinity), exclude, corder, skip, take, _commanfFlags);
            return LoadSortedSetItemArray(vals);
        }

        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public T[] GetRangeByValue<T>(object min, object max,
            DB.QueryConditions.SortType order = DB.QueryConditions.SortType.ASC, long skip = 0, long take = -1)
        {
            Order corder = GetOeder(order);
            RedisValue valMin = RedisConverter.ValueToRedisValue(min);
            RedisValue valMax = RedisConverter.ValueToRedisValue(max);
            Exclude exclude = GetObjectExclude(min, max);
            RedisValue[] vals = _client.SortedSetRangeByValue(_key, valMin, valMax,
                exclude, corder,skip,take, _commanfFlags);
            return LoadValues<T>(vals);
        }

        

        /// <summary>
        /// 获取索引
        /// </summary>
        /// <param name="value"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public long GetIndex(object value, 
            DB.QueryConditions.SortType order = DB.QueryConditions.SortType.ASC) 
        {
            RedisValue val = RedisConverter.ValueToRedisValue(value);
            Order corder = GetOeder(order);
            return _client.SortedSetRank(_key,val, corder, _commanfFlags).GetValueOrDefault(-1);
        }
        /// <summary>
        /// 获取索引
        /// </summary>
        /// <param name="value"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public double? GetKeyByValue(object value)
        {
            RedisValue val = RedisConverter.ValueToRedisValue(value);
            return _client.SortedSetScore(_key, val,  _commanfFlags);
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long Remove(object value)
        {
            RedisValue val = RedisConverter.ValueToRedisValue(value);
            return _client.SortedSetRemove(_key, val,_commanfFlags)?1:0;
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long Remove(object[] values) 
        {
            RedisValue[] redValues = LoadRedisValues(values);
            return _client.SortedSetRemove(_key, redValues, _commanfFlags);
        }

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long RemoveRangeByRank(long start=0, long stop=-1)
        {
            return _client.SortedSetRemoveRangeByRank(_key, start, stop, _commanfFlags);
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long RemoveRangeByKey(double? start, double? stop)
        {
            Exclude exclude = GetExclude<double>(start, stop);
            return _client.SortedSetRemoveRangeByScore(_key, start.GetValueOrDefault(double.NegativeInfinity),
                stop.GetValueOrDefault(double.PositiveInfinity), exclude, _commanfFlags);
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long RemoveRangeByValue(object min, object max)
        {
            RedisValue valMin = RedisConverter.ValueToRedisValue(min);
            RedisValue valMax = RedisConverter.ValueToRedisValue(max);
            Exclude exclude = GetObjectExclude(min, max);
            return _client.SortedSetRemoveRangeByValue(_key, valMin, valMax,
                exclude, _commanfFlags);
        }
    }

   
}
