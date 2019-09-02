using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.Kernel.ZipUnit;

namespace Buffalo.Data.DB2
{
    /// <summary>
    /// ������������
    /// </summary>
    public class AdapterLoader : IAdapterLoader
    {
        /// <summary>
        /// ��ʼ��DB2
        /// </summary>
        private void InitDB2()
        {
            
            string path = this.GetType().Assembly.Location;
            FileInfo finfo = new FileInfo(path);
            string dicBase = finfo.DirectoryName.TrimEnd('\\') + "\\";
            string dic = dicBase + "clidriver\\";
            if (Directory.Exists(dic))
            {
                return;
            }
            string zipFile = dicBase + "clidriver.zip";
            if (!File.Exists(zipFile))
            {
                return;
            }
            using (FileStream file = new FileStream(zipFile, FileMode.Open, FileAccess.Read))
            {
                SharpUnZipFile zip = new SharpUnZipFile(file);
                zip.UnZipFiles(dicBase);
            }
        }
        private static object _errorLock = false;
        /// <summary>
        /// ��¼��־
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message,string baseRoot)
        {
            string dir = baseRoot;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string name = "error" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string fileName = dir + name;
            lock (_errorLock)
            {
                using (StreamWriter sw = new StreamWriter(fileName, true,System.Text.Encoding.Default))
                {
                    sw.WriteLine("====" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "=====");
                    sw.WriteLine(message);
                    sw.WriteLine("===========");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("");
                }
            }

        }
        /// <summary>
        /// ��ǰ���ݿ�������
        /// </summary>
        public IDBAdapter DbAdapter 
        {
            get 
            {
                try
                {
                    InitDB2();
                }catch(Exception ex)
                {
                    string path = this.GetType().Assembly.Location;
                    FileInfo finfo = new FileInfo(path);
                    string dicBase = finfo.DirectoryName.TrimEnd('\\') + "\\";

                    LogError(ex.ToString(), dicBase);
                }
                return new DBAdapter();
            }
        }

        public IAggregateFunctions AggregateFunctions 
        {
            get
            {
                return new AggregateFunctions();
            }
        }

        public IMathFunctions MathFunctions 
        {
            get 
            {
                return new MathFunctions();
            }
        }
        public IConvertFunction ConvertFunctions 
        {
            get 
            {
                return new ConvertFunction();
            }
        }
        public ICommonFunction CommonFunctions 
        {
            get 
            {
                return new CommonFunction();
            }
        }

        public IDBStructure DBStructure
        {
            get
            {
                return new DBStructure();
            }
        }
        private string _version;
        public void SetDBVersion(string version)
        {
            _version = version;
        }
    }
}
