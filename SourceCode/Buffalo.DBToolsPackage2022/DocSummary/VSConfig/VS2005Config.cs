using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.DocSummary.VSConfig
{
    /// <summary>
    /// VS2005��λ��������Ϣ
    /// </summary>
    public class VS2005Config : IVSConfig
    {
        /// <summary>
        /// ���ɫ��X
        /// </summary>
        public float MemberMarginX
        {
            get
            {
                return 0.4f;
            }
        }
        /// <summary>
        /// �и�
        /// </summary>
        public float MemberLineHeight
        {
            get 
            {
                return 0.19f;
            }
        }

        /// <summary>
        /// �붥�˵ľ���
        /// </summary>
        public float MemberStartMargin 
        {
            get 
            {
                return 0.25f;
            }
        }

        /// <summary>
        /// �붥�˵ľ���
        /// </summary>
        public float MemberSummaryHeight 
        {
            get 
            {
                return 0.19f;
            }
        }


        /// <summary>
        /// ��ע��
        /// </summary>
        public float MemberSummaryY
        {
            get
            {
                return 0.25f;
            }
        }
        /// <summary>
        /// ����ע��
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
