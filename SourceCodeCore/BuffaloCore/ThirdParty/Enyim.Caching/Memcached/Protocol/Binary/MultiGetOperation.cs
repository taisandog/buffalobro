using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Enyim.Caching.Memcached.Results;
using Enyim.Caching.Memcached.Results.Extensions;

namespace Enyim.Caching.Memcached.Protocol.Binary
{
    public class MultiGetOperation : BinaryMultiItemOperation, IMultiGetOperation
    {
        private static readonly Enyim.Caching.ILog log = Enyim.Caching.LogManager.GetLogger(typeof(MultiGetOperation));

        private Dictionary<string, CacheItem> result;
        private Dictionary<int, string> idToKey;
        private int noopId;

        public MultiGetOperation(IList<string> keys) : base(keys) { }

        protected override BinaryRequest Build(string key)
        {
            var request = new BinaryRequest(OpCode.GetQ)
            {
                Key = key
            };

            return request;
        }

        protected internal override IList<ArraySegment<byte>> GetBuffer()
        {
            var keys = this.Keys;

            if (keys == null || keys.Count == 0)
            {
                if (log.IsWarnEnabled) log.Warn("Empty multiget!");

                return new ArraySegment<byte>[0];
            }

            if (log.IsDebugEnabled)
                log.DebugFormat("Building multi-get for {0} keys", keys.Count);

            // map the command's correlationId to the item key,
            // so we can use GetQ (which only returns the item data)
            this.idToKey = new Dictionary<int, string>();

            // get ops have 2 segments, header + key
            var buffers = new List<ArraySegment<byte>>(keys.Count * 2);

            foreach (var key in keys)
            {
                var request = this.Build(key);

                request.CreateBuffer(buffers);

                // we use this to map the responses to the keys
                idToKey[request.CorrelationId] = key;
            }

            // uncork the server
            var noop = new BinaryRequest(OpCode.NoOp);
            this.noopId = noop.CorrelationId;

            noop.CreateBuffer(buffers);

            return buffers;
        }


        private readonly PooledSocket currentSocket;
        private readonly BinaryResponse asyncReader;
        private readonly bool? asyncLoopState;
        private readonly Action<bool> afterAsyncRead;


        protected internal override async Task<bool> ReadResponseAsync(PooledSocket socket, Action<bool> next)
        {
            var result = await ReadResponseAsync(socket);
            next(result.Success);
            return result.Success;
        }

        private void StoreResult(BinaryResponse reader)
        {
            string key;

            // find the key to the response
            if (!this.idToKey.TryGetValue(reader.CorrelationId, out key))
            {
                // we're not supposed to get here tho
                log.WarnFormat("Found response with CorrelationId {0}, but no key is matching it.", reader.CorrelationId);
            }
            else
            {
                if (log.IsDebugEnabled) log.DebugFormat("Reading item {0}", key);

                // deserialize the response
                var flags = (ushort)BinaryConverter.DecodeInt32(reader.Extra, 0);

                this.result[key] = new CacheItem(flags, reader.Data);
                this.Cas[key] = reader.CAS;
            }
        }

        protected internal override IOperationResult ReadResponse(PooledSocket socket)
        {
            this.result = new Dictionary<string, CacheItem>();
            this.Cas = new Dictionary<string, ulong>();
            var result = new TextOperationResult();

            var response = new BinaryResponse();

            while (response.Read(socket))
            {
                this.StatusCode = response.StatusCode;

                // found the noop, quit
                if (response.CorrelationId == this.noopId)
                    return result.Pass();

                string key;

                // find the key to the response
                if (!this.idToKey.TryGetValue(response.CorrelationId, out key))
                {
                    // we're not supposed to get here tho
                    log.WarnFormat("Found response with CorrelationId {0}, but no key is matching it.", response.CorrelationId);
                    continue;
                }

                if (log.IsDebugEnabled) log.DebugFormat("Reading item {0}", key);

                // deserialize the response
                int flags = BinaryConverter.DecodeInt32(response.Extra, 0);

                this.result[key] = new CacheItem((ushort)flags, response.Data);
                this.Cas[key] = response.CAS;
            }

            // finished reading but we did not find the NOOP
            return result.Fail("Found response with CorrelationId {0}, but no key is matching it.");
        }

        protected internal override async ValueTask<IOperationResult> ReadResponseAsync(PooledSocket socket)
        {
            this.result = new Dictionary<string, CacheItem>();
            this.Cas = new Dictionary<string, ulong>();
            var result = new TextOperationResult();

            var response = new BinaryResponse();

            while (await response.ReadAsync(socket))
            {
                this.StatusCode = response.StatusCode;

                // found the noop, quit
                if (response.CorrelationId == this.noopId)
                    return result.Pass();

                string key;

                // find the key to the response
                if (!this.idToKey.TryGetValue(response.CorrelationId, out key))
                {
                    // we're not supposed to get here tho
                    log.WarnFormat("Found response with CorrelationId {0}, but no key is matching it.", response.CorrelationId);
                    continue;
                }

                if (log.IsDebugEnabled) log.DebugFormat("Reading item {0}", key);

                // deserialize the response
                int flags = BinaryConverter.DecodeInt32(response.Extra, 0);

                this.result[key] = new CacheItem((ushort)flags, response.Data);
                this.Cas[key] = response.CAS;
            }

            // finished reading but we did not find the NOOP
            return result.Fail("Found response with CorrelationId {0}, but no key is matching it.");
        }

        public Dictionary<string, CacheItem> Result
        {
            get { return this.result; }
        }

        Dictionary<string, CacheItem> IMultiGetOperation.Result
        {
            get { return this.result; }
        }
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
