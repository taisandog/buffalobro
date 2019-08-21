using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.FaintnessSearchConditions
{
    /// <summary>
    /// �ж��Ƿ���Ŵʵ���
    /// </summary>
    public class NoiseWord
    {
        /// <summary>
        /// �жϹؼ����Ƿ���Ŵ�
        /// </summary>
        /// <param name="word">�ؼ���</param>
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
        /// �ж��ַ��Ƿ�����
        /// </summary>
        /// <param name="chr">�ַ�</param>
        /// <returns></returns>
        public static bool IsChineseLetter(char chr)
        {
            string input = new string(new char[] { chr });
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //��Χ��0x4e00��0x9fff��ת����int��chfrom��chend��
            int chend = Convert.ToInt32("9fff", 16);
            if (input != "")
            {
                code = Char.ConvertToUtf32(input, 0);    //����ַ���input��ָ������index���ַ�unicode����
                if (code >= chfrom && code <= chend)
                {
                    return true;     //��code�����ķ�Χ�ڷ���true
                }
                else
                {
                    return false;    //��code�������ķ�Χ�ڷ���false
                }
            }
            return false;
        }
    }
}
