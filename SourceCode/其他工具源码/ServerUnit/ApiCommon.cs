
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace ServerUnit
{
    /// <summary>
    /// WebAPI返回帮助类
    /// </summary>
    public class ApiCommon
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static ArgValues GetArgs(string args)
        {
            
            return JsonConvert.DeserializeObject<ArgValues>(args);
        }



        /// <summary>
        /// 获取成功的返回值
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static APIResault GetSuccess(string message = null, object data = null)
        {
            APIResault res = new APIResault();
            res.SetSuccess(message, data);

            return res;
        }

        /// <summary>
        /// 获取失败的返回值
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static APIResault GetFault(string message,object data=null)
        {
            APIResault res = new APIResault();
            res.SetFault(message, data);
            return res;
        }
        /// <summary>
        /// 获取失败的返回值
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static APIResault GetException(Exception ex)
        {
            APIResault res = new APIResault();
            res.SetException(ex);
            return res;
        }

        // <summary>
        /// 获取失败的返回值
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static APIResault GetTimeout(string message)
        {
            
            APIResault res = new APIResault();
            res.SetTimeout(message);
            return res;
        }

        /// <summary>
        /// XML转成字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static Dictionary<string, T> XmlToDictionary<T>(XmlDocument doc) 
        {
            XmlNodeList nodes = doc.GetElementsByTagName("item");
            Dictionary<string, T> dic = new Dictionary<string, T>();
            foreach (XmlNode node in nodes) 
            {
                XmlAttribute att = node.Attributes["name"];
                if(att==null)
                {
                    continue;
                }
                string name = att.InnerText;
                att = node.Attributes["value"];
                if (att == null)
                {
                    continue;
                }
                string value = att.InnerText;

                T val = default(T);

                if (typeof(T) == typeof(string))
                {
                    val = (T)(object)value;
                }
                else
                {
                    try
                    {
                        val = JsonConvert.DeserializeObject<T>(value);
                    }
                    catch { }
                }
                dic[name] = val;
            }
            return dic;
        }

        /// <summary>
        /// 新建一个XML文档
        /// </summary>
        /// <returns></returns>
        public static XmlDocument NewXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", "no"));
            return doc;
        }

        public static XmlDocument DictionaryToXml<T>(Dictionary<string, T> dic) 
        {
            
            XmlDocument doc = NewXmlDocument();
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);
            foreach (KeyValuePair<string, T> kvp in dic) 
            {

                XmlNode item = doc.CreateElement("item");
                root.AppendChild(item);
                XmlAttribute att = doc.CreateAttribute("name");
                att.InnerText = kvp.Key;
                item.Attributes.Append(att);

                att = doc.CreateAttribute("value");
                att.InnerText = JsonConvert.SerializeObject(kvp.Value);
                item.Attributes.Append(att);
            }
            return doc;
        }

        public static XmlDocument ListToXml(IList lst)
        {

            XmlDocument doc = NewXmlDocument();
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);
            foreach (object obj in lst)
            {

                XmlNode item = doc.CreateElement("item");
                root.AppendChild(item);
                //XmlAttribute att = doc.CreateAttribute("name");
                //att.InnerText = kvp.Key;
                //item.Attributes.Append(att);

                XmlAttribute att = doc.CreateAttribute("value");
                att.InnerText = JsonConvert.SerializeObject(obj);
                item.Attributes.Append(att);
            }
            return doc;
        }

        /// <summary>
        /// XML转成字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<T> XmlToList<T>(XmlDocument doc)
        {
            XmlNodeList nodes = doc.GetElementsByTagName("item");
            List<T> dic = new List<T>();
            foreach (XmlNode node in nodes)
            {
                //XmlAttribute att = node.Attributes["name"];
                //if (att == null)
                //{
                //    continue;
                //}
                //string name = att.InnerText;
                XmlAttribute att = node.Attributes["value"];
                if (att == null)
                {
                    continue;
                }
                string value = att.InnerText;

                T val = default(T);

                if (typeof(T) == typeof(string))
                {
                    val = (T)(object)value;
                }
                else
                {
                    try
                    {
                        val = JsonConvert.DeserializeObject<T>(value);
                    }
                    catch { }
                }
                dic.Add(val);
            }
            return dic;
        }
    }
}