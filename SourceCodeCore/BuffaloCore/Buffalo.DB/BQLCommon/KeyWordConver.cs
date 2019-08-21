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
    /// 关键字转换器
    /// </summary>
    internal class KeyWordConver
    {
        private Stack<BQLQuery> stkKeyWord = new Stack<BQLQuery>();
        
        /// <summary>
        /// 开始转换
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal AbsCondition ToConver(BQLQuery item, KeyWordInfomation info) 
        {
            
            CollectItem(item, info);
            return DoConver(info);
        }

        /// <summary>
        /// 遍历关键字链，把它放进关键字栈(把倒置的关键字项转回来)
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
        ///// 加载From的别名表的信息
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
        /// 开始分析
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
