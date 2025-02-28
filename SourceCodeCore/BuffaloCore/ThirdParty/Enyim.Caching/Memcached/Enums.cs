using Enyim.Caching.Memcached.Protocol.Binary;
using System;

namespace Enyim.Caching.Memcached
{
    public enum MutationMode : byte { Increment = 0x05, Decrement = 0x06, Touch = OpCode.Touch };
    public enum ConcatenationMode : byte { Append = 0x0E, Prepend = 0x0F };
    public enum MemcachedProtocol { Binary, Text }
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
