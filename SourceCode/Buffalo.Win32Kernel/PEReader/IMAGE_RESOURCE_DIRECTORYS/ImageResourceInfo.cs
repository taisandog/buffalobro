using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    public class ImageResourceInfo
    {
        PeHeaderReader pe;
        uint deep;
        long baseOffest;
        private IMAGE_RESOURCE_DIRECTORY resourceDirectory;

        /// <summary>
        /// ��Դ��Ϣ
        /// </summary>
        public IMAGE_RESOURCE_DIRECTORY ResourceDirectory
        {
            get { return resourceDirectory; }
            set { resourceDirectory = value; }
        }

        private List<ResourceDirectoryEntryInfo> lstResourceInfo;

        /// <summary>
        /// ��Դ��Ϣ����
        /// </summary>
        public List<ResourceDirectoryEntryInfo> LstResourceInfo
        {
            get { return lstResourceInfo; }
            set { lstResourceInfo = value; }
        }

        public ImageResourceInfo(PeHeaderReader pe, long baseOffest, uint deep)
        {
            this.deep = deep;
            this.baseOffest = baseOffest;
            this.pe = pe;
            resourceDirectory = CommonMethods.RawDeserialize<IMAGE_RESOURCE_DIRECTORY>(pe.BaseStream);

            int count = resourceDirectory.NumberOfNamedEntries + resourceDirectory.NumberOfIdEntries;//��Դ����
            lstResourceInfo = new List<ResourceDirectoryEntryInfo>(count);
            for (int i = 0; i < count; i++)
            {
                ResourceDirectoryEntryInfo info = new ResourceDirectoryEntryInfo(pe, baseOffest);
                lstResourceInfo.Add(info);
            }
            
        }

        const uint highest = 0x80000000;//10000000000000000000000000000000
        const uint low = 0x0000FFFF;//00000000000000001111111111111111

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="deep">����</param>
        /// <param name="baseOffest">����ַ</param>
        internal void LoadInfo()
        {
            foreach (ResourceDirectoryEntryInfo resourceDirectorEntry in lstResourceInfo)
            {
                if (deep == 1)//����ǵڶ���ʱ�� 
                {

                    uint res = resourceDirectorEntry.ResourceDirectorEntry.Name & highest;//����and����
                    if (res == highest) //������λ����1ʱ����Դָ��ImageResourceDirStringU
                    {

                        uint offest = resourceDirectorEntry.ResourceDirectorEntry.Name & low;//��ȡƫ��(��16λ)
                        pe.BaseStream.Position = offest + baseOffest;
                        resourceDirectorEntry.ResourceDirString = new IMAGE_RESOURCE_DIR_STRING_U(pe);
                    }

                }

                //�ж��Ƿ�����һ��Ŀ¼
                uint resOffest = resourceDirectorEntry.ResourceDirectorEntry.OffsetToData & highest;//����and����
                uint nextOffest = resourceDirectorEntry.ResourceDirectorEntry.OffsetToData & low;//��ȡƫ��
                pe.BaseStream.Position = nextOffest + baseOffest;
                if (resOffest == highest) //���λ����1��ָ�������һ��Ŀ¼
                {

                    resourceDirectorEntry.NextDirectory = new ImageResourceInfo(pe, baseOffest, deep + 1);//��һ��Ŀ¼
                    resourceDirectorEntry.NextDirectory.LoadInfo();
                }
                else//������λ��0����ָ�� ImageResourceDataEntry
                {
                    resourceDirectorEntry.ValueData = CommonMethods.RawDeserialize<IMAGE_RESOURCE_DATA_ENTRY>(pe.BaseStream);
                }
            }
        }

    }
}
