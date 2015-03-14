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
        /// 资源数据的RVA
        /// </summary>
        public uint OffsetToData;
        /// <summary>
        /// 资源数据长度
        /// </summary>
        public uint Size;
        /// <summary>
        /// 代码页，一般为0
        /// </summary>
        public uint CodePage;
        /// <summary>
        /// 保留字段
        /// </summary>
        public uint Reserved;
    }
}
