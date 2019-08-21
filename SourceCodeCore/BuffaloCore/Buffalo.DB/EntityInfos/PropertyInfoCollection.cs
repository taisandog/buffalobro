using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// ������Ϣ����
    /// </summary>
    public class PropertyInfoCollection : IEnumerable<EntityPropertyInfo>
    {
        private Dictionary<string, EntityPropertyInfo> propertyInfoHandles;
        
        /// <summary>
        /// ������Ϣ����
        /// </summary>
        /// <param name="propertyInfoHandles">������Ϣ�Ĺ�ϣ��</param>
        public PropertyInfoCollection(Dictionary<string, EntityPropertyInfo> propertyInfoHandles) 
        {
            this.propertyInfoHandles = propertyInfoHandles;
        }

        /// <summary>
        /// ������������ȡ������Ϣ
        /// </summary>
        /// <param name="propertyName">������</param>
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
        /// ��ǰ���Ե�����
        /// </summary>
        public int Count 
        {
            get 
            {
                return propertyInfoHandles.Count;
            }
        }

        #region IEnumerable<EntityPropertyInfo> ��Ա

        public IEnumerator<EntityPropertyInfo> GetEnumerator()
        {
            return new PropertyEnumerator(propertyInfoHandles);
        }

        #endregion

        #region IEnumerable ��Ա

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new PropertyEnumerator(propertyInfoHandles);
        }

        #endregion
    }
}
