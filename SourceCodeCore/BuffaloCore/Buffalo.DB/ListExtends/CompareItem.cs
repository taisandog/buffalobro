using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;

namespace Buffalo.DB.ListExtends
{
    /// <summary>
    /// List过滤器器中的暂存类
    /// </summary>
    class CompareItem
    {
        private bool curCompare;
        private ConnectType connectType;

        /// <summary>
        /// 比较结果存放类
        /// </summary>
        /// <param name="curCompare">比较结果</param>
        /// <param name="connectType">连接条件</param>
        internal CompareItem(bool curCompare, ConnectType connectType) 
        {
            this.curCompare = curCompare;
            this.connectType = connectType;
        }


        /// <summary>
        /// 比较结果
        /// </summary>
        public bool CurCompare 
        {
            get 
            {
                return curCompare;
            }
        }

        /// <summary>
        /// 连接条件
        /// </summary>
        public ConnectType ConnectType 
        {
            get 
            {
                return connectType;
            }
        }
    }
}
