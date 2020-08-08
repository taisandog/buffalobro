using Buffalo.ArgCommon;

using Buffalo.MQ.RedisMQ;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        IMqttClientOptions _options = null;
        private static Encoding DefaultEncoding = Encoding.UTF8;
        private Queue<MQRedisMessage> _que = null;
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

        }

        

        protected override APIResault SendMessage(string key, byte[] body)
        {
            
            if (_que != null)
            {
                MQRedisMessage mess = new MQRedisMessage();
                mess.RoutingKey = key;
                mess.Value = body;
                _que.Enqueue(mess);
            }
            else
            {
                MqttClientPublishResult res = _mqttClient.PublishAsync(key, body).Result;
            }
            return ApiCommon.GetSuccess();
        }
        

        protected override void Close()
        {
            if (_mqttClient != null)
            {
                _mqttClient.DisconnectAsync().Wait();
                _mqttClient.Dispose();
                _mqttClient = null;
            }
            if (_que != null)
            {
                _que.Clear();
            }
            _que = null;
            
        }

        public override void DeleteTopic(bool ifUnused)
        {

        }

        public override void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        {
            
        }



        protected override void Open()
        {
            if (_mqttClient == null)
            {
                MqttFactory factory = new MqttFactory();
                _mqttClient = factory.CreateMqttClient() as MqttClient;

                _options = _config.Options.Build();
                MqttClientAuthenticateResult res = _mqttClient.ConnectAsync(_options).Result;

                
            }
            
        }
        

        protected override APIResault StartTran()
        {
            Open();
            _que = new Queue<MQRedisMessage>();
            return ApiCommon.GetSuccess();
        }

        protected override APIResault CommitTran()
        {
            
            if (_que != null)
            {
                Queue<Task<MqttClientPublishResult>> que = new Queue<Task<MqttClientPublishResult>>();
                MQRedisMessage mess = null;
                while (_que.Count > 0)
                {
                    mess = _que.Dequeue();
                    que.Enqueue(_mqttClient.PublishAsync(mess.RoutingKey, mess.Value));
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

        protected override APIResault RoolbackTran()
        {
            if (_que != null)
            {
                _que.Clear();
            }
            return ApiCommon.GetSuccess();
        }
        ~MQTTConnection()
        {
            Close();

        }
    }
}