namespace OBS.Model
{
    using System;
    using System.Collections.Generic;

    public class NotificationConfiguration
    {
        private IList<TopicConfiguration> topicConfigurations;

        public IList<TopicConfiguration> TopicConfigurations
        {
            get
            {
                return (this.topicConfigurations ?? (this.topicConfigurations = new List<TopicConfiguration>()));
            }
            set
            {
                this.topicConfigurations = value;
            }
        }
    }
}

