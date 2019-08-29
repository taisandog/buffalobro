namespace OBS.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LifecycleRule
    {
        private IList<NoncurrentVersionTransition> noncurrentVersionTransitions;
        private IList<Transition> transitions;

        public OBS.Model.Expiration Expiration { get; set; }

        public string Id { get; set; }

        public OBS.Model.NoncurrentVersionExpiration NoncurrentVersionExpiration { get; set; }

        public IList<NoncurrentVersionTransition> NoncurrentVersionTransitions
        {
            get
            {
                return (this.noncurrentVersionTransitions ?? (this.noncurrentVersionTransitions = new List<NoncurrentVersionTransition>()));
            }
            set
            {
                this.noncurrentVersionTransitions = value;
            }
        }

        public string Prefix { get; set; }

        public RuleStatusEnum Status { get; set; }

        public IList<Transition> Transitions
        {
            get
            {
                return (this.transitions ?? (this.transitions = new List<Transition>()));
            }
            set
            {
                this.transitions = value;
            }
        }
    }
}

