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
    /// 配置信息
    /// </summary>
    public class ConfigManager
    {
        private Dictionary<string, object> _dic;
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// 文件名
        /// </summary>
        private string _fileName;
        /// <summary>
        /// 配置信息
        /// </summary>
        /// <param name="fileName">配置文件名(null则为默认名)</param>
        public ConfigManager(string fileName) 
        {
            _fileName = fileName;
            if (string.IsNullOrEmpty(_fileName)) 
            {
                _fileName=Process.GetCurrentProcess().MainModule.FileName+".config.json";
            }
        }
        /// <summary>
        /// 配置信息
        /// </summary>
        
        public ConfigManager():this(null)
        {
           
        }
        /// <summary>
        /// 加载信息
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
        /// 保存信息
        /// </summary>
        public void Save() 
        {
            string fileName = CommonMethods.GetBaseRoot(_fileName);

            string content = JsonConvert.SerializeObject(_dic);
            File.WriteAllText(fileName, content);
        }
        /// <summary>
        /// 设置或获取值
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
