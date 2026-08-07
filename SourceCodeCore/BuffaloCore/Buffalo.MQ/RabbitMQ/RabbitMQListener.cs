using Buffalo.ArgCommon;
using Buffalo.Kernel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Buffalo.MQ.RabbitMQ
{
    public partial class RabbitMQListener:MQListener
    {
        private IChannel _channel;
        
        /// <summary>
        /// 信道
        /// </summary>
        public IChannel Channel
        {
            get
            {
                return _channel;
            }
        }
        private IConnection _connection;
        private RabbitMQConfig _config;
        private readonly ConcurrentDictionary<string, byte> _declaredFailureQueues =
            new ConcurrentDictionary<string, byte>();
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RabbitMQListener(RabbitMQConfig config)
        {
            _config = config;
            ConfigureRetry(config);
        }
        /// <summary>
        /// 打来连接
        /// </summary>
        private async Task OpenAsync()
        {
            _declaredFailureQueues.Clear();
            _connection = await _config.Factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            //IBasicProperties properties = _channel.CreateBasicProperties();
            //properties.DeliveryMode = (DeliveryModes)_config.DeliveryMode;

            //UInt32 prefetchSize,  每次取的长度
            //UInt16 prefetchCount,     每次取几条
            //Boolean global    是否对connection通用
            await _channel.BasicQosAsync(0, _config.PrefetchCount, false);
            await _channel.ExchangeDeclareAsync(_config.ExchangeName, _config.ExchangeMode, _config.DeliveryMode == 2, _config.AutoDelete, null);
            if (_config.QueueName != null)
            {
                foreach (string name in _config.QueueName)
                {
                    await _channel.QueueDeclareAsync(name, _config.DeliveryMode == 2, false, _config.AutoDelete, null);
                    await _channel.QueueBindAsync(name, _config.ExchangeName, "", null);
                }
            }

            //_connection = _config.Factory.CreateConnection();
            //_channel = _connection.CreateModel();
            //IBasicProperties properties = _channel.CreateBasicProperties();
            //properties.DeliveryMode = _config.DeliveryMode;
            
            ////UInt32 prefetchSize,  每次取的长度
            ////UInt16 prefetchCount,     每次取几条
            ////Boolean global    是否对connection通用
            //_channel.BasicQos(0, 1, true);
            //_channel.ExchangeDeclare(_config.ExchangeName, _config.ExchangeMode, _config.DeliveryMode == 2, _config.AutoDelete, null);
            //if (_config.QueueName != null)
            //{
            //    foreach (string name in _config.QueueName)
            //    {
            //        _channel.QueueDeclare(name, _config.DeliveryMode == 2, false, _config.AutoDelete, null);
            //        _channel.QueueBind(name, _config.ExchangeName, "", null);
            //    }
            //}
        }
        /// <summary>
        /// 打开事件监听
        /// </summary>
        public override void StartListend(IEnumerable<string> listenKeys)
        {
            StartListendAsync(listenKeys).GetAwaiter().GetResult();
        }

        public override async Task StartListendAsync(IEnumerable<string> listenKeys)
        {
            await StartConsumersAsync(listenKeys, false);
        }

        public override async Task StartDeadLetterListenAsync(IEnumerable<string> listenKeys)
        {
            IsDeadLetterListener = true;
            await StartConsumersAsync(listenKeys, true);
        }

        private async Task StartConsumersAsync(IEnumerable<string> listenKeys,
            bool deadLetterMode)
        {
            await OpenAsync();
            ResetWait();
            List<string> keys = listenKeys.ToList();

            if (_config.QueueName != null)
            {
                foreach (string name in _config.QueueName)
                {
                    if (deadLetterMode)
                    {
                        string deadLetterQueue = GetDeadLetterQueueName(name);
                        await _channel.QueueDeclareAsync(deadLetterQueue,
                            _config.DeliveryMode == 2, false, _config.AutoDelete, null);
                        AsyncEventingBasicConsumer deadLetterConsumer =
                            new AsyncEventingBasicConsumer(_channel);
                        deadLetterConsumer.ReceivedAsync += (sender, args) =>
                            Consumer_Received(deadLetterQueue, true, sender, args);
                        await _channel.BasicConsumeAsync(deadLetterQueue, false,
                            deadLetterConsumer);
                        continue;
                    }
                    await _channel.QueueDeclareAsync(name, _config.DeliveryMode == 2, false, _config.AutoDelete, null);

                    foreach (string key in keys)
                    {
                        await _channel.QueueBindAsync(name, _config.ExchangeName, key, null);
                        await DeclareFailureQueuesAsync(name, key);
                    }

                    AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);
                    consumer.ReceivedAsync += (sender, args) =>
                        Consumer_Received(name, false, sender, args);
                    await _channel.BasicConsumeAsync(name, false, consumer);
                }
            }
            SetWait();
        }
        //public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        //{
        //    StartListend(MQUnit.GetLintenKeys(listenKeys));
        //}
        private async Task Consumer_Received(string queueName, bool deadLetterConsumer,
            object sender, BasicDeliverEventArgs e)
        {
            await DeclareFailureQueuesAsync(queueName, e.RoutingKey,
                deadLetterConsumer);
            RabbitCallbackMessage mess = new RabbitCallbackMessage(e.RoutingKey, e.Exchange,
                queueName, GetRetryQueueName(queueName, e.RoutingKey),
                GetDeadLetterQueueName(queueName), e.Body.ToArray(), _channel, e,
                _config.RetryOptions.RetryDelayMilliseconds);

            await CallBack(mess);
        }

        private async Task DeclareFailureQueuesAsync(string queueName, string routingKey,
            bool deadLetterConsumer = false)
        {
            string declarationKey = queueName + "\n" + routingKey + "\n" + deadLetterConsumer;
            if (!_declaredFailureQueues.TryAdd(declarationKey, 0))
            {
                return;
            }
            bool durable = _config.DeliveryMode == 2;
            string retryQueueName = GetRetryQueueName(queueName, routingKey);
            Dictionary<string, object> retryArguments = new Dictionary<string, object>
            {
                ["x-dead-letter-exchange"] = deadLetterConsumer
                    ? string.Empty : _config.ExchangeName,
                ["x-dead-letter-routing-key"] = deadLetterConsumer
                    ? queueName : routingKey
            };
            try
            {
                await _channel.QueueDeclareAsync(retryQueueName, durable, false,
                    _config.AutoDelete, retryArguments);
                await _channel.QueueDeclareAsync(GetDeadLetterQueueName(queueName), durable,
                    false, _config.AutoDelete, null);
            }
            catch
            {
                _declaredFailureQueues.TryRemove(declarationKey, out _);
                throw;
            }
        }

        private string GetDeadLetterQueueName(string queueName)
        {
            return queueName + _config.RetryOptions.DeadLetterSuffix;
        }

        private string GetRetryQueueName(string queueName, string routingKey)
        {
            string identity = (routingKey ?? string.Empty) + "\n" +
                _config.ExchangeName + "\nv2";
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(identity));
            return queueName + ".retry." + Convert.ToHexString(hash, 0, 8).ToLowerInvariant();
        }

        

        public override void Dispose()
        {
            Close();
        }

        public override void Close()
        {
            CloseAsync().GetAwaiter().GetResult();
        }

        public override async Task CloseAsync()
        {
            if (_channel != null)
            {
                try
                {
                    await _channel.CloseAsync();
                }
                catch (Exception ex)
                {
                    await OnException(ex);
                }
                _channel = null;
            }
            if (_connection != null)
            {
                try
                {
                    await _connection.CloseAsync();
                }
                catch (Exception ex)
                {
                    await OnException(ex);
                }
                _connection = null;
            }
            await DisponseWait();
        }
    }
}
