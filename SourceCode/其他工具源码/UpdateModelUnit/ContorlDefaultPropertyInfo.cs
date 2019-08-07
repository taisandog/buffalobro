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
        /// ���Ե���ֵ����
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
        /// ���Ե�Get/Setί��
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
        /// ���Ե�Ĭ��ֵ
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
