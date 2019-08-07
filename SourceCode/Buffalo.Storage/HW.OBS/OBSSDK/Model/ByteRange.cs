namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class ByteRange
    {
        public ByteRange()
        {
        }

        public ByteRange(long start, long end)
        {
            this.Start = start;
            this.End = end;
        }

        public long End { get; set; }

        public long Start { get; set; }
    }
}

