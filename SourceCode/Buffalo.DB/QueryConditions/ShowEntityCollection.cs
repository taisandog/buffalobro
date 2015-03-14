using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.QueryConditions
{
    public class ShowEntityCollection : List<BQLEntityTableHandle>
    {
        private ScopeList _belongList;//所属的集合

        /// <summary>
        /// 要现实的实体集合
        /// </summary>
        /// <param name="belong"></param>
        internal ShowEntityCollection(ScopeList belong) 
        {
            _belongList = belong;
        }

        /// <summary>
        /// 添加一个项
        /// </summary>
        /// <param name="prm"></param>
        public new void Add(BQLEntityTableHandle prm)
        {
            if (_belongList != null)
            {
                _belongList.HasInner = true;
            }
            base.Add(prm);
        }

        /// <summary>
        /// 添加一个输出字段
        /// </summary>
        /// <param name="prm"></param>
        public void AddRange(params BQLEntityTableHandle[] prms)
        {
            if (_belongList != null)
            {
                _belongList.HasInner = true;
            }
            base.AddRange(prms);
        }
    }
}
