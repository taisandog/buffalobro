namespace OBS.Internal
{
    using System;

    internal interface HttpResponseHandler
    {
        void Handle(HttpResponse response);
    }
}

