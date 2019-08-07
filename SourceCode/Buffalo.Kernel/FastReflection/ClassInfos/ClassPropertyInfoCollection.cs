using System;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    public class ClassPropertyInfoCollection : IEnumerable<PropertyInfoHandle>
    {
        private Dictionary<string, PropertyInfoHandle> _propertyInfoHandles;
        /// <summary>
        /// 属性信息集合
        /// </summary>
        /// <param name="propertyInfoHandles">属性信息的哈希表</param>
        public ClassPropertyInfoCollection(Dictionary<string, PropertyInfoHandle> propertyInfoHandles) 
        {
            this._propertyInfoHandles = propertyInfoHandles;

        }

        /// <summary>
        /// 根据属性名获取属性信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public PropertyInfoHandle this[string propertyName]
        {
            get
            {
                if (_propertyInfoHandles != null)
                {
                    PropertyInfoHandle ret = null;
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

        public IEnumerator<PropertyInfoHandle> GetEnumerator()
        {
            return new ClassPropertyEnumerator(_propertyInfoHandles);
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ClassPropertyEnumerator(_propertyInfoHandles);
        }

        #endregion
    }
}
