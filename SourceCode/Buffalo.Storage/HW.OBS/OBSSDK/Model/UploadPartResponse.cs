namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class UploadPartResponse : ObsWebServiceResponse
    {
        public string ETag { get; internal set; }

        public int PartNumber { get; internal set; }
    }
}

