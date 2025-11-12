using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling.Diagrams;
using EnvDTE;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using System.IO;
using Buffalo.DBTools.HelperKernel;
using System.Xml;

namespace Buffalo.DBTools
{
    /// <summary>
    /// 类图信息
    /// </summary>
    public class ClassDesignerInfo
    {
        private Diagram _selectedDiagram = null;

        /// <summary>
        /// 选中的关系图
        /// </summary>
        public Diagram SelectedDiagram
        {
            get { return _selectedDiagram; }
        }
        private Project _currentProject;
        /// <summary>
        /// 当前项目
        /// </summary>
        public Project CurrentProject
        {
            get
            {
                return _currentProject;
            }
        }
        
        ClassDesignerDocView _selectDocView;
        /// <summary>
        /// 选择的文档
        /// </summary>
        public ClassDesignerDocView SelectDocView
        {
            get { return _selectDocView; }
        }
        /// <summary>
        /// 获取类图所在路径
        /// </summary>
        /// <param name="docView"></param>
        /// <returns></returns>
        public string GetClassDesignerPath()
        {
            FileInfo docFile = new FileInfo(SelectDocView.DocData.FileName);
            return docFile.DirectoryName;
        }

        private bool _isFramework=true;
        /// <summary>
        /// 是否.net Framework类库
        /// </summary>
        public bool IsNetFrameworkLib
        {
            get { return _isFramework; }
        }


        /// <summary>
        /// 获取命名空间
        /// </summary>
        /// <param name="docView">类设计图</param>
        /// <param name="project">工程</param>
        /// <returns></returns>
        public string GetNameSpace()
        {
            if (_selectDocView == null) 
            {
                throw new NullReferenceException("获取当前文件错误，请关闭.cd类图文件重新打开重试");
            }
            FileInfo docFile = new FileInfo(SelectDocView.DocData.FileName);
            FileInfo projectFile = new FileInfo(CurrentProject.FileName);
            string dic = docFile.Directory.Name.Replace(projectFile.Directory.Name, "");
            string ret = dic.Replace("\\", ".");

            ret = CurrentProject.Name + "." + ret;
            ret = ret.Trim('.');
            return ret;
        }
        /// <summary>
        /// 类图信息
        /// </summary>
        /// <param name="selectedDiagram"></param>
        /// <param name="currentProject"></param>
        /// <param name="selectDocView"></param>
        public ClassDesignerInfo(Diagram selectedDiagram, Project currentProject,
            ClassDesignerDocView selectDocView) 
        {
            _selectedDiagram = selectedDiagram;
            _currentProject = currentProject;
            _selectDocView = selectDocView;

            _isFramework = GetIsFramework(_currentProject);
        }

        /// <summary>
        /// 是否Framework类库
        /// </summary>
        /// <param name="currentProject"></param>
        /// <returns></returns>
        private bool GetIsFramework(Project currentProject)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            string fileName = currentProject.FileName;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNodeList lstPropertyGroup = doc.GetElementsByTagName("PropertyGroup");
            foreach(XmlNode nodePropertyGroup in lstPropertyGroup)
            {
                foreach (XmlNode nodechild in nodePropertyGroup.ChildNodes)
                {
                    if (string.Equals(nodechild.Name, "TargetFramework"))
                    {
                        return false;
                    }
                    if (string.Equals(nodechild.Name, "TargetFrameworkVersion"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
