using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.Kernel;
using EnvDTE;
using Buffalo.Win32Kernel;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DBTools.HelperKernel
{
    public class EntityRemoveHelper
    {
        public static void RemoveEntity(EntityConfig entity) 
        {
            FileInfo info = new FileInfo(entity.DesignerInfo.SelectDocView.DocData.FileName);

            //业务层
            string fileName = info.DirectoryName + "\\Business\\" + entity.ClassName + "Business.cs"; ;
            RemoveFromProject(fileName, entity.DesignerInfo);

            //数据层
            string dicPath = info.DirectoryName + "\\DataAccess";
            entity.InitDBConfig();
            DataAccessMappingConfig dalconfig = new DataAccessMappingConfig(entity);
            foreach (ComboBoxItem itype in Generate3Tier.DataAccessTypes)
            {
                string type = itype.Value.ToString();
                string dalPath = dicPath + "\\" + type;
                fileName = dalPath + "\\" + entity.ClassName + "DataAccess.cs";
                RemoveFromProject(fileName, entity.DesignerInfo);

                dalconfig.DeleteDal(dalconfig.DataAccessNamespace + "." + type + "." + entity.ClassName + "DataAccess");
                dalconfig.DeleteBo(dalconfig.BusinessNamespace + "." + entity.ClassName + "Business");
            }
            dalconfig.SaveXML();
            string idalPath = dicPath + "\\IDataAccess";
            fileName = idalPath + "\\I" + entity.ClassName + "DataAccess.cs";
            RemoveFromProject(fileName, entity.DesignerInfo);
            idalPath = dicPath + "\\Bql";
            fileName = idalPath + "\\" + entity.ClassName + "DataAccess.cs";
            RemoveFromProject(fileName, entity.DesignerInfo);
            
            

            //删除BQLEntity
            fileName = info.DirectoryName + "\\BQLEntity\\" + entity.ClassName + ".cs";
            RemoveFromProject(fileName, entity.DesignerInfo);

            //BEM.xml
            fileName = info.DirectoryName + "\\BEM\\" + entity.ClassName + DataAccessLoader.EnitityNameEnd;
            RemoveFromProject(fileName, entity.DesignerInfo);

            //移除实体
            fileName = entity.FileName;
            RemoveFromProject(fileName, entity.DesignerInfo);
            fileName = entity.FileName.Replace(".cs", ".extend.cs");
            RemoveFromProject(fileName, entity.DesignerInfo);
        }

        /// <summary>
        /// 从项目中删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="project"></param>
        private static void RemoveFromProject(string fileName, ClassDesignerInfo info) 
        {
            Project project = info.CurrentProject;
            FileInfo finfo = new FileInfo(project.FileName);
            fileName = fileName.Replace(finfo.DirectoryName, "");
            string[] strPart = fileName.Split(new char[] { '\\' });
            ProjectItems curProjects = project.ProjectItems;
            ProjectItem curItem = null;
            foreach (string part in strPart) 
            {
                if (string.IsNullOrEmpty(part)) 
                {
                    continue;
                }
                curItem = FindItem(curProjects, part);
                if (curItem == null) 
                {
                    return;
                }
                curProjects = curItem.ProjectItems;
            }
            curItem.Delete();
        }

        /// <summary>
        /// 通过名字查找项目中的项
        /// </summary>
        /// <param name="items"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static ProjectItem FindItem(ProjectItems items, string name) 
        {
            for (int i = 1; i <= items.Count; i++)
            {
                try
                {
                    ProjectItem item = items.Item(i);
                    if (name == item.Name)
                    {
                        return item;
                    }
                }
                catch { }
            }
            return null;
        }
    }
}
