using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemcacheClient;
using Buffalo.DB.EntityInfos;

namespace MemcacheClient
{
    /// <summary>
    /// 内存流属性信息
    /// </summary>
    public class MemPropertyInfo
    {
        private MemTypeItem _memItem;

        /// <summary>
        /// 读写器
        /// </summary>
        public MemTypeItem MemItem
        {
            get { return _memItem; }
            set { _memItem = value; }
        }

        private EntityPropertyInfo _propertyInfo;

        /// <summary>
        /// 属性信息
        /// </summary>
        public EntityPropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
            set { _propertyInfo = value; }
        }
    }
}
