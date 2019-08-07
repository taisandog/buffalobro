namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class Redirect : RedirectBasic
    {
        public string HttpRedirectCode { get; set; }

        public string ReplaceKeyPrefixWith { get; set; }

        public string ReplaceKeyWith { get; set; }
    }
}

