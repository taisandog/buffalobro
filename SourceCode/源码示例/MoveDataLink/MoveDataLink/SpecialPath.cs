using System;
using System.Collections.Generic;
using System.Text;

namespace MoveDataLink
{
    /// <summary>
    /// 特殊文件夹
    /// </summary>
    public class SpecialPath
    {
        /// <summary>
        /// 特殊文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="summary">文件夹说明</param>
        private SpecialPath(string path,string summary) 
        {
            _path = path;
            _summary = summary;
        }
        /// <summary>
        /// 特殊文件夹
        /// </summary>
        private SpecialPath() { }

        private string _path;
        /// <summary>
        /// 文件夹路径
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        private string _summary;
        /// <summary>
        /// 文件夹说明
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }

        public override string ToString()
        {
            return Summary;
        }


        private static List<SpecialPath> _defaultPath = GetSpecialPaths();

        /// <summary>
        /// 默认可选文件夹
        /// </summary>
        public static List<SpecialPath> DefaultPath
        {
            get { return SpecialPath._defaultPath; }
        }
        /// <summary>
        /// 获取特殊文件夹
        /// </summary>
        /// <returns></returns>
        public static List<SpecialPath> GetSpecialPaths() 
        {
            List<SpecialPath> lstPath = new List<SpecialPath>();
            SpecialPath path = new SpecialPath();
            path._path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            path._summary = "个人文件夹";
            lstPath.Add(path);

            path = new SpecialPath();
            path._path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+"\\Google\\Chrome\\User Data\\Default\\Cache\\";
            path._summary = "google缓存";
            lstPath.Add(path);
            path = new SpecialPath();
            path._path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\User Data\\Default\\Cache\\";
            path._summary = "google缓存";
            lstPath.Add(path);

            return lstPath;
        }

    }
}
