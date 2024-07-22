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
    /// List方式的缓存操作
    /// </summary>
    public class RedisList : ICacheList
    {
        private IDatabase _client;

        private string _key;

        private CommandFlags _commanfFlags;

        private TimeSpan _expiration;

        /// <summary>
        ///  List方式的缓存操作
        /// </summary>
        /// <param name="lst"></param>
        public RedisList(IDatabase client, string key, CommandFlags commanfFlags, TimeSpan expiration)
        {
            _client = client;
            _key = key;
            _commanfFlags = commanfFlags;
            _expiration = expiration;
        }

        public long AddValue(object value, long index,  SetValueType setType)
        {
            TimeSpan ts = _expiration;

            RedisValue val = RedisConverter.ValueToRedisValue(value);
            When when = RedisAdaperByStackExchange.GetSetValueMode(setType);
            if (index == 0)
            {
                return _client.ListLeftPush(_key, val, when, _commanfFlags);
            }
            if (index == -1)
            {
                return _client.ListRightPush(_key, val, when, _commanfFlags);
            }
            _client.ListSetByIndex(_key, index, val, _commanfFlags);
            
            return 1;
        }

        public E GetValue<E>( long index, E defaultValue)
        {
            TimeSpan ts = _expiration;

            RedisValue value = _client.ListGetByIndex(_key, index, _commanfFlags);

            return RedisConverter.RedisValueToValue<E>(value, defaultValue);
        }

        public long GetLength()
        {
            return _client.ListLength(_key, _commanfFlags);
        }

        public long RemoveValue( object value, long count)
        {
            TimeSpan ts = _expiration;
            RedisValue rvalue = RedisConverter.ValueToRedisValue(value);
            return _client.ListRemove(_key, rvalue, count, _commanfFlags);
        }

        public List<E> AllValues<E>( long start, long end)
        {
            RedisValue[] values = _client.ListRange(_key, start, end, _commanfFlags);
            List<E> result = new List<E>();
            foreach (RedisValue item in values)
            {
                result.Add(RedisConverter.RedisValueToValue<E>(item, default(E)));
            }

            return result;
        }

        public long AddRangValue(IEnumerable values, long index,  SetValueType setType)
        {
            TimeSpan ts = _expiration;
            List<RedisValue> lstValues = new List<RedisValue>();
            foreach (object obj in values)
            {
                RedisValue val = RedisConverter.ValueToRedisValue(obj);
                lstValues.Add(val);
            }
            When when = RedisAdaperByStackExchange.GetSetValueMode(setType);
            if (index == 0)
            {
                return _client.ListLeftPush(_key, lstValues.ToArray(), _commanfFlags);
            }
            if (index == -1)
            {
                return _client.ListRightPush(_key, lstValues.ToArray(), _commanfFlags);
            }
            foreach (RedisValue val in lstValues)
            {
                _client.ListSetByIndex(_key, index, val, _commanfFlags);
            }
            return 1;
        }

        public long InsertAfter( object pivot, object value)
        {
            TimeSpan ts = _expiration;

            RedisValue val = RedisConverter.ValueToRedisValue(value);
            RedisValue pivotval = RedisConverter.ValueToRedisValue(pivot);

            
            _client.ListInsertAfter(_key, pivotval, val, _commanfFlags);
            return 1;
        }

        public long InsertBefore( object pivot, object value)
        {
            TimeSpan ts = _expiration;

            RedisValue val = RedisConverter.ValueToRedisValue(value);
            RedisValue pivotval = RedisConverter.ValueToRedisValue(pivot);


            _client.ListInsertBefore(_key, pivotval, val, _commanfFlags);
            return 1;
        
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

            TimeSpan ts = _expiration;
            RedisValue value = RedisValue.Null;
            if (isPopEnd)
            {
                value = _client.ListRightPop(_key, _commanfFlags);
            }
            else
            {
                value = _client.ListLeftPop(_key, _commanfFlags);
            }
            return RedisConverter.RedisValueToValue<E>(value, defaultValue);
        }

        public async Task<long> AddValueAsync(object value, long index = -1, SetValueType setType = SetValueType.Set)
        {
            TimeSpan ts = _expiration;

            RedisValue val = RedisConverter.ValueToRedisValue(value);
            When when = RedisAdaperByStackExchange.GetSetValueMode(setType);
            if (index == 0)
            {
                return await _client.ListLeftPushAsync(_key, val, when, _commanfFlags);
            }
            if (index == -1)
            {
                return await _client.ListRightPushAsync(_key, val, when, _commanfFlags);
            }
            await _client.ListSetByIndexAsync(_key, index, val, _commanfFlags);

            return 1L;
        }

        public async Task<long> AddRangValueAsync(IEnumerable values, long index = -1, SetValueType setType = SetValueType.Set)
        {
            TimeSpan ts = _expiration;
            List<RedisValue> lstValues = new List<RedisValue>();
            foreach (object obj in values)
            {
                RedisValue val = RedisConverter.ValueToRedisValue(obj);
                lstValues.Add(val);
            }
            When when = RedisAdaperByStackExchange.GetSetValueMode(setType);
            if (index == 0)
            {
                return await _client.ListLeftPushAsync(_key, lstValues.ToArray(), _commanfFlags);
            }
            if (index == -1)
            {
                return await _client.ListRightPushAsync(_key, lstValues.ToArray(), _commanfFlags);
            }
            foreach (RedisValue val in lstValues)
            {
                await _client.ListSetByIndexAsync(_key, index, val, _commanfFlags);
            }
            return 1;
        }

        public async Task<E> GetValueAsync<E>(long index, E defaultValue = default)
        {
            TimeSpan ts = _expiration;

            RedisValue value = await _client.ListGetByIndexAsync(_key, index, _commanfFlags);

            return RedisConverter.RedisValueToValue<E>(value, defaultValue);
        }

        public  Task<long> GetLengthAsync()
        {
            return  _client.ListLengthAsync(_key, _commanfFlags);
        }

        public Task<long> RemoveValueAsync(object value, long count = 0)
        {
            TimeSpan ts = _expiration;
            RedisValue rvalue = RedisConverter.ValueToRedisValue(value);
            return _client.ListRemoveAsync(_key, rvalue, count, _commanfFlags);
        }

        public async Task<List<E>> AllValuesAsync<E>(long start = 0, long end = -1)
        {
            RedisValue[] values = await _client.ListRangeAsync(_key, start, end, _commanfFlags);
            List<E> result = new List<E>();
            foreach (RedisValue item in values)
            {
                result.Add(RedisConverter.RedisValueToValue<E>(item, default(E)));
            }

            return result;
        }

        public async Task<long> InsertAfterAsync(object pivot, object value)
        {
            TimeSpan ts = _expiration;

            RedisValue val = RedisConverter.ValueToRedisValue(value);
            RedisValue pivotval = RedisConverter.ValueToRedisValue(pivot);


            await _client.ListInsertAfterAsync(_key, pivotval, val, _commanfFlags);
            return 1;
        }

        public async Task<long> InsertBeforeAsync(object pivot, object value)
        {
            TimeSpan ts = _expiration;

            RedisValue val = RedisConverter.ValueToRedisValue(value);
            RedisValue pivotval = RedisConverter.ValueToRedisValue(pivot);


            await _client.ListInsertBeforeAsync(_key, pivotval, val, _commanfFlags);
            return 1;
        }

        public async Task<E> PopValueAsync<E>(bool isPopEnd = false, E defaultValue = default)
        {
            TimeSpan ts = _expiration;
            RedisValue value = RedisValue.Null;
            if (isPopEnd)
            {
                value = await _client.ListRightPopAsync(_key, _commanfFlags);
            }
            else
            {
                value = await _client.ListLeftPopAsync(_key, _commanfFlags);
            }
            return RedisConverter.RedisValueToValue<E>(value, defaultValue);
        }
    }
}
