using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_RESOURCE_DIRECTORY_ENTRY
    {
        /// <summary>
        /// Ŀ¼�������ַ���ָ���ID
        /// </summary>
        public uint Name;
        /// <summary>
        /// ��Դ����ƫ�Ƶ�ַ����Ŀ¼ƫ�Ƶ�ַ
        /// </summary>
        public uint OffsetToData;


    }
}
