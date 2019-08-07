using LitJson;
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
        /// 吧类型转成指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T JsonValue<T>(JsonData jvalue)
        {
            if (jvalue == null)
            {
                return default(T);
            }
            object value = null;
            JsonType type = jvalue.GetJsonType();
            switch (type)
            {
                case JsonType.Boolean:
                    value = (bool)jvalue;
                    break;
                case JsonType.Double:
                    value = (double)jvalue;
                    break;
                case JsonType.Int:
                    value = (int)jvalue;
                    break;
                case JsonType.Long:
                    value = (long)jvalue;
                    break;
                case JsonType.Object:
                    value = (object)jvalue;
                    break;
                case JsonType.String:
                    value = (string)jvalue;
                    break;
                default:
                    value = (object)jvalue;
                    break;
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
        /// 获取Json值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jd"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetJsonDataValue<T>(JsonData jd, string key, T defalutValue = default(T))
        {
            IDictionary ijd = jd as IDictionary;
            if (!ijd.Contains(key))
            {
                return defalutValue;
            }
            object value = ijd[key];
            JsonData sValue = value as JsonData;
            if (sValue != null)
            {
                return JsonValue<T>(sValue);
            }
            return ConvertValue<T>(value, defalutValue);
        }

        /// <summary>
        /// 获取Json值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jd">JsonData</param>
        /// <param name="key">指定Key</param>
        /// <param name="defalutValue">找不到时候返回的默认值</param>
        /// <returns></returns>
        public static T GetJsonValue<T>(this JsonData jd, string key, T defalutValue = default(T))
        {

            return GetJsonDataValue<T>(jd, key, defalutValue);
        }
        /// <summary>
        /// 把JsonData转成指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jvalue">JsonData</param>
        /// <returns></returns>
        public static T JsonToValue<T>(this JsonData jvalue)
        {

            return JsonValue<T>(jvalue);
        }

    }

}