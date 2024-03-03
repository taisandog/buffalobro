using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordHavingItem : BQLQuery
    {
        private BQLCondition condition;

        /// <summary>
        /// Where�ؼ�����
        /// </summary>
        /// <param name="condition">����</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordHavingItem(BQLCondition condition, BQLQuery previous)
            : base(previous) 
        {
            this.condition = condition;
        }
        
        internal override void LoadInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(condition, info);
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="noWait">�����ͻ�Ƿ񲻵ȴ�</param>
        /// <returns></returns>
        public KeyWorkLockUpdateItem LockUpdate(bool noWait)
        {
            KeyWorkLockUpdateItem item = new KeyWorkLockUpdateItem(noWait, this);
            return item;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordOrderByItem OrderBy(params BQLParamHandle[] paramhandles)
        {
            KeyWordOrderByItem item = new KeyWordOrderByItem(paramhandles, this);
            return item;
        }
        ///// <summary>
        ///// ����
        ///// </summary>
        //internal BQLCondition Condition 
        //{
        //    get
        //    {
        //        return condition;
        //    }
        //}

        internal override void Tran(KeyWordInfomation info)
        {
            info.IsWhere = true;
            info.Condition.Having.Append(condition.DisplayValue(info));
            info.IsWhere = false;
        }
    }
}
