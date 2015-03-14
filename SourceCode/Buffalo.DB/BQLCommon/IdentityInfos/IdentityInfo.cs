using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.BQLCommon.IdentityInfos
{
    /// <summary>
    /// 自动增长信息
    /// </summary>
    public class IdentityInfo
    {
        private EntityInfoHandle entityInfo;
        private EntityPropertyInfo propertyInfo;

        /// <summary>
        /// 实体信息
        /// </summary>
        public EntityInfoHandle EntityInfo 
        {
            get 
            {
                return entityInfo;
            }
        }

        /// <summary>
        /// 对应的属性信息
        /// </summary>
        public EntityPropertyInfo PropertyInfo 
        {
            get 
            {
                return propertyInfo;
            }
        }

        /// <summary>
        /// 自动增长信息
        /// </summary>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="propertyInfo">对应的属性信息</param>
        internal IdentityInfo(EntityInfoHandle entityInfo, EntityPropertyInfo propertyInfo) 
        {
            this.entityInfo = entityInfo;
            this.propertyInfo = propertyInfo;
        }
    }
}
