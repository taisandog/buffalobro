using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.UIHelper.ModelLoader;
using EnvDTE;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// ��Ŀ��
    /// </summary>
    public class UIProject
    {
        private string _name;
        /// <summary>
        /// ��Ŀ��
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<UIProjectItem> _lstItems=new List<UIProjectItem>();

        /// <summary>
        /// ��Ŀ��
        /// </summary>
        public List<UIProjectItem> LstItems
        {
            get { return _lstItems; }
        }

         /// <summary>
        /// ���ɴ���
        /// </summary>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        /// <param name="classConfig">UI������Ϣ</param>
        /// <param name="selectPropertys">ѡ����Ҫ���ɵ�������Ϣ</param>
        /// <returns></returns>
        public void GenerateCode(EntityInfo entityInfo, UIConfigItem classConfig,Project selectedProject, List<UIModelItem> selectPropertys, UIModelItem classInfo)
        {
            GenerateCode(entityInfo, classConfig,selectedProject, selectPropertys,classInfo, _lstItems, null);
        }

        /// <summary>
        /// �����Ŀ�ı��뻺��
        /// </summary>
        public void ClearCache(EntityInfo entityInfo)
        {
            CodeGenCache.Clear();
        }
        ///// <summary>
        ///// �����Ŀ�ı��뻺��
        ///// </summary>
        //public void ClearCache(EntityInfo entityInfo, List<UIProjectItem> lstItem) 
        //{
        //    foreach (UIProjectItem pitem in lstItem)
        //    {
        //        string mPath = UIConfigItem.FormatParameter(pitem.ModelPath, entityInfo);
        //        CodeGenCache.DeleteGenerationer(mPath);
        //        if (pitem.ChildItems != null && pitem.ChildItems.Count > 0)
        //        {
        //            ClearCache(entityInfo, pitem.ChildItems);
        //        }
        //    }
        //}

        /// <summary>
        /// ���ɴ���
        /// </summary>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        /// <param name="classConfig">UI������Ϣ</param>
        /// <param name="selectPropertys">ѡ����Ҫ���ɵ�������Ϣ</param>
        /// <param name="lstItem">UI��Ŀ��</param>
        /// <param name="parentItem">����</param>
        /// <returns></returns>
        private void GenerateCode(EntityInfo entityInfo, UIConfigItem classConfig, Project selectedProject,
            List<UIModelItem> selectPropertys,UIModelItem classInfo,List<UIProjectItem> lstItem,ProjectItem parentItem) 
        {
            Encoding fileEncoding = null;

            foreach (UIProjectItem pitem in lstItem) 
            {
                string mPath = UIConfigItem.FormatParameter(pitem.ModelPath, entityInfo,selectedProject);
                string tPath = UIConfigItem.FormatParameter(pitem.TargetPath, entityInfo, selectedProject);
                CodeGenInfo info=CodeGenCache.GetGenerationer(mPath,entityInfo);
                string content=info.Invoke(entityInfo, classConfig, selectPropertys,classInfo);
                fileEncoding = pitem.Encoding;
                ProjectItem item = SaveItem(tPath, selectedProject, content, pitem.GenType, parentItem, fileEncoding);
                if (pitem.ChildItems != null && pitem.ChildItems.Count > 0) 
                {
                    GenerateCode(entityInfo, classConfig, selectedProject, selectPropertys, classInfo, pitem.ChildItems, item);
                }
            }

            
        }

        /// <summary>
        /// �������ļ�
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="selectedProject">ѡ�е���Ŀ</param>
        /// <param name="content">�ļ�����</param>
        /// <param name="baction">�����ļ�������</param>
        /// <param name="parentItem"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private ProjectItem SaveItem(string fileName, Project selectedProject,
            string content, BuildAction baction, ProjectItem parentItem,Encoding encoding) 
        {
            
            CodeFileHelper.SaveFile(fileName, content, encoding);
            EnvDTE.ProjectItem newit = null;
            if (parentItem != null)
            {
                newit = parentItem.ProjectItems.AddFromFile(fileName);

                    newit.Properties.Item("BuildAction").Value = (int)baction;
                
            }
            else 
            {
                newit = selectedProject.ProjectItems.AddFromFile(fileName);

                    newit.Properties.Item("BuildAction").Value = (int)baction;
                
            }
            return newit;
        }

    }
}
