using Buffalo.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel
{
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
    public delegate void DelDelayAction();
    /// <summary>
    /// 延迟动作（添加当前线程的延迟动作）
    /// </summary>
    public class TranDelayAction:IDisposable
    {
       
        private Dictionary<DelayActionLevel, Queue<DelDelayAction>> _dic = null;

        /// <summary>
        /// 延迟动作的队列
        /// </summary>
        private Queue<DelDelayAction> GetActionQueue(DelayActionLevel level)
        {

            Queue<DelDelayAction> que = null;
            if (!_dic.TryGetValue(level, out que))
            {
                que = new Queue<DelDelayAction>();
                _dic[level] = que;
            }

            return que;
            //ContextValue.Current[ActionKey] = value;

        }
        private static ThreadLocal<Dictionary<string, bool>> _actionTag = new ThreadLocal<Dictionary<string, bool>>();
        private static ThreadLocal<Dictionary<DelayActionLevel, Queue<DelDelayAction>>> _actionHandle = 
            new ThreadLocal<Dictionary<DelayActionLevel, Queue<DelDelayAction>>>();

        /// <summary>
        /// 获取已经存在的动作去重标记
        /// </summary>
        public static Dictionary<string, bool> ActionTag
        {
            get
            {
                Dictionary<string, bool> dic = _actionTag.Value;
                if (dic == null)
                {
                    dic = new Dictionary<string, bool>();
                    _actionTag.Value = dic;
                }
                return dic;
            }
        }
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
            _actionTag.Value = null;
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
                _dic = new Dictionary<DelayActionLevel, Queue<DelDelayAction>>();
                _actionHandle.Value = _dic;
            }
        }

        /// <summary>
        /// 添加延迟动作到队列
        /// </summary>
        /// <param name="action">执行动作</param>
        /// <param name="tag">去重标记(null为不去重)</param>
        /// <param name="level">优先级(High最优先，Medium次之，Low最后)</param>
        public void AddAction(DelDelayAction action, string tag = null, DelayActionLevel level= DelayActionLevel.Medium)
        {
            if (!string.IsNullOrWhiteSpace(tag))
            {
                Dictionary<string, bool> dic = ActionTag;
                if (dic.ContainsKey(tag))
                {
                    return;
                }
                dic[tag] = true;
            }
            Queue<DelDelayAction> currentQueue = GetActionQueue(level);

            currentQueue.Enqueue(action);
        }
        /// <summary>
        /// 提交动作(-1为未提交，整数为已经提交的条数)
        /// </summary>
        /// <returns></returns>
        public int Commit(IList<Exception> lstException=null,bool logError=false)
        {
            if (!_isNew)
            {
                return -1;
            }
            int count = 0;
            Queue < DelDelayAction > currentQueue = GetActionQueue(DelayActionLevel.High);
            count +=RunAction(currentQueue, lstException, logError);
            currentQueue = GetActionQueue(DelayActionLevel.Medium);
            count += RunAction(currentQueue, lstException, logError);
            currentQueue = GetActionQueue(DelayActionLevel.Low);
            count += RunAction(currentQueue, lstException, logError);
            return count;
        }

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="currentQueue"></param>
        /// <param name="lstException"></param>
        /// <returns></returns>
        private int RunAction(Queue<DelDelayAction> currentQueue, IList<Exception> lstException, bool logError)
        {
            int count = 0;
            DelDelayAction action = null;
            while (currentQueue.Count > 0)
            {
                action = currentQueue.Dequeue();
                if (action == null)
                {
                    continue;
                }
                try
                {
                    action();
                    count++;
                }
                catch (Exception ex)
                {
                    if (lstException != null)
                    {
                        lstException.Add(ex);
                    }
                    if (logError)
                    {
                        ApplicationLog.LogException("DelayAction", ex);
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
