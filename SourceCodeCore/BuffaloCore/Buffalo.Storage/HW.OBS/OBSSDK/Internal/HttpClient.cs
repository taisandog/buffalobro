namespace OBS.Internal
{
    using OBS;
    using OBS.Internal.Auth;
    //using OBS.Internal.Log;
    using OBS.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class HttpClient
    {
        private HttpResponseHandler handler;

        internal HttpClient(ObsConfig obsConfig)
        {
            ServicePointManager.DefaultConnectionLimit = obsConfig.ConnectionLimit;
            ServicePointManager.MaxServicePointIdleTime = obsConfig.MaxIdleTime;
            ServicePointManager.Expect100Continue = false;
            this.AuthType = obsConfig.AuthType;
        }

        internal HttpResponse doRequest(HttpRequest httpRequest, HttpContext context)
        {
            HttpResponse response;
            DateTime now = DateTime.Now;
            this.GetSigner().DoAuth(httpRequest, context, this.GetIHeaders());
            HttpWebRequest webRequest = HttpWebRequestFactory.BuildWebRequest(httpRequest, context);
            SetContent(webRequest, httpRequest, context.ObsConfig);
            try
            {
                response = new HttpResponse(webRequest.GetResponse() as HttpWebResponse);
            }
            catch (WebException exception)
            {
                if (!(exception.Response is HttpWebResponse))
                {
                    webRequest.Abort();
                    throw exception;
                }
                response = new HttpResponse(exception, webRequest);
            }
            catch (Exception exception2)
            {
                webRequest.Abort();
                throw exception2;
            }
            finally
            {
                //if (LoggerMgr.IsInfoEnabled)
                //{
                //    LoggerMgr.Info(string.Format("Send http request end, cost {0} ms", (DateTime.Now.Ticks - now.Ticks) / 0x2710L));
                //}
            }
            return response;
        }

        internal IHeaders GetIHeaders()
        {
            if (this.AuthType <= AuthTypeEnum.V4)
            {
                return V2Headers.GetInstance();
            }
            return ObsHeaders.GetInstance();
        }

        internal Signer GetSigner()
        {
            AuthTypeEnum authType = this.AuthType;
            if (authType != AuthTypeEnum.V2)
            {
                if (authType == AuthTypeEnum.V4)
                {
                    return V4Signer.GetInstance();
                }
                return ObsSigner.GetInstance();
            }
            return V2Signer.GetInstance();
        }

        private ObsException ParseObsException(HttpResponse response, string message)
        {
            return this.ParseObsException(response, message, null);
        }

        private ObsException ParseObsException(HttpResponse response, string message, Exception ex)
        {
            ObsException exception = new ObsException(message, ex);
            if (response != null)
            {
                string str;
                exception.StatusCode = response.StatusCode;
                try
                {
                    CommonParser.ParseErrorResponse(response.Content, exception);
                }
                catch (Exception exception2)
                {
                    //if (LoggerMgr.IsErrorEnabled)
                    //{
                    //    LoggerMgr.Error(exception2.Message, exception2);
                    //}
                }
                if (response.Headers.TryGetValue(this.GetIHeaders().RequestId2Header(), out str))
                {
                    exception.ObsId2 = str;
                }
                if (string.IsNullOrEmpty(exception.RequestId) && response.Headers.TryGetValue(this.GetIHeaders().RequestIdHeader(), out str))
                {
                    exception.RequestId = str;
                }
                response.Abort();
            }
            exception.ErrorType = ErrorType.Receiver;
            return exception;
        }

        internal HttpResponse PerformRequest(HttpRequest request, HttpContext context)
        {
            CommonUtil.RenameHeaders(request, this.GetIHeaders().HeaderPrefix(), this.GetIHeaders().HeaderMetaPrefix());
            context.Handlers.Add(this.DefaultResponseHandler);
            //if (LoggerMgr.IsDebugEnabled)
            //{
            //    LoggerMgr.Debug(string.Format("Perform {0} request for {1}", request.Method, request.GetUrl()));
            //    LoggerMgr.Debug("Perform http request with headers:" + CommonUtil.ConvertHeadersToString(request.Headers));
            //}
            HttpResponse response = this.PerformRequest(request, context, 0);
            using (IEnumerator<HttpResponseHandler> enumerator = context.Handlers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Handle(response);
                }
            }
            return response;
        }

        internal HttpResponse PerformRequest(HttpRequest request, HttpContext context, int retryCount)
        {
            HttpResponse response2;
            long originPos = -1L;
            HttpResponse response = null;
            try
            {
                if ((request.Content != null) && request.Content.CanSeek)
                {
                    originPos = request.Content.Position;
                }
                response = this.doRequest(request, context);
                int num2 = Convert.ToInt32(response.StatusCode);
                //if (LoggerMgr.IsDebugEnabled)
                //{
                //    LoggerMgr.Debug(string.Format("Response with statusCode {0} and headers {1}", num2, CommonUtil.ConvertHeadersToString(response.Headers)));
                //}
                int maxErrorRetry = context.ObsConfig.MaxErrorRetry;
                if (((num2 >= 300) && (num2 < 400)) && (num2 != 0x130))
                {
                    if (response.Headers.ContainsKey("Location"))
                    {
                        string str = response.Headers["Location"];
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (str.IndexOf("?") < 0)
                            {
                                str = str + "?" + CommonUtil.ConvertParamsToString(request.Params);
                            }
                            //if (LoggerMgr.IsWarnEnabled)
                            //{
                            //    LoggerMgr.Warn(string.Format("Redirect to {0}", str));
                            //}
                            context.RedirectLocation = str;
                            retryCount--;
                            if (this.ShouldRetry(request, null, retryCount, maxErrorRetry))
                            {
                                this.PrepareRetry(request, retryCount, originPos, false);
                                return this.PerformRequest(request, context, ++retryCount);
                            }
                            if (retryCount > maxErrorRetry)
                            {
                                throw this.ParseObsException(response, "Exceeded 3xx redirect limit");
                            }
                        }
                    }
                    throw this.ParseObsException(response, "Try to redirect, but location is null!");
                }
                if (((num2 >= 400) && (num2 < 500)) || (num2 == 0x130))
                {
                    ObsException exception = this.ParseObsException(response, "Request error");
                    if ("RequestTimeout".Equals(exception.ErrorCode))
                    {
                        if (this.ShouldRetry(request, null, retryCount, maxErrorRetry))
                        {
                            //if (LoggerMgr.IsWarnEnabled)
                            //{
                            //    LoggerMgr.Warn("Retrying connection that failed with RequestTimeout error");
                            //}
                            this.PrepareRetry(request, retryCount, originPos, false);
                            return this.PerformRequest(request, context, ++retryCount);
                        }
                        //if ((retryCount > maxErrorRetry) && LoggerMgr.IsErrorEnabled)
                        //{
                        //    LoggerMgr.Error("Exceeded maximum number of retries for RequestTimeout errors");
                        //}
                    }
                    throw exception;
                }
                if (num2 >= 500)
                {
                    if (this.ShouldRetry(request, null, retryCount, maxErrorRetry))
                    {
                        this.PrepareRetry(request, retryCount, originPos, true);
                        return this.PerformRequest(request, context, ++retryCount);
                    }
                    //if ((retryCount > maxErrorRetry) && LoggerMgr.IsErrorEnabled)
                    //{
                    //    LoggerMgr.Error("Encountered too many 5xx errors");
                    //}
                    throw this.ParseObsException(response, "Request error");
                }
                response2 = response;
            }
            catch (Exception exception2)
            {
                try
                {
                    if (exception2 is ObsException)
                    {
                        //if (LoggerMgr.IsErrorEnabled)
                        //{
                        //    LoggerMgr.Error("Rethrowing as a ObsException error in PerformRequest", exception2);
                        //}
                        throw exception2;
                    }
                    if (this.ShouldRetry(request, exception2, retryCount, context.ObsConfig.MaxErrorRetry))
                    {
                        this.PrepareRetry(request, retryCount, originPos, true);
                        response2 = this.PerformRequest(request, context, ++retryCount);
                    }
                    else
                    {
                        //if ((retryCount > context.ObsConfig.MaxErrorRetry) && LoggerMgr.IsWarnEnabled)
                        //{
                        //    LoggerMgr.Warn("Too many errors excced the max error retry count", exception2);
                        //}
                        //if (LoggerMgr.IsErrorEnabled)
                        //{
                        //    LoggerMgr.Error("Rethrowing as a ObsException error in PerformRequest", exception2);
                        //}
                        throw this.ParseObsException(response, exception2.Message, exception2);
                    }
                }
                finally
                {
                    if (response != null)
                    {
                        try
                        {
                            response.Dispose();
                        }
                        catch (Exception exception3)
                        {
                            //if (LoggerMgr.IsErrorEnabled)
                            //{
                            //    LoggerMgr.Error(exception3.Message, exception3);
                            //}
                        }
                    }
                }
            }
            return response2;
        }

        private void PrepareRetry(HttpRequest request, int retryCount, long originPos, bool sleep)
        {
            if (((request.Content != null) && (originPos >= 0L)) && request.Content.CanSeek)
            {
                request.Content.Seek(originPos, SeekOrigin.Begin);
            }
            if (sleep)
            {
                int num = ((int) Math.Pow(2.0, (double) retryCount)) * 300;
                //if (LoggerMgr.IsWarnEnabled)
                //{
                //    LoggerMgr.Warn(string.Format("Send http request error, will retry in {0} ms", num));
                //}
                Thread.Sleep(num);
            }
        }

        private static void SetContent(HttpWebRequest webRequest, HttpRequest httpRequest, ObsConfig obsConfig)
        {
            Stream content = httpRequest.Content;
            if (((httpRequest.Method == HttpVerb.PUT) || (httpRequest.Method == HttpVerb.POST)) || (httpRequest.Method == HttpVerb.DELETE))
            {
                if (content == null)
                {
                    content = new MemoryStream();
                }
                long num = -1L;
                if (httpRequest.Headers.ContainsKey("Content-Length"))
                {
                    num = long.Parse(httpRequest.Headers["Content-Length"]);
                }
                if (num >= 0L)
                {
                    webRequest.ContentLength = num;
                }
                else
                {
                    webRequest.SendChunked = true;
                    webRequest.AllowWriteStreamBuffering = false;
                }
                using (Stream stream2 = webRequest.GetRequestStream())
                {
                    if (!webRequest.SendChunked)
                    {
                        CommonUtil.WriteTo(content, stream2, webRequest.ContentLength, obsConfig.BufferSize);
                    }
                    else
                    {
                        CommonUtil.WriteTo(content, stream2, obsConfig.BufferSize);
                    }
                }
            }
        }

        private bool ShouldRetry(HttpRequest request, Exception ex, int retryCount, int maxErrorRetry)
        {
            if ((retryCount > maxErrorRetry) || !request.IsRepeatable)
            {
                return false;
            }
            if ((ex != null) && !(ex is IOException))
            {
                return false;
            }
            return true;
        }

        internal AuthTypeEnum AuthType { get; set; }

        internal HttpResponseHandler DefaultResponseHandler
        {
            get
            {
                return (this.handler ?? (this.handler = new MergeResponseHeaderHandler(this.GetIHeaders())));
            }
        }
    }
}

