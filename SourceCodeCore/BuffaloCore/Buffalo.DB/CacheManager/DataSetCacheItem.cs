using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 缓存的DataSet项
    /// </summary>
    public class DataSetCacheItem
    {
        private string _md5;
        /// <summary>
        /// 关联的MD5
        /// </summary>
        public string MD5
        {
            get { return _md5; }
            set { _md5 = value; }
        }

        private DataSet _data;
        /// <summary>
        /// 数据
        /// </summary>
        public DataSet Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private Dictionary<string, int> _tablesVer;
        /// <summary>
        /// 表的版本
        /// </summary>
        public Dictionary<string, int> TablesVersion
        {
            get { return _tablesVer; }
            set { _tablesVer = value; }
        }
    }
}
