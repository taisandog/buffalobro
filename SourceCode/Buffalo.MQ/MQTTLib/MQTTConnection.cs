using Buffalo.ArgCommon;
using Buffalo.MQ.MQTTLib.MQTTnet;
using Buffalo.MQ.MQTTLib.MQTTnet.Client;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Connecting;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Disconnecting;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Options;
using Buffalo.MQ.RedisMQ;
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
                _mqttClient.PublishAsync(key, body);
            }
            return ApiCommon.GetSuccess();
        }
        

        protected override void Close()
        {
            if (_mqttClient != null)
            {
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

                //_mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(new Func<MqttClientConnectedEventArgs, Task>(Connected));
                //_mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(new Func<MqttClientDisconnectedEventArgs, Task>(Disconnected));

            }
            
        }
        //private async Task Connected(MqttClientConnectedEventArgs e)
        //{
        //    try
        //    {
        //        //List<MqttTopicFilter> listTopic = new List<MqttTopicFilter>();
        //        //if (listTopic.Count() <= 0)
        //        //{
        //        //    MqttTopicFilter topicFilterBulder = new MqttTopicFilterBuilder().WithTopic(Topic).Build();
        //        //    listTopic.Add(topicFilterBulder);
        //        //    Console.WriteLine("Connected >>Subscribe " + Topic);
        //        //}
        //        //await _mqttClient.SubscribeAsync(listTopic.ToArray());
        //        //Console.WriteLine("Connected >>Subscribe Success");
        //    }
        //    catch (Exception exp)
        //    {
        //        //Console.WriteLine(exp.Message);
        //    }
        //}
        //private async Task Disconnected(MqttClientDisconnectedEventArgs e)
        //{
        //    try
        //    {
        //        //Console.WriteLine("Disconnected >>Disconnected Server");
        //        //await Task.Delay(TimeSpan.FromSeconds(5));
        //        //try
        //        //{
        //        //    await _mqttClient.ConnectAsync(_options);
        //        //}
        //        //catch (Exception exp)
        //        //{
        //        //    Console.WriteLine("Disconnected >>Exception " + exp.Message);
        //        //}
        //    }
        //    catch (Exception exp)
        //    {
        //        //Console.WriteLine(exp.Message);
        //    }
        //}

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
                MQRedisMessage mess = null;
                while (_que.Count > 0)
                {
                    mess = _que.Dequeue();
                    _mqttClient.PublishAsync(mess.RoutingKey, mess.Value);
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