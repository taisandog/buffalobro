using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLEntityHandle<T> where T:EntityBase
    {
        EntityInfoHandle entityInfo;
        public BQLEntityHandle() 
        {
            entityInfo = EntityInfoManager.GetEntityHandle(typeof(T));
        }
    }
}
