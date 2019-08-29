namespace OBS.Internal.Auth
{
    using OBS.Internal;
    //using OBS.Internal.Log;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal abstract class AbstractSigner : Signer
    {
        protected AbstractSigner()
        {
        }

        protected override void _DoAuth(HttpRequest request, HttpContext context, IHeaders iheaders)
        {
            string str = this.GetSignature(request, context, iheaders)["Signature"];
            string str2 = new StringBuilder(this.GetAuthPrefix()).Append(" ").Append(context.SecurityProvider.Ak).Append(":").Append(str).ToString();
            request.Headers.Add("Authorization", str2);
        }

        protected abstract string GetAuthPrefix();
        internal override IDictionary<string, string> GetSignature(HttpRequest request, HttpContext context, IHeaders iheaders)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(request.Method.ToString()).Append("\n");
            string key = "Date".ToLower();
            string str2 = "Content-Type".ToLower();
            string str3 = "Content-MD5".ToLower();
            string str4 = iheaders.HeaderPrefix();
            string str5 = iheaders.HeaderMetaPrefix();
            IDictionary<string, string> collection = new Dictionary<string, string>();
            if (request.Headers.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in request.Headers)
                {
                    if (!string.IsNullOrEmpty(pair.Key))
                    {
                        string str6 = pair.Key.Trim().ToLower();
                        if ((str6.StartsWith(str4) || str6.Equals(str2)) || str6.Equals(str3))
                        {
                            collection.Add(str6, pair.Value);
                        }
                    }
                }
            }
            if (request.Headers.ContainsKey(key))
            {
                collection.Add(key, request.Headers[key]);
            }
            else
            {
                collection.Add(key, "");
            }
            if (!collection.ContainsKey(str3))
            {
                collection.Add(str3, "");
            }
            if (!collection.ContainsKey(str2))
            {
                collection.Add(str2, "");
            }
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(collection);
            collection.Clear();
            list.Sort(Signer.Comparison1 ?? (Signer.Comparison1 = new Comparison<KeyValuePair<string, string>>(Signer.CurSigner.GetSignature1)));
            foreach (KeyValuePair<string, string> pair2 in list)
            {
                if (pair2.Key.StartsWith(str5))
                {
                    StringBuilder introduced17 = builder.Append(pair2.Key).Append(":");
                    introduced17.Append(pair2.Value.Trim());
                }
                else if (pair2.Key.StartsWith(str4))
                {
                    StringBuilder introduced18 = builder.Append(pair2.Key).Append(":");
                    introduced18.Append(pair2.Value);
                }
                else
                {
                    builder.Append(pair2.Value);
                }
                builder.Append("\n");
            }
            list.Clear();
            builder.Append("/");
            if (!string.IsNullOrEmpty(request.BucketName))
            {
                builder.Append(CommonUtil.UrlEncode(request.BucketName));
                if (!request.PathStyle)
                {
                    builder.Append("/");
                }
                if (request.ObjectKey != null)
                {
                    if (request.PathStyle)
                    {
                        builder.Append("/");
                    }
                    builder.Append(CommonUtil.UrlEncode(request.ObjectKey, null, "/"));
                }
            }
            if (request.Params.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair3 in request.Params)
                {
                    if (!string.IsNullOrEmpty(pair3.Key) && Constants.AllowedResourceParameters.Contains(pair3.Key.ToLower()))
                    {
                        collection.Add(pair3.Key, pair3.Value);
                    }
                }
            }
            list = new List<KeyValuePair<string, string>>(collection);
            collection.Clear();
            list.Sort(Signer.Comparison2 ?? (Signer.Comparison2 = new Comparison<KeyValuePair<string, string>>(Signer.CurSigner.GetSignature2)));
            if (list.Count > 0)
            {
                bool flag = true;
                foreach (KeyValuePair<string, string> pair4 in list)
                {
                    if (flag)
                    {
                        builder.Append("?");
                        flag = false;
                    }
                    else
                    {
                        builder.Append("&");
                    }
                    builder.Append(pair4.Key);
                    if (pair4.Value != null)
                    {
                        builder.Append("=").Append(pair4.Value);
                    }
                }
            }
            //if (LoggerMgr.IsDebugEnabled)
            //{
            //    LoggerMgr.Debug("StringToSign:" + builder.ToString());
            //}
            Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
            dictionary1.Add("Signature", Convert.ToBase64String(CommonUtil.HmacSha1(context.SecurityProvider.Sk, builder.ToString())));
            return dictionary1;
        }

        [Serializable, CompilerGenerated]
        private sealed class Signer
        {
            public static readonly AbstractSigner.Signer CurSigner = new AbstractSigner.Signer();

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

