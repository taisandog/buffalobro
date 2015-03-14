using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS;

namespace Buffalo.Win32Kernel.PEReader
{
    public class PeHeaderReader:IDisposable
    {

        /// <summary>
        /// The DOS header
        /// </summary>
        private IMAGE_DOS_HEADER dosHeader;
        /// <summary>
        /// The file header
        /// </summary>
        private IMAGE_FILE_HEADER fileHeader;
        /// <summary>
        /// Optional 32 bit file header 
        /// </summary>
        private IMAGE_OPTIONAL_HEADER32 optionalHeader32;
        /// <summary>
        /// Optional 64 bit file header 
        /// </summary>
        private IMAGE_OPTIONAL_HEADER64 optionalHeader64;
        /// <summary>
        /// Image Section headers. Number of sections is in the file header.
        /// </summary>
        private IMAGE_SECTION_HEADER[] imageSectionHeaders;

        Stream _stm;
        /// <summary>
        /// 文件流
        /// </summary>
        internal Stream BaseStream
        {
            get
            {
                return _stm;
            }
        }

        private BinaryReader _reader;
        /// <summary>
        /// PE读取器
        /// </summary>
        public BinaryReader PeReader
        {
            get
            {
                return _reader;
            }

        }
        /// <summary>
        /// 把此位置的表加载成资源表
        /// </summary>
        public ImageResourceManager LoadToResourceDirector(IMAGE_DATA_DIRECTORY resourceTable)
        {
            if (resourceTable.VirtualAddress <= 0)
            {
                return null;
            }
            long baseOffest = RVAToFileOffest(resourceTable.VirtualAddress);
            BaseStream.Position = baseOffest;
            return new ImageResourceManager(this, baseOffest);
        }

        /// <summary>
        /// 虚拟偏移转换成文件相对位置
        /// </summary>
        /// <param name="rav">内存相对地址</param>
        /// <returns></returns>
        public long RVAToFileOffest(long rav)
        {
            long imageBase = 0;
            if (this.Is32BitHeader)
            {
                imageBase=optionalHeader32.ImageBase;
            }
            else
            {
                imageBase = (long)optionalHeader64.ImageBase;
            }
            //uint offsetRav = rav - imageBase;
            //IMAGE_SECTION_HEADER belongHeader = null;
            //找出所属的区块

            IMAGE_SECTION_HEADER sectionHead = FindSectionHeader(rav);

            if (!sectionHead.IsNull)
            {

                uint k = sectionHead.VirtualAddress - sectionHead.PointerToRawData;
                return rav - k;
            }
            return 0;
        }
        /// <summary>
        /// 找出当前虚拟偏移所在的区段
        /// </summary>
        /// <returns></returns>
        public IMAGE_SECTION_HEADER FindSectionHeader(long virtualAddress)
        {
            //IMAGE_SECTION_HEADER header;
            for (int i = 0; i < imageSectionHeaders.Length; i++)
            {

                if (i >= imageSectionHeaders.Length - 1)
                {
                    return imageSectionHeaders[i];
                }
                else if (imageSectionHeaders[i].VirtualAddress <= virtualAddress && imageSectionHeaders[i + 1].VirtualAddress > virtualAddress)
                {
                    return imageSectionHeaders[i];
                }
            }
            return new IMAGE_SECTION_HEADER();
        }

        /// <summary>
        /// PE读取器
        /// </summary>
        /// <param name="filePath"></param>
        public PeHeaderReader(Stream stm)
        {
            // Read in the DLL or EXE and get the timestamp
            _stm = stm;
            _reader = new BinaryReader(stm);
            dosHeader = FromBinaryReader<IMAGE_DOS_HEADER>(_reader);

                // Add 4 bytes to the offset
            stm.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);

                UInt32 ntHeadersSignature = _reader.ReadUInt32();
                fileHeader = FromBinaryReader<IMAGE_FILE_HEADER>(_reader);
                if (this.Is32BitHeader)
                {
                    optionalHeader32 = FromBinaryReader<IMAGE_OPTIONAL_HEADER32>(_reader);
                }
                else
                {
                    optionalHeader64 = FromBinaryReader<IMAGE_OPTIONAL_HEADER64>(_reader);
                }

                imageSectionHeaders = new IMAGE_SECTION_HEADER[fileHeader.NumberOfSections];
                for (int headerNo = 0; headerNo < imageSectionHeaders.Length; ++headerNo)
                {
                    imageSectionHeaders[headerNo] = FromBinaryReader<IMAGE_SECTION_HEADER>(_reader);
                }

            
        }

        ///// <summary>
        ///// Gets the header of the .NET assembly that called this function
        ///// </summary>
        ///// <returns></returns>
        //public static PeHeaderReader GetCallingAssemblyHeader()
        //{
        //    // Get the path to the calling assembly, which is the path to the
        //    // DLL or EXE that we want the time of
        //    string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;

        //    // Get and return the timestamp

        //    return new PeHeaderReader(filePath);
        //}

        ///// <summary>
        ///// Gets the header of the .NET assembly that called this function
        ///// </summary>
        ///// <returns></returns>
        //public static PeHeaderReader GetAssemblyHeader()
        //{
        //    // Get the path to the calling assembly, which is the path to the
        //    // DLL or EXE that we want the time of
        //    string filePath = System.Reflection.Assembly.GetAssembly(typeof(PeHeaderReader)).Location;

        //    // Get and return the timestamp
        //    return new PeHeaderReader(filePath);
        //}

        /// <summary>
        /// Reads in a block from a file and converts it to the struct
        /// type specified by the template parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T FromBinaryReader<T>(BinaryReader reader)
        {
            // Read in a byte array
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

            // Pin the managed memory while, copy it out the data, then unpin it
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }


        #region Properties

        /// <summary>
        /// Gets if the file header is 32 bit or not
        /// </summary>
        public bool Is32BitHeader
        {
            get
            {
                UInt16 IMAGE_FILE_32BIT_MACHINE = 0x0100;
                return (IMAGE_FILE_32BIT_MACHINE & FileHeader.Characteristics) == IMAGE_FILE_32BIT_MACHINE;
            }
        }

        /// <summary>
        /// Gets the file header
        /// </summary>
        public IMAGE_FILE_HEADER FileHeader
        {
            get
            {
                return fileHeader;
            }
        }

        /// <summary>
        /// Gets the optional header
        /// </summary>
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader32
        {
            get
            {
                return optionalHeader32;
            }
        }

        /// <summary>
        /// Gets the optional header
        /// </summary>
        public IMAGE_OPTIONAL_HEADER64 OptionalHeader64
        {
            get
            {
                return optionalHeader64;
            }
        }

        public IMAGE_SECTION_HEADER[] ImageSectionHeaders
        {
            get
            {
                return imageSectionHeaders;
            }
        }

        /// <summary>
        /// Gets the timestamp from the file header
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                // Timestamp is a date offset from 1970
                DateTime returnValue = new DateTime(1970, 1, 1, 0, 0, 0);

                // Add in the number of seconds since 1970/1/1
                returnValue = returnValue.AddSeconds(fileHeader.TimeDateStamp);
                // Adjust to local timezone
                returnValue += TimeZone.CurrentTimeZone.GetUtcOffset(returnValue);

                return returnValue;
            }
        }

        #endregion Properties

        #region IDisposable 成员

        public void Dispose()
        {
            _reader.Close();
        }

        #endregion
    }

}