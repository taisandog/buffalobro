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
        /// ��ԴĿ¼���
        /// </summary>
        public IMAGE_RESOURCE_DIRECTORY_ENTRY ResourceDirectorEntry
        {
            get { return resourceDirectorEntry; }
            set { resourceDirectorEntry = value; }
        }
        private IMAGE_RESOURCE_DIR_STRING_U resourceDirString;

        /// <summary>
        /// �ַ���
        /// </summary>
        public IMAGE_RESOURCE_DIR_STRING_U ResourceDirString
        {
            get { return resourceDirString; }
            set { resourceDirString = value; }
        }

        private ImageResourceInfo nextDirectory;

        /// <summary>
        /// ��һ��Ŀ¼
        /// </summary>
        public ImageResourceInfo NextDirectory
        {
            get { return nextDirectory; }
            set { nextDirectory = value; }
        }

        private IMAGE_RESOURCE_DATA_ENTRY valueData;

        /// <summary>
        /// ָ�����Դ
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
