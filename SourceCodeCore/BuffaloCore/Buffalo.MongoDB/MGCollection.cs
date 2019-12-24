using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// 标记所属集合
    /// </summary>
    public class MGCollection:Attribute
    {
        private string _collectionName;
        /// <summary>
        /// 集合名
        /// </summary>
        public string CollectionName
        {
            get
            {
                return _collectionName;
            }
        }
        private string _dbkey;
        /// <summary>
        /// 集合名
        /// </summary>
        public string DBKey
        {
            get
            {
                return _dbkey;
            }
        }

        /// <summary>
        /// 标记实体所属集合
        /// </summary>
        /// <param name="dbKey">数据库的键</param>
        /// <param name="collectionName">集合</param>
        public MGCollection(string dbKey,string collectionName)
        {
            _dbkey = dbKey;
            _collectionName = collectionName;
        }
        
    }
}
