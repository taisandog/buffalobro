
using System;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel.FastReflection;
using System.Web;
using Buffalo.Kernel.FastReflection.ClassInfos;
using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace Buffalo.Kernel
{

    /// <summary>
    /// 常用的方法类
    /// </summary>
    public class CommonMethods
    {
        private CommonMethods()
        {

        }
        private static string _baseRoot = null;//基目录
        /// <summary>
        /// 获取基路径
        /// </summary>
        /// <param name="configRoot"></param>
        /// <returns></returns>
        public static string GetBaseRoot(string configRoot)
        {

            if (_baseRoot == null)
            {
                if (IsWebContext)
                {
                    _baseRoot = HttpContext.Current.Server.MapPath("~//");
                }
                else
                {
                    _baseRoot = AppDomain.CurrentDomain.BaseDirectory;
                }
                //if (!string.IsNullOrEmpty(_baseRoot))
                //{
                //    _baseRoot = _baseRoot.Trim().TrimEnd('\\') + "\\";
                //}
            }
            if (Path.IsPathRooted(configRoot)) 
            {
                return configRoot;
            }
            string retRoot = Path.Combine(_baseRoot,configRoot);
            return retRoot;
        }

        /*
         作者:http://blog.sina.com.cn/s/blog_752ca76a01017s8l.html
         */
        /// <summary>
        /// 通配符比较
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">通配符，允许使用的通配符：?，*，其中? 代表任意一个字符，* 代表零或多个任意字符</param>
        /// <returns></returns>
        public static unsafe bool IsPatternMatch(string input, string pattern)
        {
            Boolean matched = false;
            fixed (char* p_wild = pattern)
            fixed (char* p_str = input)
            {
                char* wild = p_wild;
                char* str = p_str;
                char* cp = null;
                char* mp = null;

                while ((*str) != 0 && (*wild != '*'))
                {
                    if ((*wild != *str) && (*wild != '?'))
                    {
                        return matched;
                    }
                    wild++;
                    str++;
                }

                while (*str != 0)
                {
                    if (*wild == '*')
                    {
                        if (0 == (*++wild))
                        {//如果*后面没有其它模式符，则判定匹配
                            matched = true;
                            return matched;
                        }
                        mp = wild;
                        cp = str + 1;
                    }
                    else if ((*wild == *str) || (*wild == '?'))
                    {
                        wild++;
                        str++;
                    }
                    else
                    {
                        //模式串未到结尾，而输入字串已经走到结尾，判定不匹配
                        wild = mp;//冻结，固定在不匹配的模式字符上
                        str = cp++;
                    }
                }

                //修正模式串
                while (*wild == '*')
                {
                    wild++;
                }
                return (*wild) == 0 ? true : false;
            }
        
        }
        /// <summary>
        /// 检测是否Web程序
        /// </summary>
        public static bool IsWebContext
        {
            get
            {
                return (HttpContext.Current != null);
            }
        }


        /// <summary>
        /// 获取应用程序的基目录
        /// </summary>
        /// <returns></returns>
        public static string GetBaseRoot()
        {

            return GetBaseRoot("");
        }

        /// <summary>
        /// 通过版本号判断拷贝文件
        /// </summary>
        /// <param name="source">源文件</param>
        /// <param name="target">目标文件</param>
        /// <returns></returns>
        public static bool CopyNewer(string source, string target) 
        {
            if (!File.Exists(source)) 
            {
                return false;
            }
            string sourceVersion = null;
            string targetVersion = null;
            FileVersionInfo sourceVersionInfo = FileVersionInfo.GetVersionInfo(source);
            sourceVersion=string.Format("{0}.{1}.{2}.{3}", sourceVersionInfo.FileMajorPart,
                sourceVersionInfo.FileMinorPart, sourceVersionInfo.FileBuildPart,
                sourceVersionInfo.FilePrivatePart);
            if (File.Exists(target))
            {
                FileVersionInfo targetVersionInfo = FileVersionInfo.GetVersionInfo(target);
                targetVersion = string.Format("{0}.{1}.{2}.{3}", targetVersionInfo.FileMajorPart,
                targetVersionInfo.FileMinorPart, targetVersionInfo.FileBuildPart,
                targetVersionInfo.FilePrivatePart);
            }
            if (sourceVersion != targetVersion) 
            {
                File.Copy(source, target, true);
                return true;
            }
            return false;
        }
        

        


        /// <summary>
        /// 判断字符串是 null、空还是仅由空白字符组成（为.net2.0扩展的方法，等同于string.IsNullOrWhiteSpace）
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(string value) 
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 判断集合是 null还是空的集合。
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static bool IsCollectionNullOrEmpty(ICollection lst) 
        {
            if (lst == null) 
            {
                return true;
            }
            if (lst.Count == 0) 
            {
                return true;
            }
            return false;
        }

        
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool IsNull(object value) 
        {
            return object.ReferenceEquals(value, null);
        }

        /// <summary>
        /// GUID转成字符串
        /// </summary>
        /// <param name="id"></param>
        ///  <param name="isUpper">是否大写</param>
        /// <returns></returns>
        public static string GuidToString(Guid id,bool isUpper)
        {
            return BytesToHexString(id.ToByteArray(),isUpper);
        }
        /// <summary>
        /// GUID转成字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GuidToString(Guid id)
        {
            return GuidToString(id,true);
        }


        /// <summary>
        /// 字符串转回GUID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Guid StringToGuid(string id)
        {
            byte[] arrId = HexStringToBytes(id);
            if (arrId.Length != 16)
            {
                throw new Exception("错误的GUID字符串");
            }
            return new Guid(arrId);
        }
        /// <summary>
        /// 把DataSet打成XML字符串
        /// </summary>
        /// <param name="ds">要处理的DataSet</param>
        /// <param name="mode">指定如何从 System.Data.DataSet 写入 XML 数据和关系架构</param>
        /// <returns></returns>
        public static string DataSetToXML(DataSet ds,XmlWriteMode mode)
        {
            string ret = null;
            using (MemoryStream stm = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(stm, System.Text.Encoding.UTF8);
                ds.WriteXml(writer, mode);
                byte[] buffer = stm.ToArray();
                ret = System.Text.Encoding.UTF8.GetString(buffer);
            }
            return ret;
        }
        
        /// <summary>
        /// <summary>
        /// 把DataSet打成XML字符串
        /// </summary>
        /// <param name="ds">要处理的DataSet</param>
        /// <returns></returns>
        public static string DataSetToXML(DataSet ds) 
        {
            return DataSetToXML(ds, XmlWriteMode.WriteSchema);
        }

        /// <summary>
        /// XML字符串转成DataSet
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <param name="mode">指定如何将 XML 数据和关系架构读入 System.Data.DataSet</param>
        /// <returns></returns>
        public static DataSet XMLToDataSet(string xml,XmlReadMode mode)
        {
            DataSet ds = new DataSet();
            using (MemoryStream stm = new MemoryStream())
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(xml);
                stm.Write(buffer, 0, buffer.Length);
                stm.Position = 0;
                ds.ReadXml(stm, mode);
            }
            return ds;
        }
        /// <summary>
        /// XML字符串转成DataSet
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <returns></returns>
        private DataSet XMLToDataSet(string xml)
        {

            return XMLToDataSet(xml,XmlReadMode.ReadSchema);
        }

        static readonly DateTime StartTimeUTC = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        static readonly DateTime StartTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

        /// <summary>
        /// 将时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <param name="useSecond">true传入的是秒数,false传入的是毫秒数</param>
        /// <param name="isUTC">时间戳是否格林威治标准时间</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d,bool useSecond, bool isUTC)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = isUTC? StartTimeUTC: StartTime;
            if (useSecond)
            {
                time = startTime.AddSeconds(d);
            }
            else 
            {
                time= startTime.AddMilliseconds(d);
            }
            return time;
        }
        /// <summary>
        /// 将时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            return ConvertIntDateTime(d,false,false);
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式(秒)
        /// </summary>
        /// <param name="time">时间</param>
        ///  <param name="useSecond">true返回秒数,false返回毫秒数</param>
        ///  <param name="isUTC">是否返回格林威治标准时间戳</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(System.DateTime time, bool useSecond, bool isUTC)
        {
            System.DateTime startTime = isUTC ? StartTimeUTC : StartTime;
            TimeSpan ts=time.Subtract(startTime);
            if (useSecond)
            {
                return ts.TotalSeconds;
            }
            return ts.TotalMilliseconds;
        }
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式(秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            
            return ConvertDateTimeInt(time,false,false);
        }
        /// <summary>
        /// 反序列化结构体
        /// </summary>
        /// <param name="rawdatas"></param>
        /// <returns></returns>
        public static object RawDeserialize(byte[] rawdatas,Type objType)
        {

            //Type anytype = typeof(T);

            int rawsize = Marshal.SizeOf(objType);
            object retobj = null;
            if (rawsize > rawdatas.Length)
            {
                return retobj;
            }
            
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            try
            {
                Marshal.Copy(rawdatas, 0, buffer, rawsize);

                retobj = Marshal.PtrToStructure(buffer, objType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return retobj;
        }
        /// <summary>
        /// 反序列化结构体
        /// </summary>
        /// <param name="rawdatas"></param>
        /// <returns></returns>
        public static T RawDeserialize<T>(byte[] rawdatas)
        {
            object obj=RawDeserialize(rawdatas,typeof(T));
            return (T)obj;
        }

        /// <summary>
        /// 从流中读出元素
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static object RawDeserialize(Stream stm, Type objType) 
        {
            int rawsize = Marshal.SizeOf(objType);
            byte[] fbuffer = new byte[rawsize];
            rawsize=stm.Read(fbuffer, 0, rawsize);
            object retobj = null;
            
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            try
            {
                Marshal.Copy(fbuffer, 0, buffer, rawsize);

                retobj = Marshal.PtrToStructure(buffer, objType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return retobj;
        }

        /// <summary>
        /// 从流中读出元素
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns></returns>
        public static T RawDeserialize<T>(Stream stm)
        {


            return (T)RawDeserialize(stm,typeof(T));
        }

        /// <summary>
        /// 对象序列化成字节数组
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static byte[] RawSerialize(object obj)
        {

            int rawsize = Marshal.SizeOf(obj);

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);

            byte[] rawdatas = null;
            try
            {
                Marshal.StructureToPtr(obj, buffer, false);

                rawdatas = new byte[rawsize];

                Marshal.Copy(buffer, rawdatas, 0, rawsize);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return rawdatas;
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetLocalIP() 
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = ipEntry.AddressList[0];
            return ip;
        }

        /// <summary>
        /// 实体类型转换
        /// </summary>
        /// <param name="sValue">源值</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object EntityProChangeType(object sValue, Type targetType) 
        {
            Type valType = sValue.GetType();//实际值的类型

            if (!targetType.Equals(valType))
            {
                sValue = ChangeType(sValue, targetType);

            }
            return sValue;
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="sValue">值</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object ChangeType(object sValue, Type targetType) 
        {
            if (targetType == DefaultType.GUIDType) 
            {
                string str = sValue as string;
                if (!string.IsNullOrEmpty(str))
                {
                    return StringToGuid(str);
                }
            }
            return Convert.ChangeType(sValue,targetType);
        }

        

        /// <summary>
        /// 读取流的内容
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns></returns>
        public static byte[] LoadStreamData(Stream stm) 
        {
            byte[] tmp = null;
            if (stm != null && stm.Length > 0)
            {
                tmp = new byte[stm.Length];
                stm.Read(tmp, 0, tmp.Length);
            }
            return tmp;
        }
        /// <summary>
        /// 读取流的内容
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns></returns>
        public static byte[] LoadStreamData2(Stream stm)
        {
            using (MemoryStream tmpStm = new MemoryStream())
            {
                byte[] buffer = new byte[512];
                if (stm != null)
                {
                    int loaded = 0;
                    while ((loaded = stm.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        tmpStm.Write(buffer, 0, loaded);
                    }
                }
                return tmpStm.ToArray();
            }
        }
        /// <summary>
        /// 流内容复制
        /// </summary>
        /// <param name="stmSource">源</param>
        /// <param name="stmTarget">目标</param>
        public static void CopyStreamData(Stream stmSource,Stream stmTarget)
        {
            byte[] buffer = new byte[1024];
            int readed = 0;
            do
            {
                readed = stmSource.Read(buffer, 0, buffer.Length);
                if (readed > 0)
                {
                    stmTarget.Write(buffer, 0, readed);
                }
            } while (readed>0);
            
        }
        /// <summary>
        /// 流内容复制
        /// </summary>
        /// <param name="stmSource">源</param>
        /// <param name="stmTarget">目标</param>
        /// <param name="length">长度(小于0则全部复制)</param>
        public static void CopyStreamData(Stream stmSource, Stream stmTarget,long length,HttpResponse response)
        {
            byte[] buffer = new byte[1024];
            int readed = 0;
            long left = length;//剩余字节
            int len=buffer.Length;
            bool hasLeft = left > 0;
            do
            {
                if (response != null && !response.IsClientConnected) 
                {
                    return;
                }
                if (hasLeft && left < len) 
                {
                    len = (int)left;
                }
                readed = stmSource.Read(buffer, 0, len);
                if (readed > 0)
                {
                    stmTarget.Write(buffer, 0, readed);
                }
                stmTarget.Flush();
                
                if (hasLeft) 
                {
                    left -= readed;
                    if (left <= 0)
                    {
                        break;
                    }
                }
            } while (readed > 0);

        }
        /// <summary>
        /// 获取字符串里边的所有数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetAllNumber(string str) 
        {
            if (IsNullOrWhiteSpace(str))
            {
                return "";
            }
            StringBuilder ret = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str, i))
                {
                    ret.Append(str[i]);
                }
            }
            return ret.ToString() ;
        }


        /// <summary>
        /// 把字节数组转成十六进制字符串
        /// </summary>
        /// <param name="bye">字节数组</param>
        /// <param name="isUpper">是否大写</param>
        /// <returns></returns>
        public static string BytesToHexString(byte[] bye,bool isUpper)
        {
            StringBuilder retStr = new StringBuilder(bye.Length * 2);
            string format = "X2";
            if (!isUpper)
            {
                format = "x2";
            }
            for (int i = 0; i < bye.Length; i++)
            {
                retStr.Append(bye[i].ToString(format));
            }
            return retStr.ToString();
        }

        /// <summary>
        /// 把文字转成十六进制字符码
        /// </summary>
        /// <returns></returns>
        public static string ToByteString(string str, bool isUpper) 
        {
            byte[] byes = System.Text.Encoding.Unicode.GetBytes(str);
            StringBuilder ret = new StringBuilder(str.Length*3);
            string format = "X2";
            if (!isUpper)
            {
                format = "x2";
            }
            for (int i = 0; i < byes.Length; i++) 
            {
                string tmp = byes[i].ToString(format);
                ret.Append(tmp);
            }
            return ret.ToString();
        }
        /// <summary>
        /// 把文字转成十六进制字符码
        /// </summary>
        /// <returns></returns>
        public static string ToByteString(string str)
        {
            return ToByteString(str,true);
        }
        /// <summary>
        /// 把十六进制字符串转成字节数组
        /// </summary>
        /// <param name="str">十六进制字符串</param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string str)
        {
            byte[] ret = new byte[str.Length / 2];
            char[] chr = new char[2];
            for (int i = 0; i < str.Length / 2; i++)
            {
                chr[0] = str[i * 2];
                chr[1] = str[i * 2 + 1];
                string strCurNum = new string(chr);
                int curNum = int.Parse(strCurNum, System.Globalization.NumberStyles.AllowHexSpecifier);
                ret[i] = (byte)curNum;
            }
            return ret;
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatString(object str)
        {
            if (str == null)
            {
                return "";
            }
            return str.ToString();
        }

        /// <summary>
        /// 格式化长字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatLongString(object str,int maxchr)
        {
            if (str == null)
            {
                return "";
            }
            string retStr = str.ToString();
            if (retStr.Length > maxchr)
            {
                retStr = retStr.Substring(0, maxchr-3) + "...";
            }
            return retStr;
        }
        private static char[] chrs;
        /// <summary>
        ///  随机生成字符串
        /// </summary>
        /// <param name="length">生成多少位随机字符串</param>
        /// <returns></returns>
        public static string GetCode(int length)
        {
            string lcode = "";
            if (chrs == null)//如果字符库还没初始化就初始化
            {
                chrs = new char[36];
                for (int i = 0; i < 26; i++)
                {
                    char chr = (char)('A' + i);
                    chrs[i] = chr;
                }
                for (int k = 0; k < 10; k++)
                {
                    chrs[26 + k] = (char)('0' + k);
                }
            }
            int seed = DateTime.Now.Day * 1000 + DateTime.Now.Hour * 100 + DateTime.Now.Minute * 10 + DateTime.Now.Second;
            Random rnd = new Random(seed);
            for (int x = 0; x < 4; x++)
            {
                int ind = (int)((float)(chrs.Length) * rnd.NextDouble());
                lcode += chrs[ind].ToString();
            }

            return lcode;
        }

        /// <summary>
        /// 格式化输出的日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string FormatDateTimeString(DateTime dt)
        {

            string ret = dt.Year.ToString() + ".";

            string tmp = dt.Month.ToString();
            if (tmp.Length < 2)
            {
                tmp = "0" + tmp;
            }
            ret += tmp + ".";

            tmp = dt.Day.ToString();
            if (tmp.Length < 2)
            {
                tmp = "0" + tmp;
            }
            ret += tmp + " ";

            tmp = dt.Hour.ToString();
            if (tmp.Length < 2)
            {
                tmp = "0" + tmp;
            }
            ret += tmp + ":";

            tmp = dt.Minute.ToString();
            if (tmp.Length < 2)
            {
                tmp = "0" + tmp;
            }
            ret += tmp;
            return ret;
        }

        
        /// <summary>
        /// 判断该字符串是否整型数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsIntNumber(string str)
        {
            if (str == null || str == "")
            {
                return false;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsDigit(str, i))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 把集合转换成字典类
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型 </typeparam>
        /// <param name="collection">集合类</param>
        /// <param name="keyProperty">键名</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ListToDictionary<TKey, TValue>(IEnumerable collection, string keyProperty) 
        {
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();

            FastPropertyHandler handle = FastValueGetSet.GetGetMethodInfo(keyProperty, typeof(TValue));
            object[] emptyParams=new object[] { };
            foreach (TValue objValue in collection) 
            {
                object obj=objValue;
                if(obj==null)
                {
                    continue;
                }
                TKey key=default(TKey);
                object objKey = handle(obj, emptyParams);
                if (typeof(TKey) == DefaultType.StringType)
                {
                    object strkey = objKey.ToString();
                    dic[(TKey)strkey] = objValue;
                }
                else 
                {
                    key = (TKey)objKey;
                    dic[key] = objValue;
                }
                
            }
            return dic;
        }

        /// <summary>
        /// 判断该字符串是否整型数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            if (str == null || str == "")
            {
                return false;
            }
            bool hasPoint=false;
            for (int i = 0; i < str.Length; i++)
            {
                char chr = str[i];
                if (str[i] == '.')
                {
                    if (hasPoint)//如果已经出现过点的话，就返回错误
                    {
                        return false;
                    }
                    hasPoint = true;
                }
                else
                {
                    if (!char.IsDigit(chr))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 清除事件绑定的函数
        /// </summary>
        /// <param name="objectHasEvents">拥有事件的实例</param>
        public static void ClearAllEvents(object objectHasEvents)
        {
            ObjectEventsUnit.ClearAllEvents(objectHasEvents);
        }
    }
}