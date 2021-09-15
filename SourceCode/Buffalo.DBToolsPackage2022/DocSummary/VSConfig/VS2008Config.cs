using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.DocSummary.VSConfig
{
    /// <summary>
    /// VS2008��λ��������Ϣ
    /// </summary>
    public class VS2008Config : IVSConfig
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
                return 0.165f;
            }
        }

        /// <summary>
        /// �붥�˵ľ���
        /// </summary>
        public float MemberStartMargin 
        {
            get 
            {
                return 0.26f;
            }
        }

        /// <summary>
        /// �붥�˵ľ���
        /// </summary>
        public float MemberSummaryHeight 
        {
            get 
            {
                return  0.17f;
            }
        }

        /// <summary>
        /// ��ע��
        /// </summary>
        public float MemberSummaryY
        {
            get
            {
                return 0.20f;
            }
        }
        /// <summary>
        /// ����ע��
        /// </summary>
        public float MemberExtendSummaryY
        {
            get
            {
                return 0.35f;
            }
        }

    }
}
