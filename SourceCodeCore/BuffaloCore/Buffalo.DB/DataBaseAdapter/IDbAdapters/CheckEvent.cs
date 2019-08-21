using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    /// <summary>
    /// ���ݿ����¼�
    /// </summary>
    public enum CheckEvent
    {
        /// <summary>
        /// ����
        /// </summary>
        TableBeginCreate,

        /// <summary>
        /// ������
        /// </summary>
        TableCreated,

        /// <summary>
        /// ����
        /// </summary>
        TablenBeginCheck,

        /// <summary>
        /// �����
        /// </summary>
        TableChecked,
        /// <summary>
        /// ��ϵ���
        /// </summary>
        RelationBeginCheck,

        /// <summary>
        /// ��ϵ����
        /// </summary>
        RelationChecked,

        /// <summary>
        /// �������
        /// </summary>
        PrimaryChecke,
    }
}
