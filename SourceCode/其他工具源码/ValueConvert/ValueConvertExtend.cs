using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ValueConvertExtend
    {
        /// <summary>
        /// 把object类型转成指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ConvertValue<T>(object value, T defaultValue = default(T))
        {
            if (value == null || value is DBNull)
            {

                return defaultValue;
            }
            if (value is T)
            {
                return (T)value;
            }
            Type t = typeof(T);
            if (t == typeof(string))
            {
                return (T)((object)value.ToString());
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }
        /// <summary>
        /// 转换数值到制定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object value, T defaultValue = default(T))
        {
            return ConvertValue<T>(value, defaultValue);
        }

    }

}