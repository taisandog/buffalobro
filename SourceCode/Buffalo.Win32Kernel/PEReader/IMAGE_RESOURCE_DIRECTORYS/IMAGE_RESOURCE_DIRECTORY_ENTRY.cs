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
        /// 目录项名称字符串指针或ID
        /// </summary>
        public uint Name;
        /// <summary>
        /// 资源数据偏移地址或子目录偏移地址
        /// </summary>
        public uint OffsetToData;


    }
}
