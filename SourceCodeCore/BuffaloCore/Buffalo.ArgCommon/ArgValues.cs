
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace Buffalo.ArgCommon
{

    /// <summary>
    /// 参数信息
    /// </summary>
    public class ArgValues : Dictionary<string, object>
    {

        /// <summary>
        /// 会话码
        /// </summary>
        public string Token
        {
            get 
            {
                return GetDataValue<string>("Token");
            }
            
        }
        

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public object GetObject(string key) 
        {
            object val = null;
            if (!TryGetValue(key, out val))
            {
                return null;
            }
            return val;
        }

         /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        public T GetDataValue<T>()
        {
            return GetDataValue<T>("data", default(T));
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        public T GetDataValue<T>(string key)
        {
            return GetDataValue<T>(key, default(T));
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        public T GetDataValue<T>(string key,T defalutValue)
        {
            object val = null;
            if (!TryGetValue(key, out val))
            {
                return defalutValue;
            }
            
            JToken jt = val as JToken;
            if (jt != null)
            {
                try
                {
                    JToken jtInner = jt[key] as JToken;
                    if (jtInner != null)
                    {
                        return jtInner.ToObject<T>();
                    }
                }
                catch { }
                return jt.ToObject<T>();
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                Type realType = Enum.GetUnderlyingType(type);
                type = realType;
            }

            return (T)Convert.ChangeType(val, type);
            
        }
        
    }
}
