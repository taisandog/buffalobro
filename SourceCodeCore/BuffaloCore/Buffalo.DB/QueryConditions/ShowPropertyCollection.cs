using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// ���Լ���
    /// </summary>
    public class ScopePropertyCollection:List<BQLParamHandle>
    {
        ScopeList _belong;
        public ScopePropertyCollection(ScopeList belong) 
        {
            _belong = belong;
        }
        /// <summary>
        /// ������Լ���
        /// </summary>
        /// <param name="prms"></param>
        public void AddPropertys(params BQLParamHandle[] prms) 
        {
            foreach (BQLParamHandle handle in prms) 
            {
                Add(handle);
            }
        }

        /// <summary>
        /// ���һ��Ԫ��
        /// </summary>
        /// <param name="item"></param>
        public new void Add(BQLParamHandle item) 
        {
            _belong.HasInner = true;
            base.Add(item);
        }
    }
}
