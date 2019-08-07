using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    /// <summary>
    /// ��ԴĿ¼
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_RESOURCE_DIRECTORY
    {
        /// <summary>
        /// ��ʶ����Դ������,��ͨ��Ϊ0
        /// </summary>
        public uint Characteristics;

        /// <summary>
        /// ��Դ����ʱ��
        /// </summary>
        public uint TimeDateStamp;

        /// <summary>
        /// �汾��
        /// </summary>
        public ushort MajorVersion;
        /// <summary>
        /// �ΰ汾��
        /// </summary>
        public ushort MinorVersion;
        /// <summary>
        /// ʹ��������Դ��Ŀ�ĸ���
        /// </summary>
        public ushort NumberOfNamedEntries;
        /// <summary>
        /// ʹ��ID������Դ��Ŀ�ĸ���
        /// </summary>
        public ushort NumberOfIdEntries;
    }
}
