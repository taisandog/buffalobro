using System.Collections.Generic;
using System.Threading.Tasks;
using Enyim.Caching.Memcached.Results;
using Enyim.Caching.Memcached.Results.Extensions;

namespace Enyim.Caching.Memcached.Protocol.Text
{
    public class FlushOperation : Operation, IFlushOperation
    {
        public FlushOperation() { }

        protected internal override IList<System.ArraySegment<byte>> GetBuffer()
        {
            return TextSocketHelper.GetCommandBuffer("flush_all" + TextSocketHelper.CommandTerminator);
        }

        protected internal override IOperationResult ReadResponse(PooledSocket socket)
        {
            TextSocketHelper.ReadResponse(socket);
            return new TextOperationResult().Pass();
        }

        protected internal override ValueTask<IOperationResult> ReadResponseAsync(PooledSocket socket)
        {
            TextSocketHelper.ReadResponse(socket);
            return new ValueTask<IOperationResult>(new TextOperationResult().Pass());
        }

        protected internal override Task<bool> ReadResponseAsync(PooledSocket socket, System.Action<bool> next)
        {
            throw new System.NotSupportedException();
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
