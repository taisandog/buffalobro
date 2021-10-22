using Buffalo.DB.DbCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    /// <summary>
    /// Hash方式的缓存项
    /// </summary>
    public interface ICacheHash
    {
        /// <summary>
        /// 批量给HashSet设置值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="dicSet">值</param>
        /// <returns></returns>
        void SetRangeValue(IDictionary<string, object> dicSet);
        /// <summary>
        /// HashSet设置值
        /// </summary>
        /// <param name="key">哈希表的键</param>
        /// <param name="value">哈希表的值</param>
        /// <param name="type">设置方式</param>
        bool SetValue(string key, object value, SetValueType type= SetValueType.Set);
        /// <summary>
        /// 获取哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">哈希表的键</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        E GetValue<E>(string key, E defaultValue=default(E));
        /// <summary>
        /// 获取所有哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="defaultValue">默认值</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        List<KeyValuePair<K, V>> GetAllValues<K, V>(V defaultValue);
        /// <summary>
        /// 删除哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">哈希表的键</param>
        /// <returns></returns>
        bool DeleteValue(string key);
        /// <summary>
        /// 批量删除哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="keys">要删除哈希表的键</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        long DeleteValues(IEnumerable<string> keys);

        /// <summary>
        /// 是否存在此键
        /// </summary>
        /// <param name="hashkey"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// hash值自增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">要增加的值</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        long Increment(string key, long value=1);
        /// <summary>
        /// hash值自减
        /// </summary>
        /// <param name="hashkey">键</param>
        /// <param name="value">值</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        long Decrement(string key, long value=1);
    }
}
