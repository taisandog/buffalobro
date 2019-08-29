namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;

    public class GetBucketTaggingResponse : ObsWebServiceResponse
    {
        private IList<Tag> tags;

        public IList<Tag> Tags
        {
            get
            {
                return (this.tags ?? (this.tags = new List<Tag>()));
            }
            internal set
            {
                this.tags = value;
            }
        }
    }
}

