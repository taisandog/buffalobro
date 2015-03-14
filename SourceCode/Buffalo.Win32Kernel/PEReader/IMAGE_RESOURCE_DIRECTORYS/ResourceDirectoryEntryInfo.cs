using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    public class ResourceDirectoryEntryInfo
    {

        IMAGE_RESOURCE_DIRECTORY_ENTRY resourceDirectorEntry;

        /// <summary>
        /// 资源目录入口
        /// </summary>
        public IMAGE_RESOURCE_DIRECTORY_ENTRY ResourceDirectorEntry
        {
            get { return resourceDirectorEntry; }
            set { resourceDirectorEntry = value; }
        }
        private IMAGE_RESOURCE_DIR_STRING_U resourceDirString;

        /// <summary>
        /// 字符串
        /// </summary>
        public IMAGE_RESOURCE_DIR_STRING_U ResourceDirString
        {
            get { return resourceDirString; }
            set { resourceDirString = value; }
        }

        private ImageResourceInfo nextDirectory;

        /// <summary>
        /// 下一层目录
        /// </summary>
        public ImageResourceInfo NextDirectory
        {
            get { return nextDirectory; }
            set { nextDirectory = value; }
        }

        private IMAGE_RESOURCE_DATA_ENTRY valueData;

        /// <summary>
        /// 指向的资源
        /// </summary>
        public IMAGE_RESOURCE_DATA_ENTRY ValueData
        {
            get
            {
                return valueData;
            }
            set 
            {
                valueData = value;
            }
        }
        public ResourceDirectoryEntryInfo()
        {

        }
        PeHeaderReader pe;
        long baseOffest;
        public ResourceDirectoryEntryInfo(PeHeaderReader pe, long baseOffest) 
        {
            this.pe = pe;
            this.baseOffest = baseOffest;
            resourceDirectorEntry = CommonMethods.RawDeserialize<IMAGE_RESOURCE_DIRECTORY_ENTRY>(pe.BaseStream);

        }



        
    }
}
