using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.UpdateModelUnit
{
    /// <summary>
    /// ����ǰ׺
    /// </summary>
    public enum PrefixType
    {
        /// <summary>
        /// û��ǰ׺
        /// </summary>
        None,
        /// <summary>
        /// Camel����ǰ�߶���Сд
        /// </summary>
        Camel,
        /// <summary>
        /// ǰ׺�������ַ�
        /// </summary>
        Three,
        /// <summary>
        /// ������������(�ؼ�����_���������磺txt_Name)
        /// </summary>
        HungarianNotation
    }

    public class UpdateModelInfo 
    {
        /// <summary>
        /// �������������ȡ������
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="pType">����</param>
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
        /// ����Camel�����ȡ������
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
        /// ����HungarianNotation�����ȡ������
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
