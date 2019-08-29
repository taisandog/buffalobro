namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetObjectAclResponse : ObsWebServiceResponse
    {
        public OBS.Model.AccessControlList AccessControlList { get; internal set; }
    }
}

