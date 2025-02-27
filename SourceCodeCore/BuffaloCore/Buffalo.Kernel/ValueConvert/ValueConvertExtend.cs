using System;
using System.Collections.Generic;
using System.Text;

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
    public static T ConvertValue<T>(object value, T defaultValue)
    {
        if (value == null || value is DBNull || (value as string)=="")
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
    /// 把object类型转成指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static object ConvertValue(object value, Type targetType,object defaultValue)
    {
        if (value == null || value is DBNull || (value as string) == "")
        {

            return defaultValue;
        }
        if (value.GetType() == targetType)
        {
            return value;
        }

        
        if (targetType == typeof(string))
        {
            return value.ToString();
        }
        return Convert.ChangeType(value, targetType);
    }
    /// <summary>
    /// 把object类型转成指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static T ConvertValue<T>(object value)
    {


        return ConvertValue<T>(value, default(T));
    }
#if !NET_2_0
    /// <summary>
    /// 转换数值到指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static T ConvertTo<T>(this object value, T defaultValue)
    {
        return ConvertValue<T>(value, defaultValue);
    }
    /// <summary>
    /// 转换数值到指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static T ConvertTo<T>(this object value)
    {
        return ConvertValue<T>(value, default(T));
    }
#endif
}

