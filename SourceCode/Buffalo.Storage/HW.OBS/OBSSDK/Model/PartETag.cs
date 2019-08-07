namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class PartETag : IComparable<PartETag>
    {
        public PartETag()
        {
        }

        public PartETag(int partNumber, string etag)
        {
            this.PartNumber = partNumber;
            this.ETag = etag;
        }

        public int CompareTo(PartETag other)
        {
            if (other == null)
            {
                return 1;
            }
            return this.PartNumber.CompareTo(other.PartNumber);
        }

        public string ETag { get; set; }

        public int PartNumber { get; set; }
    }
}

