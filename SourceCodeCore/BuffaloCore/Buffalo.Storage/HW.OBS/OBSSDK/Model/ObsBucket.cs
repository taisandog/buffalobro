namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class ObsBucket
    {
        public override string ToString()
        {
            object[] objArray1 = new object[] { "BucketName:", this.BucketName, ", CreationDate:", this.CreationDate, ", Location:", this.Location };
            return string.Concat(objArray1);
        }

        public string BucketName { get; internal set; }

        public DateTime? CreationDate { get; internal set; }

        public string Location { get; internal set; }
    }
}

