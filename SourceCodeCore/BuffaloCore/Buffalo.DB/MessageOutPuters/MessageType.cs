using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// ��Ϣ����
    /// </summary>
    public enum MessageType
    {
        [Description("��ѯ���ݿ�")]
        Query=1,
        [Description("ִ�����")]
        Execute=2,
        [Description("��������")]
        OtherOper=3,
        [Description("��ѯ����")]
        QueryCache=4,
        [Description("����������쳣")]
        CacheException=4,
        [Description("���ݿ�����쳣")]
        DataBaseException=5,
    }
}
