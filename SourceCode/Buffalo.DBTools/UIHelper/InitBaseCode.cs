using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// 初始化基础dll
    /// </summary>
    public class InitBaseCode
    {
        /// <summary>
        /// 初始化基础dll
        /// </summary>
        public static void InitBaseDll() 
        {
            Assembly ass = typeof(Buffalo.DBTools.CommandBar).Assembly;
            string path = new Uri(ass.CodeBase).LocalPath;
            FileInfo file = new FileInfo(path);
            path = file.DirectoryName + "\\Buffalo.GeneratorInfo.dll";
            if (!File.Exists(path))
            {
                return;
            }
            file = new FileInfo(path);
            string fileName = CommonMethods.GetBaseRoot(file.Name);

            CommonMethods.CopyNewer(path, fileName);
        }
    }
}
