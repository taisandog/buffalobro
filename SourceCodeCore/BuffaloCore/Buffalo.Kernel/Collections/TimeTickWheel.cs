using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Buffalo.Kernel.Collections
{
    /// <summary>
    /// 计算过期的时间器
    /// </summary>
    public class TimeTickWheel<T>: IDisposable
    {
        /// <summary>
        /// 时间轴
        /// </summary>
        private SortedDictionary<long,LinkedList<T>> _timelist = null;
        
        /// <summary>
        /// 数据
        /// </summary>
        public SortedDictionary<long, LinkedList<T>> Data 
        {
            get 
            {
                return _timelist;
            }
        }
        /// <summary>
        /// 时间间隔刻度(毫秒)
        /// </summary>
        private long _timescale;
        /// <summary>
        /// 时间间隔刻度(毫秒)
        /// </summary>
        public long Timescale 
        {
            get { return _timescale; }
        }
        /// <summary>
        /// 时间轮
        /// </summary>
        /// <param name="timescale">时间间隔刻度(毫秒)</param>
        public TimeTickWheel(long timescale=0) 
        {
            if(timescale <= 0) 
            {
                timescale = 1000;
            }
            _timescale = timescale;
            _timelist=new SortedDictionary<long, LinkedList<T>>();
        }

        /// <summary>
        /// 增加值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="expireTick">到期的时间戳</param>
        /// <returns></returns>
        public void AddValue(T value,long expireTick) 
        {
            if (expireTick <= 0)
            {
                expireTick = (long)CommonMethods.ConvertDateTimeInt(DateTime.Now);
            }

            long scale = GetScale(expireTick);
            LinkedList<T> lst = GetListByScale(scale);
            lst.AddLast(value);
        }
        /// <summary>
        /// 把链表加回时间轮
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="expireTick">到期的时间戳</param>
        /// <returns></returns>
        public void AddLinkedList(LinkedList<T> value, long expireTick)
        {
            if (expireTick <= 0)
            {
                expireTick = (long)CommonMethods.ConvertDateTimeInt(DateTime.Now);
            }

            long scale = GetScale(expireTick);
            LinkedList<T> lst = GetListByScale(scale,false);
            if (lst == null) 
            {
                lst = value;
                _timelist[scale] = lst;
            }
            else 
            {
                LinkedListNode<T> node = null;
                while (value.Count>0) 
                {
                    node = value.First;
                    value.Remove(node);
                    lst.AddLast(node);
                }
            }

        }

        /// <summary>
        /// 通过刻度获取链表
        /// </summary>
        /// <param name="scale">刻度</param>
        /// <param name="createNewList">如果不存在创建新集合</param>
        /// <returns></returns>
        private LinkedList<T> GetListByScale(long scale,bool createNewIfNotExists=true) 
        {
            LinkedList<T> lst = null;
            if (!_timelist.TryGetValue(scale, out lst))
            {
                if (!createNewIfNotExists) 
                {
                    return null;
                }
                lst = new LinkedList<T>();
                _timelist[scale] = lst;
            }
            return lst;
        }

        /// <summary>
        /// 计算刻度
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        protected long GetScale(long currentTime) 
        {
            long ret = currentTime / _timescale;
            if (currentTime % _timescale > 0) 
            {
                ret++;
            }
            return ret;
        }

        /// <summary>
        /// 输出并移除到期的对象
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> PutAndDeleteExpiredItems(long currentTime=0) 
        {
            if (currentTime <= 0) 
            {
                currentTime = (long)CommonMethods.ConvertDateTimeInt(DateTime.Now);
            }
            long curScale = currentTime / _timescale;
            Queue<T> ret = new Queue<T>(128);
            Queue<long> queDelete = new Queue<long>();
            LinkedList<T> curList = null;
            foreach (KeyValuePair<long, LinkedList<T>> kvp in _timelist) 
            {
                if(kvp.Key> curScale) 
                {
                    break;
                }
                queDelete.Enqueue(kvp.Key);
                curList=kvp.Value;
                foreach(T item in curList) 
                {
                    ret.Enqueue(item);
                }
                curList.Clear();
            }
            foreach(long key in queDelete) 
            {
                _timelist.Remove(key);
            }
            queDelete = null;
            curList = null;
            return ret;
        }
        /// <summary>
        /// 输出到期的对象链表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LinkedList<T>> PutExpiredLinkedList(long currentTime = 0)
        {
            if (currentTime <= 0)
            {
                currentTime = (long)CommonMethods.ConvertDateTimeInt(DateTime.Now);
            }
            long curScale = currentTime / _timescale;
            Queue<LinkedList<T>> ret = new Queue<LinkedList<T>>(32);
            Queue<long> queDelete = new Queue<long>();
            LinkedList<T> curList = null;
            foreach (KeyValuePair<long, LinkedList<T>> kvp in _timelist)
            {
                if (kvp.Key > curScale)
                {
                    break;
                }
                queDelete.Enqueue(kvp.Key);
                curList = kvp.Value;
                ret.Enqueue(curList);
            }

            foreach (long key in queDelete)
            {
                _timelist.Remove(key);
            }

            queDelete = null;
            curList = null;
            return ret;
        }
        /// <summary>
        /// 清空所有元素
        /// </summary>
        public void Clear() 
        {
            Queue<LinkedList<T>> lkList = new Queue<LinkedList<T>>(_timelist.Count);
            foreach (KeyValuePair<long, LinkedList<T>> kvp in _timelist)
            {
                lkList.Enqueue(kvp.Value);
            }
            _timelist.Clear();
            foreach(LinkedList<T> lk in lkList) 
            {
                lk.Clear();
            }
            lkList.Clear();
            lkList = null;
        }

        /// <summary>
        /// 去重检查
        /// </summary>
        /// <returns></returns>
        public long FilterRepeat() 
        {
            Dictionary<T, bool> dic = new Dictionary<T, bool>();
            Queue<LinkedListNode<T>> queWillDelete = new Queue<LinkedListNode<T>>();
            long count = 0;
            LinkedList<T> lk = null;
            LinkedListNode<T> node = null;
            T value=default(T);
            foreach (KeyValuePair<long, LinkedList<T>> kvp in _timelist)
            {
                lk =kvp.Value;
                if (lk == null) 
                {
                    continue;
                }
                node = lk.First;
                while (node != null)
                {
                    value = node.Value;
                    
                    if (dic.ContainsKey(value))
                    {
                        queWillDelete.Enqueue(node);
                        
                    }
                    else
                    {
                        dic[value] = true;
                    }

                    node = CommonMethods.LinkedListNodeMoceNext<T>(lk, node);
                }
            }

            LinkedListNode<T> delNode = null;
            count = queWillDelete.Count;
            while (queWillDelete.TryDequeue(out delNode))
            {
                if (delNode.List != null)
                {
                    delNode.List.Remove(delNode);
                }
            }
            queWillDelete=null;
            return count;
            
            
        }

        

        public void Dispose()
        {
            Clear();
            _timelist = null;

        }

        
    }
}
