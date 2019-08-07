using Buffalo.Kernel.FastReflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// 自增长属性
    /// </summary>
    public class MongoPropertyInfo
    {
        /// <summary>
        /// 自增长属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="key">键</param>
        /// <param name="propertyHanle">属性反射</param>
        public MongoPropertyInfo(string propertyName,string key, PropertyInfoHandle propertyHandle)
        {
            _propertyName = propertyName;
            _key = key;
            _propertyHandle = propertyHandle;
        }
        /// <summary>
        /// 属性
        /// </summary>
        private string _propertyName;

        /// <summary>
        /// 属性
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
            
        }

        private string _key;
        /// <summary>
        /// 缓存键
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
        }

        private PropertyInfoHandle _propertyHandle;

        /// <summary>
        /// 属性反射
        /// </summary>
        public PropertyInfoHandle PropertyHandle
        {
            get
            {
                return _propertyHandle;
            }
        }
    }
}
