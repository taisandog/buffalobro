using Buffalo.Kernel.TreadPoolManager;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
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
        }
        

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
            {
                
                consumer.Subscribe(topics);
                
                //if (topicsOffest != null)
                //{
                    
                    foreach (string key in topics)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            try
                            {
                                consumer.Seek(new TopicPartitionOffset(new TopicPartition(key, _config.TopicPartitionIndex), 
                                    _config.TopicPartitionOffset));
                                break;
                            }
                            catch(Exception ex)
                            {
                                Thread.Sleep(300);
                                continue;
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
                            KafkaCallbackMessage mess = new KafkaCallbackMessage(res.Topic,res.Message.Value,
                                res.Partition, res.Offset, consumer, res);
                            CallBack(mess).Wait();
                            //consumer.Commit(res);
                        }
                        catch (Exception ex)
                        {
                            OnException(ex).Wait();
                        }
                    }
                }
                finally
                {
                    consumer.Close();
                }
            }
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
                    OnException(ex).Wait();
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
            DisponseWait().Wait();
        }

        public override void Dispose()
        {
            CloseListener();
        }

       
    }
}
