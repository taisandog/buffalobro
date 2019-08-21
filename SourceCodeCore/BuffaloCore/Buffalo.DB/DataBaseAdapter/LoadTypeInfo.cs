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
    /// �������͵Ļ�����Ϣ
    /// </summary>
    public class LoadTypeInfo
    {
        


        /// <summary>
        /// ����XML�ڵ�
        /// </summary>
        /// <param name="baseNode">���ڵ�</param>
        /// <param name="objType">����</param>
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
        /// ͨ��Xml�ڵ��������
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="assemblyTypeLoader">�ڵ�</param>
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
