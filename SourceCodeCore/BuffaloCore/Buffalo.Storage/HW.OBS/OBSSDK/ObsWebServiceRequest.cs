namespace OBS
{
    using System;

    public abstract class ObsWebServiceRequest
    {
        protected ObsWebServiceRequest()
        {
        }

        internal virtual string GetAction()
        {
            return "ObsWebServiceRequest";
        }
    }
}

