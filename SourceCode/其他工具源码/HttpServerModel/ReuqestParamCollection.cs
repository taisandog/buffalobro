using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.HttpServerModel
{
    public class ReuqestParamCollection:Dictionary<string,string>
    {
        /// <summary>
        /// ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new string this[string key]
        {
            get 
            {
                string val = null;
                if (this.TryGetValue(key, out val)) 
                {
                    return val;
                }
                return null;
            }
            set 
            {
                base[key] = value;
            }
        }
    }
}
