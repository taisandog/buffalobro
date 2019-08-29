namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ListBucketsResponse : ObsWebServiceResponse
    {
        private IList<ObsBucket> buckets;

        public IList<ObsBucket> Buckets
        {
            get
            {
                return (this.buckets ?? (this.buckets = new List<ObsBucket>()));
            }
            internal set
            {
                this.buckets = value;
            }
        }

        public OBS.Model.Owner Owner { get; internal set; }
    }
}

