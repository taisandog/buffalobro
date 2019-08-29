namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketPolicyResponse : ObsWebServiceResponse
    {
        public string Policy { get; internal set; }
    }
}

