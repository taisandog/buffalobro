using Buffalo.ArgCommon;

using Buffalo.MQ.RedisMQ;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Formatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib
{
    /// <summary>
    /// MQTT连接
    /// </summary>
    public class MQTTConnection : MQConnection
    {
        private MQTTConfig _config;
        MqttClient _mqttClient = null;
        MqttClientOptions _options = null;
        //private static Encoding DefaultEncoding = Encoding.UTF8;
        private Queue<MqttApplicationMessage> _que = null;
        /// <summary>
        /// 消息创建器
        /// </summary>
        private MqttApplicationMessageBuilder _messageBuilder = null;

        /// <summary>
        /// 消息创建器
        /// </summary>
        public MqttApplicationMessageBuilder MessageBuilder 
        {
            get { return _messageBuilder; }
        }
        public override bool IsOpen
        {
            get
            {
                return _mqttClient != null && _mqttClient.IsConnected;
            }
        }
        private TimeSpan _timeout;
        public MQTTConnection(MQTTConfig config)
        {
            _config = config;
            _messageBuilder = CreateMessageBuilder();
        }

        private MqttApplicationMessageBuilder CreateMessageBuilder() 
        {
            MqttApplicationMessageBuilder messageBuilder = new MqttApplicationMessageBuilder();
            if (_config.ProtocolVersion == MqttProtocolVersion.V500)
            {
                messageBuilder.WithRetainFlag(_config.RetainAsPublished.GetValueOrDefault());
            }
            messageBuilder.WithQualityOfServiceLevel(_config.QualityOfServiceLevel);
            
            return messageBuilder;
        }

        protected override APIResault SendMessage(string key, byte[] body)
        {
            MqttApplicationMessage message = BuildMessage(key, body);
            return SendMess(message);
        }

        protected override APIResault SendMessage(MQSendMessage mess)
        {
            MQTTMessage msg= mess as MQTTMessage;
            if(msg == null) 
            {
                return ApiCommon.GetFault("mess must to MQTTMessage"); 
            }
            MqttApplicationMessage message=msg.Message;
            
            return SendMess(message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private APIResault SendMess(MqttApplicationMessage message) 
        {
            if (message == null)
            {
                return ApiCommon.GetFault("mess.Message can't be null");
            }
            if (_que != null)
            {
                _que.Enqueue(message);
            }
            else
            {
                message.Retain = false;
                MqttClientPublishResult res = _mqttClient.PublishAsync(message).Result;

                if (res.ReasonCode != MqttClientPublishReasonCode.Success)
                {
                    return ApiCommon.GetFault(res.ReasonString, res);
                }
            }
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<APIResault> SendMessAsync(MqttApplicationMessage message)
        {
            if (message == null)
            {
                return ApiCommon.GetFault("mess.Message can't be null");
            }
            if (_que != null)
            {
                _que.Enqueue(message);
            }
            else
            {
                message.Retain = false;
                MqttClientPublishResult res = await _mqttClient.PublishAsync(message);

                if (res.ReasonCode != MqttClientPublishReasonCode.Success)
                {
                    return ApiCommon.GetFault(res.ReasonString, res);
                }
            }
            return ApiCommon.GetSuccess();
        }
        private MqttApplicationMessage BuildMessage(string key, byte[] body) 
        {

            _messageBuilder.WithTopic(key).WithPayload(body);
            
            return _messageBuilder.Build();
        }

        public override void Close()
        {
            CloseAsync().Wait();
        }

        //public override void DeleteTopic(bool ifUnused)
        //{

        //}

        //public override void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        //{
            
        //}



        protected override void Open()
        {
            if (IsOpen)
            {
                return;
            }
            lock(this){
                if (!IsOpen)
                {
                    MqttFactory factory = new MqttFactory();
                    _mqttClient = factory.CreateMqttClient() as MqttClient;

                    _options = _config.Options.Build();

                    MqttClientConnectResult res = _mqttClient.ConnectAsync(_options).Result;
                    if (res.ResultCode != MqttClientConnectResultCode.Success)
                    {
                        throw new MqttConnectingFailedException("Connect Fault", null, res);
                    }


                }
            }
        }
        protected override async Task OpenAsync()
        {
           Open();
        }

        protected override APIResault StartTran()
        {
            Open();
            _que = new Queue<MqttApplicationMessage>();
            return ApiCommon.GetSuccess();
        }

        protected override APIResault CommitTran()
        {
            
            if (_que != null)
            {
                Queue<Task<MqttClientPublishResult>> que = new Queue<Task<MqttClientPublishResult>>();
                MqttApplicationMessage mess = null;
                while (_que.Count > 0)
                {
                    mess = _que.Dequeue();
                    que.Enqueue(_mqttClient.PublishAsync(mess));
                }

                Task<MqttClientPublishResult> tmp = null;
                while (que.Count > 0)
                {
                    tmp = que.Dequeue();
                    MqttClientPublishResult res=tmp.Result;
                }
            }
            return ApiCommon.GetSuccess();
        }
        protected override async Task<APIResault> CommitTranAsync()
        {
            if (_que != null)
            {
                MqttApplicationMessage mess = null;
                while (_que.Count > 0)
                {
                    mess = _que.Dequeue();
                    await _mqttClient.PublishAsync(mess);
                }

                
            }
            return ApiCommon.GetSuccess();
        }
        protected override APIResault RoolbackTran()
        {
            if (_que != null)
            {
                _que.Clear();
            }
            return ApiCommon.GetSuccess();
        }

        protected override async Task<APIResault> SendMessageAsync(MQSendMessage mess)
        {
            MQTTMessage msg = mess as MQTTMessage;
            if (msg == null)
            {
                return ApiCommon.GetFault("mess must to MQTTMessage");
            }
            MqttApplicationMessage message = msg.Message;

            return await SendMessAsync(message);
        }

        protected async override Task<APIResault> SendMessageAsync(string key, byte[] body)
        {
            MqttApplicationMessage message = BuildMessage(key, body);
            if (message == null)
            {
                return ApiCommon.GetFault("mess must to MQTTMessage");
            }
            return await SendMessAsync(message);
        }

        protected override async Task<APIResault> StartTranAsync()
        {
            await OpenAsync();
           
            _que = new Queue<MqttApplicationMessage>();
            return ApiCommon.GetSuccess();
        }

        protected override async Task<APIResault> RoolbackTranAsync()
        {
            return RoolbackTran();
        }

        public override async Task CloseAsync()
        {
            if (_mqttClient != null)
            {
                await _mqttClient.DisconnectAsync();
                
                _mqttClient.Dispose();
            }
            _mqttClient = null;
            if (_que != null)
            {
                _que.Clear();
            }
            _que = null;
        }

        ~MQTTConnection()
        {
            Close();

        }
    }
}