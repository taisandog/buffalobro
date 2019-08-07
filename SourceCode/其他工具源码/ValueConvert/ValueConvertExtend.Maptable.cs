using System;
using System.Collections;
using System.Text;
namespace Buffalo.Kernel
{
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
        public static T GetMapDataValue<T>(IDictionary ht, string key, T defalutValue = default(T))
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
        /// <param name="key">指定Key</param>
        /// <param name="defalutValue">找不到时候返回的默认值</param>
        /// <returns></returns>
        public static T GetMapValue<T>(this IDictionary ht, string key, T defalutValue = default(T))
        {

            return GetMapDataValue<T>(ht, key, defalutValue);
        }
        

    }

}