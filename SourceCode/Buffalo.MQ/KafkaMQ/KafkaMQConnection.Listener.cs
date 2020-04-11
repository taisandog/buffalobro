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
    public partial class KafkaMQConnection
    {
        private ConsumerConfig _cseconfig;
        private CancellationTokenSource _running =null;
        private AutoResetEvent _handle = null;
        private Thread _thd;

        public override void StartListend()
        {
            if (_cseconfig == null)
            {
                InitConsumerConfig();
            }

            _running = new CancellationTokenSource();
            _handle = new AutoResetEvent(true);
            _thd = new Thread(new ThreadStart(OnListend));
            _thd.Start();

            
        }
        /// <summary>
        /// 初始化消费者配置
        /// </summary>
        private void InitConsumerConfig()
        {
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(_connString);
            _cseconfig = new ConsumerConfig();
            SetBaseConfig(_cseconfig, hs);
            
            _cseconfig.GroupId = hs.GetDicValue<string, string>("groupId");
            _cseconfig.EnableAutoCommit = hs.GetDicValue<string, string>("autoCommit") == "1";
           

            string value = hs.GetDicValue<string, string>("interval");
            int statisticsIntervalMs = 5000;
            if (!string.IsNullOrWhiteSpace(value))
            {
                statisticsIntervalMs = value.ConvertTo<int>(5000);
            }
            //_cseconfig.ConsumeGroupRebalanceRetryIntervalMs = statisticsIntervalMs;
            
            value = hs.GetDicValue<string, string>("sessionTimeout");
            int sessionTimeoutMs = 5000;
            if (!string.IsNullOrWhiteSpace(value))
            {
                sessionTimeoutMs = value.ConvertTo<int>(5000);
            }
            _cseconfig.SessionTimeoutMs = sessionTimeoutMs;

            

            int offsetType = hs.GetDicValue<string, string>("offsetType").ConvertTo<int>((int)AutoOffsetReset.Earliest);
            
            _cseconfig.AutoOffsetReset = (AutoOffsetReset)offsetType;

            string server = hs.GetDicValue<string, string>("server");
            if (!server.Contains(":"))
                server = server + ":2181";
            _cseconfig.BootstrapServers = server;
            
        }

        /// <summary>
        /// 监听信息
        /// </summary>
        private void OnListend()
        {
            ConsumerBuilder<string, byte[]> builder = new ConsumerBuilder<string, byte[]>(_cseconfig);

            CancellationToken token = _running.Token;
            using (IConsumer<string, byte[]> consumer = builder.Build())
            {
                

                while (!_running.IsCancellationRequested)
                {
                    ConsumeResult<string, byte[]>  res=consumer.Consume(token);

                    CallBack(res.Message.Key, _topic, res.Message.Value);
                    consumer.Commit(res);
                }
            }
        }
       
        
        /// <summary>
        /// 关闭监听
        /// </summary>
        private void CloseListener()
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
    }
}
