using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    /// <summary>
    /// 资源信息
    /// </summary>
    public class ResourceEntry
    {

        ResourceItem baseItem;

        public ResourceEntry(ResourceItem baseItem) 
        {
            this.baseItem = baseItem;
        }

        string name;

        /// <summary>
        /// 资源名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        IMAGE_RESOURCE_DATA_ENTRY resourceInfo;

        /// <summary>
        /// 资源的指向信息
        /// </summary>
        public IMAGE_RESOURCE_DATA_ENTRY ResourceInfo
        {
            get { return resourceInfo; }
            set { resourceInfo = value; }
        }
        static readonly byte[] arrHead ={ 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x20, 0x20, 0x10, 0x00, 0x01, 0x00, 0x04, 0x00, 0xE8, 0x02, 0x00, 0x00, 0x16, 0x00, 0x00, 0x00 };
        public void SetData(byte[] data,Stream stm) 
        {
            stm.Position = resourceInfo.OffsetToData;
            stm.Write(data, 0, (int)resourceInfo.Size);
        }

        public void SetIconData(byte[] data, Stream stm)
        {
            int starIndex = (int)data.Length - (int)resourceInfo.Size;
            stm.Position = resourceInfo.OffsetToData;
            stm.Write(data, starIndex, (int)resourceInfo.Size);
        }
        public byte[] GetIconData(PeHeaderReader pe) 
        {
            pe.BaseStream.Position = pe.RVAToFileOffest(resourceInfo.OffsetToData);
            uint length = resourceInfo.Size;
            int star = 0;
            if (baseItem.Type == ResourceType.Icon)
            {
                length += (uint)arrHead.Length;
                star += arrHead.Length;
            }
            byte[] buffer = new byte[length];

            if (baseItem.Type == ResourceType.Icon)
            {
                Array.Copy(arrHead, buffer, arrHead.Length);
            }
            pe.BaseStream.Read(buffer, star, (int)resourceInfo.Size);
            return buffer;
        }

        public byte[] GetData(PeHeaderReader pe) 
        {
            pe.BaseStream.Position = pe.RVAToFileOffest(resourceInfo.OffsetToData);
            uint length = resourceInfo.Size;
            byte[] buffer = new byte[length];
            pe.BaseStream.Read(buffer, 0, (int)resourceInfo.Size);
            return buffer;
        }
    }
}
