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
        private AutoResetEvent _handle = null;
        private Thread _thd;
        public override void StartListend(IEnumerable<string> listenKeys)
        {
            _running = new CancellationTokenSource();
            _handle = new AutoResetEvent(true);
            ResetWait();
            _thd = new Thread(new ParameterizedThreadStart(OnListend));
            
            _thd.Start(listenKeys);
        }
        public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        {
            _running = new CancellationTokenSource();
            _handle = new AutoResetEvent(true);
            ResetWait();
            _thd = new Thread(new ParameterizedThreadStart(OnListend));
            
            _thd.Start(listenKeys);
        }
        
        public override void Close()
        {
            CloseListener();
        }

        

        /// <summary>
        /// 监听信息
        /// </summary>
        private void OnListend(object arg)
        {
            IEnumerable<string> topics = MQUnit.GetLintenKeys(arg);

            IEnumerable<MQOffestInfo> topicsOffest = MQUnit.GetLintenOffest(arg);

            ConsumerBuilder<byte[], byte[]> builder = _config.KConsumerBuilder;

            CancellationToken token = _running.Token;

            using (IConsumer<byte[], byte[]> consumer = builder.Build())
            {
                consumer.Subscribe(topics);
                
                if (topicsOffest != null)
                {
                    
                    foreach (MQOffestInfo info in topicsOffest)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            try
                            {
                                consumer.Seek(new TopicPartitionOffset(new TopicPartition(info.Key, info.Partition), info.Offest));
                                break;
                            }
                            catch(Exception ex)
                            {
                                Thread.Sleep(300);
                                continue;
                            }
                            
                        }

                    }
                }
                
                try
                {
                    SetWait();
                    while (!_running.IsCancellationRequested)
                    {
                        try
                        {
                            ConsumeResult<byte[], byte[]> res = consumer.Consume(token);
                            
                            CallBack(res.Topic, res.Topic, res.Message.Value,res.Partition,res.Offset);
                            consumer.Commit(res);
                        }
                        catch (Exception ex)
                        {
                            OnException(ex);
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
                catch { }
            }
            
            if (_handle != null && _thd!=null)
            {
                if (!_handle.WaitOne(1000))
                {
                    try
                    {
                        _thd.Abort();
                    }
                    catch(Exception ex)
                    {
                        OnException(ex);
                    }
                }
            }
            _handle = null;
            DisponseWait();
        }

        public override void Dispose()
        {
            CloseListener();
        }
    }
}
