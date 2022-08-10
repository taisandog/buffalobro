using Buffalo.DB.DbCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 本地缓存基类
    /// </summary>
    public abstract class LocalCacheBase
    {
        /// <summary>
        /// 获取过期时间
        /// </summary>
        /// <param name="expiration"></param>
        /// <param name="expir"></param>
        /// <returns></returns>
        public static TimeSpan GetExpir(TimeSpan expiration, TimeSpan expir)
        {
            if (expir > TimeSpan.MinValue)
            {
                return expir;
            }
            return expiration;
        }
        /*
         作者:http://blog.sina.com.cn/s/blog_752ca76a01017s8l.html
         */
        /// <summary>
        /// 比较输入字符串与模式的是否匹配，将根据每个字符进行比较
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式，允许使用的通配符：?，*，其中? 代表任意一个字符，* 代表零或多个任意字符</param>
        /// <returns></returns>
        public static Boolean IsPatternMatch(string input, string pattern)
        {
            Boolean matched = false;
            Int32 inputIndex = 0;
            Int32 patternIndex = 0;

            //无通配符 * 时，比较算法（）
            while (inputIndex < input.Length && patternIndex < pattern.Length && (pattern[patternIndex] != '*'))
            {

                if ((pattern[patternIndex] != '?') && (input[inputIndex] != pattern[patternIndex]))
                {//如果模式字符不是通配符，且输入字符与模式字符不相等，则可判定整个输入字串与模式不匹配
                    return matched;
                }
                patternIndex++;
                inputIndex++;
                if (patternIndex == pattern.Length && inputIndex < input.Length)
                {
                    return matched;
                }
                if (inputIndex == input.Length && patternIndex < pattern.Length)
                {
                    return matched;
                }
                if (patternIndex == pattern.Length && inputIndex == input.Length)
                {
                    matched = true;
                    return matched;
                }
            }

            //有通配符 * 时，比较算法
            Int32 mp = 0;
            Int32 cp = 0;
            while (inputIndex < input.Length)
            {
                if (patternIndex < pattern.Length && pattern[patternIndex] == '*')
                {
                    if (++patternIndex >= pattern.Length)
                    {
                        matched = true;
                        return matched;
                    }
                    mp = patternIndex;
                    cp = inputIndex + 1;
                }
                else if (patternIndex < pattern.Length && ((pattern[patternIndex] == input[inputIndex]) || (pattern[patternIndex] == '?')))
                {
                    patternIndex++;
                    inputIndex++;
                }
                else
                {
                    patternIndex = mp;
                    inputIndex = cp++;
                }
            }

            //当输入字符为空且模式为*时
            while (patternIndex < pattern.Length && pattern[patternIndex] == '*')
            {
                patternIndex++;
            }

            return patternIndex >= pattern.Length ? true : false;

        }
    }
}
