
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

using Microsoft.Extensions.Hosting;

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
        /// 文件夹路径连接符
        /// </summary>
        public static readonly char PathCombine = GetPathCombine();
        /// <summary>
        /// 获取路径连接符
        /// </summary>
        /// <returns></returns>
        private static char GetPathCombine()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return '\\';
            }
            return '/';
        }
        /// <summary>
        /// 获取基路径,web程序需要在Configure调用CommonMethods.ConfigureServices
        /// </summary>
        /// <param name="configRoot"></param>
        /// <returns></returns>
        public static string GetBaseRoot(string configRoot)
        {

            if (_baseRoot == null)
            {
                if (IsWebContext)
                {
                    _baseRoot = _hostingEnvironment.ContentRootPath;
                }
                else
                {

                    _baseRoot = AppDomain.CurrentDomain.BaseDirectory;
                }

                //if (!string.IsNullOrEmpty(_baseRoot))
                //{
                //    _baseRoot = _baseRoot.Trim();
                //    _baseRoot = _baseRoot.TrimEnd('\\','/');
                //    _baseRoot += PathCombine;
                //}
            }
            if (string.IsNullOrEmpty(configRoot))
            {
                return _baseRoot;
            }
            if (Path.IsPathRooted(configRoot))
            {
                return configRoot;
            }
            string[] pathParts = configRoot.Split('\\', '/');
            string[] newpart = new string[pathParts.Length + 1];
            newpart[0] = _baseRoot;
            for (int i = 0; i < pathParts.Length; i++)
            {
                newpart[i + 1] = pathParts[i];
            }
            string retRoot = Path.Combine(newpart);
            return retRoot;
        }
       

        private static IHostEnvironment _hostingEnvironment;

        /// <summary>
        /// 检测是否Web程序,web程序需要在Startup.Configure调用CommonMethods.ConfigureServices
        /// </summary>
        public static bool IsWebContext
        {
            get
            {
                return (_hostingEnvironment != null);
            }
        }

        /// <summary>
        /// 设置Http上下文，web程序需要在Startup.Configure调用本方法
        /// </summary>
        /// <param name="accessor"></param>
        /// 
        public static void ConfigureServices(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
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
            sourceVersion = string.Format("{0}.{1}.{2}.{3}", sourceVersionInfo.FileMajorPart,
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
            
            return lst==null || lst.Count<=0;
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
        public static string GuidToString(Guid id, bool isUpper)
        {
            return BytesToHexString(id.ToByteArray(), isUpper);
        }
        /// <summary>
        /// GUID转成字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GuidToString(Guid id)
        {
            return GuidToString(id, true);
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



        static readonly DateTime StartTimeUTC = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Utc, TimeZoneInfo.Local);
        static readonly DateTime StartTime = new System.DateTime(1970, 1, 1);

        /// <summary>
        /// 将时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <param name="useSecond">true传入的是秒数,false传入的是毫秒数</param>
        /// <param name="isUTC">时间戳是否格林威治标准时间</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d, bool useSecond, bool isUTC)
        {
            if (useSecond)
            {
                if (isUTC)
                {
                    DateTimeOffset dto = DateTimeOffset.FromUnixTimeSeconds((long)d);
                    return dto.ToLocalTime().DateTime;
                }
                else
                {
                    return StartTime.AddSeconds(d);
                }
            }
            else
            {
                if (isUTC)
                {
                    DateTimeOffset dto = DateTimeOffset.FromUnixTimeMilliseconds((long)d);
                    return dto.ToLocalTime().DateTime;
                }
                else
                {
                    return StartTime.AddMilliseconds(d);
                }
            }


        }

        /// <summary>
        /// 将时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            return ConvertIntDateTime(d, true, true);
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
            DateTimeOffset dto = new DateTimeOffset(time);
            if (useSecond)
            {
                if (isUTC)
                {
                    return dto.ToUnixTimeSeconds();
                }
                else
                {
                    return time.Subtract(StartTime).TotalSeconds;
                }
            }
            else
            {
                if (isUTC)
                {
                    return dto.ToUnixTimeMilliseconds();
                }
                else
                {
                    return time.Subtract(StartTime).TotalMilliseconds;
                }
            }

        }
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式(秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(System.DateTime time)
        {

            return ConvertDateTimeInt(time, true, true);
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
            return Convert.ChangeType(sValue, targetType);
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
        public static void CopyStreamData(Stream stmSource, Stream stmTarget)
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
            } while (readed > 0);

        }

        /// <summary>
        /// 流内容复制
        /// </summary>
        /// <param name="stmSource">源</param>
        /// <param name="stmTarget">目标</param>
        /// <param name="length">长度(小于0则全部复制)</param>
        public static void CopyStreamData(Stream stmSource, Stream stmTarget, long length)
        {
            byte[] buffer = new byte[1024];
            int readed = 0;
            long left = length;//剩余字节
            int len = buffer.Length;
            bool hasLeft = left > 0;
            do
            {

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
        /// 把字节数组转成十六进制字符串
        /// </summary>
        /// <param name="bye">字节数组</param>
        /// <param name="isUpper">是否大写</param>
        /// <returns></returns>
        public static string BytesToHexString(byte[] bye, bool isUpper)
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
            StringBuilder ret = new StringBuilder(str.Length * 3);
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
            return ToByteString(str, true);
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
            object[] emptyParams = new object[] { };
            foreach (TValue objValue in collection)
            {
                object obj = objValue;
                if (obj == null)
                {
                    continue;
                }
                TKey key = default(TKey);
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
        /// 清除事件绑定的函数
        /// </summary>
        /// <param name="objectHasEvents">拥有事件的实例</param>
        public static void ClearAllEvents(object objectHasEvents)
        {
            ObjectEventsUnit.ClearAllEvents(objectHasEvents);
        }
    }
}