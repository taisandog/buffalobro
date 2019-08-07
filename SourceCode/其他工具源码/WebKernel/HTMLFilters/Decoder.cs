using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    public delegate string DoDecode(HtmlTickInfo info);
    
    public class Decoder
    {
        
        private static Dictionary<string, DoDecode> dicDecoder = new Dictionary<string,DoDecode>();
        

        /// <summary>
        /// ���UBB��ǩ������
        /// </summary>
        /// <param name="tickName">��ǩ��</param>
        /// <param name="info">��ǩ��Ϣ</param>
        internal static void AddDecoder(string tickName, DoDecode info) 
        {
            dicDecoder.Add(tickName, info);
        }

        /// <summary>
        /// ���ݱ�ǩ����ȡ��ǩ�����ί��
        /// </summary>
        /// <param name="tickName">��ǩ��</param>
        /// <returns></returns>
        public static DoDecode GetDecoder(string tickName)
        {
            DoDecode dec = null;
            if(dicDecoder.TryGetValue(tickName,out dec))
            {
                return dec;
            }
            return null;
        }
    }
    
}
