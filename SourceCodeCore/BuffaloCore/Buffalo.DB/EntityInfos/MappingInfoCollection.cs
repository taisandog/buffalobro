using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// ӳ����Ϣ����
    /// </summary>
    public class MappingInfoCollection : IEnumerable<EntityMappingInfo>
    {
        private Dictionary<string, EntityMappingInfo> mappingInfoHandles;

        /// <summary>
        /// ӳ����Ϣ����
        /// </summary>
        /// <param name="propertyInfoHandles">ӳ����Ϣ�Ĺ�ϣ��</param>
        public MappingInfoCollection(Dictionary<string, EntityMappingInfo> mappingInfoHandles)
        {
            this.mappingInfoHandles = mappingInfoHandles;
        }

        /// <summary>
        /// ������������ȡӳ��������Ϣ
        /// </summary>
        /// <param name="propertyName">������</param>
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
        /// ��ǰ���Ե�����
        /// </summary>
        public int Count
        {
            get
            {
                return mappingInfoHandles.Count;
            }
        }

        #region IEnumerable<EntityMappingInfo> ��Ա

        public IEnumerator<EntityMappingInfo> GetEnumerator()
        {
            return new MappingInfoEnumerator(mappingInfoHandles);
        }

        #endregion

        #region IEnumerable ��Ա

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new MappingInfoEnumerator(mappingInfoHandles);
        }

        #endregion
    }
}
