namespace OBS.Internal
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    internal static class CommonParser
    {
        public static void ParseErrorResponse(Stream stream, ObsException exception)
        {
            if (stream != null)
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    while (reader.Read())
                    {
                        if ("Code".Equals(reader.Name))
                        {
                            exception.ErrorCode = reader.ReadString();
                        }
                        else
                        {
                            if ("Message".Equals(reader.Name))
                            {
                                exception.ErrorMessage = reader.ReadString();
                                continue;
                            }
                            if ("RequestId".Equals(reader.Name))
                            {
                                exception.RequestId = reader.ReadString();
                                continue;
                            }
                            if ("HostId".Equals(reader.Name))
                            {
                                exception.HostId = reader.ReadString();
                            }
                        }
                    }
                }
            }
        }

        public static void ParseObsWebServiceResponse(HttpResponse httpResponse, ObsWebServiceResponse response, IHeaders iheaders)
        {
            response.StatusCode = httpResponse.StatusCode;
            if (httpResponse.Headers.ContainsKey(iheaders.RequestIdHeader()))
            {
                response.RequestId = httpResponse.Headers[iheaders.RequestIdHeader()];
            }
            if (httpResponse.Headers.ContainsKey("Content-Length"))
            {
                response.ContentLength = Convert.ToInt64(httpResponse.Headers["Content-Length"]);
            }
            foreach (KeyValuePair<string, string> pair in httpResponse.Headers)
            {
                string key = pair.Key;
                if (key.StartsWith(iheaders.HeaderMetaPrefix()))
                {
                    key = key.Substring(iheaders.HeaderMetaPrefix().Length);
                }
                else if (key.StartsWith(iheaders.HeaderPrefix()))
                {
                    key = key.Substring(iheaders.HeaderPrefix().Length);
                }
                response.Headers.Add(key, pair.Value);
            }
        }
    }
}

