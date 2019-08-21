using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.FaintnessSearchConditions
{
    /// <summary>
    /// 判断是否干扰词的类
    /// </summary>
    public class NoiseWord
    {
        /// <summary>
        /// 判断关键词是否干扰词
        /// </summary>
        /// <param name="word">关键字</param>
        /// <returns></returns>
        public static bool IsNoiseWord(string word) 
        {
            if (word.Length <= 1) 
            {
                return true;
            }
            for (int i = 0; i < word.Length; i++) 
            {
                char chr = word[i];
                if (char.IsLetterOrDigit(chr) || IsChineseLetter(chr)) 
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 判断字符是否中文
        /// </summary>
        /// <param name="chr">字符</param>
        /// <returns></returns>
        public static bool IsChineseLetter(char chr)
        {
            string input = new string(new char[] { chr });
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            if (input != "")
            {
                code = Char.ConvertToUtf32(input, 0);    //获得字符串input中指定索引index处字符unicode编码
                if (code >= chfrom && code <= chend)
                {
                    return true;     //当code在中文范围内返回true
                }
                else
                {
                    return false;    //当code不在中文范围内返回false
                }
            }
            return false;
        }
    }
}
