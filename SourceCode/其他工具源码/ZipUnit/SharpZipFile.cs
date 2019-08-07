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
        /// 压缩文件
        /// </summary>
        /// <param name="baseStream">基础流</param>
        /// <param name="ziplevel">压缩等级</param>
        public SharpZipFile(Stream baseStream,int ziplevel) 
        {
            _zipStream = new ZipOutputStream(baseStream);
            _zipStream.SetLevel(ziplevel);
        }

        public SharpZipFile(Stream baseStream)
            : this(baseStream, 9) 
        {

        }

        #region 压缩文件

        /// <summary>
        /// 获取文件夹名
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
        /// 获取文件名
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
        /// 添加压缩项目
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容m</param>
        public void AddZipEntry(string fileName, byte[] content)
        {
            ZipEntry dicEntry = new ZipEntry(fileName);
            _zipStream.PutNextEntry(dicEntry);

            _zipStream.Write(content, 0, content.Length);
        }
		/// <summary>
        /// 添加文件夹
        /// </summary>
        /// <param name="fileName">文件夹</param>
        public void AddDir(string fileName)
        {
            ZipEntry dicEntry = new ZipEntry(fileName);
            
            _zipStream.PutNextEntry(dicEntry);

            
        }
        /// <summary>
        /// 添加压缩项目
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="stm">内容</param>
        public void AddZipEntry(string fileName, Stream stm)
        {
            byte[] buffer = CommonMethods.LoadStreamData2(stm);
            AddZipEntry(fileName, buffer);
        }

        /// <summary>
        /// 添加压缩项目
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件路径</param>
        public void AddZipEntry(string fileName, string filePath)
        {
            using (Stream stm=File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                AddZipEntry(fileName, stm);
            }
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            _zipStream.Finish();
            _zipStream.Close();
        }

        #endregion
    }
}

//*******************例子**************
//using (FileStream file = new FileStream("D:\\aa.zip", FileMode.Create, FileAccess.Write))
//{
//    using (SharpZipFile zip = new SharpZipFile(file))
//    {
//        zip.AddZipEntry("Kind.png", "D:\\Kind.png");
//        zip.AddZipEntry("NO.png", "D:\\NO.png");
//    }
//}