using System;
using System.Collections.Generic;
using System.Text;

namespace WebShare
{
    /// <summary>
    /// 共享信息管理
    /// </summary>
    public class ShareInfoCollection:List<ShareInfo>
    {
        public ShareInfoCollection() 
        {
            
        }

        /// <summary>
        /// 添加共享信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string AddShareInfo(string name, string path) 
        {
            if (FindItem(name)!=null) 
            {
                return "已经存在名称:" + name;
            }
            this.Add(new ShareInfo(name, path));
            return null;
        }
        /// <summary>
        /// 根据路径查找信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ShareInfo FindItemByPath(string path)
        {
            foreach (ShareInfo info in this)
            {
                if (info.Path.Equals(path, StringComparison.CurrentCultureIgnoreCase))
                {
                    return info;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据名称查找信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ShareInfo FindItem(string name) 
        {
            foreach (ShareInfo info in this) 
            {
                if (info.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) 
                {
                    return info;
                }
            }
            return null;
        }

    }
}
