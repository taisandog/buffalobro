using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 映射信息集合
    /// </summary>
    public class MappingInfoCollection : IEnumerable<EntityMappingInfo>
    {
        private Dictionary<string, EntityMappingInfo> mappingInfoHandles;

        /// <summary>
        /// 映射信息集合
        /// </summary>
        /// <param name="propertyInfoHandles">映射信息的哈希表</param>
        public MappingInfoCollection(Dictionary<string, EntityMappingInfo> mappingInfoHandles)
        {
            this.mappingInfoHandles = mappingInfoHandles;
        }

        /// <summary>
        /// 根据属性名获取映射属性信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public EntityMappingInfo this[string propertyName]
        {
            get
            {
                if (mappingInfoHandles != null)
                {
                    EntityMappingInfo ret = null;
                    if (mappingInfoHandles.TryGetValue(propertyName, out ret))
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
                return mappingInfoHandles.Count;
            }
        }

        #region IEnumerable<EntityMappingInfo> 成员

        public IEnumerator<EntityMappingInfo> GetEnumerator()
        {
            return new MappingInfoEnumerator(mappingInfoHandles);
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new MappingInfoEnumerator(mappingInfoHandles);
        }

        #endregion
    }
}
