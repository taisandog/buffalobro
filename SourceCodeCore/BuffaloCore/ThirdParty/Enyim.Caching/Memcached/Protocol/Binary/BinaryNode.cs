using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Enyim.Caching.Configuration;
using Enyim.Collections;
using System.Security;
using Microsoft.Extensions.Logging;

namespace Enyim.Caching.Memcached.Protocol.Binary
{
    /// <summary>
    /// A node which is used by the BinaryPool. It implements the binary protocol's SASL auth. mechanism.
    /// </summary>
    public class BinaryNode : MemcachedNode
    {
        private readonly ILogger _logger;
        readonly ISaslAuthenticationProvider authenticationProvider;

        public BinaryNode(
            EndPoint endpoint,
            ISocketPoolConfiguration config,
            ISaslAuthenticationProvider authenticationProvider,
            ILogger logger)
            : base(endpoint, config, logger)
        {
            this.authenticationProvider = authenticationProvider;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates the new socket before it is put into the pool.
        /// </summary>
        protected internal override PooledSocket CreateSocket()
        {
            var retval = base.CreateSocket();

            if (this.authenticationProvider != null && !this.Auth(retval))
            {
                _logger.LogError("Authentication failed: " + this.EndPoint);

                throw new SecurityException("auth failed: " + this.EndPoint);
            }

            return retval;
        }

        /// <summary>
        /// Implements memcached's SASL auth sequence. (See the protocol docs for more details.)
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private bool Auth(PooledSocket socket)
        {
            SaslStep currentStep = new SaslStart(this.authenticationProvider);

            socket.Write(currentStep.GetBuffer());

            while (!currentStep.ReadResponse(socket).Success)
            {
                // challenge-response authentication
                if (currentStep.StatusCode == 0x21)
                {
                    currentStep = new SaslContinue(this.authenticationProvider, currentStep.Data);
                    socket.Write(currentStep.GetBuffer());
                }
                else
                {
                    _logger.LogWarning("Authentication failed, return code: 0x{0:x}", currentStep.StatusCode);

                    // invalid credentials or other error
                    return false;
                }
            }

            return true;
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
