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
        /// 添加UBB标签解释器
        /// </summary>
        /// <param name="tickName">标签名</param>
        /// <param name="info">标签信息</param>
        internal static void AddDecoder(string tickName, DoDecode info) 
        {
            dicDecoder.Add(tickName, info);
        }

        /// <summary>
        /// 根据标签名获取标签处理的委托
        /// </summary>
        /// <param name="tickName">标签名</param>
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
