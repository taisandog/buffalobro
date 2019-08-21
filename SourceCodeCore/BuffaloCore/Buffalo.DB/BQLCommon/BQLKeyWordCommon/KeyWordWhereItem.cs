using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordWhereItem:BQLQuery
    {
        private BQLCondition _condition;

        /// <summary>
        /// Where�ؼ�����
        /// </summary>
        /// <param name="condition">����</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordWhereItem(BQLCondition condition, BQLQuery previous)
            : base(previous) 
        {
            this._condition = condition;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(_condition, info);
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
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordGroupByItem GroupBy(params BQLParamHandle[] paramhandles)
        {
            KeyWordGroupByItem item = new KeyWordGroupByItem(paramhandles, this);
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
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="condition">����</param>
        /// <returns></returns>
        public KeyWordHavingItem Having(BQLCondition condition)
        {
            KeyWordHavingItem item = new KeyWordHavingItem(condition, this);
            return item;
        }
        ///// <summary>
        ///// ��ѯ��Χ
        ///// </summary>
        ///// <param name="star">��ʼ����</param>
        ///// <param name="totalRecord">��ʾ����</param>
        ///// <returns></returns>
        //public KeyWordLimitItem Limit(uint star, uint totalRecord)
        //{
        //    KeyWordLimitItem item = new KeyWordLimitItem(star, totalRecord, this);
        //    return item;
        //}
        internal override void Tran(KeyWordInfomation info)
        {
            info.IsWhere = true;
            string strCondition = _condition.DisplayValue(info);
            info.Condition.Condition.Append(strCondition);
            info.IsWhere = false;
        }
    }
}
