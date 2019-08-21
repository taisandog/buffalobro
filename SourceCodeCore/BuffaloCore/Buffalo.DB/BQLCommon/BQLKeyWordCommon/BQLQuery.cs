using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// BQL查询
    /// </summary>
    public abstract class BQLQuery
    {
        private BQLQuery previous;
        /// <summary>
        /// 关键字项
        /// </summary>
        /// <param name="keyWordName">关键字名</param>
        /// <param name="previous">上一个关键字</param>
        internal BQLQuery( BQLQuery previous) 
        {
            this.previous = previous;
        }
       
        /// <summary>
        /// 上一个关键字
        /// </summary>
        internal BQLQuery Previous
        {
            get
            {
                return previous;
            }
        }
        /// <summary>
        /// 关键字解释
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal abstract void Tran(KeyWordInfomation info);
        internal abstract void LoadInfo(KeyWordInfomation info);
        /// <summary>
        /// 给这个查询定义一个别名
        /// </summary>
        /// <param name="asName">别名(如果不想要别名这里为null或"")</param>
        /// <returns></returns>
        public BQLAliasHandle AS(string asName) 
        {
            BQLAliasHandle item = new BQLAliasHandle(this, asName);
            return item;
        }
    }
}
