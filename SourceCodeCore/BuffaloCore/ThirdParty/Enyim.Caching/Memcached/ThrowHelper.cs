using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Enyim.Caching.Memcached
{
    internal static class ThrowHelper
    {
        public static void ThrowSocketWriteError(EndPoint endpoint)
        {
            throw new IOException(string.Format("Failed to write to the socket '{0}'.", endpoint));
        }

        public static void ThrowSocketWriteError(EndPoint endpoint, SocketError error)
        {
            // move the string into resource file
            throw new IOException(string.Format("Failed to write to the socket '{0}'. Error: {1}", endpoint, error));
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
