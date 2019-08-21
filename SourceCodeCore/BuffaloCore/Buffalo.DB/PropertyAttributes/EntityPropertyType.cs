using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// ʵ�����������
    /// </summary>
    public enum EntityPropertyType:uint
    {
        /// <summary>
        /// ��ͨ����
        /// </summary>
        Normal=1,
        /// <summary>
        /// ��������
        /// </summary>
        PrimaryKey=2,
        /// <summary>
        /// �汾������
        /// </summary>
        Version=4,
        /// <summary>
        /// �Զ������ֶ�
        /// </summary>
        Identity=8
    }
}
