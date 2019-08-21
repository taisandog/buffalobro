using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.BQLCommon.IdentityInfos
{
    /// <summary>
    /// �Զ�������Ϣ
    /// </summary>
    public class IdentityInfo
    {
        private EntityInfoHandle entityInfo;
        private EntityPropertyInfo propertyInfo;

        /// <summary>
        /// ʵ����Ϣ
        /// </summary>
        public EntityInfoHandle EntityInfo 
        {
            get 
            {
                return entityInfo;
            }
        }

        /// <summary>
        /// ��Ӧ��������Ϣ
        /// </summary>
        public EntityPropertyInfo PropertyInfo 
        {
            get 
            {
                return propertyInfo;
            }
        }

        /// <summary>
        /// �Զ�������Ϣ
        /// </summary>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        /// <param name="propertyInfo">��Ӧ��������Ϣ</param>
        internal IdentityInfo(EntityInfoHandle entityInfo, EntityPropertyInfo propertyInfo) 
        {
            this.entityInfo = entityInfo;
            this.propertyInfo = propertyInfo;
        }
    }
}
