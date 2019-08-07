using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    /// <summary>
    /// ��Դ����
    /// </summary>
    public enum ResourceType : uint
    {
        /// <summary>
        /// ����Դ
        /// </summary>
        Unknow = 0x00,
        /// <summary>
        /// ���
        /// </summary>
        Cursor = 0x01,
        /// <summary>
        /// λͼ
        /// </summary>
        Bitmap = 0x02,
        /// <summary>
        /// ͼ��
        /// </summary>
        Icon = 0x03,
        /// <summary>
        /// �˵�
        /// </summary>
        Menu = 0x04,
        /// <summary>
        /// �Ի���
        /// </summary>
        Dialog = 0x05,
        /// <summary>
        /// �ַ���
        /// </summary>
        String = 0x06,
        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        FontDirectory = 0x07,
        /// <summary>
        /// ����
        /// </summary>
        Font = 0x08,
        /// <summary>
        /// ���ټ�
        /// </summary>
        Accelerators = 0x09,
        /// <summary>
        /// δ��ʽ��Դ
        /// </summary>
        Unformatted = 0x0A,
        /// <summary>
        /// ��Ϣ��
        /// </summary>
        MessageTable = 0x0B,
        /// <summary>
        /// �����
        /// </summary>
        GroupCursor = 0x0C,
        /// <summary>
        /// ͼ����
        /// </summary>
        GroupIcon = 0x0E,
        /// <summary>
        /// �汾��Ϣ
        /// </summary>
        VersionInformation = 0x10
    }
}
