using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    /// <summary>
    /// 资源目录
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_RESOURCE_DIRECTORY
    {
        /// <summary>
        /// 标识此资源的类型,但通常为0
        /// </summary>
        public uint Characteristics;

        /// <summary>
        /// 资源建立时间
        /// </summary>
        public uint TimeDateStamp;

        /// <summary>
        /// 版本号
        /// </summary>
        public ushort MajorVersion;
        /// <summary>
        /// 次版本号
        /// </summary>
        public ushort MinorVersion;
        /// <summary>
        /// 使用名字资源条目的个数
        /// </summary>
        public ushort NumberOfNamedEntries;
        /// <summary>
        /// 使用ID数字资源条目的个数
        /// </summary>
        public ushort NumberOfIdEntries;
    }
}
