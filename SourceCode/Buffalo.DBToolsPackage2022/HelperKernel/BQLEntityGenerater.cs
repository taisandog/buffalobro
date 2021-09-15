using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DBTools.ROMHelper;
using EnvDTE;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DBTools.UIHelper;
using DBTools;
using Buffalo.DBToolsPackage;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// BQL实体生成
    /// </summary>
    public class BQLEntityGenerater:GrneraterBase
    {
        public BQLEntityGenerater(DBEntityInfo config, ClassDesignerInfo info)
            : base(config, info)
        {
        } 

        public BQLEntityGenerater(EntityConfig config) :base(config)
        {
        }

        /// <summary>
        /// 生成DB声明
        /// </summary>
        public void GenerateBQLEntityDB() 
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            //FileInfo info = new FileInfo(EntityFileName);
            string dicPath = GenerateBasePath + "\\BQLEntity";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string fileName = dicPath + "\\" + DBName + ".cs";
            
            string model = Models.bqldb;

            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                    tmp = tmp.Replace("<%=DBName%>", DBName);
                    
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }

        /// <summary>
        /// 生成BQL实体
        /// </summary>
        public void GenerateBQLEntity()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            TagManager tag=new TagManager();
            //FileInfo info = new FileInfo(EntityFileName);
            string dicPath = GenerateBasePath + "\\BQLEntity";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }

            string fileName = dicPath + "\\"+ ClassName + ".cs";

            string idal = Models.bqlentity;
            List<string> codes = new List<string>();
            string baseType = null;
            if (EntityConfig.IsSystemTypeName(EntityBaseTypeName))
            {
                baseType = "BQLEntityTableHandle";
            }
            else 
            {
                baseType = BaseNamespace + ".BQLEntity." + FormatClassName(EntityBaseTypeShortName);
                
            }
            
            using (StringReader reader = new StringReader(idal))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    if (tmp.StartsWith("<%#IF TableName%>")) 
                    {
                        tag.AddTag("TableName");
                    }
                    else if (tmp.StartsWith("<%#ENDIF%>"))
                    {
                        tag.PopTag();
                    }
                    else
                    {
                        //if (tag.CurrentTag == "TableName" && string.IsNullOrEmpty(Table.TableName)) 
                        //{
                        //    continue;
                        //}
                        tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                        tmp = tmp.Replace("<%=BQLClassName%>", FormatClassName(ClassName));
                        
                        tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                        tmp = tmp.Replace("<%=Summary%>", Table.Description);
                        tmp = tmp.Replace("<%=DBName%>", DBName);
                        string args = GetBastGenericArgs();
                        string realbasetype = baseType;
                        if (!string.IsNullOrEmpty(args)) 
                        {
                            realbasetype += "<" + args + ">";
                        }
                        tmp = tmp.Replace("<%=BQLEntityBaseType%>", realbasetype);
                        tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);

                        string className = ClassName ;

                        tmp = tmp.Replace("<%=ClassName%>", className);
                        if (GenericInfo != null && GenericInfo.Count != 0) //有泛型
                        {
                            StringBuilder generic = new StringBuilder(200);
                            StringBuilder where = new StringBuilder(200);
                            GetGeneric(generic, where);
                            tmp = tmp.Replace("<%=Generic%>", generic.ToString());
                            tmp = tmp.Replace("<%=GenericWhere%>", where.ToString());
                            tmp = tmp.Replace("<%=HasGeneric%>", "<>");
                        }
                        else 
                        {
                            tmp = tmp.Replace("<%=Generic%>", "");
                            tmp = tmp.Replace("<%=GenericWhere%>", "");
                            tmp = tmp.Replace("<%=HasGeneric%>", "");
                        }
                        string entityClassName = ClassName;
                        tmp = tmp.Replace("<%=EntityClassName%>", entityClassName);
                        tmp = tmp.Replace("<%=PropertyDetail%>", GenProperty());
                        tmp = tmp.Replace("<%=RelationDetail%>", GenRelation());
                        tmp = tmp.Replace("<%=PropertyInit%>", GenInit());
                        codes.Add(tmp);
                    }
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }

        /// <summary>
        /// 获取基类的泛型参数
        /// </summary>
        /// <returns></returns>
        private string GetBastGenericArgs() 
        {
            if (GenericArgs == null || GenericArgs.Count <= 0) 
            {
                return null;
            }
            List<ClrClass> lstcls = BuffaloToolCDCommand.GetAllClass(DesignerInfo.SelectedDiagram);
            Dictionary<string, ClrClass> dic = new Dictionary<string, ClrClass>();
            StringBuilder sbType=new StringBuilder();
            foreach (ClrClass cls in lstcls) 
            {
                sbType.Append(cls.OwnerNamespace.Name);
                if (sbType.Length > 0)
                {
                    sbType.Append(".");
                }
                sbType.Append(cls.Name);
                dic[sbType.ToString()] = cls;
                sbType.Remove(0, sbType.Length);
            }
            StringBuilder sb = new StringBuilder();
            foreach (string str in GenericArgs) 
            {
                ClrClass curClass = null;

                if (dic.TryGetValue(str,out curClass))
                {
                    sb.Append("DB_" + curClass.Name);
                }
                else 
                {
                    sb.Append("BQLEntityParamHandle");
                }
                sb.Append(",");
            }
            if (sb.Length > 0) 
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取泛型字符串
        /// </summary>
        /// <returns></returns>
        private void GetGeneric(StringBuilder generic,StringBuilder where) 
        {
            if (GenericInfo == null || GenericInfo.Count == 0) 
            {
                return;
            }
            generic.Append("<");
            foreach (KeyValuePair<string, List<string>> kvp in GenericInfo) 
            {
                generic.Append(kvp.Key);
                generic.Append(",");
                if (kvp.Value != null && kvp.Value.Count > 0)
                {
                    
                    where.Append("\nwhere " + kvp.Key + ":");
                    foreach (string whereItem in kvp.Value)
                    {
                        if (whereItem.IndexOf("EntityBase") >= 0 || whereItem.IndexOf("ThinModelBase") >= 0) 
                        {
                            where.Append("BQLEntityParamHandle");
                        }
                        else if (whereItem.IndexOf("(") < 0)
                        {
                            where.Append(FormatClassName(whereItem));
                            
                        }
                        else 
                        {
                            where.Append(whereItem);
                        }
                        where.Append(",");
                    }
                    if (where.Length > 0)
                    {
                        where.Remove(where.Length - 1, 1);
                    }
                }
            }
            if (generic.Length > 0) 
            {
                generic.Remove(generic.Length - 1, 1);
            }

            
            generic.Append(">");
        }
        /// <summary>
        /// 生成属性
        /// </summary>
        /// <returns></returns>
        private string GenProperty() 
        {
            StringBuilder sbProperty = new StringBuilder();
            if (Table.Params == null)
            {
                return sbProperty.ToString();
            }
            foreach (EntityParam epf in Table.Params) 
            {
                //if (!epf.IsGenerate)
                //{
                //    continue;
                //}
                sbProperty.AppendLine("        private BQLEntityParamHandle " + epf.FieldName + " = null;");
                sbProperty.AppendLine("        /// <summary>");
                sbProperty.AppendLine(DBEntityInfo.FormatSummary(epf.Description));
                sbProperty.AppendLine("        /// </summary>");
                sbProperty.AppendLine("        public BQLEntityParamHandle " + epf.PropertyName + "");
                sbProperty.AppendLine("        {");
                sbProperty.AppendLine("            get");
                sbProperty.AppendLine("            {");
                sbProperty.AppendLine("                return " + epf.FieldName + ";");
                sbProperty.AppendLine("            }");
                sbProperty.AppendLine("         }");
            }
            return sbProperty.ToString();
        }

        /// <summary>
        /// 格式化类名
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private string FormatClassName(string typeName)
        {
            StringBuilder sbName = new StringBuilder(typeName.Length + 10);
            sbName.Append("BQL_");
            sbName.Append(typeName);
            return sbName.ToString();
        }

        /// <summary>
        /// 生成映射属性
        /// </summary>
        /// <returns></returns>
        private string GenRelation()
        {
            StringBuilder sbRelation = new StringBuilder();
            if (Table.RelationItems == null) 
            {
                return sbRelation.ToString();
            }
            foreach (TableRelationAttribute er in Table.RelationItems)
            {
                //if (!er.IsGenerate)
                //{
                //    continue;
                //}
                //string targetType = er.FInfo.MemberTypeShortName;
                //if (er.IsParent)
                //{
                string targetType = er.FieldTypeName;
                sbRelation.AppendLine("        /// <summary>");
                sbRelation.AppendLine(DBEntityInfo.FormatSummary(er.Description));
                sbRelation.AppendLine("        /// </summary>");

                string type = null;
                if (!er.IsParent)
                {
                    targetType = er.FieldTypeName;
                    int indexStart = targetType.IndexOf("<");
                    int indexEnd = targetType.LastIndexOf(">");
                    if (indexStart > 0 && indexEnd > 0)
                    {
                        targetType = targetType.Substring(indexStart + 1, indexEnd - indexStart - 1);
                    }

                }

                type = FormatClassName(targetType);

                sbRelation.AppendLine("        public " + type + " " + er.PropertyName + "");
                sbRelation.AppendLine("        {");
                sbRelation.AppendLine("            get");
                sbRelation.AppendLine("            {");

                sbRelation.AppendLine("               return new " + FormatClassName(targetType) + "(this,\"" + er.PropertyName + "\");");

                //else
                //{
                //    sbRelation.Append("               Type objType = typeof(" + type + ");");

                //    sbRelation.Append("               return (" + type + ")Activator.CreateInstance(objType, this, \"" + er.PropertyName + "\");");
                //}
                sbRelation.AppendLine("            }");
                sbRelation.AppendLine("         }");
                //}

            }
            return sbRelation.ToString();
        }

        /// <summary>
        /// 生成映射属性
        /// </summary>
        /// <returns></returns>
        private string GenInit()
        {
            StringBuilder sbInit = new StringBuilder();

            foreach (EntityParam epf in Table.Params)
            {
                //if (!epf.IsGenerate) 
                //{
                //    continue;
                //}
                sbInit.AppendLine("            " + epf.FieldName + "=CreateProperty(\"" + epf.PropertyName + "\");");
                
            }
            return sbInit.ToString();
        }
    }
}
