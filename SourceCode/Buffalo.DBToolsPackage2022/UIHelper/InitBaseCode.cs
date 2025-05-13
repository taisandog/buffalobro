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
        ///// <summary>
        ///// 初始化基础dll
        ///// </summary>
        //public static string InitBaseDll() 
        //{
        //    Assembly ass = typeof(Buffalo.DBTools.CommandBar).Assembly;
        //    string path = new Uri(ass.CodeBase).LocalPath;
        //    FileInfo file = new FileInfo(path);
        //    path = file.DirectoryName + "\\Buffalo.GeneratorInfo.dll";
        //    if (!File.Exists(path))
        //    {
        //        return null;
        //    }
        //    file = new FileInfo(path);
        //    string fileName = CommonMethods.GetBaseRoot(file.Name);
        //    try
        //    {
        //        CommonMethods.CopyNewer(path, fileName);
        //    }
        //    catch (Exception ex) 
        //    {
        //        FileInfo f=new FileInfo(fileName);
        //        return "拷贝Buffalo.GeneratorInfo.dll失败，请尝试检查" + f.DirectoryName + "的权限\n错误信息:\n" + ex.Message;
        //    }
        //    return null;
        //}
    }
}
