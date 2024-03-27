using Buffalo.ArgCommon;
using Buffalo.Kernel;
using MQTTnet;
using MQTTnet.Adapter;

using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib
{

    public class MQTTListener : MQListener
    {
        public event Func<MqttClientDisconnectedEventArgs, Task> OnDisconnected;
        private MQTTConfig _config;
        MqttClient _mqttClient2 = null;
        MqttClientOptions _options = null;
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
            if (_mqttClient2 == null)
            {
                _isRunning = true;
                ResetWait();
                try
                {
                    MqttFactory factory = new MqttFactory();
                    _mqttClient2 = factory.CreateMqttClient() as MqttClient;
                    
                    _options = _config.Options.Build();
                    
                    _mqttClient2.ConnectedAsync +=  Connected;
                    //_mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(new Func<MqttClientConnectedEventArgs, Task>(Connected));
                    _mqttClient2.DisconnectedAsync +=Disconnected;
                    
                    _mqttClient2.ApplicationMessageReceivedAsync += ApplicationMessageReceivedAsync;
                    Task<MqttClientConnectResult> tsk1 = _mqttClient2.ConnectAsync(_options);
                   
                    

                    MqttClientConnectResult res = tsk1.Result;
                    if (res.ResultCode != MqttClientConnectResultCode.Success)
                    {
                        throw new MqttConnectingFailedException("Connect Fault", null, res);
                    }

                    
                    
                }
                finally 
                {
                    SetWait();
                }
                
            }


        }

        

        private Task ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            try
            {
                byte[] value = arg.ApplicationMessage.Payload;
                string topic = arg.ApplicationMessage.Topic;
                //string qos = e.ApplicationMessage.QualityOfServiceLevel.ToString();
                //string retained = e.ApplicationMessage.Retain.ToString();
                MQTTCallbackMessage message = new MQTTCallbackMessage(topic, value, arg);
                CallBack(message);
                


            }
            catch (Exception exp)
            {
                OnException(exp);
            }
            return Task.CompletedTask;
        }

        MqttClientSubscribeOptions _option = null;



        private Task Connected(MqttClientConnectedEventArgs e)
        {
            try
            {
                Task<MqttClientSubscribeResult> tsk2 = SubTopic();
                MqttClientSubscribeResult res1 = tsk2.Result;

            }
            catch (Exception exp)
            {
                OnException(exp);
            }
            return Task.CompletedTask;
        }

        private  Task<MqttClientSubscribeResult> SubTopic() 
        {
            if (_option == null)
            {
                MqttClientSubscribeOptionsBuilder subBuilder = new MqttClientSubscribeOptionsBuilder();
                foreach (MQOffestInfo info in _lstTopic)
                {
                    subBuilder.WithTopicFilter(info.Key, _config.QualityOfServiceLevel, _config.NoLocal.GetValueOrDefault(),
                        _config.RetainAsPublished.GetValueOrDefault(false), _config.RetainHandling.GetValueOrDefault(MqttRetainHandling.SendAtSubscribe));
                    //subBuilder.WithTopicFilter(info.Key, _config.QualityOfServiceLevel, _config.NoLocal.GetValueOrDefault(),
                    //    _config.RetainAsPublished.GetValueOrDefault(false), _config.RetainHandling.GetValueOrDefault(MqttRetainHandling.SendAtSubscribe));

                }
                _option = subBuilder.Build();
            }
            
            return _mqttClient2.SubscribeAsync(_option);
        }

        private  async Task Disconnected(MqttClientDisconnectedEventArgs e)
        {
            if (!_isRunning) 
            {
                return  ;
            }
            try
            {
                
                //await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await _mqttClient2.ConnectAsync(_options);
                }
                catch (Exception exp)
                {
                    OnException(exp);
                }
                if (OnDisconnected != null) 
                {
                    await OnDisconnected(e);
                }
            }
            catch (Exception exp)
            {
                OnException(exp);
            }
            return ;
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
            if (_mqttClient2 != null)
            {
                try
                {
                    _mqttClient2.DisconnectAsync().Wait();
                    _mqttClient2.Dispose();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
                _mqttClient2 = null;
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
