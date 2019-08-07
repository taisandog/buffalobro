using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Replacer
{
    public class DicReplace
    {
        /// <summary>
        /// 替换集合的字符
        /// </summary>
        /// <param name="itemCollection">要替换的集合</param>
        /// <returns></returns>
        public static string Replace(string source, ReplaceItemCollection itemCollection)
        {
            StringBuilder sbRet = new StringBuilder(source.Length);
            int index = 0;
            int length = source.Length;
            while (index < length)
            {
                string value = GetString(source, ref index, itemCollection);
                if(value!=null)
                {
                    sbRet.Append(value);
                }
                else
                {
                    sbRet.Append(source[index]);
                    index++;
                }
            }
            return sbRet.ToString();
        }

        

        /// <summary>
        /// 判断源字符串从指定位置开始是否有匹配的keyword
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="index">指定位置</param>
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        private static string GetString(string source,ref int index, ReplaceItemCollection itemCollection)
        {
            string value = null;
            int maxLen=itemCollection.MaxLength;

            int minLen=itemCollection.MinLength;

            int left = source.Length - index;
            if (left < minLen) 
            {
                return value;
            }

            if (left < maxLen)
            {
                maxLen = left;
            }

            for (int i = maxLen; i >= minLen; i--)
            {
                string tmp = source.Substring(index, i);
                
                if (itemCollection.TryGetValue(tmp, out value)) 
                {
                    index += i;
                    break;
                }
            }
            return value;
        }


    }
}
