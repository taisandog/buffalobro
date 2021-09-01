using Buffalo.Kernel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    public partial class RedisMQListener : MQListener
    {
        private ConnectionMultiplexer _redis = null;

        /// <summary>
        /// 配置
        /// </summary>
        RedisMQConfig _config;
        /// <summary>
        /// 发布者
        /// </summary>
        public ISubscriber _subscriber;

        private IDatabase _db;
        /// <summary>
        /// 锁对象
        /// </summary>
        private LockObjects<string> _lok = new LockObjects<string>();
        /// <summary>
        /// 正在运行轮询
        /// </summary>
        private bool _pollrunning = false;
        /// <summary>
        /// 阻塞
        /// </summary>
        private AutoResetEvent _pollEvent;
        /// <summary>
        /// 轮询线程
        /// </summary>
        private Thread _thdPolling = null;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RedisMQListener(RedisMQConfig config)
        {
            _config = config;

        }


        /// <summary>
        /// 打来连接
        /// </summary>
        public void Open()
        {
            if (_redis == null)
            {
                _redis = RedisMQConnection.CreateManager(_config.Options);
            }

        }
        /// <summary>
        /// 获取Redis操作类
        /// </summary>
        /// <returns></returns>
        private IDatabase GetDB()
        {
            if (_db == null)
            {
                _db = _redis.GetDatabase(_config.UseDatabase);
            }
            return _db;
        }
        private void OnRedisCallback(RedisChannel key, RedisValue value)
        {
            string skey = key.ToString();
            byte[] svalue = (byte[])value;
            if (_config.SaveToQueue)
            {
                FlushQueue(skey);
            }
            else
            {
                CallBack(skey, skey, (byte[])value, 0, 0);
            }
        }


        /// <summary>
        /// 读入队列信息
        /// </summary>
        private void FlushQueue(string skey)
        {
            object lok = _lok.GetObject(skey);
            lock (lok)
            {
                string pkey = RedisMQConfig.BuffaloMQHead + skey;
                byte[] svalue = null;
                IDatabase db = GetDB();
                RedisValue tmpval = RedisValue.Null;
                do
                {
                    try
                    {
                        tmpval = db.ListRightPop(pkey, _config.CommanfFlags);
                        if (!tmpval.HasValue)
                        {
                            break;
                        }
                        svalue = tmpval;
                        CallBack(skey, skey, svalue, 0, 0);
                    }catch(Exception ex) 
                    {
                        OnException(ex);
                    }

                } while (tmpval.HasValue);
            }
        }

        public override void StartListend(IEnumerable<string> listenKeys)
        {
            Close();
            Open();
            foreach (string lis in listenKeys)
            {
                FlushQueue(lis);
            }

            ResetWait();
            if (_config.Mode == RedisMQMessageMode.Subscriber)
            {
                _subscriber = _redis.GetSubscriber();
                foreach (string lis in listenKeys)
                {
                    _subscriber.Subscribe(lis, OnRedisCallback, _config.CommanfFlags);

                }
            }
            else
            {
                _thdPolling = new Thread(new ParameterizedThreadStart(DoListening));
                _thdPolling.Start(listenKeys);
            }
            SetWait();
        }

        /// <summary>
        /// 轮询方式监听
        /// </summary>
        /// <param name="objKeys"></param>
        public void DoListening(object objKeys)
        {
            IEnumerable<string> listenKeys = objKeys as IEnumerable<string>;
            if (listenKeys == null)
            {
                return;
            }
            _pollrunning = true;
            _pollEvent = new AutoResetEvent(true);
            _pollEvent.Reset();
            try
            {
                while (_pollrunning)
                {
                    foreach (string key in listenKeys)
                    {
                        FlushQueue(key);
                    }
                    Thread.Sleep(_config.PollingInterval);
                }
            }
            finally
            {
                _pollEvent.Set();
            }
        }


        public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        {
            StartListend(MQUnit.GetLintenKeys(listenKeys));
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            if (_subscriber != null)
            {
                try
                {
                    _subscriber.UnsubscribeAll();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
            }
            _pollrunning = false;
            if (_pollEvent != null)
            {
                if (!_pollEvent.WaitOne(5000))
                {
                    _thdPolling.Abort();
                    Thread.Sleep(100);
                }
            }
            _pollEvent = null;

            _subscriber = null;
            if (_redis != null)
            {
                try
                {
                    _redis.Close();
                    _redis.Dispose();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
                _redis = null;
            }

            _db = null;
            DisponseWait();
        }

        public override void Dispose()
        {
            Close();
        }



        ~RedisMQListener()
        {
            Close();
            GC.SuppressFinalize(this);
        }
    }
}
