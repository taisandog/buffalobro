using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel.FastReflection.ClassInfos;
using Buffalo.DB.QueryConditions;
namespace Buffalo.DB.ListExtends
{
    public class ListXMLExtends
    {
        internal static Encoding defaultEncoding = Encoding.UTF8;
        /// <summary>
        /// 收集集合里边所有对应属性的值
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static ArrayList CollectValues(IEnumerable list, string propertyName)
        {
            ArrayList lst = new ArrayList();
            
            
            PropertyInfoHandle pInfo = null;
            //PropertyInfoHandle pInfo = FastValueGetSet.GetPropertyInfoHandle(propertyName, objType);
            //if (pInfo == null)
            //{
            //    throw new Exception("找不到该属性");
            //}
            foreach (object obj in list)
            {
                if (pInfo == null) 
                {
                    Type objType = obj.GetType();
                    pInfo = FastValueGetSet.GetPropertyInfoHandle(propertyName, objType);
                    if (pInfo == null)
                    {
                        throw new Exception("找不到该属性");
                    }
                }
                lst.Add(pInfo.GetValue(obj));
            }
            return lst;
        }
        /// <summary>
        /// 根据集合里边对应属性的值组合出Dictionary
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static Dictionary<T, K> CollectValues<T, K>(IEnumerable<K> list, string propertyName)
        {
            Dictionary<T, K> dic = new Dictionary<T, K>();
            

            Type objType = typeof(K);

            PropertyInfoHandle pInfo = FastValueGetSet.GetPropertyInfoHandle(propertyName, objType);
            if (pInfo == null)
            {
                throw new Exception("找不到该属性");
            }
            
            foreach (K obj in list)
            {
                dic[(T)pInfo.GetValue(obj)]=obj;
            }
            return dic;
        }

        /// <summary>
        /// 根据集合组装出Dictionary
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static Dictionary<T, bool> CollectValues<T>(IEnumerable list)
        {
            Dictionary<T, bool> dic = new Dictionary<T, bool>();

            foreach (T obj in list)
            {
                dic[obj] = true;
            }
            return dic;
        }

        /// <summary>
        /// 把集合写入XML文件
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="root">路径</param>
        public static void WriteXml(IList list, string root)
        {
            FileStream stm = new FileStream(root, FileMode.Create, FileAccess.Write);
            try
            {
                WriteXml(list, stm);
            }
            finally
            {
                stm.Flush();
                stm.Close();
            }
        }

        /// <summary>
        /// 把集合写入XML流
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="stm">流</param>
        /// <param name="objPage">分页</param>
        /// <param name="encoding">编码</param>
        public static void WriteXml(IList list, Stream stm, PageContent objPage, Encoding encoding)
        {
            if (list.Count <= 0)
            {
                return;
            }
            Type type = list[0].GetType();
            ClassInfoHandle entityInfo=null;
            if (!type.IsValueType) 
            {
                entityInfo = ClassInfoManager.GetClassHandle(type);
            }
             
            //Dictionary<string,EntityPropertyInfo>.Enumerator enuEntityProInfo=entityInfo.PropertyInfo.GetPropertyEnumerator();
            XmlDocument doc = new XmlDocument();
            string strxmlBase = "";
            strxmlBase += "<?xml version=\"1.0\" encoding=\"" + encoding.BodyName + "\" ?>";
            strxmlBase += "<root></root>";
            doc.LoadXml(strxmlBase);
            XmlNode rootNode = doc.GetElementsByTagName("root")[0];
            foreach (object obj in list)
            {
                XmlElement ele = doc.CreateElement("item");
                if (entityInfo != null)
                {
                    foreach (PropertyInfoHandle info in entityInfo.PropertyInfo)
                    {
                        if (info.PropertyType.IsValueType || info.PropertyType == typeof(string) || info.PropertyType == typeof(Nullable<>))
                        {
                            XmlElement eleItem = doc.CreateElement(info.PropertyName);
                            object value = info.GetValue(obj);
                            string curValue = ValueToString(value);

                            eleItem.InnerText = curValue;
                            ele.AppendChild(eleItem);

                            
                        }
                    }
                }
                else 
                {
                    ele.InnerText = obj.ToString();
                }
                rootNode.AppendChild(ele);
            }
            if (objPage != null) //写入分页信息
            {
                XmlAttribute attTotalPage = doc.CreateAttribute("totalPage");
                attTotalPage.InnerText = objPage.TotalPage.ToString();
                rootNode.Attributes.Append(attTotalPage);
                XmlAttribute attCurrentPage = doc.CreateAttribute("currentPage");
                attCurrentPage.InnerText = objPage.CurrentPage.ToString();
                rootNode.Attributes.Append(attCurrentPage);
                XmlAttribute attPageSize = doc.CreateAttribute("pageSize");
                attPageSize.InnerText = objPage.PageSize.ToString();
                rootNode.Attributes.Append(attPageSize);
            }
            doc.Save(stm);
        }

        /// <summary>
        /// 把类型转换成字符串
        /// </summary>
        /// <param name="value">类型</param>
        /// <returns></returns>
        public static string ValueToString(object value) 
        {
            if (value == null)
            {
                return "";
            }
            else if (DefaultType.EqualType(value.GetType() , DefaultType.BytesType))
            {
                return ToByteString((byte[])value);
            }
            
            return value.ToString();
            
        }

        /// <summary>
        /// 把集合写入XML流
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="stm">流</param>
        public static void WriteXml(IList list, Stream stm, Encoding encoding)
        {
            WriteXml(list, stm, null, encoding);
        }
        /// <summary>
        /// 把集合写入XML流
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="stm">流</param>
        public static void WriteXml(IList list, Stream stm)
        {
            WriteXml(list, stm, null, defaultEncoding);
        }
        /// <summary>
        /// 从流中读取XML
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns></returns>
        public static List<T> ReadXML<T>(Stream stm, PageContent objPage)
        {
            stm.Position = 0;
            Type type = typeof(T);
            List<T> lst = new List<T>();
            XmlDocument doc = new XmlDocument();
            doc.Load(stm);
            if (objPage != null) //读取分页信息
            {
                XmlNodeList rootList = doc.GetElementsByTagName("root");
                if (rootList.Count > 0)
                {
                    XmlAttribute attTotalRecord = rootList[0].Attributes["totalRecord"];
                    XmlAttribute attCurrentPage = rootList[0].Attributes["currentPage"];
                    XmlAttribute attPageSize = rootList[0].Attributes["pageSize"];
                    if (attTotalRecord != null)
                    {
                        objPage.TotalRecords = Convert.ToInt64(attTotalRecord.InnerText);
                    }
                    if (attCurrentPage != null)
                    {
                        objPage.CurrentPage = Convert.ToInt32(attCurrentPage.InnerText);
                    }
                    if (attPageSize != null)
                    {
                        objPage.PageSize = Convert.ToInt32(attPageSize.InnerText);
                    }
                }
            }
            ClassInfoHandle entityInfo = null;
            if (!type.IsValueType)
            {
                entityInfo = ClassInfoManager.GetClassHandle(type);
            }
            XmlNodeList nodeList = doc.GetElementsByTagName("item");
            foreach (XmlNode node in nodeList)
            {
                if (entityInfo != null)
                {
                    T obj = (T)entityInfo.CreateInstance();
                    foreach (XmlNode itemNode in node.ChildNodes)
                    {
                        string tagName = itemNode.Name;
                        string value = itemNode.InnerText;
                        PropertyInfoHandle info = entityInfo.PropertyInfo[tagName];
                        if (info != null)
                        {
                            Type resType = info.PropertyType;

                            object curValue = StringToValue(value, resType);//转换成对象的值
                            info.SetValue(obj, curValue);//赋值
                        }

                    }
                    lst.Add(obj);
                }
                else 
                {
                    T curValue =(T) StringToValue(node.InnerText, type);//转换成对象的值
                    lst.Add(curValue);
                }
            }
            return lst;
        }

        /// <summary>
        /// 把字符串的值还原成原类型的值
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="type">源类型</param>
        /// <returns></returns>
        public static object StringToValue(string value,Type type) 
        {
            //if (type.IsGenericType)
            //{
            //    Type[] types = type.GetGenericArguments();
            //    if (types.Length > 0)
            //    {
            //        type = types[0];
            //    }
            //}
            Type objType = DefaultType.GetRealValueType(type);
            return ConvertTo(value, type);
        }

        /// <summary>
        /// 从流中读取XML
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns></returns>
        public static List<T> ReadXML<T>(Stream stm)
        {
            return ReadXML<T>(stm, null);
        }
        /// <summary>
        /// 从文件中读取XML
        /// </summary>
        /// <param name="root">文件路径</param>
        /// <returns></returns>
        public static List<T> ReadXML<T>(string root)
        {
            FileStream stm = new FileStream(root, FileMode.Open, FileAccess.Read);
            List<T> ret = default(List<T>);
            try
            {
                ret = ReadXML<T>(stm);
            }
            finally
            {
                stm.Flush();
                stm.Close();
            }
            return ret;
        }

        /// <summary>
        /// 把该值转成指定类型
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        private static object ConvertTo(object value, Type type)
        {
            if (DefaultType.EqualType(type , DefaultType.BytesType))
            {
                return StringToBytes(value.ToString());
            }
            return Buffalo.Kernel.CommonMethods.ChangeType(value, type);
        }

        /// <summary>
        /// 把字节数组打成字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string ToByteString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte curByte in bytes)
            {
                string strByte = curByte.ToString("X");
                if (strByte.Length < 2)
                {
                    strByte = "0" + strByte;
                }
                sb.Append(strByte);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把字符串组合回字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static byte[] StringToBytes(string value)
        {
            int length = (int)Math.Ceiling((double)value.Length / (double)2);//数组长度
            byte[] ret = new byte[length];
            StringBuilder sb = new StringBuilder(value);
            int index = 0;
            while (sb.Length > 1)
            {
                string strCurValue = sb[0].ToString() + sb[1].ToString();
                byte curValue = Convert.ToByte(strCurValue, 16);
                ret[index] = curValue;
                sb.Remove(0, 2);//移出已经转换好的字符
                index++;
            }
            if (sb.Length > 0) //如果还剩一个就连这个也转换了(容错性要求)
            {
                string strCurValue = sb[0].ToString();
                byte curValue = Convert.ToByte(strCurValue, 16);
                ret[index] = curValue;
                sb.Remove(0, 1);//移出已经转换好的字符
            }
            return ret;
        }


    }
}
