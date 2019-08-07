using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;


namespace Buffalo.Kernel.ZipUnit
{
    public class SharpZipFile:IDisposable
    {
        private ZipOutputStream _zipStream;
        
        /// <summary>
        /// ѹ���ļ�
        /// </summary>
        /// <param name="baseStream">������</param>
        /// <param name="ziplevel">ѹ���ȼ�</param>
        public SharpZipFile(Stream baseStream,int ziplevel) 
        {
            _zipStream = new ZipOutputStream(baseStream);
            _zipStream.SetLevel(ziplevel);
        }

        public SharpZipFile(Stream baseStream)
            : this(baseStream, 9) 
        {

        }

        #region ѹ���ļ�

        /// <summary>
        /// ��ȡ�ļ�����
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        internal static string GetDirectoryName(string root) 
        {
            root = root.Replace("/", "\\");
            int index = root.LastIndexOf('\\');
            if (index >= 0) 
            {
                return root.Substring(0, index+1);

            }
            return "\\";
        }
        /// <summary>
        /// ��ȡ�ļ���
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        internal static string GetFileName(string root)
        {
            root = root.Replace("/", "\\");
            int index = root.LastIndexOf('\\');
            if (index >= 0)
            {
                return root.Substring(index + 1, root.Length - index - 1);

            }
            return root;
        }

        /// <summary>
        /// ���ѹ����Ŀ
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="content">����m</param>
        public void AddZipEntry(string fileName, byte[] content)
        {
            ZipEntry dicEntry = new ZipEntry(fileName);
            _zipStream.PutNextEntry(dicEntry);

            _zipStream.Write(content, 0, content.Length);
        }

        /// <summary>
        /// ���ѹ����Ŀ
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="stm">����</param>
        public void AddZipEntry(string fileName, Stream stm)
        {
            byte[] buffer = CommonMethods.LoadStreamData2(stm);
            AddZipEntry(fileName, buffer);
        }

        /// <summary>
        /// ���ѹ����Ŀ
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="filePath">�ļ�·��</param>
        public void AddZipEntry(string fileName, string filePath)
        {
            using (Stream stm=File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                AddZipEntry(fileName, stm);
            }
        }
        #endregion

        #region IDisposable ��Ա

        public void Dispose()
        {
            _zipStream.Finish();
            _zipStream.Close();
        }

        #endregion
    }
}

//*******************����**************
//using (FileStream file = new FileStream("D:\\aa.zip", FileMode.Create, FileAccess.Write))
//{
//    using (SharpZipFile zip = new SharpZipFile(file))
//    {
//        zip.AddZipEntry("Kind.png", "D:\\Kind.png");
//        zip.AddZipEntry("NO.png", "D:\\NO.png");
//    }
//}