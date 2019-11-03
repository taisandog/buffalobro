
using Buffalo.Kernel.AutoServicesLib;
using Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.Kernel.AutoServicesLib
{
    /// <summary>
    /// 自动服务
    /// </summary>
    public class AutoServicesRun:IDisposable
    {
        private Thread _thd;
        private ServicesLoader _loader;
        private IShowMessage _message;
        private IStatePanel _statePanel;
        private ManualResetEvent _event;
        private int _delay;
        /// <summary>
        /// 轮询间隔
        /// </summary>
        public int Delay
        {
            get
            {
                return _delay;
            }
        }
        /// <summary>
        /// 信息
        /// </summary>
        public IShowMessage Message
        {
            get { return _message; }
            
        }
        /// <summary>
        /// 信息
        /// </summary>
        public IStatePanel StatePanel
        {
            get { return _statePanel; }

        }
        /// <summary>
        /// 服务加载器
        /// </summary>
        public ServicesLoader Loader
        {
            get { return _loader; }
        }
        private bool _isRun = true;
        /// <summary>
        /// 是否运行中
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return _isRun;
            }
        }
        /// <summary>
        /// 自动服务
        /// </summary>
        /// <param name="delay">执行间隔</param>
        public AutoServicesRun(int delay, IShowMessage message, IStatePanel statePanel) 
        {
            _loader = new ServicesLoader(this);
            _loader.OnThrowException += new ThrowExceptionHandle(_loader_OnThrowException);
            _delay = delay;
            _message = message;
            _statePanel = statePanel;
            _event = new ManualResetEvent(true);
        }
        public AutoServicesRun(IShowMessage message, IStatePanel statePanel=null) :this(60*1000,message, statePanel)
        {

        }

        static void _loader_OnThrowException(AbsServicesHandle handle, Exception ex)
        {
            ApplicationLog.LogException("Server.AutoServices", ex);
        }
        private void RunAuto()
        {

            _event.Reset();
            try
            {
                while (_isRun)
                {

                    try
                    {
                        _loader.DoTick();

                    }
                    catch (Exception ex)
                    {
                        ApplicationLog.LogException("Server.AutoServices", ex);
                        if (_message != null)
                        {
                            _message.LogError(ex.ToString());
                        }
                    }
                    Thread.Sleep(_delay);//执行间隔
                }
            }
            finally
            {
                _event.Set();
            }
        }
        private Encoding _defaultEncoding = Encoding.GetEncoding("GBK");

        /// <summary>
        /// 开始执行服务
        /// </summary>
        public void StartService() 
        {
            _isRun = true;
            _thd = new Thread(new ThreadStart(RunAuto));
            _thd.Start();
        }

        /// <summary>
        /// 终止服务
        /// </summary>
        /// <param name="waitMillisecondsTimeout">终止服务时候线程等待多久就强行终止</param>
        public void StopService(int waitMillisecondsTimeout=1000)
        {
            if (_thd != null)
            {
                
                _isRun = false;
                if (!_event.WaitOne(waitMillisecondsTimeout))
                {
                    try
                    {
                        _thd.Abort();
                        Thread.Sleep(500);
                    }
                    catch { }
                }
                
                
                _thd = null;
            }
            _loader.ClearAnyc();
        }

        public void Dispose()
        {
            StopService();
        }
        ~AutoServicesRun() 
        {
            StopService();
        }
    }
}
