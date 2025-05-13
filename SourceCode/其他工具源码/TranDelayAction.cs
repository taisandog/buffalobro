using Buffalo.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 要执行的函数
    /// </summary>
    /// <param name="args">参数</param>
    public delegate void DelDelayActionByArgs(object[] args);
    /// <summary>
    /// 要执行的函数
    /// </summary>
    /// <param name="args">参数</param>
    public delegate void DelDelayAction();
    /// <summary>
    /// 执行优先级
    /// </summary>
    public enum DelayActionLevel:int
    {
        /// <summary>
        /// 低优先级
        /// </summary>
        Low=1,
        /// <summary>
        /// 中优先级
        /// </summary>
        Medium=2,
        /// <summary>
        /// 高优先级
        /// </summary>
        High=3
    }
    /// <summary>
    /// 方法信息
    /// </summary>
    public class ActionInfo:IDisposable
    {
        private DelDelayAction _action;
        private DelDelayActionByArgs _actionArgs;
        private object[] _args;
        /// <summary>
        /// 方法信息
        /// </summary>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public ActionInfo(DelDelayAction action) 
        {
            _action = action;
        }
        /// <summary>
        /// 方法信息
        /// </summary>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public ActionInfo(DelDelayActionByArgs actionArgs, object[] args)
        {
            _actionArgs = actionArgs;
            _args = args;
        }
        public void Dispose()
        {
            _action = null;
            _args = null;
            _actionArgs = null;
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 方法
        /// </summary>
        public DelDelayAction Action
        {
            get { return _action; }
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
        public void RunAction() 
        {
            if (_action != null) 
            {
                _action();
                return;
            }
            if (_actionArgs != null) 
            {
                _actionArgs(_args);
                return;
            }
        }

        ~ActionInfo() 
        {
            Dispose();
        }
    }

    
    /// <summary>
    /// 延迟动作（添加当前线程的延迟动作）
    /// </summary>
    public class TranDelayAction:IDisposable
    {
       
        private SortedDictionary<int, Queue<ActionInfo>> _dic = null;

        
        private static ThreadLocal<SortedDictionary<int, Queue<ActionInfo>>> _actionHandle = 
            new ThreadLocal<SortedDictionary<int, Queue<ActionInfo>>>();

        
        /// <summary>
        /// 清空延迟动作的队列
        /// </summary>
        private void ClearActionQueue()
        {
            if (!_isNew)
            {
                return;
            }
            if(_dic!=null)
            {
                
                _dic.Clear();
            }
            _actionHandle.Value = null;
        }



        /// <summary>
        /// 是否空的延迟队列
        /// </summary>
        private static bool IsNullQueue()
        {

            return _actionHandle.Value==null;
        }

        /// <summary>
        /// 是否创建的队列(巢状最外层)
        /// </summary>
        private bool _isNew = false;

        /// <summary>
        ///  延迟动作
        /// </summary>
        public TranDelayAction()
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
                _dic = new SortedDictionary<int, Queue<ActionInfo>>();
                _actionHandle.Value = _dic;
            }
        }
        /// <summary>
        /// 延迟动作的队列
        /// </summary>
        private Queue<ActionInfo> GetActionQueue(int level)
        {

            Queue<ActionInfo> que = null;
            if (!_dic.TryGetValue(level, out que))
            {
                que = new Queue<ActionInfo>();
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
        public void AddArgsAction(DelDelayActionByArgs action, object[] args, DelayActionLevel level= DelayActionLevel.Medium)
        {
            AddArgsActionByLevel(action,args, (int)level);
        }
        /// <summary>
        /// 添加带参数延迟动作到队列
        /// </summary>
        /// <param name="action">执行动作</param>
        /// <param name="args">参数</param>
        /// <param name="level">优先级(越大越优先)</param>
        public void AddArgsActionByLevel(DelDelayActionByArgs action, object[] args, int level)
        {

            Queue<ActionInfo> currentQueue = GetActionQueue(level);
            ActionInfo info = new ActionInfo(action, args);
            currentQueue.Enqueue(info);
        }

        /// <summary>
        /// 添加无参数延迟动作到队列
        /// </summary>
        /// <param name="action">执行动作</param>
        /// <param name="level">优先级(High最优先，Medium次之，Low最后)</param>
        public void AddAction(DelDelayAction action, DelayActionLevel level = DelayActionLevel.Medium)
        {
            AddActionByLevel(action, (int)level);
        }
        /// <summary>
        /// 添加无参数延迟动作到队列
        /// </summary>
        /// <param name="action">执行动作</param>
        /// <param name="level">优先级(越大越优先)</param>
        public void AddActionByLevel(DelDelayAction action, int level)
        {

            Queue<ActionInfo> currentQueue = GetActionQueue(level);
            ActionInfo info = new ActionInfo(action);
            currentQueue.Enqueue(info);
        }


        /// <summary>
        /// 提交动作(-1为未提交，整数为已经提交的条数)
        /// </summary>
        /// <returns></returns>
        public int Commit(IList<Exception> lstException=null)
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
            Queue<ActionInfo> currentQueue = null;
            foreach (KeyValuePair<int, Queue<ActionInfo>> kvp in _dic.Reverse()) 
            {
                currentQueue =kvp.Value;
                count += RunAction(currentQueue, lstException);
            }

            
            return count;
        }

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="currentQueue"></param>
        /// <param name="lstException"></param>
        /// <returns></returns>
        private int RunAction(Queue<ActionInfo> currentQueue, IList<Exception> lstException)
        {
            int count = 0;
            ActionInfo action = null;
            while (currentQueue.Count > 0)
            {
                action = currentQueue.Dequeue();
                if (action == null)
                {
                    continue;
                }
                try
                {
                    action.RunAction();
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
