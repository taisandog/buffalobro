using Buffalo.DB.CacheManager;
using Buffalo.DB.CacheManager.CacheCollection;
using Buffalo.DB.DbCommon;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.QueryCache.RedisCollections
{
    /// <summary>
    /// Hash方式的缓存操作
    /// </summary>
    public class RedisHash : ICacheHash
    {
        private IDatabase _client;

        private string _key;

        private CommandFlags _commanfFlags;

        private TimeSpan _expiration;
        public RedisHash(IDatabase client,string key, CommandFlags commanfFlags, TimeSpan expiration) 
        {
            _client = client;
            _key = key;
            _commanfFlags = commanfFlags;
            _expiration = expiration;
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
            TimeSpan ts = _expiration;
            HashEntry[] arrValue = new HashEntry[dicSet.Count];
            int index = 0;
            RedisValue rkey = RedisValue.Null;
            RedisValue rvalue = RedisValue.Null;
            foreach (KeyValuePair<string, object> kvp in dicSet)
            {
                rkey = RedisConverter.ValueToRedisValue(kvp.Key);
                rvalue = RedisConverter.ValueToRedisValue(kvp.Value);
                arrValue[index] = new HashEntry(rkey, rvalue);
                index++;
            }
            _client.HashSet(_key, arrValue, _commanfFlags);

        }
        /// <summary>
        /// HashSet设置值
        /// </summary>
        /// <param name="key">哈希表的键</param>
        /// <param name="value">哈希表的值</param>
        /// <param name="type">设置方式</param>
        public bool SetValue(string key, object value, SetValueType type)
        {

            RedisValue rkey = RedisConverter.ValueToRedisValue(key);
            RedisValue rvalue = RedisConverter.ValueToRedisValue(value);
            HashEntry item = new HashEntry(rkey, rvalue);

            return _client.HashSet(_key, rkey, rvalue, RedisAdaperByStackExchange.GetSetValueMode(type), _commanfFlags);
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
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);

            RedisValue rvalue = _client.HashGet(_key, rkey, _commanfFlags);
            return RedisConverter.RedisValueToValue<E>(rvalue, defaultValue);
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
            
            HashEntry[] values = _client.HashGetAll(_key, _commanfFlags);
            List<KeyValuePair<string, V>> ret = new List<KeyValuePair<string, V>>(values.Length);

            string vkey = null;
            V vValue = default(V);
            foreach (HashEntry entry in values)
            {

                vValue = RedisConverter.RedisValueToValue<V>(entry.Value, defaultValue);
                vkey = RedisConverter.RedisValueToValue<string>(entry.Name, null);
                KeyValuePair<string, V> val = new KeyValuePair<string, V>(vkey, vValue);
                ret.Add(val);
            }

            return ret;
        }

        /// <summary>
        /// 获取所有哈希表的键
        /// </summary>
        /// <returns></returns>
        public ICollection<string> GetAllKeys()
        {
            RedisValue[] keys= _client.HashKeys(_key, _commanfFlags);
            List<string> lst = new List<string>();
            foreach (RedisValue val in keys)
            {
                lst.Add(RedisConverter.RedisValueToValue<string>(val,null));
            }
            return lst;
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

            RedisValue rkey = RedisConverter.ValueToRedisValue(key);

            return _client.HashDelete(_key, rkey, _commanfFlags);
        }
        /// <summary>
        /// 批量删除哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="keys">要删除哈希表的键</param>
        /// <returns></returns>
        public long DeleteValues(IEnumerable<string> keys)
        {
            List<RedisValue> lstrKeys = new List<RedisValue>();
            RedisValue rkey = RedisValue.Null;
            foreach (object obj in keys)
            {
                rkey = RedisConverter.ValueToRedisValue(obj);
                lstrKeys.Add(rkey);
            }

            return _client.HashDelete(_key, lstrKeys.ToArray(), _commanfFlags);
        }

        public bool Exists(string key)
        {
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);
            return _client.HashExists(_key, rkey, _commanfFlags);
        }

        public long Increment(string key, long value)
        {
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);
            return (long)_client.HashIncrement(_key, rkey, (double)value, _commanfFlags);
        }

        public long Decrement(string key, long value)
        {
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);
            return (long)_client.HashDecrement(_key, rkey, (double)value, _commanfFlags);
        }

        public Task SetRangeValueAsync(IDictionary<string, object> dicSet)
        {
            TimeSpan ts = _expiration;
            HashEntry[] arrValue = new HashEntry[dicSet.Count];
            int index = 0;
            RedisValue rkey = RedisValue.Null;
            RedisValue rvalue = RedisValue.Null;
            foreach (KeyValuePair<string, object> kvp in dicSet)
            {
                rkey = RedisConverter.ValueToRedisValue(kvp.Key);
                rvalue = RedisConverter.ValueToRedisValue(kvp.Value);
                arrValue[index] = new HashEntry(rkey, rvalue);
                index++;
            }
            return _client.HashSetAsync(_key, arrValue, _commanfFlags);
        }

        public Task<bool> SetValueAsync(string key, object value, SetValueType type = SetValueType.Set)
        {
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);
            RedisValue rvalue = RedisConverter.ValueToRedisValue(value);
            HashEntry item = new HashEntry(rkey, rvalue);

            return _client.HashSetAsync(_key, rkey, rvalue, RedisAdaperByStackExchange.GetSetValueMode(type), _commanfFlags);
        }

        public async Task<E> GetValueAsync<E>(string key, E defaultValue = default)
        {
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);

            RedisValue rvalue = await _client.HashGetAsync(_key, rkey, _commanfFlags);
            return RedisConverter.RedisValueToValue<E>(rvalue, defaultValue);
        }

        public async Task<List<KeyValuePair<string, V>>> GetAllValuesAsync<V>(V defaultValue)
        {
            HashEntry[] values = await _client.HashGetAllAsync(_key, _commanfFlags);
            List<KeyValuePair<string, V>> ret = new List<KeyValuePair<string, V>>(values.Length);

            string vkey = null;
            V vValue = default(V);
            foreach (HashEntry entry in values)
            {

                vValue = RedisConverter.RedisValueToValue<V>(entry.Value, defaultValue);
                vkey = RedisConverter.RedisValueToValue<string>(entry.Name, null);
                KeyValuePair<string, V> val = new KeyValuePair<string, V>(vkey, vValue);
                ret.Add(val);
            }

            return ret;
        }

        public async Task<ICollection<string>> GetAllKeysAsync()
        {
            RedisValue[] keys = await _client.HashKeysAsync(_key, _commanfFlags);
            List<string> lst = new List<string>();
            foreach (RedisValue val in keys)
            {
                lst.Add(RedisConverter.RedisValueToValue<string>(val, null));
            }
            return lst;
        }

        public Task<bool> DeleteValueAsync(string key)
        {
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);

            return _client.HashDeleteAsync(_key, rkey, _commanfFlags);
        }

        public Task<long> DeleteValuesAsync(IEnumerable<string> keys)
        {
            List<RedisValue> lstrKeys = new List<RedisValue>();
            RedisValue rkey = RedisValue.Null;
            foreach (object obj in keys)
            {
                rkey = RedisConverter.ValueToRedisValue(obj);
                lstrKeys.Add(rkey);
            }

            return _client.HashDeleteAsync(_key, lstrKeys.ToArray(), _commanfFlags);
        }

        public Task<bool> ExistsAsync(string key)
        {
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);
            return _client.HashExistsAsync(_key, rkey, _commanfFlags);
        }

        public async Task<long> IncrementAsync(string key, long value = 1)
        {
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);
            return (long) (await _client.HashIncrementAsync(_key, rkey, (double)value, _commanfFlags));
        }

        public async Task<long> DecrementAsync(string key, long value = 1)
        {
            RedisValue rkey = RedisConverter.ValueToRedisValue(key);
            return (long) (await _client.HashDecrementAsync(_key, rkey, (double)value, _commanfFlags));
        }
    }
}
