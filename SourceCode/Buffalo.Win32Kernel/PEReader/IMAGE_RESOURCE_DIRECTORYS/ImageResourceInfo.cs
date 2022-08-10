using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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
            resourceDirectory = RawDeserialize<IMAGE_RESOURCE_DIRECTORY>(pe.BaseStream);

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
                    resourceDirectorEntry.ValueData = RawDeserialize<IMAGE_RESOURCE_DATA_ENTRY>(pe.BaseStream);
                }
            }
        }
        /// <summary>
        /// �����л��ṹ��
        /// </summary>
        /// <param name="rawdatas"></param>
        /// <returns></returns>
        public static object RawDeserialize(byte[] rawdatas, Type objType)
        {

            //Type anytype = typeof(T);

            int rawsize = Marshal.SizeOf(objType);
            object retobj = null;
            if (rawsize > rawdatas.Length)
            {
                return retobj;
            }

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            try
            {
                Marshal.Copy(rawdatas, 0, buffer, rawsize);

                retobj = Marshal.PtrToStructure(buffer, objType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return retobj;
        }
        /// <summary>
        /// �����л��ṹ��
        /// </summary>
        /// <param name="rawdatas"></param>
        /// <returns></returns>
        public static T RawDeserialize<T>(byte[] rawdatas)
        {
            object obj = RawDeserialize(rawdatas, typeof(T));
            return (T)obj;
        }

        /// <summary>
        /// �����ж���Ԫ��
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static object RawDeserialize(Stream stm, Type objType)
        {
            int rawsize = Marshal.SizeOf(objType);
            byte[] fbuffer = new byte[rawsize];
            rawsize = stm.Read(fbuffer, 0, rawsize);
            object retobj = null;

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            try
            {
                Marshal.Copy(fbuffer, 0, buffer, rawsize);

                retobj = Marshal.PtrToStructure(buffer, objType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return retobj;
        }

        /// <summary>
        /// �����ж���Ԫ��
        /// </summary>
        /// <param name="stm">��</param>
        /// <returns></returns>
        public static T RawDeserialize<T>(Stream stm)
        {


            return (T)RawDeserialize(stm, typeof(T));
        }

        /// <summary>
        /// �������л����ֽ�����
        /// </summary>
        /// <param name="obj">����</param>
        /// <returns></returns>
        public static byte[] RawSerialize(object obj)
        {

            int rawsize = Marshal.SizeOf(obj);

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);

            byte[] rawdatas = null;
            try
            {
                Marshal.StructureToPtr(obj, buffer, false);

                rawdatas = new byte[rawsize];

                Marshal.Copy(buffer, rawdatas, 0, rawsize);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return rawdatas;
        }
    }
}
