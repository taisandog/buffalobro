using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.FastReflection.ClassInfos;
using System.Reflection;
using Buffalo.Kernel.Defaults;
using System.Collections;
using System.Web.Script.Serialization;

namespace Buffalo.Kernel.ObjectSerializer
{
    /// <summary>
    /// 序列化助手
    /// </summary>
    public partial class SerializerHelper
    {
         private static JavaScriptSerializer _serializer = new JavaScriptSerializer();
         #region Json序列化
         /// <summary>
         /// 填充到Json节点
         /// </summary>
         /// <param name="node"></param>
         public static void FillJsonItem(object source, Dictionary<string,object> node)
         {
             Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(source.GetType());
             FillJsonNode(source, node, dicSource);
         }
         /// <summary>
         /// 填充到Json节点
         /// </summary>
         /// <param name="node"></param>
         private static void FillJsonNode(object source, Dictionary<string, object> node, Dictionary<string, PropertyInfoHandle> dicSource)
         {
             foreach (KeyValuePair<string, PropertyInfoHandle> kvp in dicSource)
             {
                 object value = kvp.Value.GetValue(source);

                 node[kvp.Key] = ToJsonValue(value);
             }
         }
         /// <summary>
         /// 填充到Json节点
         /// </summary>
         /// <param name="node"></param>
         public static string ObjectToJson(object source)
         {
             
             if (source == null)
             {
                 return "";
             }
             Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(source.GetType());

             Dictionary<string, object> dicObj = new Dictionary<string, object>();

             FillJsonNode(source, dicObj, dicSource);

             return _serializer.Serialize(dicObj);
         }
         /// <summary>
         /// 填充到Json节点
         /// </summary>
         /// <param name="node"></param>
         public static string ListToJson(IList lstSource)
         {
             if (lstSource == null)
             {
                 return "";
             }
             Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(lstSource[0].GetType());
             List<Dictionary<string, object>> lstRet = new List<Dictionary<string, object>>();
             foreach (object source in lstSource)
             {
                 Dictionary<string, object> dicObj = new Dictionary<string, object>();
                 lstRet.Add(dicObj);
                 FillJsonNode(source, dicObj, dicSource);
             }
             return _serializer.Serialize(lstRet);
         }
         /// <summary>
         /// 填充到Json节点
         /// </summary>
         /// <param name="node"></param>
         private static void JsonFillObject(Dictionary<string, object> node, Type objType, object obj, Dictionary<string, PropertyInfoHandle> dicSource)
         {
             object tmp=null;
             foreach (KeyValuePair<string, PropertyInfoHandle> kvp in dicSource)
             {

                 if (node.TryGetValue(kvp.Key,out tmp))
                 {
                     tmp = JsonValueToValue(tmp, kvp.Value.PropertyType);
                     kvp.Value.SetValue(obj, tmp);
                 }
             }
         }
         /// <summary>
         /// Json节点填充到实体
         /// </summary>
         /// <param name="node"></param>
         public static void JsonFillObject(Dictionary<string, object> node, object obj)
         {
             Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(obj.GetType());
             JsonFillObject(node, obj.GetType(), obj, dicSource);
         }
         /// <summary>
         /// Json填充到实体
         /// </summary>
         /// <param name="node"></param>
         public static object JsonToObject(string json, Type objType)
         {
             Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(objType);
             List<Dictionary<string, object>> lstNode = _serializer.Deserialize<List<Dictionary<string, object>>>(json);

             if (lstNode.Count == 0)
             {
                 return null;
             }
             Dictionary<string, object> item = lstNode[0];
             object ret = Activator.CreateInstance(objType);
             JsonFillObject(item, objType, ret, dicSource);
             return ret;
         }
         /// <summary>
         /// Json节点填充到实体
         /// </summary>
         /// <param name="node"></param>
         public static T JsonToObject<T>(string json) where T : new()
         {
             Type objType = typeof(T);
             object ret = JsonToObject(json, objType);
             return ret == null ? default(T) : (T)ret;
         }
         /// <summary>
         /// Json节点填充到实体
         /// </summary>
         /// <param name="node"></param>
         public static List<T> JsonToList<T>(string json) where T : new()
         {
             List<T> ret = new List<T>();

             List<Dictionary<string, object>> lstNode = _serializer.Deserialize<List<Dictionary<string, object>>>(json);
             Type objType = typeof(T);
             Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(objType);
             foreach (Dictionary<string, object> node in lstNode)
             {
                 T item = (T)Activator.CreateInstance(objType);
                 JsonFillObject(node, objType, item, dicSource);
                 ret.Add(item);
             }
             return ret;
         }
         #endregion
         /// <summary>
         /// 把值转成字符串
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         private static object ToJsonValue(object value)
         {
             if (value == null)
             {
                 return null;
             }
             if (value is Enum)
             {
                 return ((int)value);
             }
             if (value is byte[])
             {
                 return CommonMethods.BytesToHexString((byte[])value);
             }
             if (value is bool)
             {
                 return (((bool)value) ? 1 :0);
             }
             return value;
         }
         /// <summary>
         /// 把值转成字符串
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         private static object JsonValueToValue(object value, Type realType)
         {
             if (value == null) 
             {
                 return null;
             }
             
             realType = DefaultType.GetRealValueType(realType);
             if (value.GetType() == realType) 
             {
                 return value;
             }
             if (DefaultType.IsInherit(realType, typeof(Enum)))
             {
                 int ivalue = Convert.ToInt32(value);
                 return ivalue;
             }
             if (DefaultType.IsInherit(realType, typeof(byte[])))
             {
                 return CommonMethods.HexStringToBytes(value as string);
             }
             if (DefaultType.IsInherit(realType, typeof(bool)))
             {
                 return Convert.ToInt32(value) != 0;
             }
             return Convert.ChangeType(value, realType);
         }
    }
}
