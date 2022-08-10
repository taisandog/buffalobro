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
        /// 资源信息
        /// </summary>
        public IMAGE_RESOURCE_DIRECTORY ResourceDirectory
        {
            get { return resourceDirectory; }
            set { resourceDirectory = value; }
        }

        private List<ResourceDirectoryEntryInfo> lstResourceInfo;

        /// <summary>
        /// 资源信息集合
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

            int count = resourceDirectory.NumberOfNamedEntries + resourceDirectory.NumberOfIdEntries;//资源总数
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
        /// 加载信息
        /// </summary>
        /// <param name="deep">层深</param>
        /// <param name="baseOffest">基地址</param>
        internal void LoadInfo()
        {
            foreach (ResourceDirectoryEntryInfo resourceDirectorEntry in lstResourceInfo)
            {
                if (deep == 1)//如果是第二层时候 
                {

                    uint res = resourceDirectorEntry.ResourceDirectorEntry.Name & highest;//进行and运算
                    if (res == highest) //如果最高位等于1时候，资源指向ImageResourceDirStringU
                    {

                        uint offest = resourceDirectorEntry.ResourceDirectorEntry.Name & low;//获取偏移(低16位)
                        pe.BaseStream.Position = offest + baseOffest;
                        resourceDirectorEntry.ResourceDirString = new IMAGE_RESOURCE_DIR_STRING_U(pe);
                    }

                }

                //判断是否还有下一层目录
                uint resOffest = resourceDirectorEntry.ResourceDirectorEntry.OffsetToData & highest;//进行and运算
                uint nextOffest = resourceDirectorEntry.ResourceDirectorEntry.OffsetToData & low;//获取偏移
                pe.BaseStream.Position = nextOffest + baseOffest;
                if (resOffest == highest) //最高位等于1，指向的是下一层目录
                {

                    resourceDirectorEntry.NextDirectory = new ImageResourceInfo(pe, baseOffest, deep + 1);//下一级目录
                    resourceDirectorEntry.NextDirectory.LoadInfo();
                }
                else//如果最高位是0，则指向 ImageResourceDataEntry
                {
                    resourceDirectorEntry.ValueData = RawDeserialize<IMAGE_RESOURCE_DATA_ENTRY>(pe.BaseStream);
                }
            }
        }
        /// <summary>
        /// 反序列化结构体
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
        /// 反序列化结构体
        /// </summary>
        /// <param name="rawdatas"></param>
        /// <returns></returns>
        public static T RawDeserialize<T>(byte[] rawdatas)
        {
            object obj = RawDeserialize(rawdatas, typeof(T));
            return (T)obj;
        }

        /// <summary>
        /// 从流中读出元素
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
        /// 从流中读出元素
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns></returns>
        public static T RawDeserialize<T>(Stream stm)
        {


            return (T)RawDeserialize(stm, typeof(T));
        }

        /// <summary>
        /// 对象序列化成字节数组
        /// </summary>
        /// <param name="obj">对象</param>
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
