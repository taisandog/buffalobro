using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
{
    /// <summary>
    /// 数值转换函数
    /// </summary>
    public class ConvertFunction :Buffalo.DB.DataBaseAdapter.IDbAdapters.IConvertFunction
    {
        private static Dictionary<string, FormatCode> _dicDateTimeFormat = GetFormat();
        private static Dictionary<string, FormatCode> GetFormat() 
        {
            Dictionary<string, FormatCode> dic = new Dictionary<string, FormatCode>();
            dic.Add("MM/dd/yyyy",new FormatCode(101,-1));
            dic.Add("MM/dd/yy", new FormatCode(1,-1));

            dic.Add("yyyy.MM.dd", new FormatCode(102,-1));
            dic.Add("yy.MM.dd", new FormatCode(2,-1));

            dic.Add("dd/MM/yyyy", new FormatCode(103,-1));
            dic.Add("dd/MM/yy", new FormatCode(3,-1));

            dic.Add("dd.MM.yyyy", new FormatCode(104,-1));
            dic.Add("dd.MM.yy", new FormatCode(4,-1));

            dic.Add("dd-MM-yyyy", new FormatCode(105,-1));
            dic.Add("dd-MM-yy", new FormatCode(5,-1));

            dic.Add("dd MM yyyy", new FormatCode(106,-1));
            dic.Add("dd MM yy", new FormatCode(6,-1));

            dic.Add("MM dd, yyyy", new FormatCode(107,-1));
            dic.Add("MM dd, yy", new FormatCode(7,-1));

            dic.Add("hh:m:ss", new FormatCode(108,-1));

            dic.Add("MM-dd-yyyy", new FormatCode(110,-1));
            dic.Add("MM-dd-yy", new FormatCode(10,-1));

            dic.Add("yyyy/MM/dd", new FormatCode(111,-1));
            dic.Add("yy/MM/dd", new FormatCode(11,-1));

            dic.Add("yyyyMMdd", new FormatCode(112,-1));
            dic.Add("yyMMdd", new FormatCode(12,-1));

            dic.Add("dd MM yyyy hh:mm:ss:ms", new FormatCode(13,-1));

            dic.Add("hh:mm:ss:ms", new FormatCode(14,-1));

            dic.Add("yyyy-MM-dd hh:mm:ss", new FormatCode(20,-1));
            dic.Add("yyyy-MM-dd hh:mm:ss.ms", new FormatCode(21,-1));
            dic.Add("yyyy-MM-dd Thh:mm:ss:mmm", new FormatCode(126, -1));
            dic.Add("yyyy-MM-dd", new FormatCode(20, 10));
            dic.Add("yyyy-MM", new FormatCode(20, 7));
            dic.Add("yyyy", new FormatCode(20, 4));
            return dic;
        }
        private static List<string> _lstItems = null;
        private static Dictionary<string, string> _dicDateTimeItem = GetDateTimeItem();
        

        private static Dictionary<string, string> GetDateTimeItem()
        {
            _lstItems = new List<string>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            
            dic.Add("yyyy", "convert(varchar,datepart(yyyy,{dt}))");
            _lstItems.Add("yyyy");
            dic.Add("yy", "right(convert(varchar,datepart(yy,{dt})),2)");
            _lstItems.Add("yy");
            dic.Add("MM", "right('0'+convert(varchar,datepart(mm,{dt})),2)");
            _lstItems.Add("MM");
            dic.Add("dd", "right('0'+convert(varchar,datepart(dd,{dt})),2)");
            _lstItems.Add("dd");
            dic.Add("hh", "right('0'+convert(varchar,datepart(hh,{dt})),2)");
            _lstItems.Add("hh");
            dic.Add("mm", "right('0'+convert(varchar,datepart(mi,{dt})),2)");
            _lstItems.Add("mm");
            dic.Add("ss", "right('0'+convert(varchar,datepart(ss,{dt})),2)");
            _lstItems.Add("ss");
            dic.Add("ms", "convert(varchar,datepart(ms,{dt}))");
            _lstItems.Add("ms");
            return dic;
        }
        /// <summary>
        /// 替换集合的字符
        /// </summary>
        /// <param name="itemCollection">要替换的集合</param>
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
        /// 判断源字符串从指定位置开始是否有匹配的keyword
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="index">指定位置</param>
        /// <param name="keyWord">关键字</param>
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
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <returns></returns>
        private static string FormatDateTime(string dateTime, string format)
        {
            StringBuilder ret = new StringBuilder();


            List<string> lstPart = SplitItems(format, _lstItems);
            for (int i = 0; i < lstPart.Count;i++ )
            {
                string part = lstPart[i];
                string newString = null;
                if (_dicDateTimeItem.TryGetValue(part, out newString))
                {
                    ret.Append(newString.Replace("{dt}", dateTime));
                }
                else
                {
                    ret.Append("'");
                    ret.Append(part);
                    ret.Append("'");
                }

                if (i < lstPart.Count - 1) 
                {
                    ret.Append("+");
                }
            }


            return ret.ToString();
        }

        /// <summary>
        /// 获取格式化字符
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private FormatCode GetFormatCode(string format) 
        {
            FormatCode ret =null;
            if (_dicDateTimeFormat.TryGetValue(format, out ret)) 
            {
                return ret;
            }
            return null;
        }

        /// <summary>
        /// 日期转字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string DateTimeToString(string dateTime, string format) 
        {
            if (format != null) 
            {
                FormatCode code = GetFormatCode(format);
                if (code ==null) 
                {
                    return FormatDateTime(dateTime, format);
                }
                string type = "varchar";
                if (code.VarcharLength > 0) 
                {
                    type += "(" + code.VarcharLength.ToString() + ")";
                }
                return "CONVERT(" + type + " , " + dateTime + ", " + code.Code.ToString() + ")";
            }
            return "CONVERT(varchar , " + dateTime + ")";
        }

        /// <summary>
        /// 字符串转成日期
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format)
        {
            return "convert(datetime," + value + ")";
        }

        /// <summary>
        /// 把数据转成指定类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="dbType">指定类型</param>
        /// <returns></returns>
        public string ConvetTo(string value, DbType dbType) 
        {
            SqlDbType sdb = (SqlDbType)DBAdapter.CurrentDbType(dbType);
            return "convert(" + sdb.ToString() + "," + value + ")";
        }
    }
}
