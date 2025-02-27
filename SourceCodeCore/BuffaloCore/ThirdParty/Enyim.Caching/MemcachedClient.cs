﻿using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Enyim.Caching.Memcached.Results;
using Enyim.Caching.Memcached.Results.Extensions;
using Enyim.Caching.Memcached.Results.Factories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Enyim.Caching
{
    /// <summary>
    /// Memcached client.
    /// </summary>
    public partial class MemcachedClient : IMemcachedClient, IMemcachedResultsClient, IDistributedCache
    {
        /// <summary>
        /// Represents a value which indicates that an item should never expire.
        /// </summary>
        public static readonly TimeSpan Infinite = TimeSpan.Zero;
        //internal static readonly MemcachedClientSection DefaultSettings = ConfigurationManager.GetSection("enyim.com/memcached") as MemcachedClientSection;
        private ILogger<MemcachedClient> _logger;

        private IServerPool pool;
        private IMemcachedKeyTransformer keyTransformer;
        private ITranscoder transcoder;

        public IStoreOperationResultFactory StoreOperationResultFactory { get; set; }
        public IGetOperationResultFactory GetOperationResultFactory { get; set; }
        public IMutateOperationResultFactory MutateOperationResultFactory { get; set; }
        public IConcatOperationResultFactory ConcatOperationResultFactory { get; set; }
        public IRemoveOperationResultFactory RemoveOperationResultFactory { get; set; }

        protected IServerPool Pool { get { return this.pool; } }
        protected IMemcachedKeyTransformer KeyTransformer { get { return this.keyTransformer; } }
        protected ITranscoder Transcoder { get { return this.transcoder; } }

        public MemcachedClient(ILoggerFactory loggerFactory, IMemcachedClientConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<MemcachedClient>();

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.keyTransformer = configuration.CreateKeyTransformer() ?? new DefaultKeyTransformer();
            this.transcoder = configuration.CreateTranscoder() ?? new DefaultTranscoder();

            this.pool = configuration.CreatePool();
            this.StartPool();

            StoreOperationResultFactory = new DefaultStoreOperationResultFactory();
            GetOperationResultFactory = new DefaultGetOperationResultFactory();
            MutateOperationResultFactory = new DefaultMutateOperationResultFactory();
            ConcatOperationResultFactory = new DefaultConcatOperationResultFactory();
            RemoveOperationResultFactory = new DefaultRemoveOperationResultFactory();
        }

        [Obsolete]
        private MemcachedClient(IServerPool pool, IMemcachedKeyTransformer keyTransformer, ITranscoder transcoder)
        {
            if (pool == null) throw new ArgumentNullException("pool");
            if (keyTransformer == null) throw new ArgumentNullException("keyTransformer");
            if (transcoder == null) throw new ArgumentNullException("transcoder");

            this.keyTransformer = keyTransformer;
            this.transcoder = transcoder;

            this.pool = pool;
            this.StartPool();
        }

        private void StartPool()
        {
            this.pool.NodeFailed += (n) => { var f = this.NodeFailed; if (f != null) f(n); };
            this.pool.Start();
        }

        public event Action<IMemcachedNode> NodeFailed;

        public bool Add(string key, object value, int cacheSeconds)
        {
            return Store(StoreMode.Add, key, value, TimeSpan.FromSeconds(cacheSeconds));
        }

        public async Task<bool> AddAsync(string key, object value, int cacheSeconds)
        {
            return await StoreAsync(StoreMode.Add, key, value, TimeSpan.FromSeconds(cacheSeconds));
        }

        public bool Set(string key, object value, int cacheSeconds)
        {
            return Store(StoreMode.Set, key, value, TimeSpan.FromSeconds(cacheSeconds));
        }

        public async Task<bool> SetAsync(string key, object value, int cacheSeconds)
        {
            return await StoreAsync(StoreMode.Set, key, value, TimeSpan.FromSeconds(cacheSeconds));
        }

        public bool Replace(string key, object value, int cacheSeconds)
        {
            return Store(StoreMode.Replace, key, value, TimeSpan.FromSeconds(cacheSeconds));
        }

        public async Task<bool> ReplaceAsync(string key, object value, int cacheSeconds)
        {
            return await StoreAsync(StoreMode.Replace, key, value, TimeSpan.FromSeconds(cacheSeconds));
        }

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the item to retrieve.</param>
        /// <returns>The retrieved item, or <value>null</value> if the key was not found.</returns>
        public object Get(string key)
        {
            object tmp;

            return this.TryGet(key, out tmp) ? tmp : null;
        }

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the item to retrieve.</param>
        /// <returns>The retrieved item, or <value>default(T)</value> if the key was not found.</returns>
        public T Get<T>(string key)
        {
            var result = PerformGet<T>(key);
            return result.Success ? result.Value : default(T);
        }

        public IGetOperationResult<T> PerformGet<T>(string key)
        {
            if (!CreateGetCommand<T>(key, out var result, out var node, out var command))
            {
                return result;
            }

            try
            {
                var commandResult = node.Execute(command);
                return BuildGetCommandResult<T>(result, command, commandResult);

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"{nameof(PerformGet)}(\"{key}\")");
                result.Fail(ex.Message);
                return result;
            }
        }

        private bool CreateGetCommand<T>(string key, out IGetOperationResult<T> result, out IMemcachedNode node, out IGetOperation command)
        {
            result = new DefaultGetOperationResultFactory<T>().Create();
            var hashedKey = this.keyTransformer.Transform(key);

            node = this.pool.Locate(hashedKey);
            if (node == null)
            {
                var errorMessage = $"Unable to locate node with \"{key}\" key";
                _logger.LogError(errorMessage);
                result.Fail(errorMessage);
                command = null;
                return false;
            }

            command = this.pool.OperationFactory.Get(hashedKey);
            return true;
        }

        private IGetOperationResult<T> BuildGetCommandResult<T>(IGetOperationResult<T> result, IGetOperation command, IOperationResult commandResult)
        {
            if (commandResult.Success)
            {
                result.Value = transcoder.Deserialize<T>(command.Result);
                result.Pass();
            }
            else
            {
                commandResult.Combine(result);
            }

            return result;
        }

        public async Task<IGetOperationResult<T>> GetAsync<T>(string key)
        {
            if (!CreateGetCommand<T>(key, out var result, out var node, out var command))
            {
                _logger.LogInformation($"Failed to CreateGetCommand for '{key}' key");
                return result;
            }

            try
            {
                var commandResult = await node.ExecuteAsync(command);
                return BuildGetCommandResult<T>(result, command, commandResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"{nameof(GetAsync)}(\"{key}\")");
                result.Fail(ex.Message);
                return result;
            }
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            var result = await GetAsync<T>(key);
            return result.Success ? result.Value : default(T);
        }

        public async Task<T> GetValueOrCreateAsync<T>(string key, int cacheSeconds, Func<Task<T>> generator)
        {
            var result = await GetAsync<T>(key);
            if (result.Success)
            {
                _logger.LogDebug($"Cache is hint. Key is '{key}'.");
                return result.Value;
            }

            _logger.LogDebug($"Cache is missed. Key is '{key}'.");

            var value = await generator?.Invoke();
            if (value != null)
            {
                try
                {
                    await AddAsync(key, value, cacheSeconds);
                    _logger.LogDebug($"Added value into cache. Key is '{key}'. " + value);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{nameof(AddAsync)}(\"{key}\", ..., {cacheSeconds})");
                }
            }
            return value;
        }

        /// <summary>
        /// Tries to get an item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the item to retrieve.</param>
        /// <param name="value">The retrieved item or null if not found.</param>
        /// <returns>The <value>true</value> if the item was successfully retrieved.</returns>
        public bool TryGet(string key, out object value)
        {
            ulong cas = 0;

            return this.PerformTryGet(key, out cas, out value).Success;
        }

        /// <summary>
        /// Tries to get an item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the item to retrieve.</param>
        /// <param name="value">The retrieved item or null if not found.</param>
        /// <returns>The <value>true</value> if the item was successfully retrieved.</returns>
        public bool TryGet<T>(string key, out T value)
        {
            ulong cas = 0;

            return this.PerformTryGet(key, out cas, out value).Success;
        }

        public CasResult<object> GetWithCas(string key)
        {
            return this.GetWithCas<object>(key);
        }

        public CasResult<T> GetWithCas<T>(string key)
        {
            CasResult<T> tmp;

            return this.TryGetWithCas(key, out tmp)
                    ? new CasResult<T> { Cas = tmp.Cas, Result = tmp.Result }
                    : new CasResult<T> { Cas = tmp.Cas, Result = default };
        }

        public bool TryGetWithCas(string key, out CasResult<object> value)
        {
            object tmp;
            ulong cas;

            var retval = this.PerformTryGet(key, out cas, out tmp);

            value = new CasResult<object> { Cas = cas, Result = tmp };

            return retval.Success;
        }

        public bool TryGetWithCas<T>(string key, out CasResult<T> value)
        {
            var retVal = PerformTryGet(key, out var cas, out T tmp);

            value = new CasResult<T> { Cas = cas, Result = tmp };

            return retVal.Success;
        }

        protected virtual IGetOperationResult PerformTryGet(string key, out ulong cas, out object value)
        {
            var hashedKey = this.keyTransformer.Transform(key);
            var node = this.pool.Locate(hashedKey);
            var result = GetOperationResultFactory.Create();

            cas = 0;
            value = null;

            if (node != null)
            {
                var command = this.pool.OperationFactory.Get(hashedKey);
                var commandResult = node.Execute(command);

                if (commandResult.Success)
                {
                    result.Value = value = this.transcoder.Deserialize(command.Result);
                    result.Cas = cas = command.CasValue;

                    result.Pass();
                    return result;
                }
                else
                {
                    commandResult.Combine(result);
                    return result;
                }
            }

            result.Value = value;
            result.Cas = cas;

            result.Fail("Unable to locate node");
            return result;
        }

        protected virtual IGetOperationResult PerformTryGet<T>(string key, out ulong cas, out T value)
        {
            var hashedKey = keyTransformer.Transform(key);
            var node = pool.Locate(hashedKey);
            var result = GetOperationResultFactory.Create();

            cas = 0;
            value = default;

            if (node != null)
            {
                var command = pool.OperationFactory.Get(hashedKey);
                var commandResult = node.Execute(command);

                if (commandResult.Success)
                {
                    result.Value = value = transcoder.Deserialize<T>(command.Result);
                    result.Cas = cas = command.CasValue;

                    result.Pass();
                    return result;
                }

                commandResult.Combine(result);
                return result;
            }

            result.Value = value;
            result.Cas = cas;

            result.Fail("Unable to locate node");
            return result;
        }


        #region [ Store                        ]

        /// <summary>
        /// Inserts an item into the cache with a cache key to reference its location.
        /// </summary>
        /// <param name="mode">Defines how the item is stored in the cache.</param>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <remarks>The item does not expire unless it is removed due memory pressure.</remarks>
        /// <returns>true if the item was successfully stored in the cache; false otherwise.</returns>
        public bool Store(StoreMode mode, string key, object value)
        {
            ulong tmp = 0;
            int status;

            return this.PerformStore(mode, key, value, 0, ref tmp, out status).Success;
        }

        /// <summary>
        /// Inserts an item into the cache with a cache key to reference its location.
        /// </summary>
        /// <param name="mode">Defines how the item is stored in the cache.</param>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <param name="validFor">The interval after the item is invalidated in the cache.</param>
        /// <returns>true if the item was successfully stored in the cache; false otherwise.</returns>
        public bool Store(StoreMode mode, string key, object value, TimeSpan validFor)
        {
            ulong tmp = 0;
            int status;

            return this.PerformStore(mode, key, value, MemcachedClient.GetExpiration(validFor, null), ref tmp, out status).Success;
        }

        public async Task<bool> StoreAsync(StoreMode mode, string key, object value, DateTime expiresAt)
        {
            return (await this.PerformStoreAsync(mode, key, value, MemcachedClient.GetExpiration(null, expiresAt))).Success;
        }

        public async Task<bool> StoreAsync(StoreMode mode, string key, object value, TimeSpan validFor)
        {
            return (await this.PerformStoreAsync(mode, key, value, MemcachedClient.GetExpiration(validFor, null))).Success;
        }

        /// <summary>
        /// Inserts an item into the cache with a cache key to reference its location.
        /// </summary>
        /// <param name="mode">Defines how the item is stored in the cache.</param>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <param name="expiresAt">The time when the item is invalidated in the cache.</param>
        /// <returns>true if the item was successfully stored in the cache; false otherwise.</returns>
        public bool Store(StoreMode mode, string key, object value, DateTime expiresAt)
        {
            ulong tmp = 0;
            int status;

            return this.PerformStore(mode, key, value, MemcachedClient.GetExpiration(null, expiresAt), ref tmp, out status).Success;
        }

        /// <summary>
        /// Inserts an item into the cache with a cache key to reference its location and returns its version.
        /// </summary>
        /// <param name="mode">Defines how the item is stored in the cache.</param>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <remarks>The item does not expire unless it is removed due memory pressure.</remarks>
        /// <returns>A CasResult object containing the version of the item and the result of the operation (true if the item was successfully stored in the cache; false otherwise).</returns>
        public CasResult<bool> Cas(StoreMode mode, string key, object value, ulong cas)
        {
            var result = this.PerformStore(mode, key, value, 0, cas);
            return new CasResult<bool> { Cas = result.Cas, Result = result.Success, StatusCode = result.StatusCode.Value };

        }

        /// <summary>
        /// Inserts an item into the cache with a cache key to reference its location and returns its version.
        /// </summary>
        /// <param name="mode">Defines how the item is stored in the cache.</param>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <param name="validFor">The interval after the item is invalidated in the cache.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <returns>A CasResult object containing the version of the item and the result of the operation (true if the item was successfully stored in the cache; false otherwise).</returns>
        public CasResult<bool> Cas(StoreMode mode, string key, object value, TimeSpan validFor, ulong cas)
        {
            var result = this.PerformStore(mode, key, value, MemcachedClient.GetExpiration(validFor, null), cas);
            return new CasResult<bool> { Cas = result.Cas, Result = result.Success, StatusCode = result.StatusCode.Value };
        }

        /// <summary>
        /// Inserts an item into the cache with a cache key to reference its location and returns its version.
        /// </summary>
        /// <param name="mode">Defines how the item is stored in the cache.</param>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <param name="expiresAt">The time when the item is invalidated in the cache.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <returns>A CasResult object containing the version of the item and the result of the operation (true if the item was successfully stored in the cache; false otherwise).</returns>
        public CasResult<bool> Cas(StoreMode mode, string key, object value, DateTime expiresAt, ulong cas)
        {
            var result = this.PerformStore(mode, key, value, MemcachedClient.GetExpiration(null, expiresAt), cas);
            return new CasResult<bool> { Cas = result.Cas, Result = result.Success, StatusCode = result.StatusCode.Value };
        }

        /// <summary>
        /// Inserts an item into the cache with a cache key to reference its location and returns its version.
        /// </summary>
        /// <param name="mode">Defines how the item is stored in the cache.</param>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <remarks>The item does not expire unless it is removed due memory pressure. The text protocol does not support this operation, you need to Store then GetWithCas.</remarks>
        /// <returns>A CasResult object containing the version of the item and the result of the operation (true if the item was successfully stored in the cache; false otherwise).</returns>
        public CasResult<bool> Cas(StoreMode mode, string key, object value)
        {
            var result = this.PerformStore(mode, key, value, 0, 0);
            return new CasResult<bool> { Cas = result.Cas, Result = result.Success, StatusCode = result.StatusCode.Value };
        }

        private IStoreOperationResult PerformStore(StoreMode mode, string key, object value, uint expires, ulong cas)
        {
            ulong tmp = cas;
            int status;

            var retval = this.PerformStore(mode, key, value, expires, ref tmp, out status);
            retval.StatusCode = status;

            if (retval.Success)
            {
                retval.Cas = tmp;
            }
            return retval;
        }

        protected virtual IStoreOperationResult PerformStore(StoreMode mode, string key, object value, uint expires, ref ulong cas, out int statusCode)
        {
            var hashedKey = this.keyTransformer.Transform(key);
            var node = this.pool.Locate(hashedKey);
            var result = StoreOperationResultFactory.Create();

            statusCode = -1;


            if (value == null)
            {
                result.Fail("value is null");
                return result;
            }

            if (node != null)
            {
                CacheItem item;

                try { item = this.transcoder.Serialize(value); }
                catch (Exception e)
                {
                    _logger.LogError("PerformStore", e);

                    result.Fail("PerformStore failed", e);
                    return result;
                }

                var command = this.pool.OperationFactory.Store(mode, hashedKey, item, expires, cas);
                var commandResult = node.Execute(command);

                result.Cas = cas = command.CasValue;
                result.StatusCode = statusCode = command.StatusCode;

                if (commandResult.Success)
                {
                    result.Pass();
                    return result;
                }

                commandResult.Combine(result);
                return result;
            }

            //if (this.performanceMonitor != null) this.performanceMonitor.Store(mode, 1, false);

            result.Fail("Unable to locate node");
            return result;
        }

        protected async virtual Task<IStoreOperationResult> PerformStoreAsync(StoreMode mode, string key, object value, uint expires)
        {
            var hashedKey = this.keyTransformer.Transform(key);
            var node = this.pool.Locate(hashedKey);
            var result = StoreOperationResultFactory.Create();

            int statusCode = -1;
            ulong cas = 0;
            if (value == null)
            {
                result.Fail("value is null");
                return result;
            }

            if (node != null)
            {
                CacheItem item;

                try { item = this.transcoder.Serialize(value); }
                catch (Exception e)
                {
                    _logger.LogError(new EventId(), e, $"{nameof(PerformStoreAsync)} for '{key}' key");

                    result.Fail("PerformStore failed", e);
                    return result;
                }

                var command = this.pool.OperationFactory.Store(mode, hashedKey, item, expires, cas);
                var commandResult = await node.ExecuteAsync(command);

                result.Cas = cas = command.CasValue;
                result.StatusCode = statusCode = command.StatusCode;

                if (commandResult.Success)
                {
                    result.Pass();
                    return result;
                }

                commandResult.Combine(result);
                return result;
            }

            //if (this.performanceMonitor != null) this.performanceMonitor.Store(mode, 1, false);

            result.Fail("Unable to locate memcached node");
            return result;
        }

        #endregion
        #region [ Mutate                       ]

        #region [ Increment                    ]

        /// <summary>
        /// Increments the value of the specified key by the given amount. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to increase the item.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public ulong Increment(string key, ulong defaultValue, ulong delta)
        {
            return this.PerformMutate(MutationMode.Increment, key, defaultValue, delta, 0).Value;
        }

        /// <summary>
        /// Increments the value of the specified key by the given amount. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to increase the item.</param>
        /// <param name="validFor">The interval after the item is invalidated in the cache.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public ulong Increment(string key, ulong defaultValue, ulong delta, TimeSpan validFor)
        {
            return this.PerformMutate(MutationMode.Increment, key, defaultValue, delta, MemcachedClient.GetExpiration(validFor, null)).Value;
        }

        /// <summary>
        /// Increments the value of the specified key by the given amount. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to increase the item.</param>
        /// <param name="expiresAt">The time when the item is invalidated in the cache.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public ulong Increment(string key, ulong defaultValue, ulong delta, DateTime expiresAt)
        {
            return this.PerformMutate(MutationMode.Increment, key, defaultValue, delta, MemcachedClient.GetExpiration(null, expiresAt)).Value;
        }

        /// <summary>
        /// Increments the value of the specified key by the given amount, but only if the item's version matches the CAS value provided. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to increase the item.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public CasResult<ulong> Increment(string key, ulong defaultValue, ulong delta, ulong cas)
        {
            var result = this.CasMutate(MutationMode.Increment, key, defaultValue, delta, 0, cas);
            return new CasResult<ulong> { Cas = result.Cas, Result = result.Value, StatusCode = result.StatusCode.Value };
        }

        /// <summary>
        /// Increments the value of the specified key by the given amount, but only if the item's version matches the CAS value provided. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to increase the item.</param>
        /// <param name="validFor">The interval after the item is invalidated in the cache.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public CasResult<ulong> Increment(string key, ulong defaultValue, ulong delta, TimeSpan validFor, ulong cas)
        {
            var result = this.CasMutate(MutationMode.Increment, key, defaultValue, delta, MemcachedClient.GetExpiration(validFor, null), cas);
            return new CasResult<ulong> { Cas = result.Cas, Result = result.Value, StatusCode = result.StatusCode.Value };
        }

        /// <summary>
        /// Increments the value of the specified key by the given amount, but only if the item's version matches the CAS value provided. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to increase the item.</param>
        /// <param name="expiresAt">The time when the item is invalidated in the cache.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public CasResult<ulong> Increment(string key, ulong defaultValue, ulong delta, DateTime expiresAt, ulong cas)
        {
            var result = this.CasMutate(MutationMode.Increment, key, defaultValue, delta, MemcachedClient.GetExpiration(null, expiresAt), cas);
            return new CasResult<ulong> { Cas = result.Cas, Result = result.Value, StatusCode = result.StatusCode.Value };
        }

        #endregion
        #region [ Decrement                    ]
        /// <summary>
        /// Decrements the value of the specified key by the given amount. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to decrease the item.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public ulong Decrement(string key, ulong defaultValue, ulong delta)
        {
            return this.PerformMutate(MutationMode.Decrement, key, defaultValue, delta, 0).Value;
        }

        /// <summary>
        /// Decrements the value of the specified key by the given amount. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to decrease the item.</param>
        /// <param name="validFor">The interval after the item is invalidated in the cache.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public ulong Decrement(string key, ulong defaultValue, ulong delta, TimeSpan validFor)
        {
            return this.PerformMutate(MutationMode.Decrement, key, defaultValue, delta, MemcachedClient.GetExpiration(validFor, null)).Value;
        }

        /// <summary>
        /// Decrements the value of the specified key by the given amount. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to decrease the item.</param>
        /// <param name="expiresAt">The time when the item is invalidated in the cache.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public ulong Decrement(string key, ulong defaultValue, ulong delta, DateTime expiresAt)
        {
            return this.PerformMutate(MutationMode.Decrement, key, defaultValue, delta, MemcachedClient.GetExpiration(null, expiresAt)).Value;
        }

        /// <summary>
        /// Decrements the value of the specified key by the given amount, but only if the item's version matches the CAS value provided. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to decrease the item.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public CasResult<ulong> Decrement(string key, ulong defaultValue, ulong delta, ulong cas)
        {
            var result = this.CasMutate(MutationMode.Decrement, key, defaultValue, delta, 0, cas);
            return new CasResult<ulong> { Cas = result.Cas, Result = result.Value, StatusCode = result.StatusCode.Value };
        }

        /// <summary>
        /// Decrements the value of the specified key by the given amount, but only if the item's version matches the CAS value provided. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to decrease the item.</param>
        /// <param name="validFor">The interval after the item is invalidated in the cache.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public CasResult<ulong> Decrement(string key, ulong defaultValue, ulong delta, TimeSpan validFor, ulong cas)
        {
            var result = this.CasMutate(MutationMode.Decrement, key, defaultValue, delta, MemcachedClient.GetExpiration(validFor, null), cas);
            return new CasResult<ulong> { Cas = result.Cas, Result = result.Value, StatusCode = result.StatusCode.Value };
        }

        /// <summary>
        /// Decrements the value of the specified key by the given amount, but only if the item's version matches the CAS value provided. The operation is atomic and happens on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="defaultValue">The value which will be stored by the server if the specified item was not found.</param>
        /// <param name="delta">The amount by which the client wants to decrease the item.</param>
        /// <param name="expiresAt">The time when the item is invalidated in the cache.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <returns>The new value of the item or defaultValue if the key was not found.</returns>
        /// <remarks>If the client uses the Text protocol, the item must be inserted into the cache before it can be changed. It must be inserted as a <see cref="T:System.String"/>. Moreover the Text protocol only works with <see cref="System.UInt32"/> values, so return value -1 always indicates that the item was not found.</remarks>
        public CasResult<ulong> Decrement(string key, ulong defaultValue, ulong delta, DateTime expiresAt, ulong cas)
        {
            var result = this.CasMutate(MutationMode.Decrement, key, defaultValue, delta, MemcachedClient.GetExpiration(null, expiresAt), cas);
            return new CasResult<ulong> { Cas = result.Cas, Result = result.Value, StatusCode = result.StatusCode.Value };
        }

        #endregion

        #region Touch
        public async Task<IOperationResult> TouchAsync(string key, DateTime expiresAt)
        {
            return await PerformMutateAsync(MutationMode.Touch, key, 0, 0, GetExpiration(null, expiresAt));
        }

        public async Task<IOperationResult> TouchAsync(string key, TimeSpan validFor)
        {
            return await PerformMutateAsync(MutationMode.Touch, key, 0, 0, GetExpiration(validFor, null));
        }
        #endregion

        private IMutateOperationResult PerformMutate(MutationMode mode, string key, ulong defaultValue, ulong delta, uint expires)
        {
            ulong tmp = 0;

            return PerformMutate(mode, key, defaultValue, delta, expires, ref tmp);
        }

        private IMutateOperationResult CasMutate(MutationMode mode, string key, ulong defaultValue, ulong delta, uint expires, ulong cas)
        {
            var tmp = cas;
            var retval = PerformMutate(mode, key, defaultValue, delta, expires, ref tmp);
            retval.Cas = tmp;
            return retval;
        }

        protected virtual IMutateOperationResult PerformMutate(MutationMode mode, string key, ulong defaultValue, ulong delta, uint expires, ref ulong cas)
        {
            var hashedKey = this.keyTransformer.Transform(key);
            var node = this.pool.Locate(hashedKey);
            var result = MutateOperationResultFactory.Create();

            if (node != null)
            {
                var command = this.pool.OperationFactory.Mutate(mode, hashedKey, defaultValue, delta, expires, cas);
                var commandResult = node.Execute(command);

                result.Cas = cas = command.CasValue;
                result.StatusCode = command.StatusCode;

                if (commandResult.Success)
                {
                    result.Value = command.Result;
                    result.Pass();
                    return result;
                }
                else
                {
                    result.InnerResult = commandResult;
                    result.Fail("Mutate operation failed, see InnerResult or StatusCode for more details");
                }

            }

            // TODO not sure about the return value when the command fails
            result.Fail("Unable to locate node");
            return result;
        }

        protected virtual async Task<IMutateOperationResult> PerformMutateAsync(MutationMode mode, string key, ulong defaultValue, ulong delta, uint expires)
        {
            ulong cas = 0;
            var hashedKey = this.keyTransformer.Transform(key);
            var node = this.pool.Locate(hashedKey);
            var result = MutateOperationResultFactory.Create();

            if (node != null)
            {
                var command = this.pool.OperationFactory.Mutate(mode, hashedKey, defaultValue, delta, expires, cas);
                var commandResult = await node.ExecuteAsync(command);

                result.Cas = cas = command.CasValue;
                result.StatusCode = command.StatusCode;

                if (commandResult.Success)
                {
                    result.Value = command.Result;
                    result.Pass();
                    return result;
                }
                else
                {
                    result.InnerResult = commandResult;
                    result.Fail("Mutate operation failed, see InnerResult or StatusCode for more details");
                }

            }

            // TODO not sure about the return value when the command fails
            result.Fail("Unable to locate node");
            return result;
        }


        #endregion
        #region [ Concatenate                  ]

        /// <summary>
        /// Appends the data to the end of the specified item's data on the server.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="data">The data to be appended to the item.</param>
        /// <returns>true if the data was successfully stored; false otherwise.</returns>
        public bool Append(string key, ArraySegment<byte> data)
        {
            ulong cas = 0;

            return this.PerformConcatenate(ConcatenationMode.Append, key, ref cas, data).Success;
        }

        /// <summary>
        /// Inserts the data before the specified item's data on the server.
        /// </summary>
        /// <returns>true if the data was successfully stored; false otherwise.</returns>
        public bool Prepend(string key, ArraySegment<byte> data)
        {
            ulong cas = 0;

            return this.PerformConcatenate(ConcatenationMode.Prepend, key, ref cas, data).Success;
        }

        /// <summary>
        /// Appends the data to the end of the specified item's data on the server, but only if the item's version matches the CAS value provided.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <param name="data">The data to be prepended to the item.</param>
        /// <returns>true if the data was successfully stored; false otherwise.</returns>
        public CasResult<bool> Append(string key, ulong cas, ArraySegment<byte> data)
        {
            ulong tmp = cas;
            var success = PerformConcatenate(ConcatenationMode.Append, key, ref tmp, data);

            return new CasResult<bool> { Cas = tmp, Result = success.Success };
        }

        /// <summary>
        /// Inserts the data before the specified item's data on the server, but only if the item's version matches the CAS value provided.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="cas">The cas value which must match the item's version.</param>
        /// <param name="data">The data to be prepended to the item.</param>
        /// <returns>true if the data was successfully stored; false otherwise.</returns>
        public CasResult<bool> Prepend(string key, ulong cas, ArraySegment<byte> data)
        {
            ulong tmp = cas;
            var success = PerformConcatenate(ConcatenationMode.Prepend, key, ref tmp, data);

            return new CasResult<bool> { Cas = tmp, Result = success.Success };
        }

        protected virtual IConcatOperationResult PerformConcatenate(ConcatenationMode mode, string key, ref ulong cas, ArraySegment<byte> data)
        {
            var hashedKey = this.keyTransformer.Transform(key);
            var node = this.pool.Locate(hashedKey);
            var result = ConcatOperationResultFactory.Create();

            if (node != null)
            {
                var command = this.pool.OperationFactory.Concat(mode, hashedKey, cas, data);
                var commandResult = node.Execute(command);

                if (commandResult.Success)
                {
                    result.Cas = cas = command.CasValue;
                    result.StatusCode = command.StatusCode;
                    result.Pass();
                }
                else
                {
                    result.InnerResult = commandResult;
                    result.Fail("Concat operation failed, see InnerResult or StatusCode for details");
                }

                return result;
            }

            result.Fail("Unable to locate node");
            return result;
        }

        #endregion

        /// <summary>
        /// Removes all data from the cache. Note: this will invalidate all data on all servers in the pool.
        /// </summary>
        public void FlushAll()
        {
            foreach (var node in this.pool.GetWorkingNodes())
            {
                var command = this.pool.OperationFactory.Flush();

                node.Execute(command);
            }
        }

        public async Task FlushAllAsync()
        {
            var tasks = new List<Task>();

            foreach (var node in this.pool.GetWorkingNodes())
            {
                var command = this.pool.OperationFactory.Flush();

                tasks.Add(node.ExecuteAsync(command));
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Returns statistics about the servers.
        /// </summary>
        /// <returns></returns>
        public ServerStats Stats()
        {
            return this.Stats(null);
        }

        public ServerStats Stats(string type)
        {
            var results = new Dictionary<EndPoint, Dictionary<string, string>>();
            var tasks = new List<Task>();

            foreach (var node in this.pool.GetWorkingNodes())
            {
                var cmd = this.pool.OperationFactory.Stats(type);
                var action = new Func<IOperation, IOperationResult>(node.Execute);
                var endpoint = node.EndPoint;

                tasks.Add(Task.Run(() =>
                {
                    action(cmd);
                    lock (results)
                        results[endpoint] = cmd.Result;
                }));
            }

            if (tasks.Count > 0)
            {
                Task.WaitAll(tasks.ToArray());
            }

            return new ServerStats(results);
        }

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the item to delete.</param>
        /// <returns>true if the item was successfully removed from the cache; false otherwise.</returns>
        public bool Remove(string key)
        {
            return ExecuteRemove(key).Success;
        }

        //TODO: Not Implement
        public async Task<bool> RemoveAsync(string key)
        {
            return (await ExecuteRemoveAsync(key)).Success;
        }

        /// <summary>
        /// Retrieves multiple items from the cache.
        /// </summary>
        /// <param name="keys">The list of identifiers for the items to retrieve.</param>
        /// <returns>a Dictionary holding all items indexed by their key.</returns>
        public IDictionary<string, T> Get<T>(IEnumerable<string> keys)
        {
            return PerformMultiGet<T>(keys, (mget, kvp) => this.transcoder.Deserialize<T>(kvp.Value));
        }

        public async Task<IDictionary<string, T>> GetAsync<T>(IEnumerable<string> keys)
        {
            return await PerformMultiGetAsync<T>(keys, (mget, kvp) => this.transcoder.Deserialize<T>(kvp.Value));
        }

        public IDictionary<string, CasResult<object>> GetWithCas(IEnumerable<string> keys)
        {
            return PerformMultiGet(keys, (mget, kvp) => new CasResult<object>
            {
                Result = this.transcoder.Deserialize(kvp.Value),
                Cas = mget.Cas[kvp.Key]
            });
        }

        public async Task<IDictionary<string, CasResult<object>>> GetWithCasAsync(IEnumerable<string> keys)
        {
            return await PerformMultiGetAsync(keys, (mget, kvp) => new CasResult<object>
            {
                Result = this.transcoder.Deserialize(kvp.Value),
                Cas = mget.Cas[kvp.Key]
            });
        }

        protected virtual IDictionary<string, T> PerformMultiGet<T>(IEnumerable<string> keys, Func<IMultiGetOperation, KeyValuePair<string, CacheItem>, T> collector)
        {
            // transform the keys and index them by hashed => original
            // the mget results will be mapped using this index
            var hashed = new Dictionary<string, string>();
            foreach (var key in keys) hashed[this.keyTransformer.Transform(key)] = key;

            var byServer = GroupByServer(hashed.Keys);

            var retval = new Dictionary<string, T>(hashed.Count);
            var tasks = new List<Task>();

            //execute each list of keys on their respective node
            foreach (var slice in byServer)
            {
                var node = slice.Key;

                var nodeKeys = slice.Value;
                var mget = this.pool.OperationFactory.MultiGet(nodeKeys);

                // run gets in parallel
                var action = new Func<IOperation, IOperationResult>(node.Execute);

                //execute the mgets in parallel
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        if (action(mget).Success)
                        {
                            // deserialize the items in the dictionary
                            foreach (var kvp in mget.Result)
                            {
                                string original;
                                if (hashed.TryGetValue(kvp.Key, out original))
                                {
                                    var result = collector(mget, kvp);
                                    // the lock will serialize the merge,
                                    // but at least the commands were not waiting on each other
                                    lock (retval) retval[original] = result;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(0, e, "PerformMultiGet");
                    }
                }));
            }

            // wait for all nodes to finish
            if (tasks.Count > 0)
            {
                Task.WaitAll(tasks.ToArray());
            }

            return retval;
        }

        protected virtual async Task<IDictionary<string, T>> PerformMultiGetAsync<T>(IEnumerable<string> keys, Func<IMultiGetOperation, KeyValuePair<string, CacheItem>, T> collector)
        {
            // transform the keys and index them by hashed => original
            // the mget results will be mapped using this index
            var hashed = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                hashed[this.keyTransformer.Transform(key)] = key;
            }

            var byServer = GroupByServer(hashed.Keys);

            var retval = new Dictionary<string, T>(hashed.Count);
            var tasks = new List<Task>();

            //execute each list of keys on their respective node
            foreach (var slice in byServer)
            {
                var node = slice.Key;
                var nodeKeys = slice.Value;
                var mget = this.pool.OperationFactory.MultiGet(nodeKeys);
                var task = Task.Run(async () =>
                {
                    if ((await node.ExecuteAsync(mget)).Success)
                    {
                        foreach (var kvp in mget.Result)
                        {
                            if (hashed.TryGetValue(kvp.Key, out var original))
                            {
                                lock (retval) retval[original] = collector(mget, kvp);
                            }
                        }
                    }
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            return retval;
        }

        protected Dictionary<IMemcachedNode, IList<string>> GroupByServer(IEnumerable<string> keys)
        {
            var retval = new Dictionary<IMemcachedNode, IList<string>>();

            foreach (var k in keys)
            {
                var node = this.pool.Locate(k);
                if (node == null) continue;

                IList<string> list;
                if (!retval.TryGetValue(node, out list))
                    retval[node] = list = new List<string>(4);

                list.Add(k);
            }

            return retval;
        }

        /// <summary>
        /// Waits for all WaitHandles and works in both STA and MTA mode.
        /// </summary>
        /// <param name="waitHandles"></param>
        private static void SafeWaitAllAndDispose(WaitHandle[] waitHandles)
        {
            try
            {
                //Not support .NET Core
                //if (Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA)
                //    WaitHandle.WaitAll(waitHandles);
                //else
                for (var i = 0; i < waitHandles.Length; i++)
                    waitHandles[i].WaitOne();
            }
            finally
            {
                for (var i = 0; i < waitHandles.Length; i++)
                    waitHandles[i].Dispose();
            }
        }

        #region [ Expiration helper            ]

        protected const int MaxSeconds = 60 * 60 * 24 * 30;
        protected static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        protected static uint GetExpiration(
            TimeSpan? validFor,
            DateTime? expiresAt = null,
            DateTimeOffset? absoluteExpiration = null,
            TimeSpan? relativeToNow = null)
        {
            if (validFor != null && expiresAt != null)
                throw new ArgumentException("You cannot specify both validFor and expiresAt.");

            if (validFor == null && expiresAt == null && absoluteExpiration == null && relativeToNow == null)
            {
                return 0;
            }

            if (absoluteExpiration != null)
            {
                return (uint)absoluteExpiration.Value.ToUnixTimeSeconds();
            }

            if (relativeToNow != null)
            {
                return (uint)(DateTimeOffset.UtcNow + relativeToNow.Value).ToUnixTimeSeconds();
            }

            // convert timespans to absolute dates
            if (validFor != null)
            {
                // infinity
                if (validFor == TimeSpan.Zero || validFor == TimeSpan.MaxValue) return 0;

                if (validFor.Value.TotalSeconds <= MaxSeconds)
                {
                    return (uint)validFor.Value.TotalSeconds;
                }

                expiresAt = DateTime.Now.Add(validFor.Value);
            }

            DateTime dt = expiresAt.Value;

            if (dt < UnixEpoch) throw new ArgumentOutOfRangeException("expiresAt", "expiresAt must be >= 1970/1/1");

            // accept MaxValue as infinite
            if (dt == DateTime.MaxValue) return 0;

            uint retval = (uint)(dt.ToUniversalTime() - UnixEpoch).TotalSeconds;

            return retval;
        }

        protected static string GetExpiratonKey(string key)
        {
            return key + "-" + nameof(DistributedCacheEntryOptions);
        }

        #endregion
        #region [ IDisposable                  ]

        ~MemcachedClient()
        {
            try { ((IDisposable)this).Dispose(); }
            catch { }
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        /// <summary>
        /// Releases all resources allocated by this instance
        /// </summary>
        /// <remarks>You should only call this when you are not using static instances of the client, so it can close all conections and release the sockets.</remarks>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (this.pool != null)
            {
                try { this.pool.Dispose(); }
                finally { this.pool = null; }
            }
        }

        #region Implement IDistributedCache

        byte[] IDistributedCache.Get(string key)
        {
            _logger.LogInformation($"{nameof(IDistributedCache.Get)}(\"{key}\")");

            return Get<byte[]>(key);
        }

        async Task<byte[]> IDistributedCache.GetAsync(string key, CancellationToken token = default(CancellationToken))
        {
            _logger.LogInformation($"{nameof(IDistributedCache.GetAsync)}(\"{key}\")");

            return await GetValueAsync<byte[]>(key);
        }

        void IDistributedCache.Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            _logger.LogInformation($"{nameof(IDistributedCache.Set)}(\"{key}\")");

            ulong cas = 0;
            var expires = MemcachedClient.GetExpiration(options.SlidingExpiration, null, options.AbsoluteExpiration, options.AbsoluteExpirationRelativeToNow);
            PerformStore(StoreMode.Set, key, value, expires, cas);
            if (expires > 0)
            {
                PerformStore(StoreMode.Set, GetExpiratonKey(key), expires, expires, cas);
            }
        }

        async Task IDistributedCache.SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            _logger.LogInformation($"{nameof(IDistributedCache.SetAsync)}(\"{key}\")");

            var expires = MemcachedClient.GetExpiration(options.SlidingExpiration, null, options.AbsoluteExpiration, options.AbsoluteExpirationRelativeToNow);
            await PerformStoreAsync(StoreMode.Set, key, value, expires);
            if (expires > 0)
            {
                await PerformStoreAsync(StoreMode.Set, GetExpiratonKey(key), expires, expires);
            }
        }

        void IDistributedCache.Refresh(string key)
        {
            _logger.LogInformation($"{nameof(IDistributedCache.Refresh)}(\"{key}\")");

            var value = Get(key);
            if (value != null)
            {
                var expirationValue = Get(GetExpiratonKey(key));
                if (expirationValue != null)
                {
                    ulong cas = 0;
                    PerformStore(StoreMode.Replace, key, value, uint.Parse(expirationValue.ToString()), cas);
                }
            }
        }

        async Task IDistributedCache.RefreshAsync(string key, CancellationToken token = default(CancellationToken))
        {
            _logger.LogInformation($"{nameof(IDistributedCache.RefreshAsync)}(\"{key}\")");

            var result = await GetAsync<byte[]>(key);
            if (result.Success)
            {
                var expirationResult = await GetAsync<uint>(GetExpiratonKey(key));
                if (expirationResult.Success)
                {
                    await PerformStoreAsync(StoreMode.Replace, key, result.Value, expirationResult.Value);
                }
            }
        }

        void IDistributedCache.Remove(string key)
        {
            Remove(key);
        }

        async Task IDistributedCache.RemoveAsync(string key, CancellationToken token = default(CancellationToken))
        {
            await RemoveAsync(key);
        }

        #endregion

        #endregion
    }
}

#region [ License information          ]
/* ************************************************************
 * 
 *    Copyright (c) 2010 Attila Kisk? enyim.com
 *    
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *    
 *        http://www.apache.org/licenses/LICENSE-2.0
 *    
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *    
 * ************************************************************/
#endregion
