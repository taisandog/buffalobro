using Buffalo.DB.DbCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    /// <summary>
    /// List方式的缓存操作
    /// </summary>
    public class MemoryCacheList: ICacheList
    {
        private IList _lst;
        /// <summary>
        ///  List方式的缓存操作
        /// </summary>
        /// <param name="lst"></param>
        public MemoryCacheList(IList lst) 
        {
            _lst = lst;
        }

        public long AddValue(object value, long index,  SetValueType setType)
        {
            lock (_lst)
            {
                if (index < 0)
                {
                    _lst.Add(value);
                }

                _lst.Insert((int)index, value);
                return 1;
            }
        }

        public E GetValue<E>( long index, E defaultValue)
        {
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return defaultValue;
                }
                if (index < 0)
                {
                    index = _lst.Count - 1;
                }
                return ValueConvertExtend.ConvertValue<E>(_lst[(int)index], defaultValue);
            }
        }

        public long GetLength()
        {
            return _lst.Count;
        }

        public long RemoveValue( object value, long count)
        {

            lock (_lst)
            {
                int ret = 0;
                for (int i = _lst.Count - 1; i >= 0; i--)
                {
                    if (_lst[i] == value)
                    {
                        _lst.RemoveAt(i);
                        ret++;
                        if (count > 0 && ret >= count)
                        {
                            break;
                        }
                    }
                }
                return ret;
            }
        }

        public List<E> AllValues<E>( long start, long end)
        {
            lock (_lst)
            {
                List<E> retlst = new List<E>(_lst.Count);
                int s = (int)start;
                int e = (int)end;
                if (e <= 0)
                {
                    e = _lst.Count - 1;
                }
                for (int i = s; i < e; i++)
                {
                    object oval = _lst;
                    retlst.Add(ValueConvertExtend.ConvertValue<E>(oval));
                }
                return retlst;
            }
        }

        public long AddRangValue(IEnumerable values, long index, SetValueType setType)
        {
            lock (_lst)
            {
                if (index < 0)
                {
                    foreach (object obj in values)
                    {
                        _lst.Add(obj);
                    }
                }
                int curIndex = (int)index;
                foreach (object obj in values)
                {
                    if (curIndex >= _lst.Count)
                    {
                        _lst.Add(obj);
                    }
                    else
                    {
                        _lst.Insert(curIndex, obj);
                    }
                    curIndex++;
                }
                return index;
            }
        }

        public long InsertAfter( object pivot, object value)
        {
            lock (_lst)
            {
                int index = -1;
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i] == pivot)
                    {
                        index = i;
                    }
                }
                if (index < 0)
                {
                    return -1;
                }
                _lst.Insert(index, value);
                return 1;
            }
        }

        public long InsertBefore( object pivot, object value)
        {
            int index = -1;
            lock (_lst)
            {
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i] == pivot)
                    {
                        index = i;
                    }
                }
                if (index < 0)
                {
                    return -1;
                }
                if (index >= _lst.Count - 1)
                {
                    _lst.Add(value);
                }
                else
                {
                    _lst.Insert(index + 1, value);
                }
                return 1;
            }
        }
        /// <summary>
        /// Pop形式输出值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <param name="isPopEnd"></param>
        /// <returns></returns>
        public E PopValue<E>( bool isPopEnd, E defaultValue)
        {

            object ret = null;
            lock (_lst)
            {
                if (_lst.Count <= 0)
                {
                    return defaultValue;
                }
                if (isPopEnd) 
                {
                    ret = _lst[_lst.Count - 1];
                    _lst.RemoveAt(_lst.Count - 1);
                }
                else 
                {
                    ret = _lst[0];
                    _lst.RemoveAt(0);
                }
            }
            return ret.ConvertTo<E>();
        }
    }
}
