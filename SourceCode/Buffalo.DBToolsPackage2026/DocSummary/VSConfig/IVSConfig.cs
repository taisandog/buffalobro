using System;
namespace Buffalo.DBTools.DocSummary.VSConfig
{
    public interface IVSConfig
    {
        ///// <summary>
        ///// 填充色的X
        ///// </summary>
        //float MemberLineHeight { get; }
        /// <summary>
        /// 填充色的X
        /// </summary>
        float MemberMarginX { get; }
        ///// <summary>
        ///// 离顶端的距离
        ///// </summary>
        //float MemberStartMargin { get; }
        ///// <summary>
        ///// 离顶端的距离
        ///// </summary>
        //float MemberSummaryHeight { get; }


        /// <summary>
        /// 类注释
        /// </summary>
        float MemberSummaryY
        {
            get;
        }
        /// <summary>
        /// 基类注释
        /// </summary>
        float MemberExtendSummaryY
        {
            get;
        }
    }
}
