using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 属性信息集合
    /// </summary>
    public class PropertyInfoCollection : IEnumerable<EntityPropertyInfo>
    {
        private Dictionary<string, EntityPropertyInfo> propertyInfoHandles;
        
        /// <summary>
        /// 属性信息集合
        /// </summary>
        /// <param name="propertyInfoHandles">属性信息的哈希表</param>
        public PropertyInfoCollection(Dictionary<string, EntityPropertyInfo> propertyInfoHandles) 
        {
            this.propertyInfoHandles = propertyInfoHandles;
        }

        /// <summary>
        /// 根据属性名获取属性信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public EntityPropertyInfo this[string propertyName] 
        {
            get
            {
                if (propertyInfoHandles != null)
                {
                    EntityPropertyInfo ret = null;
                    if (propertyInfoHandles.TryGetValue(propertyName, out ret))
                    {
                        return ret;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 当前属性的数量
        /// </summary>
        public int Count 
        {
            get 
            {
                return propertyInfoHandles.Count;
            }
        }

        #region IEnumerable<EntityPropertyInfo> 成员

        public IEnumerator<EntityPropertyInfo> GetEnumerator()
        {
            return new PropertyEnumerator(propertyInfoHandles);
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new PropertyEnumerator(propertyInfoHandles);
        }

        #endregion
    }
}
