using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// ʵ��������Ϣ
    /// </summary>
    public class EntityConfigInfo
    {
        private Type _type;

        /// <summary>
        /// ʵ������
        /// </summary>
        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private XmlDocument _configXML;
        /// <summary>
        /// ���õ�XML
        /// </summary>
        public XmlDocument ConfigXML
        {
            get { return _configXML; }
            set { _configXML = value; }
        }
    }
}
