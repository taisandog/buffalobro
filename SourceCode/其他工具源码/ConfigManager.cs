using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;


namespace Buffalo.Kernel
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class ConfigManager
    {
        private Dictionary<string, object> _dic;
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// �ļ���
        /// </summary>
        private string _fileName;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="fileName">�����ļ���(null��ΪĬ����)</param>
        public ConfigManager(string fileName) 
        {
            _fileName = fileName;
            if (string.IsNullOrEmpty(_fileName)) 
            {
                _fileName=Process.GetCurrentProcess().MainModule.FileName+".config.json";
            }
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        
        public ConfigManager():this(null)
        {
           
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public void Load() 
        {
            string fileName = CommonMethods.GetBaseRoot(_fileName);
            if(!File.Exists(fileName))
            {
                _dic=new Dictionary<string,object>();
                return;
            }
            string content = File.ReadAllText(fileName, DefaultEncoding);
            _dic= JsonConvert.DeserializeObject<Dictionary<string,object>>(content);
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public void Save() 
        {
            string fileName = CommonMethods.GetBaseRoot(_fileName);

            string content = JsonConvert.SerializeObject(_dic);
            File.WriteAllText(fileName, content);
        }
        /// <summary>
        /// ���û��ȡֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key] 
        {
            get 
            {
                object ret = null;
                if (!_dic.TryGetValue(key, out ret)) 
                {
                    return null;
                }
                return ret;
            }
            set 
            {
                _dic[key] = value;
            }
        }
    }
}
