using System;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    public class ClassPropertyInfoCollection : IEnumerable<PropertyInfoHandle>
    {
        private Dictionary<string, PropertyInfoHandle> _propertyInfoHandles;
        /// <summary>
        /// ������Ϣ����
        /// </summary>
        /// <param name="propertyInfoHandles">������Ϣ�Ĺ�ϣ��</param>
        public ClassPropertyInfoCollection(Dictionary<string, PropertyInfoHandle> propertyInfoHandles) 
        {
            this._propertyInfoHandles = propertyInfoHandles;

        }

        /// <summary>
        /// ������������ȡ������Ϣ
        /// </summary>
        /// <param name="propertyName">������</param>
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
        /// ��ǰ���Ե�����
        /// </summary>
        public int Count
        {
            get
            {
                return _propertyInfoHandles.Count;
            }
        }


        #region IEnumerable<PropertyInfoHandle> ��Ա

        public IEnumerator<PropertyInfoHandle> GetEnumerator()
        {
            return new ClassPropertyEnumerator(_propertyInfoHandles);
        }

        #endregion

        #region IEnumerable ��Ա

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ClassPropertyEnumerator(_propertyInfoHandles);
        }

        #endregion
    }
}
