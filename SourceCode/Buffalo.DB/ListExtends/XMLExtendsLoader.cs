using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.ListExtends;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Buffalo.DB.QueryConditions;

/// <summary>
/// DataLoader 的摘要说明
/// </summary>
namespace Buffalo.DB.ListExtends
{
    public class XMLExtendsLoader
    {
        private XMLExtendsLoader()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 读取信息集合
        /// </summary>
        /// <param name="info">信息</param>
        /// <returns></returns>
        public static List<T> LoadItems<T>(string info)
        {
            return LoadItems<T>(info, null);
        }

        /// <summary>
        /// 读取信息集合
        /// </summary>
        /// <param name="info">信息</param>
        /// <returns></returns>
        public static List<T> LoadItems<T>(string info, PageContent objPage, Encoding encode)
        {
            MemoryStream stm = new MemoryStream();
            StreamWriter writer = new StreamWriter(stm, encode);
            try
            {
                writer.Write(info);
                writer.Flush();
                List<T> lst = ListXMLExtends.ReadXML<T>(stm, objPage);
                return lst;
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
        public static List<T> LoadItems<T>(string info, PageContent objPage)
        {
            return LoadItems<T>(info, objPage, ListXMLExtends.defaultEncoding);
        }
        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <returns></returns>
        public static T LoadItem<T>(string info)
        {
            return LoadItem<T>(info, ListXMLExtends.defaultEncoding);
        }
        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <returns></returns>
        public static T LoadItem<T>(string info, Encoding encode)
        {
            List<T> lst = LoadItems<T>(info,null,encode);
            if (lst.Count > 0)
            {
                return lst[0];
            }
            return default(T);
        }
        /// <summary>
        /// 返回单个项的XML信息
        /// </summary>
        /// <param name="item">项</param>
        /// <returns></returns>
        public static string ItemToString(object item)
        {
            ArrayList lst = new ArrayList();
            lst.Add(item);
            return ItemsToString(lst);
        }
        /// <summary>
        /// 返回单个项的XML信息
        /// </summary>
        /// <param name="item">项</param>
        /// <returns></returns>
        public static string ItemToString(object item, Encoding encode)
        {
            ArrayList lst = new ArrayList();
            lst.Add(item);
            return ItemsToString(lst, null, encode);
        }
        /// <summary>
        /// 返回集合项的XML字符串
        /// </summary>
        /// <param name="items">集合项</param>
        /// <returns></returns>
        public static string ItemsToString(IList items, PageContent objPage, Encoding encode)
        {
            string ret = null;
            MemoryStream stm = new MemoryStream();
            ListXMLExtends.WriteXml(items, stm, objPage, encode);
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
        public static string ItemsToString(IList items, PageContent objPage)
        {
            return ItemsToString(items, objPage, ListXMLExtends.defaultEncoding);
        }
        /// <summary>
        /// 返回集合项的XML字符串
        /// </summary>
        /// <param name="items">集合项</param>
        /// <returns></returns>
        public static string ItemsToString(IList items)
        {
            return ItemsToString(items, null);
        }
    }
}