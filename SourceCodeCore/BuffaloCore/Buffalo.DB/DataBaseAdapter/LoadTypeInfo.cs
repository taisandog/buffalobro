using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml;
using Buffalo.Kernel.FastReflection;
using System.Reflection;

namespace Buffalo.DB.DataBaseAdapter
{

    /// <summary>
    /// 加载类型的缓存信息
    /// </summary>
    public class LoadTypeInfo
    {
        


        /// <summary>
        /// 创建XML节点
        /// </summary>
        /// <param name="baseNode">基节点</param>
        /// <param name="objType">类型</param>
        /// <returns></returns>
        public static XmlNode AppendNode(XmlNode baseNode,Type objType) 
        {
            XmlDocument doc=baseNode.OwnerDocument;
            XmlNode node = doc.CreateElement("item");

            XmlAttribute att=doc.CreateAttribute("assembly");
            att.InnerText = objType.Assembly.GetName().Name;
            node.Attributes.Append(att);

            att = doc.CreateAttribute("type");
            att.InnerText = objType.FullName;
            node.Attributes.Append(att);
            baseNode.AppendChild(node);
            return node;
        }

        /// <summary>
        /// 通过Xml节点加载数据
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="assemblyTypeLoader">节点</param>
        /// <returns></returns>
        public static Type LoadFromXmlNode(XmlNode node,AssemblyTypeLoader loader) 
        {
            string assembly = null;
            string typeName = null;
            foreach (XmlAttribute att in node.Attributes) 
            {
                if (att.Name.Equals("assembly", StringComparison.CurrentCultureIgnoreCase)) 
                {
                    assembly = att.InnerText;
                }
                else if (att.Name.Equals("type", StringComparison.CurrentCultureIgnoreCase))
                {
                    typeName = att.InnerText;
                }
            }

            return loader.LoadType(typeName,assembly);
        }

    }
}
