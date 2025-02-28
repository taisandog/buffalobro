﻿using Buffalo.MQ.MQTTLib.MQTTnet.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttRetainedMessagesManager
    {
        Task Start(IMqttServerOptions options, IMqttNetLogger logger);

        Task LoadMessagesAsync();

        Task ClearMessagesAsync();

        Task HandleMessageAsync(string clientId, MqttApplicationMessage applicationMessage);

        Task<IList<MqttApplicationMessage>> GetMessagesAsync();

        Task<IList<MqttApplicationMessage>> GetSubscribedMessagesAsync(ICollection<MqttTopicFilter> topicFilters);
    }
}
