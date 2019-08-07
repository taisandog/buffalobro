namespace OBS.Model
{
    using OBS;
    using System;

    public class ListBucketsRequest : ObsWebServiceRequest
    {
        private bool queryLocation = true;

        internal override string GetAction()
        {
            return "ListBuckets";
        }

        public bool IsQueryLocation
        {
            get
            {
                return this.queryLocation;
            }
            set
            {
                this.queryLocation = value;
            }
        }
    }
}

