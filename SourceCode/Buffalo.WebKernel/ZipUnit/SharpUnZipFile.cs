using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Buffalo.Kernel.ZipUnit
{
    /// <summary>
    /// SharpZip��ѹ�ļ�
    /// </summary>
    public class SharpUnZipFile
    {
        private ZipInputStream _zipStream;

        /// <summary>
        /// SharpZip��ѹ�ļ�
        /// </summary>
        /// <param name="baseStream">��ѹ������</param>
        public SharpUnZipFile(Stream baseStream) 
        {
            _zipStream = new ZipInputStream(baseStream);
        }
        #region ��ѹ��
        /// <summary>
        /// ��ѹ��
        /// </summary>
        /// <param name="p">���ѹ�����ļ����ļ���</param>
        /// <param name="ServerDir">�����ļ��о���·��</param>
        public void UnZipFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
           
            while (true)
            {
                ZipEntry zp = _zipStream.GetNextEntry();
                if (zp == null) 
                {
                    break;
                }
                if (zp.IsDirectory || zp.Crc == 00000000L)
                {
                    continue;
                }
                
                string fileDic = SharpZipFile.GetDirectoryName(directory + zp.Name);
                if (!Directory.Exists(fileDic))
                {
                    Directory.CreateDirectory(fileDic);
                }
                using (FileStream file = new FileStream(directory + zp.Name, FileMode.Create, FileAccess.Write)) 
                {
                    UnZip(file);
                }
                
            }
            

        }

        /// <summary>
        /// ��ȡ������ļ���������
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, MemoryStream> GetContentStream()
        {
            Dictionary<string, MemoryStream> dic = new Dictionary<string, MemoryStream>();

            while (true)
            {
                ZipEntry zp = _zipStream.GetNextEntry();
                if (zp == null)
                {
                    break;
                }
                if (zp.IsDirectory || zp.Crc == 00000000L)
                {
                    continue;
                }

                //string fileDic = SharpZipFile.GetDirectoryName(directory + zp.Name);
                MemoryStream stm = new MemoryStream();
                dic[zp.Name] = stm;
                UnZip(stm);
            }
            return dic;
        }

        /// <summary>
        /// ��ѹ����
        /// </summary>
        /// <param name="stm"></param>
        private void UnZip(Stream stm) 
        {
           
            byte[] buffer=new byte[2048];
            int read = 0;
            while(true)
            {
                read = _zipStream.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                stm.Write(buffer, 0, read);
            } 

            
        }

        #endregion
    }
}



//*******************����**************
//using (FileStream file = new FileStream("D:\\Archive.zip", FileMode.Open, FileAccess.Read))
//{
//    SharpUnZipFile zip = new SharpUnZipFile(file);
//    zip.UnZipFiles("D:\\test\\");
//}