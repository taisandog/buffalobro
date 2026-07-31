using Buffalo.ArgCommon;
using Buffalo.Kernel;
using MQTTnet;
using MQTTnet.Adapter;

using MQTTnet.Protocol;
using System;
using System.Buffers;
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
        IEnumerable<string> _lstTopic = null;
        bool _isRunning = false;
        private readonly SemaphoreSlim _openLock = new SemaphoreSlim(1, 1);
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
            OpenAsync().GetAwaiter().GetResult();
        }

        public async Task OpenAsync()
        {
            if (_mqttClient2 != null)
            {
                return;
            }

            await _openLock.WaitAsync();
            try
            {
                if (_mqttClient2 != null)
                {
                    return;
                }

                _isRunning = true;
                ResetWait();
                var factory = new MqttClientFactory();
                _mqttClient2 = factory.CreateMqttClient() as MqttClient;
                _options = _config.Options.Build();
                _mqttClient2.ConnectedAsync += Connected;
                _mqttClient2.DisconnectedAsync += Disconnected;
                _mqttClient2.ApplicationMessageReceivedAsync += ApplicationMessageReceivedAsync;

                MqttClientConnectResult result = await _mqttClient2.ConnectAsync(_options);
                if (result.ResultCode != MqttClientConnectResultCode.Success)
                {
                    throw new MqttConnectingFailedException(
                        "Connect Fault",
                        new Exception(result.ToString()));
                }
            }
            finally
            {
                SetWait();
                _openLock.Release();
            }
        }

        

        private async Task ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            try
            {
                byte[] value = arg.ApplicationMessage.Payload.ToArray();

                string topic = arg.ApplicationMessage.Topic;
                //string qos = e.ApplicationMessage.QualityOfServiceLevel.ToString();
                //string retained = e.ApplicationMessage.Retain.ToString();
                MQTTCallbackMessage message = new MQTTCallbackMessage(topic, value, arg);
                await CallBack(message);
                


            }
            catch (Exception exp)
            {
                await OnException(exp);
            }
        }

        MqttClientSubscribeOptions _option = null;



        private async Task Connected(MqttClientConnectedEventArgs e)
        {
            try
            {
                MqttClientSubscribeResult res1 = await SubTopic();
                 

            }
            catch (Exception exp)
            {
                await OnException(exp);
            }
        }

        private  Task<MqttClientSubscribeResult> SubTopic() 
        {
            if (_option == null)
            {
                MqttClientSubscribeOptionsBuilder subBuilder = new MqttClientSubscribeOptionsBuilder();
                foreach (string key in _lstTopic)
                {
                    subBuilder.WithTopicFilter(key, _config.QualityOfServiceLevel, _config.NoLocal.GetValueOrDefault(),
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
                    await OnException(exp);
                }
                if (OnDisconnected != null) 
                {
                    await OnDisconnected(e);
                }
            }
            catch (Exception exp)
            {
                await OnException(exp);
            }
            return ;
        }


        public override void StartListend(IEnumerable<string> listenKeys)
        {
            StartListendAsync(listenKeys).GetAwaiter().GetResult();
        }

        public override async Task StartListendAsync(IEnumerable<string> listenKeys)
        {
            _lstTopic = listenKeys;
            await OpenAsync();
        }
        //public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        //{
        //    _lstTopic = listenKeys;
            
        //    Open();
        //}
        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            CloseAsync().GetAwaiter().GetResult();
        }

        public override async Task CloseAsync()
        {
            _isRunning = false;
            if (_mqttClient2 != null)
            {
                try
                {
                    await _mqttClient2.DisconnectAsync();
                    _mqttClient2.Dispose();
                }
                catch (Exception ex)
                {
                    await OnException(ex);
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
