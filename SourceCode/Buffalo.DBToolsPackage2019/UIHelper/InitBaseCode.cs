using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// ��ʼ������dll
    /// </summary>
    public class InitBaseCode
    {
        /// <summary>
        /// ��ʼ������dll
        /// </summary>
        public static string InitBaseDll() 
        {
            Assembly ass = typeof(Buffalo.DBTools.CommandBar).Assembly;
            string path = new Uri(ass.CodeBase).LocalPath;
            FileInfo file = new FileInfo(path);
            path = file.DirectoryName + "\\Buffalo.GeneratorInfo.dll";
            if (!File.Exists(path))
            {
                return null;
            }
            file = new FileInfo(path);
            string fileName = CommonMethods.GetBaseRoot(file.Name);
            try
            {
                CommonMethods.CopyNewer(path, fileName);
            }
            catch (Exception ex) 
            {
                FileInfo f=new FileInfo(fileName);
                return "����Buffalo.GeneratorInfo.dllʧ�ܣ��볢�Լ��" + f.DirectoryName + "��Ȩ��\n������Ϣ:\n" + ex.Message;
            }
            return null;
        }
    }
}
