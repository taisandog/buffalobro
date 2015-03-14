using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.UpdateModelUnit
{
    /// <summary>
    /// 命名前缀
    /// </summary>
    public enum PrefixType
    {
        /// <summary>
        /// 没有前缀
        /// </summary>
        None,
        /// <summary>
        /// Camel规则，前边都是小写
        /// </summary>
        Camel,
        /// <summary>
        /// 前缀是三个字符
        /// </summary>
        Three,
        /// <summary>
        /// 匈牙利命名法(控件类型_属性名，如：txt_Name)
        /// </summary>
        HungarianNotation
    }

    public class UpdateModelInfo 
    {
        /// <summary>
        /// 根据命名规则获取属性名
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="pType">类型</param>
        /// <returns></returns>
        public static string GetKey(string id, PrefixType pType)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            switch (pType)
            {
                case PrefixType.Camel:
                    id = GetCamelName(id);
                    break;
                case PrefixType.Three:
                    id = id.Substring(3);
                    break;
                case PrefixType.HungarianNotation:
                    id = GetHungarianNotationName(id);
                    break;
                default:
                    break;
            }
            return id;
        }

        /// <summary>
        /// 根据Camel规则获取属性名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetCamelName(string key)
        {
            int i;
            for (i = 0; i < key.Length; i++)
            {
                if (key[i] > 'z' || key[i] < 'a')
                {
                    break;
                }
            }

            if (i > 0 && i < key.Length - 1)
            {
                key = key.Substring(i);
            }
            else
            {
                return null;
            }
            return key;
        }

        /// <summary>
        /// 根据HungarianNotation规则获取属性名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetHungarianNotationName(string key)
        {
            int i;
            for (i = 0; i < key.Length; i++)
            {
                if (key[i] == '_')
                {
                    break;
                }
            }

            if (i > 0 && i < key.Length - 1)
            {
                key = key.Substring(i + 1);
            }
            else
            {
                return null;
            }
            return key;
        }
    }
}
