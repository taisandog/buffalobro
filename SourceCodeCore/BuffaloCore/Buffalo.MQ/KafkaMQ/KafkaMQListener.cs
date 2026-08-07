using Buffalo.Kernel.TreadPoolManager;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace Buffalo.MQ.KafkaMQ
{
    public partial class KafkaMQListener:MQListener
    {
        private KafkaMQConfig _config;
        public KafkaMQListener(KafkaMQConfig config)
        {
            _config = config;
            ConfigureRetry(config);
        }
        private readonly ConcurrentDictionary<string, int> _deliveryAttempts =
            new ConcurrentDictionary<string, int>();
        

        private CancellationTokenSource _running =null;
        //private AutoResetEvent _handle = null;
        private BlockThread _thd;
        public override void StartListend(IEnumerable<string> listenKeys)
        {
            _running = new CancellationTokenSource();
            //_handle = new AutoResetEvent(true);
            ResetWait();
            //_thd = new Thread(new ParameterizedThreadStart(OnListend));
            _thd = BlockThread.Create(OnListend);
            _thd.StartThread(listenKeys);
        }
        //public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        //{
        //    _running = new CancellationTokenSource();
        //    //_handle = new AutoResetEvent(true);
        //    ResetWait();
        //    _thd = BlockThread.Create(OnListend);

        //    _thd.StartThread(listenKeys);
        //}
        
        public override void Close()
        {
            CloseListener();
        }

        

        /// <summary>
        /// 监听信息
        /// </summary>
        private void OnListend(object arg)
        {
            IEnumerable<string> topics = arg as IEnumerable<string>;

            //IEnumerable<MQOffestInfo> topicsOffest = MQUnit.GetLintenOffest(arg);

            ConsumerBuilder<byte[], byte[]> builder = _config.KConsumerBuilder;

            CancellationToken token = _running.Token;

            using (IConsumer<byte[], byte[]> consumer = builder.Build())
            using (IProducer<byte[], byte[]> deadLetterProducer = _config.ProducerBuilder.Build())
            {
                
                consumer.Subscribe(topics);
                
                //if (topicsOffest != null)
                //{
                    
                    if (_config.UseConfiguredStartOffset)
                    {
                        foreach (string key in topics)
                        {
                            for (int i = 0; i < 50; i++)
                            {
                                try
                                {
                                    consumer.Seek(new TopicPartitionOffset(
                                        new TopicPartition(key, _config.TopicPartitionIndex),
                                        _config.TopicPartitionOffset));
                                    break;
                                }
                                catch
                                {
                                    Thread.Sleep(300);
                                }
                            }
                        }
                    }
                //}
                
                try
                {
                    SetWait();
                    while (!_running.IsCancellationRequested)
                    {
                        try
                        {
                            ConsumeResult<byte[], byte[]> res = consumer.Consume(token);
                            string deliveryKey = GetDeliveryKey(res);
                            int deliveryCount = _deliveryAttempts.AddOrUpdate(
                                deliveryKey, 1, (_, count) => count + 1);
                            KafkaCallbackMessage mess = new KafkaCallbackMessage(res.Topic,res.Message.Value,
                                res.Partition, res.Offset, consumer, res, deadLetterProducer,
                                _config.RetryOptions.DeadLetterSuffix, deliveryCount,
                                () => _deliveryAttempts.TryRemove(deliveryKey, out _));
                            CallBack(mess).GetAwaiter().GetResult();
                        }
                        catch (Exception ex)
                        {
                            OnException(ex).GetAwaiter().GetResult();
                        }
                    }
                }
                finally
                {
                    consumer.Close();
                }
            }
        }

        private static string GetDeliveryKey(ConsumeResult<byte[], byte[]> result)
        {
            return result.Topic + ":" + result.Partition.Value + ":" + result.Offset.Value;
        }
       
        
        /// <summary>
        /// 关闭监听
        /// </summary>
        public void CloseListener()
        {
            if (_running != null)
            {
                try
                {
                    _running.Cancel();
                }

                catch (Exception ex)
                {
                    OnException(ex).GetAwaiter().GetResult();
                }
            }

            if (_thd != null) 
            {
                _thd.StopThread();
            }
            _thd = null;
            //if (_handle != null && _thd!=null)
            //{
            //    if (!_handle.WaitOne(1000))
            //    {
            //        try
            //        {
            //            _thd.Abort();
            //        }
            //        catch(Exception ex)
            //        {
            //            OnException(ex);
            //        }
            //    }
            //}
            //_handle = null;
            DisponseWait().GetAwaiter().GetResult();
        }

        public override void Dispose()
        {
            CloseListener();
        }

       
    }
}
