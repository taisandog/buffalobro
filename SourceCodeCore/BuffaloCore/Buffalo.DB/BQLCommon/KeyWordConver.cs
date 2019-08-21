using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;

namespace Buffalo.DB.BQLCommon
{
    /// <summary>
    /// �ؼ���ת����
    /// </summary>
    internal class KeyWordConver
    {
        private Stack<BQLQuery> stkKeyWord = new Stack<BQLQuery>();
        
        /// <summary>
        /// ��ʼת��
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal AbsCondition ToConver(BQLQuery item, KeyWordInfomation info) 
        {
            
            CollectItem(item, info);
            return DoConver(info);
        }

        /// <summary>
        /// �����ؼ������������Ž��ؼ���ջ(�ѵ��õĹؼ�����ת����)
        /// </summary>
        /// <param name="item"></param>
        internal void CollectItem(BQLQuery item, KeyWordInfomation info) 
        {
            BQLQuery curItem = item;

            while (curItem != null) 
            {
                curItem.LoadInfo(info);
                stkKeyWord.Push(curItem);
                curItem = curItem.Previous;
            }
        }

        
        ///// <summary>
        ///// ����From�ı��������Ϣ
        ///// </summary>
        ///// <param name="item"></param>
        ///// <param name="info"></param>
        //private void LoadParamInfo(BQLKeyWordItem item, KeyWordInfomation info)
        //{
        //    KeyWordSelectItem sitem = item as KeyWordSelectItem;
        //    if (sitem!=null)
        //    {
        //        sitem.LoadParamInfo(info);
        //    }
        //}
        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        private AbsCondition DoConver(KeyWordInfomation info) 
        {

            
            StringBuilder ret = new StringBuilder(2000);
            while (stkKeyWord.Count > 0) 
            {
                BQLQuery item = stkKeyWord.Pop();
                //LoadParamInfo(item, info);
                //ret.Append(item.Tran(info)) ;
                item.Tran(info);
            }
            return info.Condition;
        }

    }
}
