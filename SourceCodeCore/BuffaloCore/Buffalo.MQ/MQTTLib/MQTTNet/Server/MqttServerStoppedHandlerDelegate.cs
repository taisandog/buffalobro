﻿using System;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public class MqttServerStoppedHandlerDelegate : IMqttServerStoppedHandler
    {
        private readonly Func<EventArgs, Task> _handler;

        public MqttServerStoppedHandlerDelegate(Action<EventArgs> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _handler = eventArgs =>
            {
                handler(eventArgs);
                return Task.FromResult(0);
            };
        }

        public MqttServerStoppedHandlerDelegate(Func<EventArgs, Task> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public Task HandleServerStoppedAsync(EventArgs eventArgs)
        {
            return _handler(eventArgs);
        }
    }
}
