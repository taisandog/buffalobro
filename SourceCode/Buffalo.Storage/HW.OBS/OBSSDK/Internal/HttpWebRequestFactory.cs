using OBS;
//using OBS.Internal.Log;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
namespace OBS.Internal
{
    internal static class HttpWebRequestFactory
    {
        private static MethodInfo _addHeaderInternal;
        private static readonly object _lock = new object();
        private static string AddHeaderInternalMethodName = "AddInternal";

        private static void AddHeaders(HttpWebRequest webRequest, HttpRequest request, ObsConfig obsConfig)
        {
            webRequest.Timeout = obsConfig.Timeout;
            webRequest.ReadWriteTimeout = obsConfig.ReadWriteTimeout;
            webRequest.Method = request.Method.ToString();
            webRequest.AllowAutoRedirect = false;
            webRequest.ServicePoint.ReceiveBufferSize = obsConfig.ReceiveBufferSize;
            foreach (KeyValuePair<string, string> pair in request.Headers)
            {
                if (!pair.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
                {
                    object[] parameters = new object[] { pair.Key, pair.Value };
                    GetAddHeaderInternal().Invoke(webRequest.Headers, parameters);
                }
            }
            webRequest.UserAgent = "obs-sdk-.net/3.0.0";
        }

        private static void AddProxy(HttpWebRequest webRequest, ObsConfig obsConfig)
        {
            webRequest.Proxy = null;
            if (!string.IsNullOrEmpty(obsConfig.ProxyHost))
            {
                if (obsConfig.ProxyPort < 0)
                {
                    webRequest.Proxy = new WebProxy(obsConfig.ProxyHost);
                }
                else
                {
                    webRequest.Proxy = new WebProxy(obsConfig.ProxyHost, obsConfig.ProxyPort);
                }
                if (!string.IsNullOrEmpty(obsConfig.ProxyUserName))
                {
                    webRequest.Proxy.Credentials = string.IsNullOrEmpty(obsConfig.ProxyDomain) ? new NetworkCredential(obsConfig.ProxyUserName, obsConfig.ProxyPassword ?? string.Empty) : new NetworkCredential(obsConfig.ProxyUserName, obsConfig.ProxyPassword ?? string.Empty, obsConfig.ProxyDomain);
                }
                webRequest.PreAuthenticate = true;
                //if (LoggerMgr.IsInfoEnabled)
                //{
                //    LoggerMgr.Info(string.Format("Send http request using proxy {0}:{1}", obsConfig.ProxyHost, obsConfig.ProxyPort));
                //}
            }
        }

        internal static HttpWebRequest BuildWebRequest(HttpRequest request, HttpContext context)
        {
            //if (LoggerMgr.IsDebugEnabled)
            //{
            //    LoggerMgr.Debug("Perform http request with url:" + request.GetUrl());
            //}
            ObsConfig obsConfig = context.ObsConfig;
            HttpWebRequest webRequest = WebRequest.Create(string.IsNullOrEmpty(context.RedirectLocation) ? request.GetUrl() : context.RedirectLocation) as HttpWebRequest;
            AddHeaders(webRequest, request, obsConfig);
            AddProxy(webRequest, obsConfig);
            if (webRequest.RequestUri.Scheme.Equals("https") && !obsConfig.ValidateCertificate)
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(HttpWebRequestFactory.ValidateCertificate);
            }
            return webRequest;
        }

        private static MethodInfo GetAddHeaderInternal()
        {
            if (_addHeaderInternal == null)
            {
                object obj2 = _lock;
                lock (obj2)
                {
                    if (_addHeaderInternal == null)
                    {
                        Type[] types = new Type[] { typeof(string), typeof(string) };
                        _addHeaderInternal = typeof(WebHeaderCollection).GetMethod(AddHeaderInternalMethodName, BindingFlags.NonPublic | BindingFlags.Instance, null, types, null);
                    }
                }
            }
            return _addHeaderInternal;
        }

        public static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}

