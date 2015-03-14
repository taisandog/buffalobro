using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections;
using Buffalo.Kernel;

namespace Buffalo.WebKernel.JsonUnit
{
    public class JsonHelper
    {
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();

        /// <summary>
        /// 把Json信息解释成字典
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DeserializeObject(string json) 
        {
            Dictionary<string, object> ret = serializer.Deserialize<Dictionary<string, object>>(json);
            return ret;
        }
        /// <summary>
        /// 把json信息填充到实体
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static void DeserializeObject(object obj,string json, IEnumerable<string> propertyMap = null)
        {
            Dictionary<string, object> dic = serializer.Deserialize<Dictionary<string, object>>(json);
            EntitySerializer.DeserializeToObject(obj,dic, propertyMap);

        }
        /// <summary>
        /// 把Json信息解释成实体
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string json, IEnumerable<string> propertyMap = null)
            where T:new()
        {
            Dictionary<string, object> dic = serializer.Deserialize<Dictionary<string, object>>(json);
            T obj = EntitySerializer.DeserializeToObject<T>(dic, propertyMap);
            return obj;
        }
        /// <summary>
        /// 把字典序列化成Json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string SerializeObject(Dictionary<string, object> dic)
        {
            string json = serializer.Serialize(dic);
            return json;
        }

        /// <summary>
        /// 把字典集合序列化成Json
        /// </summary>
        /// <param name="lst">集合</param>
        /// <returns></returns>
        public static string SerializeList(List<Dictionary<string, object>> lst)
        {
            string json = serializer.Serialize(lst);
            return json;
        }
        /// <summary>
        /// 把Json转回字典集合
        /// </summary>
        /// <param name="lst">集合</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> DeSerializeList(string json)
        {
            List<Dictionary<string, object>> ret = serializer.Deserialize<List<Dictionary<string, object>>>(json);
            return ret;
        }
        /// <summary>
        /// 把Json转回字典集合
        /// </summary>
        /// <param name="lst">集合</param>
        /// <returns></returns>
        public static List<T> DeSerializeList<T>(string json, IEnumerable<string> propertyMap = null)
             where T : new()
        {
            List<Dictionary<string, object>> dic = serializer.Deserialize<List<Dictionary<string, object>>>(json);
            List<T> lst = EntitySerializer.DeserializeToList<T>(dic, propertyMap);
            return lst;
        }
        /// <summary>
        /// 获取字典里的值
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(Dictionary<string, object> dic, string key) 
        {
            object value=null;
            if (dic.TryGetValue(key, out value)) 
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// 实体集合转成Json
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyMap">Key的名字</param>
        /// <param name="formatValue">格式化值的方法</param>
        /// <returns></returns>
        public static string SerializeList(IList lstEntity,
            IEnumerable<string> propertyMap, DelFormatValue formatValue)
        {
            List<Dictionary<string, object>> lst = EntitySerializer.SerializeListToDictionary(lstEntity, propertyMap, formatValue);
            return SerializeList(lst);
        }
        /// <summary>
        /// 实体集合转成Json
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyMap">Key的名字</param>
        /// <param name="formatValue">格式化值的方法</param>
        /// <returns></returns>
        public static string SerializeList(IList lstEntity,
            IEnumerable<string> propertyMap)
        {
            return SerializeList(lstEntity, propertyMap, EntitySerializer.DefaultFormatValue);
        }
        /// <summary>
        /// 实体转成Json
        /// </summary>
        /// <param name="value">实体</param>
        /// <param name="propertyMap">Key的名字</param>
        /// <param name="formatValue">格式化值的方法</param>
        /// <returns></returns>
        public static string SerializeObject(object value,
            IEnumerable<string> propertyMap)
        {

            return SerializeObject(value, propertyMap, EntitySerializer.DefaultFormatValue);
        }
        /// <summary>
        /// 实体转成Json
        /// </summary>
        /// <param name="value">实体</param>
        /// <param name="propertyMap">Key的名字</param>
        /// <param name="formatValue">格式化值的方法</param>
        /// <returns></returns>
        public static string SerializeObject(object value,
            IEnumerable<string> propertyMap, DelFormatValue formatValue)
        {
            Dictionary<string, object> lst = EntitySerializer.SerializeToDictionary(value, propertyMap, formatValue);
            return SerializeObject(lst);
        }
       

        /// <summary>
        /// 获取要返回的结果
        /// </summary>
        /// <param name="status">结果类型</param>
        /// <param name="data">结果信息</param>
        /// <returns></returns>
        public static string GetReturnMessage(Enum status, string data) 
        {
            return GetReturnMessage(Convert.ToInt32(status), data);
        }
        /// <summary>
        /// 获取要返回的结果
        /// </summary>
        /// <param name="status">结果类型</param>
        /// <param name="data">结果信息</param>
        /// <returns></returns>
        public static string GetReturnMessage(int status, string data)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Status"] = status;
            dic["Data"] = data;
            return SerializeObject(dic);
        }
        
    }
}