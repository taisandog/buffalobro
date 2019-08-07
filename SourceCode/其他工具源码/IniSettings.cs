using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ini���ö�ȡ��
    /// </summary>
    public class IniSettings
    {
        private Dictionary<string, string> _dicConfig = new Dictionary<string, string>();
        /// <summary>
        /// ini���ö�ȡ��
        /// </summary>
        /// <param name="filePath">ini�ļ���·��</param>
        public IniSettings() 
        {
        }

        private Encoding _encoding = Encoding.Default;
        /// <summary>
        /// ����
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
        }
        /// <summary>
        /// ���ر������µ�ini�ļ�
        /// </summary>
        /// <param name="stm">�ļ���</param>
        /// <returns></returns>
        public static IniSettings Load(Stream stm)
        {
            return Load(stm, Encoding.Default);
        }
        /// <summary>
        /// ���ر������µ�ini�ļ�
        /// </summary>
        /// <param name="fileName">ini����</param>
        /// <returns></returns>
        public static IniSettings Load(Stream stm, Encoding encoding) 
        {
            IniSettings ret= new IniSettings();
            ret._encoding = encoding;
            ret.LoadIniConfig(stm);
            return ret;
        }
         /// <summary>
        /// ���ر������µ�ini�ļ�
        /// </summary>
        /// <param name="fileName">ini����</param>
        /// <returns></returns>
        public static IniSettings Load(string fileName)
        {
            return Load(fileName, Encoding.Default);
        }
        /// <summary>
        /// ���ر������µ�ini�ļ�
        /// </summary>
        /// <param name="fileName">ini����</param>
        /// <returns></returns>
        public static IniSettings Load(string fileName, Encoding encoding)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read)) 
            {
                return Load(file, encoding);
            }
        }

        /// <summary>
        /// ��ȡiniֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public string this[string key]
        {
            get 
            {
                string ret = null;
                if (_dicConfig.TryGetValue(key, out ret)) 
                {
                    return ret;
                }
                return null;
            }
        }



        /// <summary>
        /// ����Ini����
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        private void LoadIniConfig(Stream stm) 
        {
            using (StreamReader reader = new StreamReader(stm,_encoding)) 
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] items = line.Split('=');
                    if (items.Length != 2)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(items[0]))
                    {
                        continue;
                    }
                    string key = items[0].Trim();
                    _dicConfig[key] = items[1].Replace("&DY", "=").Replace("&BN", "\n").Replace("&BR", "\r");
                }
            }
        }
    }
}
