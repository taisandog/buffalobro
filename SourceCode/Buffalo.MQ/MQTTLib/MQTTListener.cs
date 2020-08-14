using Buffalo.ArgCommon;
using Buffalo.Kernel;
using Buffalo.MQ.MQTTLib.MQTTnet;
using Buffalo.MQ.MQTTLib.MQTTnet.Adapter;
using Buffalo.MQ.MQTTLib.MQTTnet.Client;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Connecting;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Disconnecting;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Options;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Receiving;
using Buffalo.MQ.MQTTLib.MQTTnet.Client.Subscribing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib
{
    public class MQTTListener : MQListener
    {
        private MQTTConfig _config;
        MqttClient _mqttClient = null;
        IMqttClientOptions _options = null;
        private static Encoding DefaultEncoding = Encoding.UTF8;
        IEnumerable<MQOffestInfo> _lstTopic = null;
        bool _isRunning = false;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public MQTTListener(MQTTConfig config)
        {
            _config = config;
        }


        /// <summary>
        /// 打来连接
        /// </summary>
        public void Open()
        {
            if (_mqttClient == null)
            {
                _isRunning = true;
                ResetWait();
                try
                {
                    MqttFactory factory = new MqttFactory();
                    _mqttClient = factory.CreateMqttClient() as MqttClient;

                    _options = _config.Options.Build();


                    _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(new Func<MqttClientConnectedEventArgs, Task>(Connected));
                    _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(new Func<MqttClientDisconnectedEventArgs, Task>(Disconnected));
                    _mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(new Action<MqttApplicationMessageReceivedEventArgs>(MqttApplicationMessageReceived));
                    MqttClientAuthenticateResult res = _mqttClient.ConnectAsync(_options).Result;
                    if (res.ResultCode != MqttClientConnectResultCode.Success)
                    {
                        throw new MqttConnectingFailedException(res);
                    }
                }
                finally 
                {
                    SetWait();
                }
                
            }


        }

        /// <summary>
        /// 接收消息触发事件
        /// </summary>
        /// <param name="e"></param>
        private void MqttApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                byte[] value = e.ApplicationMessage.Payload;
                string topic = e.ApplicationMessage.Topic;
                string qos = e.ApplicationMessage.QualityOfServiceLevel.ToString();
                string retained = e.ApplicationMessage.Retain.ToString();

                CallBack(topic, topic, value, 0, 0);
                
            }
            catch (Exception exp)
            {
                OnException(exp);
            }

        }
        private async Task Connected(MqttClientConnectedEventArgs e)
        {
            try
            {

                MqttClientSubscribeOptionsBuilder subBuilder = new MqttClientSubscribeOptionsBuilder();
                foreach (MQOffestInfo info in _lstTopic)
                {
                    subBuilder.WithTopicFilter(info.Key, _config.QualityOfServiceLevel, _config.NoLocal, _config.RetainAsPublished, _config.RetainHandling);
                }
                await _mqttClient.SubscribeAsync(subBuilder.Build());

            }
            catch (Exception exp)
            {
                OnException(exp);
            }
            
        }
        private async Task Disconnected(MqttClientDisconnectedEventArgs e)
        {
            if (!_isRunning) 
            {
                return;
            }
            try
            {
                
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await _mqttClient.ConnectAsync(_options);
                }
                catch (Exception exp)
                {
                    OnException(exp);
                }
            }
            catch (Exception exp)
            {
                OnException(exp);
            }
        }


        public override void StartListend(IEnumerable<string> listenKeys)
        {
            StartListend(MQUnit.GetLintenOffest(listenKeys));
            
        }
        public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        {
            _lstTopic = listenKeys;
            
            Open();
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            _isRunning = false;
            if (_mqttClient != null)
            {
                try
                {
                    _mqttClient.DisconnectAsync().Wait();
                    _mqttClient.Dispose();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
                _mqttClient = null;
            }
        }

        public override void Dispose()
        {
            Close();
        }



        ~MQTTListener()
        {
            Close();
            GC.SuppressFinalize(this);
        }
    }
}
