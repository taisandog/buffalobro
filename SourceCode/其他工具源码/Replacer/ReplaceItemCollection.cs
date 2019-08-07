using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Replacer
{
    public class ReplaceItemCollection:Dictionary<string,string>
    {
        int minLength = 0;

        
        int maxLength = 0;
        /// <summary>
        /// 集合中字符串最小的长度
        /// </summary>
        public int MinLength
        {
            get { return minLength; }
            set { minLength = value; }
        }

        /// <summary>
        /// 集合中字符串最大的长度
        /// </summary>
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }
    }
}
