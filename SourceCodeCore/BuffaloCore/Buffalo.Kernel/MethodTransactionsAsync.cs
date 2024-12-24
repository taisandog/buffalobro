using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 要执行的函数
    /// </summary>
    /// <param name="args">参数</param>
    public delegate Task DelDelayActionByArgsAsync(object[] args);

    
    /// <summary>
    /// 方法信息
    /// </summary>
    public class ActionInfoAsync : IDisposable
    {
        private DelDelayActionByArgsAsync _actionArgs;
        private object[] _args;
       
        /// <summary>
        /// 方法信息
        /// </summary>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public ActionInfoAsync(DelDelayActionByArgsAsync actionArgs, object[] args)
        {
            _actionArgs = actionArgs;
            _args = args;
        }
        public void Dispose()
        {
            _args = null;
            _actionArgs = null;
            GC.SuppressFinalize(this);
        }
       
        /// <summary>
        /// 参数
        /// </summary>
        public object[] Args
        {
            get
            {
                return _args;
            }
        }
        /// <summary>
        /// 运行动作
        /// </summary>
        public Task RunAction()
        {
           
            if (_actionArgs != null)
            {
                return _actionArgs(_args);
                
            }
            return Task.CompletedTask;
        }

        ~ActionInfoAsync()
        {
            Dispose();
        }
    }

    /// <summary>
    /// 延迟函数（添加当前线程的延迟动作）
    /// </summary>
    public class MethodTransactionsAsync : IDisposable
    {

        private SortedDictionary<int, Queue<ActionInfoAsync>> _dic = null;


        private static AsyncLocal<SortedDictionary<int, Queue<ActionInfoAsync>>> _actionHandle =
            new AsyncLocal<SortedDictionary<int, Queue<ActionInfoAsync>>>();


        /// <summary>
        /// 清空延迟动作的队列
        /// </summary>
        private void ClearActionQueue()
        {
            if (!_isNew)
            {
                return;
            }
            _actionHandle.Value = null;
            if (_dic != null)
            {

                _dic.Clear();
                _dic = null;
            }
            
        }



        /// <summary>
        /// 是否空的延迟队列
        /// </summary>
        private static bool IsNullQueue()
        {

            return _actionHandle.Value == null;
        }

        /// <summary>
        /// 是否创建的队列(巢状最外层)
        /// </summary>
        private bool _isNew = false;

        /// <summary>
        ///  延迟动作
        /// </summary>
        public MethodTransactionsAsync()
        {
            LoadQueue();
        }

        /// <summary>
        /// 加载队列
        /// </summary>
        /// <returns></returns>
        private void LoadQueue()
        {
            _isNew = IsNullQueue();

            _dic = _actionHandle.Value;
            if (_dic == null)
            {
                _dic = new SortedDictionary<int, Queue<ActionInfoAsync>>();
                _actionHandle.Value = _dic;
            }
        }
        /// <summary>
        /// 延迟动作的队列
        /// </summary>
        private Queue<ActionInfoAsync> GetActionQueue(int level)
        {

            Queue<ActionInfoAsync> que = null;
            if (!_dic.TryGetValue(level, out que))
            {
                que = new Queue<ActionInfoAsync>();
                _dic[level] = que;
            }

            return que;

        }
        /// <summary>
        /// 添加带参数延迟动作到队列
        /// </summary>
        /// <param name="action">执行动作</param>
        /// <param name="args">参数</param>
        /// <param name="level">优先级(High最优先，Medium次之，Low最后)</param>
        public void AddArgsAction(DelDelayActionByArgsAsync action, object[] args, DelayActionLevel level = DelayActionLevel.Medium)
        {
            AddArgsActionByLevel(action, args, (int)level);
        }
        /// <summary>
        /// 添加带参数延迟动作到队列
        /// </summary>
        /// <param name="action">执行动作</param>
        /// <param name="args">参数</param>
        /// <param name="level">优先级(越大越优先)</param>
        public void AddArgsActionByLevel(DelDelayActionByArgsAsync action, object[] args, int level)
        {

            Queue<ActionInfoAsync> currentQueue = GetActionQueue(level);
            ActionInfoAsync info = new ActionInfoAsync(action, args);
            currentQueue.Enqueue(info);
        }

        


        /// <summary>
        /// 提交动作(-1为未提交，整数为已经提交的条数)
        /// </summary>
        /// <returns></returns>
        public async Task<int> Commit(IList<Exception> lstException = null)
        {
            if (!_isNew)
            {
                return -1;
            }
            int count = 0;

            if (_dic == null)
            {
                return 0;
            }
            Queue<ActionInfoAsync> currentQueue = null;
            foreach (KeyValuePair<int, Queue<ActionInfoAsync>> kvp in _dic.Reverse())
            {
                currentQueue = kvp.Value;
                count +=await RunAction(currentQueue, lstException);
            }


            return count;
        }

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="currentQueue"></param>
        /// <param name="lstException"></param>
        /// <returns></returns>
        private async Task<int> RunAction(Queue<ActionInfoAsync> currentQueue, IList<Exception> lstException)
        {
            int count = 0;
            ActionInfoAsync action = null;
            while (currentQueue.Count > 0)
            {
                action = currentQueue.Dequeue();
                if (action == null)
                {
                    continue;
                }
                try
                {
                    await action.RunAction();
                    count++;
                }
                catch (Exception ex)
                {
                    if (lstException != null)
                    {
                        lstException.Add(ex);
                    }
                }
            }
            return count;
        }
        /// <summary>
        /// 回滚动作
        /// </summary>
        /// <returns></returns>
        public void Rollback()
        {
            ClearActionQueue();
        }

        public void Dispose()
        {
            ClearActionQueue();
        }
    }

}
