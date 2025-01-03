﻿
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib
{
    public class MQTTCallbackMessage: MQCallBackMessage
    {
        protected MqttApplicationMessageReceivedEventArgs _receivedEventArgs;

        public MqttApplicationMessageReceivedEventArgs ReceivedEventArgs
        {
            get { return _receivedEventArgs; }
        }
       
        public MQTTCallbackMessage(string topic,  byte[] body, 
            MqttApplicationMessageReceivedEventArgs receivedEventArgs) :
            base(topic,  body)
        {
            _receivedEventArgs=receivedEventArgs;

        }

        public override void Commit()
        {
            if(_receivedEventArgs != null && !_receivedEventArgs.AutoAcknowledge) 
            {
                _receivedEventArgs.AcknowledgeAsync(CancellationToken.None).Wait() ;
            }
            
        }

        public override void Dispose()
        {
            _receivedEventArgs = null;
            base.Dispose();
        }

        public override async Task CommitAsync()
        {
            if (_receivedEventArgs != null && !_receivedEventArgs.AutoAcknowledge)
            {
                await _receivedEventArgs.AcknowledgeAsync(CancellationToken.None);
            }
        }

        ~MQTTCallbackMessage()
        {
            Dispose();
        }
    }
}
