using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling.Diagrams;
using EnvDTE;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using System.IO;
using Buffalo.DBTools.HelperKernel;

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
            get { return _currentProject; }
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




        /// <summary>
        /// 获取命名空间
        /// </summary>
        /// <param name="docView">类设计图</param>
        /// <param name="project">工程</param>
        /// <returns></returns>
        public string GetNameSpace()
        {
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
        }

    }
}
