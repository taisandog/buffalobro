using System;
using System.Collections.Generic;
using System.Text;

namespace WebShare
{
    /// <summary>
    /// 共享信息
    /// </summary>
    public class ShareInfo
    {
        /// <summary>
        /// 共享信息
        /// </summary>
        public ShareInfo()
        {
        }
        /// <summary>
        /// 共享信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="path">路径</param>
        public ShareInfo(string name, string path) 
        {
            _name = name;
            _path = path;
        }

        private string _name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _path;
        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}
