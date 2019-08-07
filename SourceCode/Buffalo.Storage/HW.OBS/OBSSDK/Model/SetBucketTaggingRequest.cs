namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;

    public class SetBucketTaggingRequest : ObsBucketWebServiceRequest
    {
        private IList<Tag> tags;

        internal override string GetAction()
        {
            return "SetBucketTagging";
        }

        public IList<Tag> Tags
        {
            get
            {
                return (this.tags ?? (this.tags = new List<Tag>()));
            }
            set
            {
                this.tags = value;
            }
        }
    }
}

