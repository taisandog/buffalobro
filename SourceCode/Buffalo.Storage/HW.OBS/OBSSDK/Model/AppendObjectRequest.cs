namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class AppendObjectRequest : PutObjectRequest
    {
        internal override string GetAction()
        {
            return "AppendObject";
        }

        public long Position { get; set; }
    }
}

