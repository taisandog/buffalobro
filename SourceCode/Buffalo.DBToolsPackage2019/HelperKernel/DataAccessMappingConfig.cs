using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DBTools.ROMHelper;
using EnvDTE;
using Buffalo.DBTools.UIHelper;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// 数据层映射文件生成
    /// </summary>
    public class DataAccessMappingConfig : GrneraterBase
    {
        XmlDocument _doc = EntityMappingConfig.NewXmlDocument();
        XmlNode _rootNode;
        string _fileName = null;
        XmlNode _boNode;

        public DataAccessMappingConfig(DBEntityInfo entity, ClassDesignerInfo info)
            : base(entity, info)
        {
            Init();
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        private void Init() 
        {
            //FileInfo classFile = new FileInfo(EntityFileName);
            string dicName = GenerateBasePath + "\\BEM\\";
            if (!Directory.Exists(dicName))
            {
                Directory.CreateDirectory(dicName);
            }
            _fileName = dicName + DBName + ".BDM.xml";
            if (File.Exists(_fileName))
            {
                try
                {
                    _doc.Load(_fileName);
                    XmlNodeList rootNodes = _doc.GetElementsByTagName("dataaccess");
                    XmlNodeList boNodes = _doc.GetElementsByTagName("business");
                    if (rootNodes.Count <= 0 || boNodes.Count<=0)
                    {
                        _doc = EntityMappingConfig.NewXmlDocument();
                        
                    }
                    else
                    {
                        _rootNode = rootNodes[0];
                        _boNode = boNodes[0];
                    }
                }
                catch
                {

                }
            }
            if (_rootNode == null)
            {
                XmlNode root = _doc.CreateElement("root");
                _doc.AppendChild(root);
                XmlNode dalNode = _doc.CreateElement("dataaccess");
                root.AppendChild(dalNode);
                XmlAttribute att = _doc.CreateAttribute("name");
                att.InnerText = DBName;
                dalNode.Attributes.Append(att);

                XmlNode boNode = _doc.CreateElement("business");
                root.AppendChild(boNode);

                _boNode = boNode;
                _rootNode = dalNode;
            }
        }

        public DataAccessMappingConfig(EntityConfig entity) :base(entity)
        {
            Init();
        }

        /// <summary>
        /// 保存XML信息
        /// </summary>
        /// <param name="entity"></param>
        public void SaveXML()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            EntityMappingConfig.SaveXML(_fileName, _doc);
            
            EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(_fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Resource;
        }

        
        /// <summary>
        /// 实体生成XML配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void AppendDal(string typeName,string interfaceName) 
        {
            if (Exists(typeName)) 
            {
                return;
            }
            XmlNode node = _doc.CreateElement("item");
            XmlAttribute att = _doc.CreateAttribute("type");
            att.InnerText = typeName;
            node.Attributes.Append(att);

            att = _doc.CreateAttribute("interface");
            att.InnerText = interfaceName;
            node.Attributes.Append(att);

            _rootNode.AppendChild(node);
        }
        /// <summary>
        /// 实体生成XML配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void AppendBo(string typeName, string className)
        {
            if (BoExists(typeName))
            {
                return;
            }
            XmlNode node = _doc.CreateElement("item");
            XmlAttribute att = _doc.CreateAttribute("type");
            att.InnerText = typeName;
            node.Attributes.Append(att);

            att = _doc.CreateAttribute("class");
            att.InnerText = className;
            node.Attributes.Append(att);

            _boNode.AppendChild(node);
        }
        /// <summary>
        /// 删除DAL
        /// </summary>
        /// <param name="typeName"></param>
        public void DeleteDal(string typeName) 
        {

            XmlNodeList itemNodes = _rootNode.ChildNodes;
            for (int i = itemNodes.Count-1; i>=0; i--)
            {
                XmlNode node = itemNodes[i];
                XmlAttribute att = node.Attributes["type"];
                if (att != null)
                {
                    if (att.InnerText == typeName)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                }
            }
        }
        /// <summary>
        /// 删除DAL
        /// </summary>
        /// <param name="typeName"></param>
        public void DeleteBo(string typeName)
        {

            XmlNodeList itemNodes = _boNode.ChildNodes;
            for (int i = itemNodes.Count - 1; i >= 0; i--)
            {
                XmlNode node = itemNodes[i];
                XmlAttribute att = node.Attributes["type"];
                if (att != null)
                {
                    if (att.InnerText == typeName)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                }
            }
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public bool BoExists(string typeName)
        {
            XmlNodeList itemNodes = _boNode.ChildNodes;
            foreach (XmlNode node in itemNodes)
            {
                XmlAttribute att = node.Attributes["type"];
                if (att != null)
                {
                    if (att.InnerText == typeName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public bool Exists(string typeName) 
        {
            XmlNodeList itemNodes = _rootNode.ChildNodes;
            foreach (XmlNode node in itemNodes) 
            {
                XmlAttribute att = node.Attributes["type"];
                if (att != null) 
                {
                    if (att.InnerText == typeName) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
    }
}
