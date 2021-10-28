using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections;
using System.Xml;
using System.IO;
using Buffalo.DB;
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
        /// �ռ�����������ж�Ӧ���Ե�ֵ
        /// </summary>
        /// <param name="list">����</param>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public static ArrayList CollectValues(IEnumerable list, string propertyName)
        {
            ArrayList lst = new ArrayList();
            
            
            PropertyInfoHandle pInfo = null;
            //PropertyInfoHandle pInfo = FastValueGetSet.GetPropertyInfoHandle(propertyName, objType);
            //if (pInfo == null)
            //{
            //    throw new Exception("�Ҳ���������");
            //}
            foreach (object obj in list)
            {
                if (pInfo == null) 
                {
                    Type objType = obj.GetType();
                    pInfo = FastValueGetSet.GetPropertyInfoHandle(propertyName, objType);
                    if (pInfo == null)
                    {
                        throw new Exception("�Ҳ���������");
                    }
                }
                lst.Add(pInfo.GetValue(obj));
            }
            return lst;
        }
        /// <summary>
        /// ���ݼ�����߶�Ӧ���Ե�ֵ��ϳ�Dictionary
        /// </summary>
        /// <param name="list">����</param>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public static Dictionary<T, K> CollectValues<T, K>(IEnumerable<K> list, string propertyName)
        {
            Dictionary<T, K> dic = new Dictionary<T, K>();
            

            Type objType = typeof(K);

            PropertyInfoHandle pInfo = FastValueGetSet.GetPropertyInfoHandle(propertyName, objType);
            if (pInfo == null)
            {
                throw new Exception("�Ҳ���������");
            }
            
            foreach (K obj in list)
            {
                dic[(T)pInfo.GetValue(obj)]=obj;
            }
            return dic;
        }

        /// <summary>
        /// ���ݼ�����װ��Dictionary
        /// </summary>
        /// <param name="list">����</param>
        /// <param name="propertyName">������</param>
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
        /// �Ѽ���д��XML�ļ�
        /// </summary>
        /// <param name="list">����</param>
        /// <param name="root">·��</param>
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
        /// �Ѽ���д��XML��
        /// </summary>
        /// <param name="list">����</param>
        /// <param name="stm">��</param>
        /// <param name="objPage">��ҳ</param>
        /// <param name="encoding">����</param>
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
            if (objPage != null) //д���ҳ��Ϣ
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
        /// ������ת�����ַ���
        /// </summary>
        /// <param name="value">����</param>
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

        // <summary>
        /// �Ѽ���д��XML��
        /// </summary>
        /// <param name="list">����</param>
        /// <param name="stm">��</param>
        public static void WriteXml(IList list, Stream stm, Encoding encoding)
        {
            WriteXml(list, stm, null, encoding);
        }
        // <summary>
        /// �Ѽ���д��XML��
        /// </summary>
        /// <param name="list">����</param>
        /// <param name="stm">��</param>
        public static void WriteXml(IList list, Stream stm)
        {
            WriteXml(list, stm, null, defaultEncoding);
        }
        /// <summary>
        /// �����ж�ȡXML
        /// </summary>
        /// <param name="stm">��</param>
        /// <returns></returns>
        public static List<T> ReadXML<T>(Stream stm, PageContent objPage)
        {
            stm.Position = 0;
            Type type = typeof(T);
            List<T> lst = new List<T>();
            XmlDocument doc = new XmlDocument();
            doc.Load(stm);
            if (objPage != null) //��ȡ��ҳ��Ϣ
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

                            object curValue = StringToValue(value, resType);//ת���ɶ����ֵ
                            info.SetValue(obj, curValue);//��ֵ
                        }

                    }
                    lst.Add(obj);
                }
                else 
                {
                    T curValue =(T) StringToValue(node.InnerText, type);//ת���ɶ����ֵ
                    lst.Add(curValue);
                }
            }
            return lst;
        }

        /// <summary>
        /// ���ַ�����ֵ��ԭ��ԭ���͵�ֵ
        /// </summary>
        /// <param name="value">�ַ���</param>
        /// <param name="type">Դ����</param>
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
        /// �����ж�ȡXML
        /// </summary>
        /// <param name="stm">��</param>
        /// <returns></returns>
        public static List<T> ReadXML<T>(Stream stm)
        {
            return ReadXML<T>(stm, null);
        }
        /// <summary>
        /// ���ļ��ж�ȡXML
        /// </summary>
        /// <param name="root">�ļ�·��</param>
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
        /// �Ѹ�ֵת��ָ������
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <param name="type">ָ������</param>
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
        /// ���ֽ��������ַ���
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
        /// ���ַ�����ϻ��ֽ�����
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static byte[] StringToBytes(string value)
        {
            int length = (int)Math.Ceiling((double)value.Length / (double)2);//���鳤��
            byte[] ret = new byte[length];
            StringBuilder sb = new StringBuilder(value);
            int index = 0;
            while (sb.Length > 1)
            {
                string strCurValue = sb[0].ToString() + sb[1].ToString();
                byte curValue = Convert.ToByte(strCurValue, 16);
                ret[index] = curValue;
                sb.Remove(0, 2);//�Ƴ��Ѿ�ת���õ��ַ�
                index++;
            }
            if (sb.Length > 0) //�����ʣһ���������Ҳת����(�ݴ���Ҫ��)
            {
                string strCurValue = sb[0].ToString();
                byte curValue = Convert.ToByte(strCurValue, 16);
                ret[index] = curValue;
                sb.Remove(0, 1);//�Ƴ��Ѿ�ת���õ��ַ�
            }
            return ret;
        }


    }
}
