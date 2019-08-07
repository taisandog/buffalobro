namespace OBS.Internal
{
    //using OBS.Internal.Log;
    using OBS.Model;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class CommonUtil
    {
        private static readonly Regex ChinesePattern = new Regex("[一-龥]");
        private static readonly Regex IPPattern = new Regex(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        private static readonly IDictionary<Type, MethodInfo> ParseMethodsHolder = new Dictionary<Type, MethodInfo>();
        private static readonly object ParseMethodsHolderLock = new object();
        private static readonly IDictionary<Type, MethodInfo> TransMethodsHolder = new Dictionary<Type, MethodInfo>();
        private static readonly object TransMethodsHolderLock = new object();

        public static void AddHeader(HttpRequest request, string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                request.Headers.Add(key, value);
            }
        }

        public static void AddParam(HttpRequest request, string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                request.Params.Add(key, value);
            }
        }

        public static string Base64Md5(byte[] data)
        {
            return Convert.ToBase64String(Md5(data));
        }

        public static void CloseIDisposable(IDisposable closeable)
        {
            if (closeable != null)
            {
                closeable.Dispose();
            }
        }

        public static string ConvertHeadersToString(IDictionary<string, string> headers)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> pair in headers)
                {
                    StringBuilder introduced3 = builder.Append(pair.Key).Append(":");
                    introduced3.Append(pair.Value);
                }
            }
            builder.Append("}");
            return builder.ToString();
        }

        public static string ConvertParamsToCanonicalQueryString(List<KeyValuePair<string, string>> kvlist)
        {
            StringBuilder builder = new StringBuilder();
            if ((kvlist != null) && (kvlist.Count > 0))
            {
                int num = 0;
                foreach (KeyValuePair<string, string> pair in kvlist)
                {
                    StringBuilder introduced4 = builder.Append(UrlEncode(pair.Key, "utf-8", "/")).Append("=");
                    introduced4.Append(UrlEncode((pair.Value == null) ? "" : pair.Value, "utf-8"));
                    if (num++ != (kvlist.Count - 1))
                    {
                        builder.Append("&");
                    }
                }
            }
            return builder.ToString();
        }

        public static string ConvertParamsToString(IDictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder();
            if ((parameters != null) && (parameters.Count > 0))
            {
                bool flag = true;
                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    if (!string.IsNullOrEmpty(pair.Key))
                    {
                        if (!flag)
                        {
                            builder.Append("&");
                        }
                        flag = false;
                        builder.Append(UrlEncode(pair.Key, "utf-8", "/"));
                        if (pair.Value != null)
                        {
                            builder.Append("=").Append(UrlEncode(pair.Value, "utf-8", "/"));
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public static MethodInfo GetParseMethodInfo(Type responseType, object iparser)
        {
            if (!ParseMethodsHolder.ContainsKey(responseType))
            {
                object parseMethodsHolderLock = ParseMethodsHolderLock;
                lock (parseMethodsHolderLock)
                {
                    if (!ParseMethodsHolder.ContainsKey(responseType))
                    {
                        Type[] types = new Type[] { typeof(HttpResponse) };
                        MethodInfo info = iparser.GetType().GetMethod("Parse" + responseType.Name, BindingFlags.Public | BindingFlags.Instance, null, types, null);
                        ParseMethodsHolder.Add(responseType, info);
                    }
                }
            }
            return ParseMethodsHolder[responseType];
        }

        public static MethodInfo GetTransMethodInfo(Type requestType, object iconvertor)
        {
            if (!TransMethodsHolder.ContainsKey(requestType))
            {
                object transMethodsHolderLock = TransMethodsHolderLock;
                lock (transMethodsHolderLock)
                {
                    if (!TransMethodsHolder.ContainsKey(requestType))
                    {
                        Type[] types = new Type[] { requestType };
                        MethodInfo info = iconvertor.GetType().GetMethod("Trans", BindingFlags.Public | BindingFlags.Instance, null, types, null);
                        TransMethodsHolder.Add(requestType, info);
                    }
                }
            }
            return TransMethodsHolder[requestType];
        }

        public static string HexSha256(string toSign)
        {
            byte[] buffer;
            using (SHA256 sha = new SHA256Managed())
            {
                buffer = sha.ComputeHash(Encoding.UTF8.GetBytes(toSign));
                sha.Clear();
            }
            return ToHex(buffer);
        }

        public static byte[] HmacSha1(string key, string toSign)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toSign);
            return new HMACSHA1(Encoding.UTF8.GetBytes(key)).ComputeHash(bytes);
        }

        public static byte[] HmacSha256(byte[] key, string toSign)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toSign);
            return new HMACSHA256(key).ComputeHash(bytes);
        }

        public static byte[] HmacSha256(string key, string toSign)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toSign);
            return new HMACSHA256(Encoding.UTF8.GetBytes(key)).ComputeHash(bytes);
        }

        public static bool IsIP(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException("Endpoint is null");
            }
            UriBuilder builder = new UriBuilder(endpoint);
            return IPPattern.IsMatch(builder.Host);
        }

        public static byte[] Md5(byte[] data)
        {
            return new MD5CryptoServiceProvider().ComputeHash(data);
        }

        public static DateTime? ParseToDateTime(string value)
        {
            return ParseToDateTime(value, @"yyyy-MM-dd\THH:mm:ss.fff\Z", @"yyyy-MM-dd\THH:mm:ss\Z");
        }

        public static DateTime? ParseToDateTime(string value, string format1, string format2)
        {
            DateTime? nullable;
            try
            {
                nullable = string.IsNullOrEmpty(value) ? ((DateTime?) (nullable = null)) : new DateTime?(DateTime.ParseExact(value, format1, OBS.Internal.Constants.CultureInfo));
            }
            catch (Exception)
            {
                try
                {
                    nullable = new DateTime?(DateTime.ParseExact(value, format2, OBS.Internal.Constants.CultureInfo));
                }
                catch (Exception exception)
                {
                    //if (LoggerMgr.IsWarnEnabled)
                    //{
                    //    LoggerMgr.Warn(string.Format("Parse {0} to DateTime failed", value), exception);
                    //}
                    nullable = null;
                }
            }
            return nullable;
        }

        public static int? ParseToInt32(string value)
        {
            try
            {
                return (string.IsNullOrEmpty(value) ? ((int?) null) : new int?(Convert.ToInt32(value)));
            }
            catch (Exception exception)
            {
                //if (LoggerMgr.IsWarnEnabled)
                //{
                //    LoggerMgr.Warn(string.Format("Parse {0} to Int32 failed", value), exception);
                //}
                return null;
            }
        }

        public static long? ParseToInt64(string value)
        {
            try
            {
                return (string.IsNullOrEmpty(value) ? ((long?) null) : new long?(Convert.ToInt64(value)));
            }
            catch (Exception exception)
            {
                //if (LoggerMgr.IsWarnEnabled)
                //{
                //    LoggerMgr.Warn(string.Format("Parse {0} to Int64 failed", value), exception);
                //}
                return null;
            }
        }

        public static void RenameHeaders(HttpRequest request, string headerPrefix, string headerMetaPrefix)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, string> pair in request.Headers)
            {
                if (!string.IsNullOrEmpty(pair.Key))
                {
                    string uriToEncode = pair.Key.Trim();
                    string str2 = (pair.Value == null) ? "" : pair.Value;
                    bool flag = false;
                    if ((uriToEncode.StartsWith(headerPrefix, StringComparison.OrdinalIgnoreCase) || uriToEncode.StartsWith("x-obs-", StringComparison.OrdinalIgnoreCase)) || OBS.Internal.Constants.AllowedRequestHttpHeaders.Contains(uriToEncode.ToLower()))
                    {
                        flag = true;
                    }
                    else if ((request.Method == HttpVerb.POST) || (request.Method == HttpVerb.PUT))
                    {
                        uriToEncode = headerMetaPrefix + uriToEncode;
                        flag = true;
                    }
                    if (flag)
                    {
                        if (uriToEncode.StartsWith(headerMetaPrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            uriToEncode = UrlEncode(uriToEncode, true);
                        }
                        dictionary.Add(uriToEncode, UrlEncode(str2, true));
                    }
                }
            }
            request.Headers = dictionary;
        }

        public static string ToHex(byte[] data)
        {
            StringBuilder builder = new StringBuilder(data.Length * 2);
            foreach (byte num2 in data)
            {
                builder.AppendFormat("{0:x2}", num2);
            }
            return builder.ToString();
        }

        public static string UrlDecode(string uriToDecode)
        {
            if (!string.IsNullOrEmpty(uriToDecode))
            {
                return Uri.UnescapeDataString(uriToDecode.Replace("+", " "));
            }
            return "";
        }

        public static string UrlEncode(string uriToEncode)
        {
            return UrlEncode(uriToEncode, "utf-8", null);
        }

        public static string UrlEncode(string uriToEncode, bool chineseOnly)
        {
            if (!chineseOnly)
            {
                return UrlEncode(uriToEncode, "utf-8");
            }
            StringBuilder builder = new StringBuilder();
            foreach (char ch in uriToEncode)
            {
                if (ChinesePattern.IsMatch(ch.ToString()))
                {
                    builder.Append(UrlEncode(ch.ToString(), "utf-8"));
                }
                else
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        public static string UrlEncode(string uriToEncode, string charset)
        {
            return UrlEncode(uriToEncode, charset, null);
        }

        public static string UrlEncode(string uriToEncode, string charset, string safe)
        {
            if (string.IsNullOrEmpty(uriToEncode))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(charset))
            {
                charset = "utf-8";
            }
            StringBuilder builder = new StringBuilder(uriToEncode.Length * 2);
            foreach (byte num2 in Encoding.GetEncoding(charset).GetBytes(uriToEncode))
            {
                char ch = (char) num2;
                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~".IndexOf(ch) != -1)
                {
                    builder.Append(ch);
                }
                else if ((safe != null) && (safe.IndexOf(ch) != -1))
                {
                    builder.Append(ch);
                }
                else
                {
                    object[] args = new object[] { (int) num2 };
                    builder.Append("%").Append(string.Format(CultureInfo.InvariantCulture, "{0:X2}", args));
                }
            }
            return builder.ToString();
        }

        public static void WriteTo(Stream src, Stream dest, int bufferSize)
        {
            int num;
            DateTime now = DateTime.Now;
            byte[] buffer = new byte[bufferSize];
            while ((num = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, num);
            }
            dest.Flush();
            //if (LoggerMgr.IsInfoEnabled)
            //{
            //    LoggerMgr.Info(string.Format("Write http request stream end, cost {0} ms", (DateTime.Now.Ticks - now.Ticks) / 0x2710L));
            //}
        }

        public static long WriteTo(Stream orignStream, Stream destStream, long totalSize, int bufferSize)
        {
            DateTime now = DateTime.Now;
            byte[] buffer = new byte[bufferSize];
            long num = 0L;
            while (num < totalSize)
            {
                int count = orignStream.Read(buffer, 0, bufferSize);
                if (count <= 0)
                {
                    break;
                }
                if ((num + count) > totalSize)
                {
                    count = (int) (totalSize - num);
                }
                num += count;
                destStream.Write(buffer, 0, count);
            }
            destStream.Flush();
            //if (LoggerMgr.IsInfoEnabled)
            //{
            //    LoggerMgr.Info(string.Format("Write http request stream end, cost {0} ms", (DateTime.Now.Ticks - now.Ticks) / 0x2710L));
            //}
            return num;
        }
    }
}

