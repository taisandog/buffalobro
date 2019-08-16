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
    /// ��ͼ��Ϣ
    /// </summary>
    public class ClassDesignerInfo
    {
        private Diagram _selectedDiagram = null;

        /// <summary>
        /// ѡ�еĹ�ϵͼ
        /// </summary>
        public Diagram SelectedDiagram
        {
            get { return _selectedDiagram; }
        }
        private Project _currentProject;
        /// <summary>
        /// ��ǰ��Ŀ
        /// </summary>
        public Project CurrentProject
        {
            get { return _currentProject; }
        }
        
        ClassDesignerDocView _selectDocView;
        /// <summary>
        /// ѡ����ĵ�
        /// </summary>
        public ClassDesignerDocView SelectDocView
        {
            get { return _selectDocView; }
        }
        /// <summary>
        /// ��ȡ��ͼ����·��
        /// </summary>
        /// <param name="docView"></param>
        /// <returns></returns>
        public string GetClassDesignerPath()
        {
            FileInfo docFile = new FileInfo(SelectDocView.DocData.FileName);
            return docFile.DirectoryName;
        }




        /// <summary>
        /// ��ȡ�����ռ�
        /// </summary>
        /// <param name="docView">�����ͼ</param>
        /// <param name="project">����</param>
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
        /// ��ͼ��Ϣ
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
