namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class CopyPartResponse : ObsWebServiceResponse
    {
        public string ETag { get; internal set; }

        public DateTime? LastModified { get; internal set; }

        public int PartNumber { get; internal set; }
    }
}

