using Buffalo.Kernel.Collections;
using Buffalo.Kernel.TreadPoolManager;
using Confluent.Kafka;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
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

        private ConcurrentQueue<ConnectionMultiplexer> _queRedis =null;
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
        /// 轮询线程
        /// </summary>
        private BlockThreadPool _thdPolling = null;

        /// <summary>
        /// 主题和队列的对应关系
        /// </summary>
        private Dictionary<string, string> _dicTopicToQueue = null;
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
            
            if (_config.SaveToQueue)
            {
                FlushQueue(skey);
            }
            else
            {
                byte[] svalue = (byte[])value;
                RedisCallbackMessage mess = new RedisCallbackMessage(skey, svalue);
                CallBack(mess);
            }
        }

        
        /// <summary>
        /// 通过话题Key获取队列key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetQueueKey(string key) 
        {
            if (_dicTopicToQueue == null) 
            {
                return _config.GetDefaultQueueKey(key);
            }
            string ret = null;
            if(!_dicTopicToQueue.TryGetValue(key,out ret)) 
            {
                return _config.GetDefaultQueueKey(key);
            }
            return ret;
        }
        /// <summary>
        /// 读入队列信息
        /// </summary>
        private long FlushQueue(string skey)
        {
            string pkey = GetQueueKey(skey);
            IDatabase db = GetDB();
            return FlushQueue(skey, pkey, db);
        }
        /// <summary>
        /// 读入队列信息
        /// </summary>
        private long FlushQueue(string skey, string pkey, IDatabase db)
        {
            object lok = _lok.GetObject(skey);
            long count = 0;
            lock (lok)
            {
                //string pkey = GetQueueKey(skey);
                byte[] svalue = null;
                //IDatabase db = GetDB();

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
                        RedisCallbackMessage mess = new RedisCallbackMessage(skey, svalue);
                        CallBack(mess);
                        count++;
                    }
                    catch (Exception ex)
                    {
                        OnException(ex);
                        Thread.Sleep(300);
                    }

                } while (tmpval.HasValue);
            }
            return count;
        }

        public override void StartListend(IEnumerable<string> listenKeys)
        {
            List<MQOffestInfo> listenKeyInfos = new List<MQOffestInfo>();
            foreach (string listenKey in listenKeys) 
            {
                MQOffestInfo info = new MQOffestInfo(listenKey, 0, 0, _config.GetDefaultQueueKey(listenKey));
                listenKeyInfos.Add(info);
            }
            StartListend(listenKeyInfos);
        }

        /// <summary>
        /// 轮询方式监听
        /// </summary>
        /// <param name="objKeys"></param>
        public void DoListening(object objKeys)
        {
            string listenKey = objKeys as string;
            if (string.IsNullOrWhiteSpace(listenKey))
            {
                return;
            }
            string pkey = GetQueueKey(listenKey);
            IDatabase db = GetDB();

            int sleep = _config.PollingInterval;
            if (sleep <= 0)
            {
                sleep = 50;
            }
            while (_pollrunning)
            {
                FlushQueue(listenKey, pkey, db);
                Thread.Sleep(sleep);
            }

        }
        /// <summary>
        /// 阻塞队列方式监听
        /// </summary>
        /// <param name="objKeys"></param>
        public void DoBlockPopListening(object objKeys)
        {

            string listenKey = objKeys as string;
            if (string.IsNullOrWhiteSpace(listenKey))
            {
                return;
            }
            int timeout = 0;
            if (_config.PollingInterval == 0)
            {
                timeout = 30;//默认30
            }
            else
            {
                timeout = (int)Math.Ceiling(((double)_config.PollingInterval / 1000.00));
                if (timeout < 1)
                {
                    timeout = 1;
                }

            }
            _config.Options.SyncTimeout = (timeout * 1000) + 2000;
            string pkey = GetQueueKey(listenKey);
            byte[] svalue = null;
            RedisResult res = null;
            RedisValue tmpval = RedisValue.Null;

            using (ConnectionMultiplexer connection = RedisMQConnection.CreateManager(_config.Options))//必须开启独立连接进行监听，否则会堵塞其他指令
            {
                _queRedis.Enqueue(connection);
                IDatabase db = connection.GetDatabase(_config.UseDatabase);
                while (_pollrunning)
                {
                    try
                    {
                        res = db.Execute("brPop", pkey, timeout);


                        if (res == null || res.IsNull || res.Length < 2)
                        {
                            continue;
                        }

                        tmpval = (RedisValue)res[1];
                       
                            svalue = tmpval;
                        
                        RedisCallbackMessage mess = new RedisCallbackMessage(listenKey, svalue);
                        CallBack(mess);

                    }
                    catch (TimeoutException tex)
                    {

                    }
                    catch (Exception e)
                    {
                        OnException(e);
                        Thread.Sleep(300);
                    }
                }

            }
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="listenKeys"></param>
        public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        {
            Close();
            

            ResetWait();
            string queKey = null;
            switch (_config.Mode)
            {
                case RedisMQMessageMode.Subscriber:
                _dicTopicToQueue = new Dictionary<string, string>();
                    Open();
                    foreach (MQOffestInfo lis in listenKeys)
                    {
                        queKey = lis.QueueKey;
                        if (string.IsNullOrWhiteSpace(queKey))
                        {
                            queKey = _config.GetDefaultQueueKey(lis.Key);
                        }
                        _dicTopicToQueue[lis.Key] = queKey;
                    }

                    if (_config.SaveToQueue)
                    {
                        foreach (MQOffestInfo lis in listenKeys)
                        {
                            FlushQueue(lis.Key);
                        }
                    }

                    _subscriber = _redis.GetSubscriber();
                    foreach (MQOffestInfo lis in listenKeys)
                    {
                        _subscriber.Subscribe(lis.Key, OnRedisCallback, _config.CommanfFlags);
                    }
                    break;

                case RedisMQMessageMode.Polling:
                    Open();
                    _thdPolling = new BlockThreadPool();
                    _pollrunning = true;

                    foreach (MQOffestInfo lisKey in listenKeys)
                    {
                        _thdPolling.RunParamThread(DoListening, lisKey.Key);
                    }
                    break;
                case RedisMQMessageMode.BlockQueue://阻塞队列不需要Open，自己新建连接池
                    _thdPolling = new BlockThreadPool();
                    _pollrunning = true;
                    _queRedis = new ConcurrentQueue<ConnectionMultiplexer>();
                    foreach (MQOffestInfo lisKey in listenKeys)
                    {
                        _thdPolling.RunParamThread(DoBlockPopListening, lisKey.Key);
                    }
                    break;
                default:
                    break;
            }
            SetWait();

            //StartListend(MQUnit.GetLintenKeys(listenKeys));
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            _pollrunning = false;


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
            }
            _redis = null;
            _db = null;
            if (_thdPolling != null)
            {
                _thdPolling.StopAll();
                Thread.Sleep(100);
            }
            _thdPolling = null;

            if(_queRedis != null) 
            {
                ConnectionMultiplexer conn = null;
                while (_queRedis.Count > 0) 
                {
                    
                    if (_queRedis.TryDequeue(out conn)) 
                    {
                        try 
                        {
                            conn.Close();
                        }catch(Exception ex) { }
                    }
                }
            }
            _queRedis = null;
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
