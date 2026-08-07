using Buffalo.Kernel;
using Buffalo.Kernel.Collections;
using Buffalo.Kernel.TreadPoolManager;
using Confluent.Kafka;
using MQTTnet.Internal;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Channels;
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
        private Channel<(RedisChannel Key, RedisValue Value)> _callbackChannel;
        private Task _callbackWorker;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RedisMQListener(RedisMQConfig config)
        {
            _config = config;
            ConfigureRetry(config);
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
            if (_redis == null)
            {
                _redis = await RedisMQConnection.CreateManagerAsync(_config.Options);
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
            _callbackChannel?.Writer.TryWrite((key, value));
        }

        private void StartCallbackWorker()
        {
            _callbackChannel = Channel.CreateUnbounded<(RedisChannel Key, RedisValue Value)>(
                new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false
                });
            _callbackWorker = ProcessCallbacksAsync(_callbackChannel.Reader);
        }

        private async Task ProcessCallbacksAsync(
            ChannelReader<(RedisChannel Key, RedisValue Value)> reader)
        {
            await foreach ((RedisChannel Key, RedisValue Value) item in reader.ReadAllAsync())
            {
                try
                {
                    string key = item.Key.ToString();
                    if (_config.SaveToQueue)
                    {
                        await FlushQueueAsync(key);
                    }
                    else
                    {
                        RedisCallbackMessage message = CreateSubscriberMessage(
                            key, (byte[])item.Value);
                        await CallBack(message);
                    }
                }
                catch (Exception exception)
                {
                    await OnException(exception);
                }
            }
        }

        private async Task StopCallbackWorkerAsync()
        {
            Channel<(RedisChannel Key, RedisValue Value)> channel = _callbackChannel;
            Task worker = _callbackWorker;
            _callbackChannel = null;
            _callbackWorker = null;
            if (channel != null)
            {
                channel.Writer.TryComplete();
            }
            if (worker != null)
            {
                await worker;
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
        private async Task<long> FlushQueueAsync(string skey)
        {
            string pkey = GetQueueKey(skey);
            IDatabase db = GetDB();
            return await FlushQueueAsync(skey, pkey, db);
        }
       
        /// <summary>
        /// 读入队列信息
        /// </summary>
        private async Task<long> FlushQueueAsync(string skey, string pkey, IDatabase db)
        {
            
            
            long count = 0;
            using (AsyncTaskLock<string> lok = new AsyncTaskLock<string>(pkey))
            {
                if (! (await lok.LockAsync())) 
                {
                    return 0;
                }
                //string pkey = GetQueueKey(skey);
                byte[] svalue = null;
                //IDatabase db = GetDB();

                RedisValue tmpval = RedisValue.Null;
                do
                {
                    try
                    {
                        tmpval =await db.ListRightPopAsync(pkey, _config.CommanfFlags);
                        if (!tmpval.HasValue)
                        {
                            break;
                        }
                        svalue = tmpval;
                        RedisCallbackMessage mess = CreateListMessage(
                            skey, pkey, svalue, db);
                        await CallBack(mess);
                        count++;
                    }
                    catch (Exception ex)
                    {
                        await OnException(ex);
                       await Task.Delay(300);
                    }

                } while (tmpval.HasValue);
            }
            return count;
            
        }

        //public override void StartListend(IEnumerable<string> listenKeys)
        //{
        //    List<MQOffestInfo> listenKeyInfos = new List<MQOffestInfo>();
        //    foreach (string listenKey in listenKeys) 
        //    {
        //        MQOffestInfo info = new MQOffestInfo(listenKey,  _config.GetDefaultQueueKey(listenKey));
        //        listenKeyInfos.Add(info);
        //    }
        //    StartListend(listenKeyInfos);
        //}

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
                FlushQueueAsync(listenKey, pkey, db).GetAwaiter().GetResult();

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

                        RedisCallbackMessage mess = CreateListMessage(
                            listenKey, pkey, svalue, db);
                        CallBack(mess).GetAwaiter().GetResult();

                    }
                    catch (TimeoutException tex)
                    {

                    }
                    catch (Exception e)
                    {
                        OnException(e).GetAwaiter().GetResult();
                        Thread.Sleep(300);
                    }
                }
            }
        }
        
        

       

        private static void ToUnifiedInt64(PipeWriter writer, long value)
        {
            
        }
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="listenKeys"></param>
        public override void StartListend(IEnumerable<string> listenKeys)
        {
            StartListendAsync(listenKeys).GetAwaiter().GetResult();
        }

        private RedisCallbackMessage CreateListMessage(string topic, string queueKey,
            byte[] body, IDatabase db)
        {
            return new RedisCallbackMessage(topic, body,
                retryHandler: async (reason, delay) =>
                {
                    if (delay.GetValueOrDefault() > TimeSpan.Zero)
                    {
                        await Task.Delay(delay.Value);
                    }
                    await db.ListRightPushAsync(queueKey, body, flags: _config.CommanfFlags);
                },
                deadLetterHandler: async reason =>
                {
                    string deadLetterQueue = _config.GetDefaultQueueKey(
                        _config.RetryOptions.GetDeadLetterTopic(topic));
                    await db.ListLeftPushAsync(deadLetterQueue, body,
                        flags: _config.CommanfFlags);
                });
        }

        private RedisCallbackMessage CreateSubscriberMessage(string topic, byte[] body)
        {
            return new RedisCallbackMessage(topic, body,
                retryHandler: async (reason, delay) =>
                {
                    if (delay.GetValueOrDefault() > TimeSpan.Zero)
                    {
                        await Task.Delay(delay.Value);
                    }
                    await _subscriber.PublishAsync(RedisChannel.Literal(topic), body,
                        _config.CommanfFlags);
                },
                deadLetterHandler: async reason =>
                {
                    await _subscriber.PublishAsync(
                        RedisChannel.Literal(_config.RetryOptions.GetDeadLetterTopic(topic)),
                        body, _config.CommanfFlags);
                });
        }

        public override async Task StartListendAsync(IEnumerable<string> listenKeys)
        {
            await CloseAsync();

            ResetWait();
            await OpenAsync();

            List<string> keys = listenKeys.ToList();
            string queKey = null;
            switch (_config.Mode)
            {
                case RedisMQMessageMode.Subscriber:
                _dicTopicToQueue = new Dictionary<string, string>();
                    
                    foreach (string key in keys)
                    {
                        queKey = _config.GetDefaultQueueKey(key) ;
                        if (string.IsNullOrWhiteSpace(queKey))
                        {
                            queKey = _config.GetDefaultQueueKey(key);
                        }
                        _dicTopicToQueue[key] = queKey;
                    }

                    if (_config.SaveToQueue)
                    {
                        foreach (string key in keys)
                        {
                            await FlushQueueAsync(key);
                        }
                    }

                    StartCallbackWorker();
                    _subscriber = _redis.GetSubscriber();
                    foreach (string key in keys)
                    {
                        await _subscriber.SubscribeAsync(
                            key,
                            OnRedisCallback,
                            _config.CommanfFlags);
                    }
                    break;

                case RedisMQMessageMode.Polling:
                   
                    _thdPolling = new BlockThreadPool();
                    _pollrunning = true;

                    foreach (string lisKey in keys)
                    {
                        _thdPolling.RunParamThread(DoListening, lisKey);
                    }
                    break;
                case RedisMQMessageMode.BlockQueue://阻塞队列不需要Open，自己新建连接池
                    _thdPolling = new BlockThreadPool();
                    _pollrunning = true;
                    _queRedis = new ConcurrentQueue<ConnectionMultiplexer>();
                    foreach (string lisKey in keys)
                    {
                        _thdPolling.RunParamThread(DoBlockPopListening, lisKey);
                    }
                    break;
                case RedisMQMessageMode.Stream://阻塞队列不需要Open，自己新建连接池
                    
                    IDatabase db = GetDB();
                    _thdPolling = new BlockThreadPool();
                    _pollrunning = true;
                    _queRedis = new ConcurrentQueue<ConnectionMultiplexer>();
                    foreach (string lisKey in keys)
                    {
                       
                        _pollrunning = true;
                        _thdPolling.RunParamThread(DoStreamListening, lisKey);
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
            CloseAsync().GetAwaiter().GetResult();
        }

        public override async Task CloseAsync()
        {
            _pollrunning = false;


            if (_subscriber != null)
            {
                try
                {
                    await _subscriber.UnsubscribeAllAsync(_config.CommanfFlags);
                }
                catch (Exception ex)
                {
                    await OnException(ex);
                }
            }

            _subscriber = null;
            await StopCallbackWorkerAsync();
            if (_redis != null)
            {
                try
                {
                    await _redis.CloseAsync();
                    await _redis.DisposeAsync();
                }
                catch (Exception ex)
                {
                    await OnException(ex);
                }
            }
            _redis = null;
            _db = null;
            if (_thdPolling != null)
            {
                _thdPolling.StopAll();
                await Task.Delay(100);
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
                            await conn.CloseAsync();
                            await conn.DisposeAsync();
                        }catch(Exception ex) { }
                    }
                }
            }
            _queRedis = null;
            await DisponseWait();
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
