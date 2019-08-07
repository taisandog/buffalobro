using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Buffalo.DB.ListExtends;
using System.IO;

namespace Buffalo.DB.QueryConditions
{
    internal class ScopeXmlExtend
    {
        /// <summary>
        /// 获取范围List的XML
        /// </summary>
        /// <param name="lstScope">范围集合</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        internal static void GetScopeXml(ScopeList lstScope,Encoding encode,Stream stm) 
        {
            XmlDocument doc = new XmlDocument();
            string strxmlBase = "";
            strxmlBase += "<?xml version=\"1.0\" encoding=\"" + encode.BodyName + "\" ?>";
            strxmlBase += "<root></root>";
            doc.LoadXml(strxmlBase);
            XmlNode rootNode = doc.GetElementsByTagName("root")[0];
            foreach (Scope objScope in lstScope)
            {
                
                XmlElement ele = doc.CreateElement("item");
                //添加属性
                XmlAttribute attPropertyName=doc.CreateAttribute("propertyname");
                attPropertyName.InnerText = objScope.PropertyName;
                ele.Attributes.Append(attPropertyName);

                XmlAttribute attScopeType = doc.CreateAttribute("scopetype");
                attScopeType.InnerText = ((int)objScope.ScopeType).ToString();
                ele.Attributes.Append(attScopeType);

                XmlAttribute attConnectType = doc.CreateAttribute("connecttype");
                attConnectType.InnerText = ((int)objScope.ConnectType).ToString();
                ele.Attributes.Append(attConnectType);

                //添加值
                if (objScope.Value1 != null) 
                {
                    AppendValue(doc,ele, "value1", objScope.Value1,objScope);
                }
                if (objScope.Value2 != null)
                {
                    AppendValue(doc, ele, "value2", objScope.Value2, objScope);
                }
                rootNode.AppendChild(ele);
            }
            doc.Save(stm);
        }

        /// <summary>
        /// 添加一个值
        /// </summary>
        /// <param name="doc">XML文档</param>
        /// <param name="ele">源元素</param>
        /// <param name="valueName">值的标签</param>
        /// <param name="value">值</param>
        /// <param name="objScope">范围对象</param>
        private static void AppendValue(XmlDocument doc, XmlElement ele, string valueName, object value, Scope objScope) 
        {
            
            XmlElement eleValue = doc.CreateElement(valueName);
            XmlAttribute attType = doc.CreateAttribute("datatype");
            attType.InnerText = objScope.Value1.GetType().ToString();
            eleValue.Attributes.Append(attType);
            eleValue.InnerText = ListXMLExtends.ValueToString(value);
            ele.AppendChild(eleValue);
        }

        /// <summary>
        /// 返回集合项的XML字符串
        /// </summary>
        /// <param name="items">集合项</param>
        /// <returns></returns>
        internal static string GetScopeXmlString(ScopeList lstScope, Encoding encode)
        {
            string ret = null;
            MemoryStream stm = new MemoryStream();
            GetScopeXml(lstScope, encode,stm);
            StreamReader reader = new StreamReader(stm, encode);
            try
            {
                stm.Position = 0;
                ret = reader.ReadToEnd();
            }
            finally
            {
                reader.Close();
                stm.Close();
            }
            return ret;
        }
        /// <summary>
        /// 返回集合项的XML字符串
        /// </summary>
        /// <param name="items">集合项</param>
        /// <returns></returns>
        internal static string GetScopeXmlString(ScopeList lstScope)
        {
            return GetScopeXmlString(lstScope, ListXMLExtends.defaultEncoding);
        }

        /// <summary>
        /// 根据XML加载范围查找项
        /// </summary>
        /// <param name="xml">XML</param>
        /// <param name="lstScope">范围集合</param>
        internal static void LoadScopeItems(Stream stm, ScopeList lstScope) 
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stm);

            XmlNodeList nodeList = doc.GetElementsByTagName("item");
            foreach (XmlNode node in nodeList)
            {
                
                string propertyName = null;
                ScopeType stype = ScopeType.Between;
                ConnectType ctype = ConnectType.And;
                object value1 = null;
                object value2 = null;
                XmlAttribute attPropertyName = node.Attributes["propertyname"];
                if (attPropertyName != null) 
                {
                    propertyName = attPropertyName.InnerText;
                }

                XmlAttribute attScopeType = node.Attributes["scopetype"];
                if (attScopeType != null)
                {
                    stype = (ScopeType)Convert.ToInt32(attScopeType.InnerText);
                }

                XmlAttribute attConnectType = node.Attributes["connecttype"];
                if (attConnectType != null)
                {
                    ctype = (ConnectType)Convert.ToInt32(attConnectType.InnerText);
                }
                foreach (XmlNode itemNode in node.ChildNodes)
                {
                    object value=null;
                    Type objType = null;
                    XmlAttribute attType = itemNode.Attributes["datatype"];
                    if (attType != null)
                    {
                        objType = Type.GetType(attType.InnerText);
                    }
                    else 
                    {
                        objType = typeof(string);
                    }
                    value = ListXMLExtends.StringToValue(itemNode.InnerText, objType);
                    if (itemNode.Name == "value1") 
                    {
                        value1 = value;
                    }
                    else if (itemNode.Name == "value2") 
                    {
                        value2 = value;
                    }
                }
                Scope objScope = new Scope(propertyName,value1,value2,stype,ctype);
                lstScope.Add(objScope);
            }
        }

        /// <summary>
        /// 读取信息集合
        /// </summary>
        /// <param name="info">信息</param>
        /// <returns></returns>
        public static void LoadScopeItems(string info,ScopeList lstScope, Encoding encode)
        {
            MemoryStream stm = new MemoryStream();
            StreamWriter writer = new StreamWriter(stm, encode);
            try
            {
                writer.Write(info);
                writer.Flush();
                stm.Position = 0;
                LoadScopeItems(stm, lstScope);
            }
            finally
            {
                writer.Close();
                stm.Close();
            }
        }
        /// <summary>
        /// 读取信息集合
        /// </summary>
        /// <param name="info">信息</param>
        /// <returns></returns>
        public static void LoadScopeItems(string info,ScopeList lstScope)
        {
            LoadScopeItems(info,lstScope, ListXMLExtends.defaultEncoding);
        }
    }
}
