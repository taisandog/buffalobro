using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Replacer
{
    public class MyReplacer
    {
        
        private MyReplacer() 
        {

        }
        /// <summary>
        /// ���������滻�ַ���
        /// </summary>
        /// <param name="source">Դ�ַ���</param>
        /// <param name="conditions">�滻��������</param>
        /// <returns></returns>
        public static string ReplaceString(string source, IEnumerable<ReplaceItem> conditions)
        {
            return MyReplacer.Replace(source, conditions);
        }
        /// <summary>
        /// �滻���ϵ��ַ�
        /// </summary>
        /// <param name="itemCollection">Ҫ�滻�ļ���</param>
        /// <returns></returns>
        public static List<string> SplitItems(string source, IEnumerable<string> itemCollection)
        {
            StringBuilder sbRet = new StringBuilder(source.Length);
            int index = 0;
            int length = source.Length;
            List<string> items = new List<string>();
            StringBuilder sbTmp = new StringBuilder(20);
            bool hasFind = false;
            while (index < length)
            {
                hasFind = false;
                foreach (string keyWork in itemCollection)
                {
                    if (StringEquals(source, index, keyWork))
                    {
                        if (sbTmp.Length > 0) 
                        {
                            items.Add(sbTmp.ToString());
                            sbTmp.Remove(0, sbTmp.Length);
                        }
                        index += keyWork.Length;
                        items.Add(keyWork);
                        hasFind = true;
                        break;
                    }
                }

                if (!hasFind)
                {
                    sbTmp.Append(source[index]);
                    index += 1;
                }
            }
            if (sbTmp.Length > 0)
            {
                items.Add(sbTmp.ToString());
                sbTmp.Remove(0, sbTmp.Length);
            }
            return items;
        }

        /// <summary>
        /// �滻���ϵ��ַ�
        /// </summary>
        /// <param name="itemCollection">Ҫ�滻�ļ���</param>
        /// <returns></returns>
        public static string Replace(string source, IEnumerable<ReplaceItem> itemCollection) 
        {
            StringBuilder sbRet = new StringBuilder(source.Length);
            int index=0;
            int length = source.Length;
            while (index < length-1) 
            {
                sbRet.Append(DoReplace(source, itemCollection, ref index));
            }
            return sbRet.ToString();
        }

        /// <summary>
        /// �ж��Ƿ����滻�Ĺؼ���
        /// </summary>
        /// <param name="source">Դ�ַ���</param>
        /// <param name="itemCollection"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string DoReplace(string source, IEnumerable<ReplaceItem> itemCollection,ref int index) 
        {
            foreach (ReplaceItem item in itemCollection)
            {
                string keyWork = item.OldString;
                if (StringEquals(source, index, keyWork)) 
                {
                    index += keyWork.Length;
                    return item.NewString;
                }
            }
            string ret = source[index].ToString();
            index += 1;
            return ret;
        }

        /// <summary>
        /// �ж�Դ�ַ�����ָ��λ�ÿ�ʼ�Ƿ���ƥ���keyword
        /// </summary>
        /// <param name="source">Դ�ַ���</param>
        /// <param name="index">ָ��λ��</param>
        /// <param name="keyWord">�ؼ���</param>
        /// <returns></returns>
        private static bool StringEquals(string source, int index, string keyWord) 
        {
            for (int i = 0; i < keyWord.Length; i++)
            {
                if (source[index] != keyWord[i]) 
                {
                    return false;
                }
                index++;
            }
            return true;
        }
    }
}
