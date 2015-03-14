using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.FastReflection.ClassInfos;
using System.Reflection;
using System.Xml;
using Buffalo.Kernel.Defaults;
using System.Collections;

namespace Buffalo.Kernel.ObjectSerializer
{
    /// <summary>
    /// 序列化助手
    /// </summary>
    public partial class SerializerHelper
    {
        /// <summary>
        /// 属性拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CpoyTo(object source, object target) 
        {
            Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(source.GetType());
            Dictionary<string, PropertyInfoHandle> dicTarget = GetHandle(target.GetType());

            PropertyInfoHandle tinfo = null;
            foreach (KeyValuePair<string, PropertyInfoHandle> kvp in dicSource) 
            {
                string key = kvp.Key;
                
                if (dicTarget.TryGetValue(key, out tinfo)) 
                {
                    object value = kvp.Value.GetValue(source);
                    try
                    {
                        if (kvp.Value.PropertyType != tinfo.PropertyType) 
                        {
                            value=Convert.ChangeType(value, tinfo.PropertyType);
                        }
                        tinfo.SetValue(target, value);
                    }
                    catch { }
                }
            }
        }
        #region XML序列化
         /// <summary>
        /// 填充到XML节点
        /// </summary>
        /// <param name="node"></param>
        public static void FillXmlNode(object source, XmlNode node)
        {
            Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(source.GetType());
            FillXmlNode(source, node, dicSource);
        }
        /// <summary>
        /// 填充到XML节点
        /// </summary>
        /// <param name="node"></param>
        private static void FillXmlNode(object source, XmlNode node, Dictionary<string, PropertyInfoHandle> dicSource) 
        {
            foreach (KeyValuePair<string, PropertyInfoHandle> kvp in dicSource)
            {
                XmlAttribute att = node.OwnerDocument.CreateAttribute(kvp.Key);
                object value = kvp.Value.GetValue(source);
                att.InnerText = ValueToString(value);
                node.Attributes.Append(att);
            }
        }
        /// <summary>
        /// 填充到XML节点
        /// </summary>
        /// <param name="node"></param>
        public static XmlDocument ObjectToXml(object source)
        {
            XmlDocument doc = new XmlDocument();
            if (source == null)
            {
                return doc;
            }
            Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(source.GetType());
            
            XmlNode root=doc.CreateElement("root");
            doc.AppendChild(root);
            XmlNode node=doc.CreateElement("item");
            root.AppendChild(node);
            FillXmlNode(source, node, dicSource);
            return doc;
        }
        /// <summary>
        /// 填充到XML节点
        /// </summary>
        /// <param name="node"></param>
        public static XmlDocument ListToXml(IList lstSource)
        {
            XmlDocument doc = new XmlDocument();
            if (lstSource == null || lstSource.Count == 0) 
            {
                return doc;
            }
            Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(lstSource[0].GetType());
            
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);
            foreach (object source in lstSource)
            {
                XmlNode node = doc.CreateElement("item");
                root.AppendChild(node);
                FillXmlNode(source, node, dicSource);
            }
            return doc;
        }
        /// <summary>
        /// 填充到XML节点
        /// </summary>
        /// <param name="node"></param>
        private static void XmlFillObject(XmlNode node, Type objType, object obj, Dictionary<string, PropertyInfoHandle> dicSource)
        {
            foreach (KeyValuePair<string, PropertyInfoHandle> kvp in dicSource)
            {
                XmlAttribute att = node.Attributes[kvp.Key];
                if (att != null) 
                {
                    string val = att.InnerText;
                    if(string.IsNullOrEmpty(val))
                    {
                        continue;
                    }
                    object value = StringToValue(val, kvp.Value.PropertyType);
                    kvp.Value.SetValue(obj, value);
                }
            }
        }
        /// <summary>
        /// XML节点填充到实体
        /// </summary>
        /// <param name="node"></param>
        public static void XmlFillObject(XmlNode node,  object obj)
        {
            Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(obj.GetType());
            XmlFillObject(node, obj.GetType(), obj, dicSource);
        }
        /// <summary>
        /// XML填充到实体
        /// </summary>
        /// <param name="node"></param>
        public static object XmlToObject(XmlDocument doc,Type objType)
        {
            Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(objType);
            XmlNodeList lstNode = doc.GetElementsByTagName("item");
            
            if (lstNode.Count == 0) 
            {
                return null;
            }
            XmlNode item = lstNode[0];
            object ret = Activator.CreateInstance(objType);
            XmlFillObject(item, objType, ret, dicSource);
            return ret;
        }
        /// <summary>
        /// XML节点填充到实体
        /// </summary>
        /// <param name="node"></param>
        public static T XmlToObject<T>(XmlDocument doc) where T:new()
        {
            Type objType = typeof(T);
            object ret = XmlToObject(doc, objType);
            return ret == null ? default(T) : (T)ret;
        }
        /// <summary>
        /// XML节点填充到实体
        /// </summary>
        /// <param name="node"></param>
        public static List<T> XmlToList<T>(XmlDocument doc) where T : new()
        {
            List<T> ret = new List<T>();
            
            XmlNodeList lstNode = doc.GetElementsByTagName("item");
            Type objType = typeof(T);
            Dictionary<string, PropertyInfoHandle> dicSource = GetHandle(objType);
            foreach (XmlNode node in lstNode) 
            {
                T item = (T)Activator.CreateInstance(objType);
                XmlFillObject(node, objType, item, dicSource);
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
        private static string ValueToString(object value) 
        {
            if (value == null) 
            {
                return "";
            }
            if (value is Enum) 
            {
                return ((int)value).ToString();
            }
            if (value is byte[])
            {
                return CommonMethods.BytesToHexString((byte[])value);
            }
            if (value is bool)
            {
                return (((bool)value)?"1":"0");
            }
            return value.ToString();
        }
        /// <summary>
        /// 把值转成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object StringToValue(string value,Type realType)
        {
            if (value == null)
            {
                return null;
            }
            realType = DefaultType.GetRealValueType(realType);
            if (DefaultType.IsInherit(realType, typeof(Enum)))
            {
                int ivalue = Convert.ToInt32(value);
                return ivalue;
            }
            if (DefaultType.IsInherit(realType, typeof(byte[])))
            {
                return CommonMethods.HexStringToBytes(value);
            }
            if (DefaultType.IsInherit(realType, typeof(bool)))
            {
                return value.Trim()!="0";
            }
            return Convert.ChangeType(value,realType);
        }
        private static Type AttType = typeof(NeedSerializer);
        /// <summary>
        /// 获取需要序列化的属性信息
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static  Dictionary<string, PropertyInfoHandle> GetHandle(Type objType) 
        {
            Dictionary<string, PropertyInfoHandle> dic = new Dictionary<string, PropertyInfoHandle>();
            PropertyInfo[] destproper = objType.GetProperties(FastValueGetSet.AllBindingFlags);
            foreach (PropertyInfo pinfo in destproper) 
            {
                object[] arr = pinfo.GetCustomAttributes(AttType, true);
                if (arr != null && arr.Length > 0) 
                {
                    NeedSerializer tag = arr[0] as NeedSerializer;
                    if (tag != null) 
                    {
                        string name=tag.Name;
                        if(string.IsNullOrEmpty(name))
                        {
                            name= pinfo.Name;
                        }
                        dic[name] = FastValueGetSet.GetPropertyInfoHandle(pinfo.Name,objType);
                    }
                }
            }
            return dic;
        }

    }
}
