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
    /// Hash方式的缓存操作
    /// </summary>
    public class MemoryCacheHash: ICacheHash
    {
        IDictionary<string,object> _dic;
        public MemoryCacheHash(IDictionary<string, object> dic) 
        {
            _dic = dic;
        }


        /// <summary>
        /// 批量给HashSet设置值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="dicSet">值</param>
        /// <returns></returns>
        public void SetRangeValue(IDictionary<string, object> dicSet)
        {
            
            lock (_dic)
            {
                foreach (KeyValuePair<string,object> kvp in dicSet)
                {
                    _dic[kvp.Key] = kvp.Value;
                }
            }

        }
        /// <summary>
        /// HashSet设置值
        /// </summary>
        /// <param name="key">哈希表的键</param>
        /// <param name="value">哈希表的值</param>
        /// <param name="type">设置方式</param>
        public bool SetValue(string key, object value, SetValueType type)
        {
            
            lock (_dic)
            {
                switch (type)
                {
                    case SetValueType.AddNew:
                        if (_dic[key] != null)
                        {
                            return false;
                        }

                        break;
                    case SetValueType.Replace:
                        if (_dic[key] == null)
                        {
                            return false;
                        }

                        break;
                    default:
                        break;
                }
                _dic[key] = value;
                return true;
            }
        }
        /// <summary>
        /// 获取哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">哈希表的键</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public E GetValue<E>(string key, E defaultValue)
        {
            
            lock (_dic)
            {
                return ValueConvertExtend.ConvertValue<E>(_dic[key], defaultValue);
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
        public List<KeyValuePair<string, V>> GetAllValues< V>(V defaultValue)
        {
            List<KeyValuePair<string, V>> ret = new List<KeyValuePair<string, V>>();
            V val = default(V);
            string vkey = null;
            
            lock (_dic)
            {
                foreach (KeyValuePair<string, object> kvp in _dic)
                {
                    val = ValueConvertExtend.ConvertValue<V>(kvp.Value, defaultValue);
                    vkey = ValueConvertExtend.ConvertValue<string>(kvp.Key);
                    ret.Add(new KeyValuePair<string, V>(vkey, val));
                }
                return ret;
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
        public ICollection<string> GetAllKeys()
        {
            
            return _dic.Keys;
        }
        /// <summary>
        /// 删除哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">哈希表的键</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public bool DeleteValue(string key)
        {
           
            lock (_dic)
            {
                _dic.Remove(key);
                return true;
            }
        }
        /// <summary>
        /// 批量删除哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="keys">要删除哈希表的键</param>
        /// <returns></returns>
        public long DeleteValues(IEnumerable<string> keys)
        {
            
            long count = 0;
            lock (_dic)
            {
                foreach (string okey in keys)
                {
                    count++;
                    _dic.Remove(okey);
                }
                return count;
            }
        }

        public bool Exists(string key)
        {
            
            return _dic.ContainsKey(key);
        }

        public long Increment(string key, long value)
        {
            
            lock (_dic)
            {
                long num = ValueConvertExtend.GetDicDataValue<string,object>(_dic, key).ConvertTo<long>();
                num += value;
                _dic[key] = num;
                return num;
            }
        }

        public long Decrement(string key, long value)
        {
            
            lock (_dic)
            {
                long num = ValueConvertExtend.GetDicDataValue<string, object>(_dic, key).ConvertTo<long>();
                num -= value;
                _dic[key] = num;
                return num;
            }
        }
    }
}
