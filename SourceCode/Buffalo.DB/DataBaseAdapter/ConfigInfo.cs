using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Buffalo.DB.DataBaseAdapter
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class ConfigInfo
    {
        private string _filePath;

        private XmlDocument _document;

        List<string> _dalNamespaces;

        

        /// <summary>
        /// 配置信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="document">文档</param>
        public ConfigInfo(string filePath, XmlDocument document)
        {
            _filePath = filePath;
            _document = document;
        }

        /// <summary>
        /// 设置数据层的命名空间
        /// </summary>
        /// <param name="aNamespaces"></param>
        public void AddDalNamespaces(string aNamespaces) 
        {
            if (_dalNamespaces == null) 
            {
                _dalNamespaces = new List<string>();
            }
            if (!string.IsNullOrEmpty(aNamespaces))
            {
                string[] arrNamespaces = aNamespaces.Split('|');
                for (int i = 0; i < arrNamespaces.Length; i++)
                {
                    string str = arrNamespaces[i];
                    if (string.IsNullOrEmpty(str))
                    {
                        continue;
                    }
                    if (str[str.Length - 1] != '.')
                    {
                        str = str + ".";
                    }
                    _dalNamespaces.Add(str);
                }
            }
        }
        
        /// <summary>
        /// 是否在数据层的命名空间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsInNamespace(List<string> lst, string name)
        {
            if (string.IsNullOrEmpty(name)) 
            {
                return false;
            }
            if (lst == null)//加载数据层
            {
                return true;
            }
            foreach (string dalNamespace in lst)
            {
                if (name.IndexOf(dalNamespace) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 是否在实体的命名空间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsDalNamespace(string name)
        {
            return IsInNamespace(_dalNamespaces, name);
        }

        

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath 
        {
            get 
            {
                return _filePath;
            }
        }

        /// <summary>
        /// 文档
        /// </summary>
        public XmlDocument Document 
        {
            get 
            {
                return _document;
            }
        }

       

    }
}
