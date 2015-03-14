using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;


namespace Buffalo.Kernel.UpdateModelUnit
{
    public class ContorlDefaultPropertyInfo
    {
        private Type propertyType;
        private PropertyInfoHandle propertyHandle;
        private object defauleValue;
        /// <summary>
        /// 属性的数值类型
        /// </summary>
        public Type PropertyType 
        {
            get 
            {
                return propertyType;
            }
            set 
            {
                propertyType = value;
            }
        }

        /// <summary>
        /// 属性的Get/Set委托
        /// </summary>
        public PropertyInfoHandle PropertyHandle
        {
            get
            {
                return propertyHandle;
            }
            set
            {
                propertyHandle = value;
            }
        }

        /// <summary>
        /// 属性的默认值
        /// </summary>
        public object DefaultValue 
        {
            get
            {
                return defauleValue;
            }
            set
            {
                defauleValue = value;
            }
        }
    }
}
