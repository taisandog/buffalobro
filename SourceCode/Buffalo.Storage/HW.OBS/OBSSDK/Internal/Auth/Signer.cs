namespace OBS.Internal.Auth
{
    using OBS.Internal;
    using System;
    using System.Collections.Generic;

    internal abstract class Signer
    {
        protected Signer()
        {
        }

        protected abstract void _DoAuth(HttpRequest request, HttpContext context, IHeaders iheaders);
        internal void DoAuth(HttpRequest request, HttpContext context, IHeaders iheaders)
        {
            if (request.Headers.ContainsKey("Authorization"))
            {
                request.Headers.Remove("Authorization");
            }
            if (request.Headers.ContainsKey("Date"))
            {
                request.Headers.Remove("Date");
            }
            if (!request.Headers.ContainsKey(iheaders.DateHeader()))
            {
                request.Headers.Add("Date", DateTime.UtcNow.ToString(@"ddd, dd MMM yyyy HH:mm:ss \G\M\T", Constants.CultureInfo));
            }
            string endpoint = string.IsNullOrEmpty(context.RedirectLocation) ? request.Endpoint : context.RedirectLocation;
            request.Headers["Host"] = request.GetHost(endpoint);
            if (!string.IsNullOrEmpty(context.SecurityProvider.Ak) && !string.IsNullOrEmpty(context.SecurityProvider.Sk))
            {
                if (!string.IsNullOrEmpty(context.SecurityProvider.Token) && !request.Headers.ContainsKey(iheaders.SecurityTokenHeader()))
                {
                    request.Headers.Add(iheaders.SecurityTokenHeader(), context.SecurityProvider.Token.Trim());
                }
                this._DoAuth(request, context, iheaders);
            }
        }

        internal abstract IDictionary<string, string> GetSignature(HttpRequest request, HttpContext context, IHeaders iheaders);
    }
}

