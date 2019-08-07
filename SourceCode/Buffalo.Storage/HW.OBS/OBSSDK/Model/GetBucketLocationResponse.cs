namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketLocationResponse : ObsWebServiceResponse
    {
        public string Location { get; internal set; }
    }
}

