using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace Buffalo.DBTools.DocSummary
{
    /// <summary>
    /// ע����ʾ������
    /// </summary>
    public enum SummaryShowItem:int
    {
        /// <summary>
        /// ��������
        /// </summary>
        [Description("��������")]
        TypeName=1,
        /// <summary>
        /// ������
        /// </summary>
        [Description("������")]
        MemberName=2,
        /// <summary>
        /// ע��
        /// </summary>
        [Description("ע��")]
        Summary=4,
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        All=TypeName|MemberName|Summary
    }
}
