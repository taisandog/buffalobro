using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using System.IO;
using Buffalo.DBTools.HelperKernel;
using System.Data;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;

namespace Buffalo.DBTools
{
    /// <summary>
    /// 代码文件管理类
    /// </summary>
    public class CodeFileHelper
    {
        /// <summary>
        /// 默认编码
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.Default;



        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void SaveFile(string fileName, List<string> content)
        {
            SaveFile(fileName, content, null);
        }
        

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void SaveFile(string fileName, List<string> content, Encoding fileEncoding)
        {
            if (fileEncoding == null)
            {
                fileEncoding = GetFileEncoding(fileName);
            }
            BackupFile(fileName);
            using (StreamWriter writer = new StreamWriter(fileName, false, fileEncoding))
            {
                foreach (string str in content)
                {
                    writer.WriteLine(str);
                }
            }
        }

        /// <summary>
        /// 获取文件编码
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Encoding GetFileEncoding(string fileName) 
        {
            Encoding fileEncoding = DefaultEncoding;
            fileEncoding = FileEncodingInfo.GetEncodingType(fileName, false);
            if (fileEncoding == null)
            {
                fileEncoding = DefaultEncoding;
            }
            return fileEncoding;
        }

        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="fileName"></param>
        private static void BackupFile(string fileName) 
        {
            FileInfo fInfo = new FileInfo(fileName);
            if (!fInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fInfo.DirectoryName);
            }
            

            if (File.Exists(fileName))
            {
                string bakName = fileName + ".bak";
                if (File.Exists(bakName))
                {
                    File.Delete(bakName);
                }
               
                File.Move(fileName, bakName);

            }
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void SaveFile(string fileName, string content)
        {
            SaveFile(fileName, content, null);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void SaveFile(string fileName, string content, Encoding fileEncoding)
        {
            if (fileEncoding == null)
            {
                fileEncoding = GetFileEncoding(fileName);
            }
            BackupFile(fileName);
            using (StreamWriter writer = new StreamWriter(fileName, false, fileEncoding))
            {

                writer.WriteLine(content);
                
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> ReadFile(string fileName) 
        {
            return new List<string>(File.ReadAllLines(fileName, DefaultEncoding));
        }

    }
}
