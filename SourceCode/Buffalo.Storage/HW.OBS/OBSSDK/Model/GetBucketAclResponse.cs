namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketAclResponse : ObsWebServiceResponse
    {
        public OBS.Model.AccessControlList AccessControlList { get; internal set; }
    }
}

