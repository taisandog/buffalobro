using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    public delegate void DelOnMQReceived(MQListener sender, MQCallBackMessage message);


    public delegate Task DelOnMQReceivedAsync(MQListener sender, MQCallBackMessage message);


    public delegate Task DelOnMQException(MQListener sender, Exception ex);

    public delegate Task DelOnMQSettlement(MQListener sender, MQCallBackMessage message);

    public abstract class MQListener : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 接收数据
        /// </summary>
        public event DelOnMQReceivedAsync OnMQReceivedAsync;

        /// <summary>
        /// 发生异常
        /// </summary>
        public event DelOnMQException OnMQException;

        /// <summary>
        /// 消息请求重试后触发。事件仅用于监控，重试数据仍由 Broker 保存。
        /// </summary>
        public event DelOnMQSettlement OnMQRetry;

        /// <summary>
        /// 消息进入死信后触发。事件仅用于监控，不能代替死信队列。
        /// </summary>
        public event DelOnMQSettlement OnMQDeadLetter;

        /// <summary>
        /// 通过 StartDeadLetterListenAsync 显式监听死信时触发。
        /// </summary>
        public event DelOnMQReceivedAsync OnMQDeadLetterReceivedAsync;

        protected bool IsDeadLetterListener { get; set; }

        protected MQRetryOptions RetryOptions { get; private set; } = new MQRetryOptions();

        protected void ConfigureRetry(MQConfigBase config)
        {
            RetryOptions = config.RetryOptions;
        }

        /// <summary>
        /// 打开事件监听
        /// </summary>
        /// <param name="listenKeys">监听键</param>
        public abstract void StartListend(IEnumerable<string> listenKeys);

        /// <summary>
        /// 异步打开事件监听。派生类应在底层组件支持异步时重写此方法。
        /// </summary>
        public virtual Task StartListendAsync(IEnumerable<string> listenKeys)
        {
            StartListend(listenKeys);
            return Task.CompletedTask;
        }

        /// <summary>
        /// StartListendAsync 的正确拼写别名。
        /// </summary>
        public Task StartListenAsync(IEnumerable<string> listenKeys)
        {
            return StartListendAsync(listenKeys);
        }

        /// <summary>
        /// 显式监听对应主题的死信。建议使用独立的 Listener 实例。
        /// </summary>
        public virtual void StartDeadLetterListen(IEnumerable<string> listenKeys)
        {
            StartDeadLetterListenAsync(listenKeys).GetAwaiter().GetResult();
        }

        public virtual Task StartDeadLetterListenAsync(IEnumerable<string> listenKeys)
        {
            IsDeadLetterListener = true;
            return StartListendAsync(listenKeys.Select(RetryOptions.GetDeadLetterTopic));
        }
        ///// <summary>
        ///// 打开事件监听
        ///// </summary>
        ///// <param name="listenKeys">监听键</param>
        //public abstract void StartListend(IEnumerable<MQOffestInfo> listenKeys);

        public abstract void Dispose();

        /// <summary>
        /// 关闭连接
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// 异步关闭连接。派生类应在底层组件支持异步时重写此方法。
        /// </summary>
        public virtual Task CloseAsync()
        {
            Close();
            return Task.CompletedTask;
        }

        public virtual async ValueTask DisposeAsync()
        {
            await CloseAsync();
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// 开启监听的句柄
        /// </summary>
        private AutoResetEvent _startHandle = null;
        /// <summary>
        /// 等待监听开始
        /// </summary>
        public bool WaitStart(int millisecondsTimeout=2000)
        {
            if (_startHandle==null)
            {
                return true;
            }
            return _startHandle.WaitOne(millisecondsTimeout);
        }
        /// <summary>
        /// 重置等待
        /// </summary>
        protected void ResetWait()
        {
            _startHandle = new AutoResetEvent(true);
            _startHandle.Reset();
        }
        /// <summary>
        /// 放行阻塞
        /// </summary>
        protected void SetWait()
        {
            _startHandle.Set();
        }
        /// <summary>
        /// 清空阻塞
        /// </summary>
        protected async Task DisponseWait()
        {
            if (_startHandle != null)
            {
                try
                {
                    _startHandle.Close();
                }
                catch (Exception ex)
                {
                    await OnException(ex);
                }
            }
            _startHandle = null;
        }
        /// <summary>
        /// 监听信息后回调
        /// </summary>
        protected async Task CallBack(MQCallBackMessage message)
        {
            message.ConfigureSettlement(RetryOptions.MaxDeliveryCount,
                RetryOptions.DeadLetterEnabled, OnMessageSettled);
            DelOnMQReceivedAsync receivedHandler = IsDeadLetterListener
                ? OnMQDeadLetterReceivedAsync : OnMQReceivedAsync;
            if (receivedHandler == null)
            {
                return;
            }
            try
            {
                await receivedHandler(this, message);
                if (RetryOptions.AckMode == MQAckMode.OnSuccess &&
                    message.SettlementState == MQSettlementState.Pending)
                {
                    await message.AckAsync();
                }
                else if (RetryOptions.RetryEnabled &&
                    message.SettlementState == MQSettlementState.Pending)
                {
                    message.SetFailure(MQFailureType.AckTimeout,
                        "消费回调结束后没有确认消息");
                    await message.RetryAsync("消费回调结束后没有确认消息",
                        TimeSpan.FromMilliseconds(RetryOptions.RetryDelayMilliseconds));
                }
            }
            catch (Exception ex)
            {
                message.SetFailure(MQFailureType.HandlerException, ex.Message);
                if (RetryOptions.RetryEnabled && RetryOptions.RetryOnHandlerException &&
                    message.SettlementState == MQSettlementState.Pending)
                {
                    try
                    {
                        await message.RetryAsync(ex.ToString(),
                            TimeSpan.FromMilliseconds(RetryOptions.RetryDelayMilliseconds));
                    }
                    catch (Exception settlementException)
                    {
                        await OnException(new AggregateException(ex, settlementException));
                        return;
                    }
                }
                await OnException(ex);
            }
        }

        private async Task OnMessageSettled(MQCallBackMessage message, MQSettlementState state)
        {
            if (state == MQSettlementState.RetryRequested && OnMQRetry != null)
            {
                await OnMQRetry(this, message);
            }
            else if (state == MQSettlementState.DeadLettered && OnMQDeadLetter != null)
            {
                await OnMQDeadLetter(this, message);
            }
        }
       
        /// <summary>
        /// 监听信息后回调
        /// </summary>
        protected async Task OnException(Exception ex)
        {
            if (OnMQException == null)
            {
                return;
            }
            await OnMQException(this, ex);
        }
    }
}
