namespace OBS.Internal.Auth
{
    using OBS.Internal;
    //using OBS.Internal.Log;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal class V4Signer : Signer
    {
        private const string ContentSha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
        private static V4Signer instance = new V4Signer();
        private const string RegionKey = "region";
        private const string RequestKey = "aws4_request";
        private static readonly string ScopeSuffix = string.Format("/{0}/{1}/{2}", "region", "s3", "aws4_request");
        private const string ServiceKey = "s3";

        private V4Signer()
        {
        }

        protected override void _DoAuth(HttpRequest request, HttpContext context, IHeaders iheaders)
        {
            IDictionary<string, string> dictionary = this.GetSignature(request, context, iheaders);
            string str = new StringBuilder("AWS4-HMAC-SHA256").Append(" ").Append("Credential=").Append(context.SecurityProvider.Ak).Append("/").Append(dictionary["ShortDate"]).Append(ScopeSuffix).Append(",SignedHeaders=").Append(dictionary["SignedHeaders"]).Append(",Signature=").Append(dictionary["Signature"]).ToString();
            request.Headers.Add("Authorization", str);
        }

        internal string CaculateSignature(string stringToSign, string shortDate, string sk)
        {
            return CommonUtil.ToHex(CommonUtil.HmacSha256(CommonUtil.HmacSha256(CommonUtil.HmacSha256(CommonUtil.HmacSha256(CommonUtil.HmacSha256("AWS4" + sk, shortDate), "region"), "s3"), "aws4_request"), stringToSign));
        }

        public static V4Signer GetInstance()
        {
            return instance;
        }

        internal override IDictionary<string, string> GetSignature(HttpRequest request, HttpContext context, IHeaders iheaders)
        {
            string str;
            request.Headers.Add(iheaders.ContentSha256Header(), "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
            StringBuilder builder = new StringBuilder();
            builder.Append(request.Method).Append("\n");
            builder.Append("/");
            if (!string.IsNullOrEmpty(request.BucketName))
            {
                if (request.PathStyle)
                {
                    builder.Append(CommonUtil.UrlEncode(request.BucketName));
                }
                if (request.ObjectKey != null)
                {
                    builder.Append("/").Append(CommonUtil.UrlEncode(request.ObjectKey, null, "/"));
                }
            }
            builder.Append("\n");
            IDictionary<string, string> collection = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in request.Params)
            {
                if (!string.IsNullOrEmpty(pair.Key))
                {
                    collection.Add(pair.Key, pair.Value);
                }
            }
            List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>(collection);
            collection.Clear();
            kvlist.Sort(Signer.Comparison1 ?? (Signer.Comparison1 = new Comparison<KeyValuePair<string, string>>(Signer.CurSigner.GetSignature1)));
            builder.Append(CommonUtil.ConvertParamsToCanonicalQueryString(kvlist));
            builder.Append("\n");
            collection = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair2 in request.Headers)
            {
                if (!string.IsNullOrEmpty(pair2.Key))
                {
                    string key = pair2.Key.Trim().ToLower();
                    collection.Add(key, pair2.Value);
                }
            }
            kvlist = new List<KeyValuePair<string, string>>(collection);
            collection.Clear();
            kvlist.Sort(Signer.Comparison2 ?? (Signer.Comparison2 = new Comparison<KeyValuePair<string, string>>(Signer.CurSigner.GetSignature2)));
            StringBuilder builder2 = new StringBuilder();
            int num = 0;
            foreach (KeyValuePair<string, string> pair3 in kvlist)
            {
                StringBuilder introduced15 = builder.Append(pair3.Key).Append(":");
                introduced15.Append(pair3.Value).Append("\n");
                builder2.Append(pair3.Key);
                if (num++ != (kvlist.Count - 1))
                {
                    builder2.Append(";");
                }
            }
            builder.Append("\n");
            builder.Append(builder2);
            builder.Append("\n");
            builder.Append("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
            //if (LoggerMgr.IsDebugEnabled)
            //{
            //    LoggerMgr.Debug("CanonicalRequest:" + builder);
            //}
            if (request.Headers.ContainsKey(iheaders.DateHeader()))
            {
                str = request.Headers[iheaders.DateHeader()];
            }
            else if (request.Headers.ContainsKey("Date"))
            {
                str = DateTime.ParseExact(request.Headers["Date"], @"ddd, dd MMM yyyy HH:mm:ss \G\M\T", Constants.CultureInfo).ToString("yyyyMMddTHHmmssZ", Constants.CultureInfo);
            }
            else
            {
                str = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ", Constants.CultureInfo);
            }
            string str2 = str.Substring(0, str.IndexOf("T"));
            StringBuilder builder3 = new StringBuilder("AWS4-HMAC-SHA256").Append("\n").Append(str).Append("\n").Append(str2).Append(ScopeSuffix).Append("\n").Append(CommonUtil.HexSha256(builder.ToString()));
            //if (LoggerMgr.IsDebugEnabled)
            //{
            //    LoggerMgr.Debug("StringToSign:" + builder3.ToString());
            //}
            Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
            dictionary1.Add("Signature", this.CaculateSignature(builder3.ToString(), str2, context.SecurityProvider.Sk));
            dictionary1.Add("ShortDate", str2);
            dictionary1.Add("SignedHeaders", builder2.ToString());
            return dictionary1;
        }

        [Serializable, CompilerGenerated]
        public sealed class Signer
        {
            public static readonly V4Signer.Signer CurSigner = new V4Signer.Signer();
            public static Comparison<KeyValuePair<string, string>> Comparison1;
            public static Comparison<KeyValuePair<string, string>> Comparison2;

            internal int GetSignature1(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
            {
                return string.Compare(x.Key, y.Key, StringComparison.Ordinal);
            }

            internal int GetSignature2(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
            {
                return string.Compare(x.Key, y.Key, StringComparison.Ordinal);
            }
        }
    }
}

