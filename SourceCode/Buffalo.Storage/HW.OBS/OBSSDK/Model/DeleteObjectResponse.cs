namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class DeleteObjectResponse : ObsWebServiceResponse
    {
        public bool DeleteMarker { get; internal set; }

        public string VersionId { get; internal set; }
    }
}

