using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using Buffalo.WebKernel;

namespace Buffalo.WebKernel.WebCommons.ContorlCommons
{
    /// <summary>
    /// 自动保存JS的类
    /// </summary>
    public class JsSaver
    {
        /// <summary>
        /// 默认的JS存放文件夹
        /// </summary>
        private const string defaultDirectory = "/Scripts/";

        /// <summary>
        /// 获取JS文件路径
        /// </summary>
        /// <param name="fileName">JS文件名</param>
        /// <returns></returns>
        public static string GetDefualtJsUrl(string fileName) 
        {
            return WebCommon.GetVirPath() + defaultDirectory+fileName;
        }

        /// <summary>
        /// sctipt的基础保存目录
        /// </summary>
        public static string BaseSavePath 
        {
            get 
            {
                return HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath + defaultDirectory);
            }
        }

        /// <summary>
        /// 保存JS文件的方法
        /// </summary>
        /// <param name="fileName">JS文件名</param>
        /// <param name="jsContent">JS内容</param>
        public static void SaveJS(string fileName, byte[] file)
        {
            string dir = BaseSavePath;
            string root = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath + defaultDirectory + fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(root))
            {
                File.WriteAllBytes(root, file);
            }
        }
        /// <summary>
        /// 保存JS文件的方法
        /// </summary>
        /// <param name="fileName">JS文件名</param>
        /// <param name="jsContent">JS内容</param>
        public static void SaveJS(string fileName,string jsContent) 
        {
            string dir = BaseSavePath;
            string root = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath + defaultDirectory + fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(root))
            {
                StreamWriter writer = new StreamWriter(root, false, System.Text.Encoding.UTF8);
                writer.Write(jsContent);
                writer.Close();
            }
        }
    }
}
