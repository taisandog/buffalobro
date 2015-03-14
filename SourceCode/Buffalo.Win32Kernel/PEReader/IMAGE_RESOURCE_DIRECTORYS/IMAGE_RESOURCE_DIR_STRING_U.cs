using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_RESOURCE_DIR_STRING_U
    {
        /// <summary>
        /// ×Ö·û´®³¤¶È
        /// </summary>
        public ushort Length;
        /// <summary>
        /// unicode×Ö·û´®
        /// </summary>
        public string NameString;
        public IMAGE_RESOURCE_DIR_STRING_U(PeHeaderReader pe)
        {
            Length = pe.PeReader.ReadUInt16();

            byte[] arrName = pe.PeReader.ReadBytes(Length * 2);
            NameString = System.Text.Encoding.Unicode.GetString(arrName);
        }
    }
}
