using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Buffalo.DB.DataBaseAdapter
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class ConfigInfo
    {
        private string _filePath;

        private XmlDocument _document;

        List<string> _dalNamespaces;

        

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <param name="document">�ĵ�</param>
        public ConfigInfo(string filePath, XmlDocument document)
        {
            _filePath = filePath;
            _document = document;
        }

        /// <summary>
        /// �������ݲ�������ռ�
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
        /// �Ƿ������ݲ�������ռ�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsInNamespace(List<string> lst, string name)
        {
            if (string.IsNullOrEmpty(name)) 
            {
                return false;
            }
            if (lst == null)//�������ݲ�
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
        /// �Ƿ���ʵ��������ռ�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsDalNamespace(string name)
        {
            return IsInNamespace(_dalNamespaces, name);
        }

        

        /// <summary>
        /// �ļ�·��
        /// </summary>
        public string FilePath 
        {
            get 
            {
                return _filePath;
            }
        }

        /// <summary>
        /// �ĵ�
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
