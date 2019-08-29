namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class LoggingConfiguration : AbstractAccessControlList
    {
        public string Agency { get; set; }

        public string TargetBucketName { get; set; }

        public string TargetPrefix { get; set; }
    }
}

