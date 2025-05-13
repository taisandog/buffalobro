using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Buffalo.DBTools.HelperKernel;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using System.IO;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using System.Xml;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DBTools.UIHelper;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.Kernel;
using DBTools;
using Buffalo.DBToolsPackage;

namespace Buffalo.DBTools.ROMHelper
{
    /// <summary>
    /// 数据库到实体的信息
    /// </summary>
    public class DBEntityInfo
    {
        //private Project _currentProject;

        ///// <summary>
        ///// 当前工程
        ///// </summary>
        //public Project CurrentProject
        //{
        //    get { return _currentProject; }

        //}
        private DBConfigInfo _currentDBConfigInfo;

        /// <summary>
        /// 数据库配置信息
        /// </summary>
        public DBConfigInfo CurrentDBConfigInfo
        {
            get { return _currentDBConfigInfo; }

        }


        private DBTableInfo _belongTable;

        /// <summary>
        /// 所属表
        /// </summary>
        public DBTableInfo BelongTable
        {
            get { return _belongTable; }
        }

        //ClassDesignerDocView _docView;

        //public ClassDesignerDocView DocView
        //{
        //    get { return _docView; }
        //}

        private ClassDesignerInfo _designerInfo;

        /// <summary>
        /// 类设计图信息
        /// </summary>
        public ClassDesignerInfo DesignerInfo
        {
            get { return _designerInfo; }
            set { _designerInfo = value; }
        }

        private string _fileName;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// 注释
        /// </summary>
        public string Summary
        {
            get { return _belongTable.Description; }
        }



        /// <summary>
        /// 实体信息
        /// </summary>
        /// <param name="belong">所属的数据库信息</param>
        public DBEntityInfo(string entityNamespace, DBTableInfo belong, 
            ClassDesignerInfo designerInfo, DBConfigInfo currentDBConfigInfo,
            string baseType) 
        {
            _belongTable = belong;
            _entityNamespace = entityNamespace;
            //_docView = docView;
            //_currentProject = currentProject;
            _designerInfo = designerInfo;
            _currentDBConfigInfo = currentDBConfigInfo;
            _baseType = baseType;
            InitInfo();
            //_tiers = tiers;
            FilterBaseTypeParam();
        }

        /// <summary>
        /// 填充字段和映射信息
        /// </summary>
        /// <param name="cls"></param>
        /// <param name="designerInfo"></param>
        /// <param name="dicParam"></param>
        /// <param name="dicRelation"></param>
        public static void FillBaseTypeParam(string type,ClassDesignerInfo designerInfo,
            Dictionary<string, EntityParamField> dicParam, Dictionary<string, EntityRelationItem> dicRelation) 
        {
            if (type == typeof(EntityBase).FullName || type == typeof(ThinModelBase).FullName)
            {
                return;
            }
            ClrClass cls = BuffaloToolCDCommand.FindClrClassByName(type, designerInfo.SelectedDiagram);
            if (cls == null)
            {
                return;
            }

            EntityConfig entity = new EntityConfig(cls, designerInfo);
            if (!entity.HasConfig) 
            {
                return;
            }
            foreach (EntityParamField ep in entity.EParamFields) 
            {
                if (!ep.IsGenerate) 
                {
                    continue;
                }
                dicParam[ep.ParamName] = ep;
            }
            foreach (EntityRelationItem er in entity.ERelation)
            {
                if (!er.IsGenerate)
                {
                    continue;
                }
                string key = er.SourceProperty+"_"+er.TargetProperty;
                dicRelation[key] = er;
            }
            FillBaseTypeParam(entity.BaseTypeName, designerInfo, dicParam, dicRelation);

        }
        

        /// <summary>
        /// 去掉基类已有字段
        /// </summary>
        private void FilterBaseTypeParam() 
        {
            

            Dictionary<string, EntityParamField> dicParam=new Dictionary<string,EntityParamField>(StringComparer.CurrentCultureIgnoreCase);
            Dictionary<string, EntityRelationItem> dicRelation = new Dictionary<string, EntityRelationItem>(StringComparer.CurrentCultureIgnoreCase);
            FillBaseTypeParam(_baseType, _designerInfo, dicParam, dicRelation);
            int count = _belongTable.Params.Count;
            for (int i = count - 1; i >= 0; i--) 
            {
                string key = _belongTable.Params[i].ParamName;
                if (dicParam.ContainsKey(key)) 
                {
                    _belongTable.Params.RemoveAt(i);
                }
            }

            count = _belongTable.RelationItems.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                TableRelationAttribute er = _belongTable.RelationItems[i];
                string key = EntityFieldBase.ToPascalName(er.SourceName) + "_" + EntityFieldBase.ToPascalName(er.TargetName);
                if (dicRelation.ContainsKey(key))
                {
                    _belongTable.RelationItems.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// 初始化类信息
        /// </summary>
        private void InitInfo() 
        {
            _className = EntityFieldBase.ToPascalName(_belongTable.Name);

            _fileName = GetRealFileName(_belongTable, DesignerInfo);
        }
        /// <summary>
        /// 获取实体的文件名
        /// </summary>
        /// <param name="table">实体表信息</param>
        /// <param name="doc">文档信息</param>
        /// <returns></returns>
        public static string GetRealFileName(DBTableInfo table, ClassDesignerInfo designerInfo) 
        {
            string className = EntityFieldBase.ToPascalName(table.Name);
            FileInfo docfile = new FileInfo(designerInfo.SelectDocView.DocData.FileName);
            
            return docfile.DirectoryName + "\\" + className + ".cs";
        }
        /// <summary>
        /// 获取实体的文件名
        /// </summary>
        /// <param name="table">实体表信息</param>
        /// <param name="doc">文档信息</param>
        /// <returns></returns>
        public static string GetEntityRealFileName(DBTableInfo table,DBConfigInfo db, ClassDesignerInfo designerInfo)
        {
            string className = EntityFieldBase.ToPascalName(table.Name);
            FileInfo docfile = new FileInfo(designerInfo.SelectDocView.DocData.FileName);
            if (db.EntityToDirectory)
            {
                return docfile.DirectoryName + "\\Entity\\" + className + ".cs";
            }
            return docfile.DirectoryName + "\\" + className + ".cs";
        }
        private string _entityNamespace;

        /// <summary>
        /// 实体命名空间
        /// </summary>
        public string EntityNamespace
        {
            get
            {
                return _entityNamespace;
            }
            
        }

        private string _className;
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName 
        {
            get 
            {
                return _className;
            }
        }

        private string _baseType;

        /// <summary>
        /// 基类
        /// </summary>
        public string BaseType 
        {
            get 
            {
                
                return _baseType;
            }
            set 
            {
                _baseType = value;
            }
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="code">代码行</param>
        /// <param name="tiers">层数</param>
        public void GreanCode(XmlDocument doc) 
        {
            string model = Models.entity;

            string baseType = BaseType;

            

            List<string> codes = new List<string>();
            
            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", _belongTable.Description);
                    tmp = tmp.Replace("<%=EntityBaseType%>", baseType);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    tmp = tmp.Replace("<%=EntityFields%>", BildFields());
                    tmp = tmp.Replace("<%=EntityRelations%>", BildRelations());
                    tmp = tmp.Replace("<%=EntityContext%>", BuildContext());
                    codes.Add(tmp);
                }
            }
            string eFile=GetEntityFileName(FileName);

            CodeFileHelper.SaveFile(eFile, codes);
            EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(eFile);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
            
            GenerateExtendCode();

            BQLEntityGenerater bqlEntity = new BQLEntityGenerater(this,DesignerInfo);
            GenerateExtendCode();
            if (_currentDBConfigInfo.Tier == 3)
            {
                Generate3Tier g3t = new Generate3Tier(this, DesignerInfo);
                g3t.GenerateBusiness();
                if (!string.IsNullOrEmpty(this._belongTable.Name))
                {
                    g3t.GenerateIDataAccess();
                    g3t.GenerateDataAccess();
                    g3t.GenerateBQLDataAccess();
                }
            }
            bqlEntity.GenerateBQLEntityDB();
            bqlEntity.GenerateBQLEntity();
            EntityMappingConfig.SaveXML(this);
            SetToDiagram(doc);
        }

        /// <summary>
        /// 获取实体文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetEntityFileName(string fileName) 
        {
            string ret=fileName;
            if (CurrentDBConfigInfo.EntityToDirectory) 
            {
                FileInfo info = new FileInfo(fileName);
                ret = info.DirectoryName + "\\Entity\\" + info.Name;
            }
            return ret;
        }

        /// <summary>
        /// 把类设置到类图
        /// </summary>
        private void SetToDiagram(XmlDocument doc) 
        {
            
            XmlNodeList lstClassDiagram = doc.GetElementsByTagName("ClassDiagram");
            if (lstClassDiagram.Count > 0) 
            {
                XmlNode classDia = lstClassDiagram[0];
                bool hasClass=false;//判断类图是否找到本类
                string classFullName=EntityNamespace+"."+ClassName;

                decimal maxX = 0m;//最大的X位置

                foreach (XmlNode classNode in classDia.ChildNodes) 
                {
                    if (!classNode.Name.Equals("Class", StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        continue;
                    }
                    XmlAttribute att = classNode.Attributes["Name"];
                    if (att != null) 
                    {
                        string fullName = att.InnerText;
                        if (fullName == classFullName) 
                        {
                            hasClass = true;
                        }
                    }

                    foreach (XmlNode postionNode in classNode.ChildNodes) 
                    {
                        if (postionNode.Name.Equals("Position", StringComparison.CurrentCultureIgnoreCase)) 
                        {
                            XmlAttribute attX = postionNode.Attributes["X"];
                            if (attX != null) 
                            {
                                decimal curX = 0m;
                                if(decimal.TryParse(attX.InnerText,out curX))
                                {
                                    if (curX > maxX) 
                                    {
                                        maxX = curX;
                                    }
                                }
                            }
                            break;
                        }
                    }

                }


                if (hasClass) 
                {
                    return;
                }


                AppendNode(classDia, maxX);

                
            }
        }

        /// <summary>
        /// 获取类图文档
        /// </summary>
        /// <returns></returns>
        public static XmlDocument GetClassDiagram(string file) 
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(file);
                return doc;
            }
            catch { }
            //如果加载失败，自动用空模版类图
            doc = EntityMappingConfig.NewXmlDocument();
            doc.LoadXml(Models.classdiagram);
            return doc;
        }
        /// <summary>
        /// 添加当前类到类图
        /// </summary>
        /// <param name="classDia"></param>
        private void AppendNode(XmlNode classDia,decimal maxX)
        {
            XmlDocument doc = classDia.OwnerDocument;
            XmlNode classNode = doc.CreateElement("Class");
            classDia.AppendChild(classNode);
            XmlAttribute attName = doc.CreateAttribute("Name");
            attName.InnerText=EntityNamespace + "." + ClassName;
            classNode.Attributes.Append(attName);

            XmlNode positionNode = doc.CreateElement("Position");
            classNode.AppendChild(positionNode);
            XmlAttribute attX = doc.CreateAttribute("X");
            attX.InnerText=(maxX + 2.0m).ToString("0.0");
            positionNode.Attributes.Append(attX);
            XmlAttribute attY = doc.CreateAttribute("Y");
            attY.InnerText = "1.5";
            positionNode.Attributes.Append(attY);
            XmlAttribute attWidth = doc.CreateAttribute("Width");
            attWidth.InnerText = "1.5"; 
            positionNode.Attributes.Append(attWidth);


            XmlNode typeIdentifierNode = doc.CreateElement("TypeIdentifier");
            classNode.AppendChild(typeIdentifierNode);
            XmlNode fileNameNode = doc.CreateElement("FileName");
            string basePath = DesignerInfo.GetClassDesignerPath();
            string eFile = GetEntityFileName(FileName);
            fileNameNode.InnerText = eFile.Replace(basePath, "").TrimStart('\\');
            typeIdentifierNode.AppendChild(fileNameNode);
            XmlNode hashCodeNode = doc.CreateElement("HashCode");
            string hashCode = "AAAAAAAAAIAAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
            hashCodeNode.InnerText = hashCode;
            typeIdentifierNode.AppendChild(hashCodeNode);
        }

        /// <summary>
        /// 填充关系代码
        /// </summary>
        /// <param name="er"></param>
        /// <param name="sb"></param>
        internal static void FillRelationsInfo(TableRelationAttribute er, StringBuilder sb) 
        {
            if (er.IsParent)
            {
                er.FieldTypeName = EntityFieldBase.ToPascalName(er.TargetTable);
                //string name = EntityFieldBase.ToPascalName(er.TargetTable) + "_" + EntityFieldBase.ToPascalName(er.SourceName) + "_" + EntityFieldBase.ToPascalName(er.TargetName);
                string name = EntityFieldBase.ToPascalName(er.SourceName)+"2"+EntityFieldBase.ToPascalName(er.TargetTable);
                er.FieldName = "_belong" + name;
                er.PropertyName = name;
                er.IsToDB = true;
                sb.AppendLine("        /// <summary>");
                sb.AppendLine(FormatSummary(er.Description));
                sb.AppendLine("        /// </summary>");
                sb.AppendLine("        protected " + er.FieldTypeName + " " + er.FieldName + ";");
                sb.AppendLine("");

                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// " + er.Description);
                sb.AppendLine("        /// </summary>");
                sb.AppendLine("        public virtual " + er.FieldTypeName + " " + er.PropertyName);
                sb.AppendLine("        {");
                sb.AppendLine("            get{ return " + er.FieldName + "; }");
                sb.AppendLine("            set{ " + er.FieldName + " = value; }");
                sb.AppendLine("        }");
            }
            else
            {
                er.FieldTypeName = "List<" + EntityFieldBase.ToPascalName(er.TargetTable) + ">";
                string name = EntityFieldBase.ToPascalName(er.TargetTable) + "2"  + EntityFieldBase.ToPascalName(er.TargetName);
                er.FieldName = "_lst" + name;
                //er.PropertyName = "Lst" + name;
               
                er.PropertyName = name;
                er.IsToDB = false;
                sb.AppendLine("        /// <summary>");
                sb.AppendLine(FormatSummary(er.Description));
                sb.AppendLine("        /// </summary>");
                sb.AppendLine("        protected " + er.FieldTypeName + " " + er.FieldName + ";");
                sb.AppendLine("");

                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// " + er.Description);
                sb.AppendLine("        /// </summary>");
                sb.AppendLine("        public virtual " + er.FieldTypeName + " " + er.PropertyName);
                sb.AppendLine("        {");
                sb.AppendLine("            get{ return " + er.FieldName + "; }");
                sb.AppendLine("            set{ " + er.FieldName + " = value; }");
                sb.AppendLine("        }");
            }
        }

        /// <summary>
        /// 创建关系
        /// </summary>
        /// <returns></returns>
        private string BildRelations() 
        {
            StringBuilder sb = new StringBuilder();
            if (_belongTable.RelationItems == null) 
            {
                return sb.ToString();
            }
            foreach (TableRelationAttribute er in _belongTable.RelationItems) 
            {
                FillRelationsInfo(er, sb);
                
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化注释
        /// </summary>
        /// <param name="summary"></param>
        /// <returns></returns>
        internal static string FormatSummary(string summary) 
        {
            if (string.IsNullOrEmpty(summary)) 
            {
                return "        ///";
            }
            StringBuilder sbSummary = new StringBuilder();
            bool isHead=true;
            using (StringReader reader = new StringReader(summary)) 
            {
                string line=null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!isHead) 
                    {
                        continue;
                    }
                    sbSummary.Append("        ///");
                    if (!isHead)
                    {
                        sbSummary.Append("<para>");
                    }
                    sbSummary.Append(System.Web.HttpUtility.HtmlEncode(line));
                    if (!isHead)
                    {
                        sbSummary.Append("</para>");
                    }
                    isHead = false;
                    sbSummary.AppendLine("");
                }
            }
            for (int i = sbSummary.Length - 1; i >= 0; i--) 
            {
                char cur = sbSummary[i];
                if (cur == '\r' || cur == '\n')
                {
                    sbSummary.Remove(sbSummary.Length - 1, 1);
                }
                else 
                {
                    break;
                }
            }
            return sbSummary.ToString();
        }

        /// <summary>
        /// 添加字段代码
        /// </summary>
        /// <param name="prm"></param>
        /// <param name="sb"></param>
        internal static void AppendFieldInfo(EntityParam prm, StringBuilder sb) 
        {
            prm.FieldName = "_" + EntityFieldBase.ToCamelName(prm.ParamName);
            string typeName = ToCSharpType(prm.SqlType,prm.AllowNull);
            sb.AppendLine("        ///<summary>");
            sb.AppendLine(FormatSummary(prm.Description));
            sb.AppendLine("        ///</summary>");
            sb.AppendLine("        protected " + typeName + " " + prm.FieldName + ";");
            sb.AppendLine("");


            prm.PropertyName = EntityFieldBase.ToPascalName(prm.ParamName);

            sb.AppendLine("        /// <summary>");
            sb.AppendLine(FormatSummary(prm.Description));
            sb.AppendLine("        ///</summary>");
            sb.AppendLine("        public virtual " + typeName + " " + prm.PropertyName + "");
            sb.AppendLine("        {");
            sb.AppendLine("            get{ return " + prm.FieldName + ";}");
            sb.AppendLine("            set{ "+ prm.FieldName +"=value;}");
            sb.AppendLine("        }");
        }


        /// <summary>
        /// 生成字段
        /// </summary>
        /// <returns></returns>
        private string BildFields() 
        {
            StringBuilder sb = new StringBuilder();
            if (_belongTable.Params == null)
            {
                return sb.ToString();
            }
            foreach (EntityParam prm in _belongTable.Params) 
            {
                
                AppendFieldInfo(prm, sb);
            }

            return sb.ToString();
        }
        /// <summary>
        /// 添加到代码
        /// </summary>
        /// <param name="lstTarget"></param>
        private string BuildContext()
        {
            if (CurrentDBConfigInfo.Tier != 1 || string.IsNullOrEmpty(_belongTable.Name))
            {
                return "";
            }
            string strField = "_____baseContext";

            string strPropertie = "GetContext";
            StringBuilder sbRet = new StringBuilder();

            sbRet.AppendLine("        private static ModelContext<" + ClassName + "> " + strField + "=new ModelContext<" + ClassName + ">();");



            sbRet.AppendLine("        /// <summary>");
            sbRet.AppendLine("        /// 获取查询关联类");
            sbRet.AppendLine("        /// </summary>");
            sbRet.AppendLine("        /// <returns></returns>");
            sbRet.AppendLine("        public static ModelContext<" + ClassName + "> " + strPropertie + "() ");
            sbRet.AppendLine("        {");
            sbRet.AppendLine("            return " + strField + ";");
            sbRet.AppendLine("        }");

            return sbRet.ToString();
        }
        /// <summary>
        /// 获取C#类型
        /// </summary>
        /// <param name="type">数据库类型</param>
        /// <param name="nullAble">是否可空</param>
        /// <returns></returns>
        private static string ToCSharpType(DbType type,bool nullable) 
        {
            switch (type)
            {
                case DbType.AnsiString: return "string";
                case DbType.AnsiStringFixedLength: return "string";
                case DbType.Binary: return "byte[]";
                case DbType.Boolean:
                    if (nullable)
                    {
                        return "bool?";
                    }
                    return "bool";
                case DbType.Byte:
                    if (nullable)
                    {
                        return "byte?";
                    }
                    return "byte";
                case DbType.Currency:
                    if (nullable)
                    {
                        return "decimal?";
                    }
                    return "decimal";
                case DbType.Date: 
                    if (nullable)
                    {
                        return "DateTime?";
                    }
                    return "DateTime";
                case DbType.DateTime:
                    if (nullable)
                    {
                        return "DateTime?";
                    }
                    return "DateTime";
                case DbType.Decimal:
                    if (nullable)
                    {
                        return "decimal?";
                    }
                    return "decimal";
                case DbType.Double: 
                    if (nullable)
                    {
                        return "double?";
                    }
                    return "double";
                case DbType.Guid: 
                    if (nullable)
                    {
                        return "Guid?";
                    }
                    return "Guid";
                case DbType.Int16: 
                    if (nullable)
                    {
                        return "short?";
                    }
                    return "short";
                case DbType.Int32:
                    if (nullable)
                    {
                        return "int?";
                    }
                    return "int";
                case DbType.Int64:
                    if (nullable)
                    {
                        return "long?";
                    }
                    return "long";
                case DbType.Object: return "object";
                case DbType.SByte:
                    if (nullable)
                    {
                        return "sbyte?";
                    }
                    return "sbyte";
                case DbType.Single:
                    if (nullable)
                    {
                        return "float?";
                    }
                    return "float";
                case DbType.String: return "string";
                case DbType.StringFixedLength: return "string";
                case DbType.Time: 
                    if (nullable)
                    {
                        return "TimeSpan?";
                    }
                    return "TimeSpan";
                case DbType.UInt16: 
                    if (nullable)
                    {
                        return "ushort?";
                    }
                    return "ushort";
                case DbType.UInt32:
                    if (nullable)
                    {
                        return "uint?";
                    }
                    return "uint";
                case DbType.UInt64:
                    if (nullable)
                    {
                        return "ulong?";
                    }
                    return "ulong";
                case DbType.VarNumeric:
                    if (nullable)
                    {
                        return "decimal?";
                    }
                    return "decimal";
                default:
                    {
                        return "object" ;
                    }
            }
        }

        /// <summary>
        /// 当前表到表信息的转换
        /// </summary>
        /// <returns></returns>
        public KeyWordTableParamItem ToTableInfo()
        {
            KeyWordTableParamItem table = new KeyWordTableParamItem(_belongTable.Name, null);
            table.Description = _belongTable.Description;
            table.IsView = _belongTable.IsView;
            table.Params = _belongTable.Params;
            table.RelationItems = _belongTable.RelationItems;
            return table;
        }

        /// <summary>
        /// 生成扩展类代码文件
        /// </summary>
        private void GenerateExtendCode()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            string eFileName = GetEntityFileName(FileName);
            EnvDTE.ProjectItem classItem = EntityConfig.GetProjectItemByFileName(DesignerInfo, eFileName);

            FileInfo fileInfo = new FileInfo(eFileName);

            string exName = EntityConfig.FrameworkExName;
            if (!DesignerInfo.IsNetFrameworkLib)//项目文件清单没有主代码项，则为Core或Standard项目，需要另一个扩展名
            {
                exName = EntityConfig.StandardExName;
            }

            string fileName = fileInfo.DirectoryName + "\\" + fileInfo.Name.Replace(".cs", exName);
            if (File.Exists(fileName))
            {
                return;
            }
            EnvDTE.ProjectItem newit = null;
            if (File.Exists(fileName))
            {
                if (DesignerInfo.IsNetFrameworkLib && !EntityConfig.ExistsExtendCode(classItem, fileName))
                {
                    newit = classItem.ProjectItems.AddFromFile(fileName);
                    newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
                }
                return;
            }


            string model = Models.userentity;
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", this._belongTable.Description);


                    string classFullName = ClassName;
                    
                    tmp = tmp.Replace("<%=ClassFullName%>", classFullName);
                    codes.Add(tmp);
                }
            }
            
            CodeFileHelper.SaveFile(fileName, codes);
            newit = classItem.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }
    }
}
