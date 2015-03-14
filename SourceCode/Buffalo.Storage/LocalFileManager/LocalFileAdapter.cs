using Buffalo.Kernel.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Buffalo.Storage.LocalFileManager
{
    /// <summary>
    /// 本地文件的适配
    /// </summary>
    public class LocalFileAdapter
    {
        /// <summary>
        /// 存储根目录
        /// </summary>
        private string _fileRoot;
        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;

        /// <summary>
        /// 密码
        /// </summary>
        private string _password;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public LocalFileAdapter(string fileName, string userName, string password) 
        {
            _fileRoot = fileName;
            _userName = userName;
            _password = password;
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        /// <returns></returns>
        public void Open() 
        {
            if (string.IsNullOrEmpty(_userName)) 
            {
                return;
            }
            uint resault=FileAPI.WNetAddConnection(_userName, _password, _fileRoot, null);
            
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close() 
        {
            if (string.IsNullOrEmpty(_userName))
            {
                return;
            }
            uint resault = FileAPI.WNetCancelConnection(_fileRoot, 1, true); //取消映射
            
        }
        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <param name="searchOption">查找选项</param>
        /// <returns></returns>
        public List<string> GetFiles(string path, SearchOption searchOption) 
        {
            string sfilePath = _fileRoot + path;

            string[] files = Directory.GetFiles(sfilePath, "*.*", searchOption);
            List<string> ret = new List<string>(files.Length);
            foreach (string spath in files) 
            {
                string curPath = spath.Substring(_fileRoot.Length);
                if (curPath[0] != '\\') 
                {
                    curPath = '\\' + curPath;
                }
                ret.Add(curPath);
            }
            return ret;
        }
        /// <summary>
        /// 获取所有文件夹
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <param name="searchOption">查找选项</param>
        /// <returns></returns>
        public List<string> GetDirectories(string path, SearchOption searchOption)
        {
            string sfilePath = _fileRoot + path;
            string[] files = Directory.GetDirectories(sfilePath, "*", searchOption);
            List<string> ret = new List<string>(files.Length);
            foreach (string spath in files)
            {
                string curPath = spath.Substring(_fileRoot.Length);
                if (curPath[0] != '\\')
                {
                    curPath = '\\' + curPath;
                }
                ret.Add(curPath);
            }
            return ret;
        }

    }
}
