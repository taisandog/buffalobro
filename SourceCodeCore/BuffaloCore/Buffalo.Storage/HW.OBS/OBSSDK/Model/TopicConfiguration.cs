namespace OBS.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TopicConfiguration
    {
        private List<EventTypeEnum> _events;
        public List<FilterRule> filterRules;

        public List<EventTypeEnum> Events
        {
            get
            {
                return (this._events ?? (this._events = new List<EventTypeEnum>()));
            }
            set
            {
                this._events = value;
            }
        }

        public List<FilterRule> FilterRules
        {
            get
            {
                return (this.filterRules ?? (this.filterRules = new List<FilterRule>()));
            }
            set
            {
                this.filterRules = value;
            }
        }

        public string Id { get; set; }

        public string Topic { get; set; }
    }
}

