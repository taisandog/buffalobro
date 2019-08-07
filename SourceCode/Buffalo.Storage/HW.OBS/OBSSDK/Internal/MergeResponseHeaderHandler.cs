namespace OBS.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    internal class MergeResponseHeaderHandler : HttpResponseHandler
    {
        private IHeaders iheaders;

        public MergeResponseHeaderHandler(IHeaders iheaders)
        {
            this.iheaders = iheaders;
        }

        public void Handle(HttpResponse response)
        {
            if ((response != null) && (response.HttpWebResponse != null))
            {
                WebHeaderCollection headers = response.HttpWebResponse.Headers;
                IDictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < headers.Count; i++)
                {
                    string name = headers.Keys[i];
                    string uriToDecode = headers.Get(name);
                    if (!string.IsNullOrEmpty(name) && (uriToDecode != null))
                    {
                        if (name.StartsWith(this.iheaders.HeaderMetaPrefix(), StringComparison.OrdinalIgnoreCase))
                        {
                            name = CommonUtil.UrlDecode(name);
                        }
                        uriToDecode = CommonUtil.UrlDecode(uriToDecode);
                        if (dictionary.ContainsKey(name))
                        {
                            string str3 = dictionary[name] + "," + uriToDecode;
                            dictionary[name] = str3;
                        }
                        else
                        {
                            dictionary.Add(name, uriToDecode);
                        }
                    }
                }
                response.Headers = dictionary;
            }
        }
    }
}

