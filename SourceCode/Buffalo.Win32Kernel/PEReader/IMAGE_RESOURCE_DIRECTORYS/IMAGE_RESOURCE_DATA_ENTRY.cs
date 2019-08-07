using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_RESOURCE_DATA_ENTRY
    {
        /// <summary>
        /// ��Դ���ݵ�RVA
        /// </summary>
        public uint OffsetToData;
        /// <summary>
        /// ��Դ���ݳ���
        /// </summary>
        public uint Size;
        /// <summary>
        /// ����ҳ��һ��Ϊ0
        /// </summary>
        public uint CodePage;
        /// <summary>
        /// �����ֶ�
        /// </summary>
        public uint Reserved;
    }
}
