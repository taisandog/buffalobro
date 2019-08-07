using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 类型转换扩展
/// </summary>
public static partial class ValueConvertExtend
{

    /// <summary>
    /// 获取哈希表的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ht">哈希表</param>
    /// <param name="key">键</param>
    /// <param name="defalutValue"></param>
    /// <returns></returns>
    public static T GetMapDataValue<T>(IDictionary ht, object key, T defalutValue)
    {

        if (!ht.Contains(key))
        {
            return defalutValue;
        }
        object value = ht[key];

        return ConvertValue<T>(value, defalutValue);
    }
    /// <summary>
    /// 获取哈希表的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ht">哈希表</param>
    /// <param name="key">键</param>
    /// <param name="defalutValue"></param>
    /// <returns></returns>
    public static T GetMapDataValue<T>(IDictionary ht, object key)
    {



        return GetMapDataValue<T>(ht, key, default(T));
    }

    /// <summary>
    /// 获取哈希表的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ht">哈希表</param>
    /// <param name="key">键</param>
    /// <param name="defalutValue"></param>
    /// <returns></returns>
    public static K GetDicDataValue<T, K>(IDictionary<T, K> ht, T key, K defalutValue)
    {
        K val = defalutValue;
        if (!ht.TryGetValue(key, out val))
        {
            return defalutValue;
        }
        return val;
    }
    /// <summary>
    /// 获取哈希表的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ht">哈希表</param>
    /// <param name="key">键</param>
    /// <param name="defalutValue"></param>
    /// <returns></returns>
    public static K GetDicDataValue<T, K>(IDictionary<T, K> ht, T key)
    {
        return GetDicDataValue<T, K>(ht, key, default(K));
    }
#if !NET_2_0
    /// <summary>
    /// 获取哈希表的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ht">哈希表</param>
    /// <param name="key">指定Key</param>
    /// <param name="defalutValue">找不到时候返回的默认值</param>
    /// <returns></returns>
    public static T GetMapValue<T>(this IDictionary ht, object key, T defalutValue)
    {

        return GetMapDataValue<T>(ht, key, defalutValue);
    }
    /// <summary>
    /// 获取哈希表的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ht">哈希表</param>
    /// <param name="key">指定Key</param>
    /// <param name="defalutValue">找不到时候返回的默认值</param>
    /// <returns></returns>
    public static T GetMapValue<T>(this IDictionary ht, object key)
    {

        return GetMapValue<T>(ht, key, default(T));
    }


    /// <summary>
    /// 获取哈希表的值
    /// </summary>
    /// <typeparam name="T">键类型</typeparam>
    /// <typeparam name="K">值类型</typeparam>
    /// <param name="ht">哈希表</param>
    /// <param name="key">指定Key</param>
    /// <param name="defalutValue">找不到时候返回的默认值</param>
    /// <returns></returns>
    public static K GetDicValue<T,K>(this IDictionary<T,K> ht, T key, K defalutValue)
    {
        return GetDicDataValue<T,K>(ht, key, defalutValue);
    }
    /// <summary>
    /// 获取哈希表的值
    /// </summary>
    /// <typeparam name="T">键类型</typeparam>
    /// <typeparam name="K">值类型</typeparam>
    /// <param name="ht">哈希表</param>
    /// <param name="key">指定Key</param>
    /// <returns></returns>
    public static K GetDicValue<T, K>(this IDictionary<T, K> ht, T key)
    {
        return GetDicDataValue<T, K>(ht, key, default(K));
    }
#endif
}

