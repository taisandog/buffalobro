using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DBTools.ROMHelper;
using Buffalo.DBTools.UIHelper;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// 实体映射文件生成
    /// </summary>
    public class EntityMappingConfig
    {
        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <param name="entity"></param>
        public static bool LoadConfigInfo(EntityConfig entity) 
        {
            FileInfo classFile = new FileInfo(entity.DesignerInfo.SelectDocView.DocData.FileName);
            string fileName = classFile.DirectoryName + "\\BEM\\" + entity.ClassName + ".BEM.xml";
            if (!File.Exists(fileName)) 
            {
                return false;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            XmlNodeList classNodes = doc.GetElementsByTagName("class");
            if (classNodes.Count > 0) 
            {
                XmlNode classNode = classNodes[0];
                XmlAttribute att = classNode.Attributes["TableName"];
                if (att != null)
                {
                    entity.TableName = att.InnerText;
                }
                att = classNode.Attributes["IsTable"];
                if (att != null)
                {
                    entity.IsTable = att.InnerText=="1";
                }
                att = classNode.Attributes["UseCache"];
                if (att != null)
                {
                    
                    entity.UseCache =(att.InnerText=="1");
                }
            }

            FillPropertyInfo(doc, entity);
            FillRelationInfo(doc, entity);
            return true;
        }

        /// <summary>
        /// 填充属性信息
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        private static void FillPropertyInfo(XmlDocument doc, EntityConfig entity) 
        {
            XmlNodeList lstProperty = doc.GetElementsByTagName("property");
            foreach (XmlNode node in lstProperty)
            {
                XmlAttribute att = node.Attributes["FieldName"];
                if (att == null)
                {
                    continue;
                }
                string fName = att.InnerText;
                if (string.IsNullOrEmpty(fName))
                {
                    continue;
                }
                foreach (EntityParamField filed in entity.EParamFields)
                {
                    if (filed.FieldName == fName)
                    {
                        filed.IsGenerate = true;
                        att = node.Attributes["PropertyName"];
                        if (att != null)
                        {
                            filed.PropertyName = att.InnerText;
                        }
                        att = node.Attributes["DbType"];
                        if (att != null)
                        {
                            filed.DbType = att.InnerText;
                        }
                        att = node.Attributes["Length"];
                        if (att != null)
                        {
                            int len = 0;
                            int.TryParse(att.InnerText, out len);
                            filed.Length = len;
                        }
                        att = node.Attributes["EntityPropertyType"];
                        if (att != null)
                        {
                            int type = 0;
                            int.TryParse(att.InnerText, out type);
                            filed.EntityPropertyType = (EntityPropertyType)type;
                        }
                        att = node.Attributes["ParamName"];
                        if (att != null)
                        {
                            filed.ParamName = att.InnerText;
                        }
                        att = node.Attributes["ReadOnly"];
                        if (att != null)
                        {
                            filed.ReadOnly = att.InnerText=="1";
                        }
                        att = node.Attributes["AllowNull"];
                        if (att != null)
                        {
                            filed.AllowNull = att.InnerText == "1";
                        }
                    }
                }
                
            }
        }
        /// <summary>
        /// 填充映射信息
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        private static void FillRelationInfo(XmlDocument doc, EntityConfig entity)
        {
            XmlNodeList lstProperty = doc.GetElementsByTagName("relation");
            foreach (XmlNode node in lstProperty)
            {
                XmlAttribute att = node.Attributes["FieldName"];
                if (att == null)
                {
                    continue;
                }
                string fName = att.InnerText;
                if (string.IsNullOrEmpty(fName))
                {
                    continue;
                }
                foreach (EntityRelationItem filed in entity.ERelation)
                {
                    if (filed.FieldName == fName)
                    {
                        filed.IsGenerate = true;
                        att = node.Attributes["PropertyName"];
                        if (att != null)
                        {
                            filed.PropertyName = att.InnerText;
                        }
                        att = node.Attributes["SourceProperty"];
                        if (att != null)
                        {
                            filed.SourceProperty = att.InnerText;
                        }

                        att = node.Attributes["TargetProperty"];
                        if (att != null)
                        {

                            filed.TargetProperty = att.InnerText;
                        }
                        att = node.Attributes["IsToDB"];
                        if (att != null)
                        {
                            filed.IsToDB = att.InnerText=="1";
                        }
                        //att = node.Attributes["IsParent"];
                        //if (att != null)
                        //{
                        //    filed.IsParent = att.InnerText == "1";
                        //}
                        
                    }
                }

                

            }
        }

        /// <summary>
        /// 保存XML信息
        /// </summary>
        /// <param name="entity"></param>
        public static void SaveXML(DBEntityInfo entity)
        {

            //string fileName = entity.FileName.Replace(entity.ClassName + ".cs", entity.ClassName + ".be.xml");
            FileInfo classFile = new FileInfo(entity.DesignerInfo.SelectDocView.DocData.FileName);
            string dicName = classFile.DirectoryName + "\\BEM\\";
            if (!Directory.Exists(dicName))
            {
                Directory.CreateDirectory(dicName);
            }
            string fileName = dicName + entity.ClassName + ".BEM.xml";
            XmlDocument doc = ToXML(entity);
            SaveXML(fileName, doc);
            EnvDTE.ProjectItem newit = entity.DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Resource;
        }

        /// <summary>
        /// 保存XML信息
        /// </summary>
        /// <param name="entity"></param>
        public static void SaveXML(EntityConfig entity) 
        {
            
            //string fileName = entity.FileName.Replace(entity.ClassName + ".cs", entity.ClassName + ".be.xml");
            FileInfo classFile = new FileInfo(entity.DesignerInfo.SelectDocView.DocData.FileName);
            string dicName = classFile.DirectoryName + "\\BEM\\";
            if (!Directory.Exists(dicName)) 
            {
                Directory.CreateDirectory(dicName);
            }
            string fileName = dicName + entity.ClassName + ".BEM.xml";
            XmlDocument doc = ToXML(entity);
            SaveXML(fileName, doc);
            EnvDTE.ProjectItem newit = entity.DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Resource;
        }

        /// <summary>
        /// 保存XML
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="doc">XML文档</param>
        public static void SaveXML(string path, XmlDocument doc) 
        {
            
            doc.Save(path);
        }

        /// <summary>
        /// 新建一个XML文档
        /// </summary>
        /// <returns></returns>
        internal static XmlDocument NewXmlDocument() 
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", "no"));
            return doc;
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XmlDocument LoadXML(string path) 
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return doc;
        }


        /// <summary>
        /// 实体生成XML配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static XmlDocument ToXML(DBEntityInfo entity)
        {
            XmlDocument doc = NewXmlDocument();

            XmlNode classNode = doc.CreateElement("class");
            doc.AppendChild(classNode);

            XmlAttribute att = doc.CreateAttribute("TableName");
            att.InnerText = entity.BelongTable.Name;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("ClassName");
            string className = entity.ClassName;
            att.InnerText = entity.EntityNamespace + "." + className;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("IsTable");
            att.InnerText = entity.BelongTable.IsView?"0":"1";
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("BelongDB");
            att.InnerText = entity.CurrentDBConfigInfo.DbName;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("UseCache");
            att.InnerText = "0";
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("Description");
            att.InnerText = entity.Summary;
            classNode.Attributes.Append(att);

            AppendPropertyInfo(entity.BelongTable.Params, classNode);
            AppendRelationInfo(entity.BelongTable.RelationItems, classNode);
            return doc;
        }

        /// <summary>
        /// 添加映射信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="classNode"></param>
        private static void AppendRelationInfo(List<TableRelationAttribute> relationItems, XmlNode classNode)
        {
            XmlDocument doc = classNode.OwnerDocument;
            if (relationItems == null) 
            {
                return;
            }
            foreach (TableRelationAttribute field in relationItems)
            {
                
                //EntityParamField field = kp.Value;
                XmlNode node = doc.CreateElement("relation");
                classNode.AppendChild(node);

                XmlAttribute att = doc.CreateAttribute("FieldName");//字段名
                att.InnerText = field.FieldName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("PropertyName");//对应的属性名名
                att.InnerText = field.PropertyName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("SourceProperty");//数据库类型
                att.InnerText = EntityFieldBase.ToPascalName( field.SourceName);
                node.Attributes.Append(att);

                att = doc.CreateAttribute("TargetProperty");//数据库类型长度
                att.InnerText = EntityFieldBase.ToPascalName(field.TargetName);
                node.Attributes.Append(att);

                att = doc.CreateAttribute("IsToDB");//数据库类型长度
                att.InnerText = field.IsToDB ? "1" : "0";
                node.Attributes.Append(att);

                att = doc.CreateAttribute("IsParent");//数据库类型长度
                att.InnerText = field.IsParent ? "1" : "0";
                node.Attributes.Append(att);

                att = doc.CreateAttribute("Description");//属性注释
                att.InnerText = field.Description;
                node.Attributes.Append(att);
            }
        }

        /// <summary>
        /// 添加属性的XML信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="classNode"></param>
        private static void AppendPropertyInfo(List<EntityParam> lstParam, XmlNode classNode)
        {
            XmlDocument doc = classNode.OwnerDocument;
            if (lstParam == null)
            {
                return;
            }
            foreach (EntityParam field in lstParam)
            {
                //EntityParamField field = kp.Value;
                XmlNode node = doc.CreateElement("property");
                classNode.AppendChild(node);

                XmlAttribute att = doc.CreateAttribute("FieldName");//字段名
                att.InnerText = field.FieldName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("PropertyName");//对应的属性名名
                att.InnerText = field.PropertyName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("DbType");//数据库类型
                att.InnerText = field.SqlType.ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("Length");//数据库类型长度
                att.InnerText = field.Length.ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("EntityPropertyType");//类型
                att.InnerText = ((int)field.PropertyType).ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("ParamName");//字段名
                att.InnerText = field.ParamName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("ReadOnly");//字段名
                att.InnerText = field.ReadOnly?"1":"0";
                node.Attributes.Append(att);

                att = doc.CreateAttribute("Description");//属性注释
                att.InnerText = field.Description;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("AllowNull");//属性注释
                att.InnerText = field.AllowNull ? "1" : "0";
                node.Attributes.Append(att);
            }
        }

        /// <summary>
        /// 实体生成XML配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static XmlDocument ToXML(EntityConfig entity) 
        {
            XmlDocument doc = NewXmlDocument();
            
            XmlNode classNode = doc.CreateElement("class");
            doc.AppendChild(classNode);

            XmlAttribute att = doc.CreateAttribute("TableName");
            att.InnerText = entity.TableName;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("ClassName");
            string className=entity.ClassName;
            if (entity.ClassType.Generic) 
            {
                className += "`" + entity.ClassType.TypeParameterCount;
            }
            att.InnerText = entity.Namespace+"."+className;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("IsTable");
            att.InnerText = "1";
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("BelongDB");
            att.InnerText = entity.CurrentDBConfigInfo.DbName;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("UseCache");
            att.InnerText = entity.UseCache?"1":"0";
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("Description");
            att.InnerText = entity.Summary;
            classNode.Attributes.Append(att);

            AppendPropertyInfo(entity, classNode);
            AppendRelationInfo(entity, classNode);
            return doc;
        }

        /// <summary>
        /// 添加映射信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="classNode"></param>
        private static void AppendRelationInfo(EntityConfig entity, XmlNode classNode) 
        {
            XmlDocument doc = classNode.OwnerDocument;
            foreach (EntityRelationItem field in entity.ERelation)
            {
                if (!field.IsGenerate)
                {
                    continue;
                }
                //EntityParamField field = kp.Value;
                XmlNode node = doc.CreateElement("relation");
                classNode.AppendChild(node);

                XmlAttribute att = doc.CreateAttribute("FieldName");//字段名
                att.InnerText = field.FieldName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("PropertyName");//对应的属性名名
                att.InnerText = field.PropertyName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("SourceProperty");//数据库类型
                att.InnerText = field.SourceProperty;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("TargetProperty");//数据库类型长度
                att.InnerText = field.TargetProperty;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("IsToDB");//数据库类型长度
                att.InnerText = field.IsToDB?"1":"0";
                node.Attributes.Append(att);

                att = doc.CreateAttribute("IsParent");//数据库类型长度
                att.InnerText = field.IsParent ? "1" : "0";
                node.Attributes.Append(att);
                att = doc.CreateAttribute("Description");//属性注释
                att.InnerText = field.Summary;
                node.Attributes.Append(att);
            }


            if (entity.DbRelations != null)
            {
                AppendRelationInfo(entity.DbRelations, classNode);
            }
        }

        /// <summary>
        /// 添加属性的XML信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="classNode"></param>
        private static void AppendPropertyInfo(EntityConfig entity, XmlNode classNode) 
        {
            XmlDocument doc=classNode.OwnerDocument;
            foreach (EntityParamField field in entity.EParamFields) 
            {
                if (!field.IsGenerate) 
                {
                    continue;
                }
                //EntityParamField field = kp.Value;
                XmlNode node = doc.CreateElement("property");
                classNode.AppendChild(node);

                XmlAttribute att = doc.CreateAttribute("FieldName");//字段名
                att.InnerText = field.FieldName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("PropertyName");//对应的属性名名
                att.InnerText = field.PropertyName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("DbType");//数据库类型
                att.InnerText = field.DbType;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("Length");//数据库类型长度
                att.InnerText = field.Length.ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("EntityPropertyType");//类型
                att.InnerText = ((int)field.EntityPropertyType).ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("ParamName");//字段名
                att.InnerText = field.ParamName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("ReadOnly");//字段名
                att.InnerText = field.ReadOnly ? "1" : "0";
                node.Attributes.Append(att);

                att = doc.CreateAttribute("Description");//属性注释
                att.InnerText = field.Summary;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("AllowNull");//字段名
                att.InnerText = field.AllowNull ? "1" : "0";
                node.Attributes.Append(att);
            }

            if (entity.DbParams != null)
            {
                AppendPropertyInfo(entity.DbParams, classNode);
            }
        }
    }
}
