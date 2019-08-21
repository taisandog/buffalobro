using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class PrimaryKeyInfo:Dictionary<string,object>
    {
        public new object this[string key]
        {
            get 
            {
                object value = null;
                this.TryGetValue(key, out value);
                return value;
            }
            set 
            {
                base[key] = value;
            }
        }

        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="prm"></param>
        /// <returns></returns>
        public new object this[BQLEntityParamHandle prm] 
        {
            get 
            {
                return this[prm.PInfo.PropertyName];
            }
            set 
            {
                this[prm.PInfo.PropertyName] = value;
            }
        }

        public void FillScope(List<EntityPropertyInfo> lstPkInfo,ScopeList lstScope,bool throwException) 
        {
            foreach (EntityPropertyInfo info in lstPkInfo)
            {
                object value = this[info.PropertyName];
                if (value == null && throwException) 
                {
                    throw new KeyNotFoundException("��������:" + info.PropertyName + "δ��ֵ");
                }
                lstScope.AddEqual(info.PropertyName, value);
            }
        }
    }
}
