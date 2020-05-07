using Buffalo.DB.DbCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 本地缓存基类
    /// </summary>
    public abstract class LocalCacheBase
    {
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract IList GetCacheList(string key);
        public long ListAddValue<E>(string key, long index, E value, SetValueType setType, DataBaseOperate oper)
        {
            IList lst = GetCacheList(key);
            lock (lst)
            {
                if (index < 0)
                {
                    lst.Add(value);
                }
                lst.Insert((int)index, value);
                return 1;
            }
        }

        public E ListGetValue<E>(string key, long index, E defaultValue, DataBaseOperate oper)
        {

            IList lst = GetCacheList(key);
            lock (lst)
            {
                if (lst.Count <= 0)
                {
                    return defaultValue;
                }
                if (index < 0)
                {
                    index = lst.Count - 1;
                }
                return lst[(int)index].ConvertTo<E>(defaultValue);
            }
        }

        public long ListGetLength(string key, DataBaseOperate oper)
        {

            IList lst = GetCacheList(key);

            return lst.Count;
        }

        public E ListPopValue<E>(string key, bool isPopEnd, E defaultValue, DataBaseOperate oper)
        {

            IList lst = GetCacheList(key);
            lock (lst)
            {
                if (lst.Count <= 0)
                {
                    return defaultValue;
                }
                E ret = defaultValue;
                int index = 0;
                if (isPopEnd)
                {
                    index = lst.Count - 1;
                }
                ret = lst[index].ConvertTo<E>(defaultValue);
                lst.RemoveAt(index);
                return ret;
            }
        }

        public long ListRemoveValue(string key, object value, long count, DataBaseOperate oper)
        {

            IList lst = GetCacheList(key);
            lock (lst)
            {
                int ret = 0;
                for (int i = lst.Count - 1; i >= 0; i--)
                {
                    if (lst[i] == value)
                    {
                        lst.RemoveAt(i);
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

        public List<E> ListAllValues<E>(string key, long start, long end, DataBaseOperate oper)
        {
            IList lst = GetCacheList(key);
            lock (lst)
            {
                List<E> retlst = new List<E>(lst.Count);
                int s = (int)start;
                int e = (int)end;
                if (e <= 0)
                {
                    e = lst.Count - 1;
                }
                for (int i = s; i < e; i++)
                {
                    object oval = lst;
                    retlst.Add(oval.ConvertTo<E>());
                }
                return retlst;
            }
        }

        #region HashSet部分
        /// <summary>
        /// 获取哈希表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract IDictionary GetCacheHash(string key);

        /// <summary>
        /// 批量给HashSet设置值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="dicSet">值</param>
        /// <returns></returns>
        public void HashSetRangeValue(string key, IDictionary dicSet, DataBaseOperate oper)
        {
            IDictionary dic = GetCacheHash(key);
            lock (dic)
            {
                foreach (IDictionaryEnumerator kvp in dicSet)
                {
                    dic[kvp.Key] = kvp.Value;
                }
            }
            
        }
        /// <summary>
        /// HashSet设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希表的键</param>
        /// <param name="value">哈希表的值</param>
        /// <param name="type">设置方式</param>
        public bool HashSetValue(string key, object hashkey, object value, SetValueType type, DataBaseOperate oper)
        {
            IDictionary dic = GetCacheHash(key);
            lock (dic)
            {
                switch (type)
                {
                    case SetValueType.AddNew:
                        if (dic[hashkey] != null)
                        {
                            return false;
                        }

                        break;
                    case SetValueType.Replace:
                        if (dic[hashkey] == null)
                        {
                            return false;
                        }

                        break;
                    default:
                        break;
                }
                dic[key] = value;
                return true;
            }
        }
        /// <summary>
        /// 获取哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希表的键</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public E HashGetValue<E>(string key, object hashkey, E defaultValue, DataBaseOperate oper)
        {
            IDictionary dic = GetCacheHash(key);
            lock (dic)
            {
                return dic[hashkey].ConvertTo<E>(defaultValue);
            }
        }
        /// <summary>
        /// 获取所有哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public List<KeyValuePair<K, V>> HashGetAllValues<K,V>(string key, V defaultValue, DataBaseOperate oper)
        {
            List<KeyValuePair<K, V>> ret = new List<KeyValuePair<K, V>>();
            V val = default(V);
            K vkey= default(K);
            IDictionary dic = GetCacheHash(key);
            lock (dic)
            {
                foreach (IDictionaryEnumerator kvp in dic)
                {
                    val = kvp.Value.ConvertTo<V>(defaultValue);
                    vkey = kvp.Key.ConvertTo<K>();
                    ret.Add(new KeyValuePair<K, V>(vkey, val));
                }
                return ret;
            }
        }
        /// <summary>
        /// 删除哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希表的键</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public bool HashDeleteValue(string key, object hashkey, DataBaseOperate oper)
        {
            IDictionary dic = GetCacheHash(key);
            lock (dic)
            {
                dic.Remove(hashkey);
                return true;
            }
        }
        /// <summary>
        /// 批量删除哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="hashkeys">要删除哈希表的键</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public long HashDeleteValues(string key, IEnumerable hashkeys, DataBaseOperate oper)
        {
            IDictionary dic = GetCacheHash(key);
            long count = 0;
            lock (dic)
            {
                foreach (object okey in hashkeys)
                {
                    count++;
                    dic.Remove(okey);
                }
                return count;
            }
        }

       public bool HashExists(string key, object hashkey, DataBaseOperate oper)
        {
            IDictionary dic = GetCacheHash(key);
            return dic.Contains(hashkey);
        }

        public long HashIncrement(string key, object hashkey, long value, DataBaseOperate oper)
        {
            IDictionary dic = GetCacheHash(key);
            lock (dic)
            {
                long num = ValueConvertExtend.GetMapValue<long>(dic, hashkey);
                num += value;
                dic[hashkey] = num;
                return num;
            }
        }

        public long HashDecrement(string key, object hashkey, long value, DataBaseOperate oper)
        {
            IDictionary dic = GetCacheHash(key);
            lock (dic)
            {
                long num = ValueConvertExtend.GetMapValue<long>(dic, hashkey);
                num -= value;
                dic[hashkey] = num;
                return num;
            }
        }
        #endregion
    }
}
