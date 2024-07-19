using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    /// <summary>
    /// SortedSet方式的缓存项
    /// </summary>
    public interface ICacheSortedSet
    {
        /// <summary>
        /// 向排序表增加元素
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        long Add(object value, double key, SetValueType setType= SetValueType.Set);
        /// <summary>
        /// 向排序表增加元素
        /// </summary>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        long AddRang(IEnumerable<SortedSetItem> values, SetValueType setType = SetValueType.Set);
        /// <summary>
        /// 减少排序的元素的排序的分值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="score">要减少的分</param>
        /// <returns></returns>
        double Decrement(object value, double score=1);


        /// <summary>
        /// 增加排序的元素的排序分
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="score">要增加的分</param>
        /// <returns></returns>
        double Increment(object value, double score=1);



        /// <summary>
        /// 获取符合key范围的集合长度
        /// </summary>
        /// <param name="min">范围最小值</param>
        /// <param name="max">范围最大值</param>
        /// <returns></returns>
        long GetLength(double? min, double? max);
        

        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <param name="order">排序方式</param>
        /// <returns></returns>
        SortedSetItem[] GetRangeByRankWithScores(long start = 0, long stop = -1, SortType order = SortType.ASC);
        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <param name="order">排序方式</param>
        T[] GetRangeByRank<T>(long start = 0, long stop = -1, SortType order = SortType.ASC);



        /// <summary>
        /// 根据Key范围获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start">Key值开始</param>
        /// <param name="stop">Key值结束</param>
        /// <param name="order">排序方式</param>
        /// <param name="skip">跳过第几条</param>
        /// <param name="take">获取几条</param>
        /// <returns></returns>
        T[] GetRangeByScore<T>(double? start, double? stop,
            SortType order = SortType.ASC, long skip = 0, long take = -1);
        /// <summary>
        /// 根据Key范围获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start">Key值开始</param>
        /// <param name="stop">Key值结束</param>
        /// <param name="order">排序方式</param>
        /// <param name="skip">跳过第几条</param>
        /// <param name="take">获取几条</param>
        /// <returns></returns>
        SortedSetItem[] GetRangeByScoreWithScores(double? start, double? stop,
        SortType order = SortType.ASC, long skip = 0, long take = -1);


        /// <summary>
        /// 根据值范围获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="order">排序方式</param>
        /// <param name="skip">跳过</param>
        /// <param name="take">获取几条</param>
        /// <returns></returns>
        T[] GetRangeByValue<T>(object min, object max, long skip = 0, long take = -1);



        /// <summary>
        /// 获取符合此值的索引
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="order">搜索的排序方式</param>
        /// <returns></returns>
        long GetIndex(object value, SortType order = SortType.ASC);
        /// <summary>
        /// 根据值获取Key
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="order">搜索的排序方式</param>
        /// <returns></returns>
        double? GetKeyByValue(object value);
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        long Remove(object value);
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values">要删除的值</param>
        /// <returns></returns>
        long Remove(object[] values);

        /// <summary>
        /// 根据索引范围删除值
        /// </summary>
        /// <param name="start">索引开始</param>
        /// <param name="stop">结束索引</param>
        /// <returns></returns>
        long RemoveRangeByRank(long start = 0, long stop = -1);
        /// <summary>
        /// 根据Key删除值
        /// </summary>
        /// <param name="start">Key开始</param>
        /// <param name="stop">结束Key</param>
        /// <returns></returns>
        long RemoveRangeByKey(double? start, double? stop);
        /// <summary>
        /// 根据值范围删除
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        long RemoveRangeByValue(object min, object max);


        /// <summary>
        /// 向排序表增加元素
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">键</param>
        /// <returns></returns>
         Task<long> AddAsync(object value, double key, SetValueType setType = SetValueType.Set);
        /// <summary>
        /// 向排序表增加元素
        /// </summary>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        Task<long> AddRangAsync(IEnumerable<SortedSetItem> values, SetValueType setType = SetValueType.Set);
        /// <summary>
        /// 减少排序的元素的排序的分值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="score">要减少的分</param>
        /// <returns></returns>
        Task<double> DecrementAsync(object value, double score = 1);


        /// <summary>
        /// 增加排序的元素的排序分
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="score">要增加的分</param>
        /// <returns></returns>
        Task<double> IncrementAsync(object value, double score = 1);



        /// <summary>
        /// 获取符合key范围的集合长度
        /// </summary>
        /// <param name="min">范围最小值</param>
        /// <param name="max">范围最大值</param>
        /// <returns></returns>
        Task<long> GetLengthAsync(double? min, double? max);


        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <param name="order">排序方式</param>
        /// <returns></returns>
        Task<SortedSetItem[]> GetRangeByRankWithScoresAsync(long start = 0, long stop = -1, SortType order = SortType.ASC);
        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <param name="start">开始索引</param>
        /// <param name="stop">结束索引</param>
        /// <param name="order">排序方式</param>
        Task<T[]> GetRangeByRankAsync<T>(long start = 0, long stop = -1, SortType order = SortType.ASC);



        /// <summary>
        /// 根据Key范围获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start">Key值开始</param>
        /// <param name="stop">Key值结束</param>
        /// <param name="order">排序方式</param>
        /// <param name="skip">跳过第几条</param>
        /// <param name="take">获取几条</param>
        /// <returns></returns>
        Task<T[]> GetRangeByScoreAsync<T>(double? start, double? stop,
            SortType order = SortType.ASC, long skip = 0, long take = -1);
        /// <summary>
        /// 根据Key范围获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start">Key值开始</param>
        /// <param name="stop">Key值结束</param>
        /// <param name="order">排序方式</param>
        /// <param name="skip">跳过第几条</param>
        /// <param name="take">获取几条</param>
        /// <returns></returns>
        Task<SortedSetItem[]> GetRangeByScoreWithScoresAsync(double? start, double? stop,
        SortType order = SortType.ASC, long skip = 0, long take = -1);


        /// <summary>
        /// 根据值范围获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="order">排序方式</param>
        /// <param name="skip">跳过</param>
        /// <param name="take">获取几条</param>
        /// <returns></returns>
        Task<T[]> GetRangeByValueAsync<T>(object min, object max, long skip = 0, long take = -1);



        /// <summary>
        /// 获取符合此值的索引
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="order">搜索的排序方式</param>
        /// <returns></returns>
        Task<long> GetIndexAsync(object value, SortType order = SortType.ASC);
        /// <summary>
        /// 根据值获取Key
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="order">搜索的排序方式</param>
        /// <returns></returns>
        Task<double?> GetKeyByValueAsync(object value);
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task<long> RemoveAsync(object value);
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values">要删除的值</param>
        /// <returns></returns>
        Task<long> RemoveAsync(object[] values);

        /// <summary>
        /// 根据索引范围删除值
        /// </summary>
        /// <param name="start">索引开始</param>
        /// <param name="stop">结束索引</param>
        /// <returns></returns>
        Task<long> RemoveRangeByRankAsync(long start = 0, long stop = -1);
        /// <summary>
        /// 根据Key删除值
        /// </summary>
        /// <param name="start">Key开始</param>
        /// <param name="stop">结束Key</param>
        /// <returns></returns>
        Task<long> RemoveRangeByKeyAsync(double? start, double? stop);
        /// <summary>
        /// 根据值范围删除
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        Task<long> RemoveRangeByValueAsync(object min, object max);
    }

    
}
