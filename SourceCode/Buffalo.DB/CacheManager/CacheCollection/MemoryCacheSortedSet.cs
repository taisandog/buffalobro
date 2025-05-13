using Buffalo.DB.QueryConditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    public class MemoryCacheSortedSet: ICacheSortedSet
    {
        private SortedSet<SortedSetItem> _lst;
        /// <summary>
        ///  List方式的缓存操作
        /// </summary>
        /// <param name="lst"></param>
        public MemoryCacheSortedSet(SortedSet<SortedSetItem> lst)
        {
            
            _lst = lst;
        }
        /// <summary>
        /// 向排序表增加元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="source">排序键</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public long Add(object value, double sorce, SetValueType setType) 
        {
            lock (_lst)
            {
                
                SortedSetItem item = new SortedSetItem(value, sorce);
                _lst.Add(item);
                return 1;
            }
        }
        /// <summary>
        /// 向排序表增加元素
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="source">排序键</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public long AddRang(IEnumerable<SortedSetItem> values, SetValueType setType)
        {
            int count = 0;
            lock (_lst)
            {
                foreach (SortedSetItem item in values)
                {
                    _lst.Add(item);
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// 减少排序的元素的排序分
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="source">排序分</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public double Decrement(object value, double source) 
        {
            SortedSetItem item = FindSortedSetItemByValue(value);
            if (value == null) 
            {
                return -1;
            }
            lock (item) 
            {
                item.Key -= source;
                return item.Key;
            }
        }

        private SortedSetItem FindSortedSetItemByValue(object value) 
        {
            lock (_lst)
            {
                foreach (SortedSetItem item in _lst)
                {
                    if (item.Value == value)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 增加排序的元素的排序分
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="source">排序分</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public double Increment(object value, double source) 
        {
            SortedSetItem item = FindSortedSetItemByValue(value);
            if (value == null)
            {
                return -1;
            }
            lock (item)
            {
                item.Key += source;
                return item.Key;
            }
        }

       

        /// <summary>
        /// 获取集合长度
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public long GetLength(double? min, double? max) 
        {
            int count = 0;
            double cur = 0;
            lock (_lst)
            {
                foreach (SortedSetItem item in _lst)
                {
                    cur = item.Key;
                    if (min!=null && cur < min)
                    {
                        continue;
                    }
                    if (max != null && cur > max)
                    {
                        continue;
                    }
                    count++;
                    if (cur > max) 
                    {
                        break;
                    }
                }
            }
            return count;
        }
        /// <summary>
        /// 弹出值
        /// </summary>
        /// <param name="count">值的数量</param>
        /// <param name="order">弹出方向，SortType.ASC是从开头弹出，SortType.DESC是从尾部弹出</param>
        /// <returns></returns>
        public SortedSetItem Pop(SortType order = SortType.ASC)
        {
            SortedSetItem[] arr = Pop(1, order);
            if (arr == null ||arr.Length<=0) 
            {
                return null;
            }
            return arr[0];
        }
        /// <summary>
        /// 弹出值
        /// </summary>
        /// <param name="count">值的数量</param>
        /// <param name="order">弹出方向，SortType.ASC是从开头弹出，SortType.DESC是从尾部弹出</param>
        /// <returns></returns>
        public SortedSetItem[] Pop(long count, SortType order = SortType.ASC)
        {
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return null;
                }
                if (count > _lst.Count)
                {
                    count = _lst.Count;
                }
                SortedSetItem[] ret = new SortedSetItem[count];
                if (order == SortType.ASC)
                {
                    _lst.CopyTo(ret, 0, (int)count);
                }
                else
                {
                    int startIndex = _lst.Count - (int)count;
                    _lst.CopyTo(ret, startIndex, (int)count);
                }
                foreach (SortedSetItem item in ret)
                {
                    _lst.Remove(item);
                }
                return ret;
            }
        }

        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public SortedSetItem[] GetRangeByRankWithScores(long start = 0, long stop = -1, SortType order = SortType.ASC) 
        {
            int count = _lst.Count;
            if (stop > 0) 
            {
                count = (int)stop - (int)start;
            }
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return null;
                }
                
                SortedSetItem[] ret = new SortedSetItem[count];
                if (order == SortType.ASC)
                {
                    _lst.CopyTo(ret, 0, (int)count);
                    foreach (SortedSetItem item in ret)
                    {
                        _lst.Remove(item);
                    }
                }
                else
                {
                    int startIndex = _lst.Count - (int)count;
                    _lst.CopyTo(ret, startIndex, (int)count);
                    foreach (SortedSetItem item in ret)
                    {
                        _lst.Remove(item);
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public T[] GetRangeByRank<T>(long start = 0, long stop = -1, SortType order = SortType.ASC)
        {
            SortedSetItem[] arr = GetRangeByRankWithScores(start, stop, order);
            if (arr == null || arr.Length <= 0) 
            {
                return null;
            }
            
            
            return SortedSetItemToValue<T>(arr);
        }

        /// <summary>
        /// SortedSetItem数组转成数值数组
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private T[] SortedSetItemToValue<T>(SortedSetItem[] arr) 
        {
            T[] ret = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                ret[i] = arr[i].Value.ConvertTo<T>();
            }
            return ret;
        }
        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public T[] GetRangeByScore<T>(double? start, double? stop,
            SortType order = SortType.ASC, long skip = 0, long take = -1)
        {
            SortedSetItem[] arr = GetRangeByScoreWithScores(start, stop, order,skip,take);
            if (arr == null || arr.Length <= 0)
            {
                return null;
            }
            return SortedSetItemToValue<T>(arr);
        }
            /// <summary>
            /// 根据索引范围获取值
            /// </summary>
            /// <returns></returns>
            public SortedSetItem[] GetRangeByScoreWithScores(double? start, double? stop, 
            SortType order = SortType.ASC, long skip = 0, long take = -1)
        {

            List<SortedSetItem> lstRet = new List<SortedSetItem>();
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return null;
                }
                IEnumerable<SortedSetItem> enu = null;
                if (order == SortType.ASC)
                {
                    enu = _lst;
                }
                else
                {
                    enu = _lst.Reverse();
                }
                long needSkip = skip;
                long taked = 0;
                foreach (SortedSetItem item in enu)
                {
                    if (start != null && start.HasValue && item.Key < start)
                    {
                        continue;
                    }
                    if (stop != null && start.HasValue && item.Key > stop)
                    {
                        continue;
                    }

                    if (needSkip > 0)
                    {
                        needSkip--;
                        continue;
                    }

                    lstRet.Add(item);
                    taked++;
                    if (take > 0 && taked >= take)
                    {
                        break;
                    }

                }
                return lstRet.ToArray();
            }
        }

        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public T[] GetRangeByValue<T>(object min, object max, long skip = 0, long take = -1)
        {
            SortedSetItem[] arr = GetRangeByValueWithScores(min, max, SortType.ASC, skip, take);
            if (arr == null || arr.Length <= 0)
            {
                return null;
            }
            return SortedSetItemToValue<T>(arr);
        }

        /// <summary>
        /// 根据索引范围获取值
        /// </summary>
        /// <returns></returns>
        public SortedSetItem[] GetRangeByValueWithScores(object min ,object max , 
            SortType order = SortType.ASC, long skip = 0, long take = -1)
        {

            List<SortedSetItem> lstRet = new List<SortedSetItem>();
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return null;
                }
                IEnumerable<SortedSetItem> enu = null;
                if (order == SortType.ASC)
                {
                    enu = _lst;
                }
                else
                {
                    enu = _lst.Reverse();
                }
                long needSkip = skip;
                long taked = 0;

                IComparable cstart = min as IComparable;
                IComparable cend = max as IComparable;
                foreach (SortedSetItem item in enu)
                {
                    if (cend != null && cend.CompareTo(item.Value)<0)
                    {
                        continue;
                    }
                    if (cstart != null && cstart.CompareTo(item.Value)>0)
                    {
                        continue;
                    }

                    if (needSkip > 0)
                    {
                        needSkip--;
                        continue;
                    }

                    lstRet.Add(item);
                    taked++;
                    if (take > 0 && taked >= take)
                    {
                        break;
                    }

                }
                return lstRet.ToArray();
            }
        }

        /// <summary>
        /// 获取索引
        /// </summary>
        /// <param name="value"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public long GetIndex(object value,SortType order = SortType.ASC) 
        {
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return -1;
                }
                IEnumerable<SortedSetItem> enu = null;
                if (order == SortType.ASC)
                {
                    enu = _lst;
                }
                else
                {
                    enu = _lst.Reverse();
                }
                
                IComparable cvalue = value as IComparable;
                int index = 0;
                foreach (SortedSetItem item in enu)
                {
                    if (cvalue.CompareTo(item.Value)==0) 
                    {
                        return index;
                    }
                    index++;
                }
                return -1;
            }
        }
        /// <summary>
        /// 获取索引
        /// </summary>
        /// <param name="value"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public double? GetKeyByValue(object value)
        {
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return -1;
                }
                IEnumerable<SortedSetItem> enu = null;
                
                    enu = _lst;
               

                IComparable cvalue = value as IComparable;
               
                foreach (SortedSetItem item in enu)
                {
                    if (cvalue.CompareTo(item.Value) == 0)
                    {
                        return item.Key;
                    }
                    
                }
                return null;
            }
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long Remove(object value)
        {
            return Remove(new object[] { value });
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long Remove(object[] values) 
        {
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return -1;
                }
                List<IComparable> lstValue = new List<IComparable>(values.Length);
                foreach(object obj in values) 
                {
                    IComparable cvalue = obj as IComparable;
                    if (cvalue == null) 
                    {
                        continue;
                    }
                    lstValue.Add(cvalue);
                }
                int index = 0;
                List<SortedSetItem> lstDelete = new List<SortedSetItem>();
                foreach (SortedSetItem item in _lst)
                {
                    foreach (IComparable cvalue in lstValue)
                    {
                        if (cvalue.CompareTo(item.Value) == 0)
                        {
                            lstDelete.Add(item);
                            break;
                        }
                    }
                }
                return DeleteItems(lstDelete);
            }
        }

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long RemoveRangeByRank(long start=0, long stop=-1)
        {
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return -1;
                }
                SortedSetItem[] arrItem = GetRangeByRankWithScores(start, stop);
                return DeleteItems(arrItem);
            }
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long RemoveRangeByKey(double? start, double? stop)
        {
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return -1;
                }
                SortedSetItem[] arrItem = GetRangeByScoreWithScores(start, stop);
                return DeleteItems(arrItem);
            }
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public long RemoveRangeByValue(object min, object max)
        {
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return -1;
                }
                SortedSetItem[] arrItem = GetRangeByValueWithScores(min, max);
                return DeleteItems(arrItem);
            }
        }
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="lstDelete">要删除的集合</param>
        /// <returns></returns>
        private long DeleteItems(IEnumerable<SortedSetItem> lstDelete) 
        {
            long count = 0;
            foreach (SortedSetItem item in lstDelete)
            {
                count += _lst.Remove(item) ? 1 : 0;
            }
            return count;
        }
    }

    /// <summary>
    /// 排序项
    /// </summary>
    public class SortedSetItem:IEquatable<SortedSetItem>, IComparable, IComparable<SortedSetItem>
    {
        /// <summary>
        /// 值
        /// </summary>
        private object _value;
        /// <summary>
        /// 排序分
        /// </summary>
        private double _key;

        /// <summary>
        /// 对比分
        /// </summary>
        public double Key 
        {
            get 
            {
                return _key;
            }
            internal set 
            {
                _key = value;
            }
        }
        /// <summary>
        /// 排序项
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="score">对比分</param>
        public SortedSetItem(object value, double score) 
        {
            _key = score;
            _value = value;
        }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
        }
        public int CompareTo(object obj)
        {
            SortedSetItem target = obj as SortedSetItem;
            if (target!=null) 
            {
                return _key.CompareTo(target._key);
            }
            return -1;
        }

        public int CompareTo(SortedSetItem other)
        {
            if (other != null)
            {
                return _key.CompareTo(other._key);
            }
            return -1;
        }

        public bool Equals(SortedSetItem other)
        {
            return _key == other._key && _value == other._value;
        }
    }
}
