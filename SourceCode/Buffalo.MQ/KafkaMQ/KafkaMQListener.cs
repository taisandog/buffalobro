using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Buffalo.ArgCommon;
using Buffalo.Kernel;
using Confluent.Kafka;

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
        private IEnumerable<string> _lisTopic;
        public override void StartListend(IEnumerable<string> listenKeys)
        {
            _lisTopic = listenKeys;
            _running = new CancellationTokenSource();
            _handle = new AutoResetEvent(true);
            _thd = new Thread(new ThreadStart(OnListend));
            _thd.Start();
        }

        public override void Close()
        {
            CloseListener();
        }
        

        /// <summary>
        /// 监听信息
        /// </summary>
        private void OnListend()
        {
            ConsumerBuilder<string, byte[]> builder = _config.KConsumerBuilder;

            CancellationToken token = _running.Token;
            List<TopicPartitionOffset> lst = new List<TopicPartitionOffset>();
            foreach(string listop in _lisTopic)
            {
                lst.Add(new TopicPartitionOffset(listop, 0, 0));
            }
            using (IConsumer<string, byte[]> consumer = builder.Build())
            {

                consumer.Assign(lst);

                while (!_running.IsCancellationRequested)
                {
                    try
                    {
                        ConsumeResult<string, byte[]> res = consumer.Consume(token);

                        CallBack(res.Message.Key, res.Topic, res.Message.Value);
                        consumer.Commit(res);
                    }
                    catch (Exception ex)
                    {
                        OnException(ex);
                    }
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
            _running = null;
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
        }

        public override void Dispose()
        {
            CloseListener();
        }
    }
}
