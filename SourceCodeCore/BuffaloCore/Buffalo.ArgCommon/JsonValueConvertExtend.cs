using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;


public static partial class JsonValueConvertExtend
{
    /// <summary>
    /// 把Json的object类型转成指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static T ConvertJsonValue<T>(object value)
    {
        return ConvertJsonValue<T>(value, default(T));
    }
    /// <summary>
    /// 把Json的object类型转成指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static T ConvertJsonValue<T>(object value, T defaultValue)
    {
        if (value == null)
        {
            return default(T);
        }

        if (value is T)
        {
            return (T)value;
        }
        if (value is string)
        {
            value = JsonConvert.DeserializeObject<T>(value as string);
            return (T)value;
        }
        if (value is JObject)
        {

            value = ((JObject)value).ToObject<T>();
            return (T)value;
        }
        if (value is JArray)
        {

            value = ((JArray)value).ToObject<T>();
            return (T)value;
        }

        return ValueConvertExtend.ConvertValue(value, defaultValue);
    }
#if !NET_2_0
    /// <summary>
    /// 转换Json对象到指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static T ConvertJsonTo<T>(this object value, T defaultValue)
    {
        return ConvertJsonValue<T>(value, defaultValue);
    }
    /// <summary>
    /// 转换Json对象到指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static T ConvertJsonTo<T>(this object value)
    {
        return ConvertJsonValue<T>(value, default(T));
    }
#endif
}

