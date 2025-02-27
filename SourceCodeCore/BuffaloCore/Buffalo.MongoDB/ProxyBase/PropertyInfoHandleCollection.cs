using Buffalo.Kernel.FastReflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB.ProxyBase
{
    /// <summary>
    /// 属性信息集合
    /// </summary>
    public class PropertyInfoHandleCollection : IEnumerable<MongoPropertyInfo>
    {
        private Dictionary<string, MongoPropertyInfo> _propertyInfoHandles;

        /// <summary>
        /// 属性信息集合
        /// </summary>
        /// <param name="propertyInfoHandles">属性信息的哈希表</param>
        public PropertyInfoHandleCollection(Dictionary<string, MongoPropertyInfo> propertyInfoHandles)
        {
            _propertyInfoHandles = propertyInfoHandles;
        }

        /// <summary>
        /// 根据属性名获取属性信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public MongoPropertyInfo this[string propertyName]
        {
            get
            {
                if (_propertyInfoHandles != null)
                {
                    MongoPropertyInfo ret = null;
                    if (_propertyInfoHandles.TryGetValue(propertyName, out ret))
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
                return _propertyInfoHandles.Count;
            }
        }

        #region IEnumerable<PropertyInfoHandle> 成员

        public IEnumerator<MongoPropertyInfo> GetEnumerator()
        {
            foreach (KeyValuePair<string, MongoPropertyInfo> kvp in _propertyInfoHandles)
            {
                yield return kvp.Value;
            }
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (KeyValuePair<string, MongoPropertyInfo> kvp in _propertyInfoHandles)
            {
                yield return kvp.Value;
            }
        }

        #endregion
    }
}
