namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class ReplicationRule
    {
        public string Id { get; set; }

        public string Prefix { get; set; }

        public RuleStatusEnum Status { get; set; }

        public string TargetBucketName { get; set; }

        public StorageClassEnum? TargetStorageClass { get; set; }
    }
}

