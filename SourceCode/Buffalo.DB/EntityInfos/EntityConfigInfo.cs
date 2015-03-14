using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 实体配置信息
    /// </summary>
    public class EntityConfigInfo
    {
        private Type _type;

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private XmlDocument _configXML;
        /// <summary>
        /// 配置的XML
        /// </summary>
        public XmlDocument ConfigXML
        {
            get { return _configXML; }
            set { _configXML = value; }
        }
    }
}
