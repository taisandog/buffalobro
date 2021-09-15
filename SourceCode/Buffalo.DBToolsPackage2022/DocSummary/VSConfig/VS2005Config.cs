using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.DocSummary.VSConfig
{
    /// <summary>
    /// VS2005的位置配置信息
    /// </summary>
    public class VS2005Config : IVSConfig
    {
        /// <summary>
        /// 填充色的X
        /// </summary>
        public float MemberMarginX
        {
            get
            {
                return 0.4f;
            }
        }
        /// <summary>
        /// 行高
        /// </summary>
        public float MemberLineHeight
        {
            get 
            {
                return 0.19f;
            }
        }

        /// <summary>
        /// 离顶端的距离
        /// </summary>
        public float MemberStartMargin 
        {
            get 
            {
                return 0.25f;
            }
        }

        /// <summary>
        /// 离顶端的距离
        /// </summary>
        public float MemberSummaryHeight 
        {
            get 
            {
                return 0.19f;
            }
        }


        /// <summary>
        /// 类注释
        /// </summary>
        public float MemberSummaryY
        {
            get
            {
                return 0.25f;
            }
        }
        /// <summary>
        /// 基类注释
        /// </summary>
        public float MemberExtendSummaryY
        {
            get
            {
                return 0.40f;
            }
        }
    }
}
