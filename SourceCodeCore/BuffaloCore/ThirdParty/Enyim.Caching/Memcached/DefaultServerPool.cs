using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using Enyim.Caching.Configuration;
using Microsoft.Extensions.Logging;

namespace Enyim.Caching.Memcached
{
    public class DefaultServerPool : IServerPool, IDisposable
    {
        private readonly ILogger _logger;

        private IMemcachedNode[] allNodes;

        private readonly IMemcachedClientConfiguration configuration;
        private readonly IOperationFactory factory;
        private IMemcachedNodeLocator nodeLocator;

        private readonly object DeadSync = new Object();
        private System.Threading.Timer resurrectTimer;
        private bool isTimerActive;
        private readonly int deadTimeoutMsec;
        private bool isDisposed;
        private event Action<IMemcachedNode> nodeFailed;

        public DefaultServerPool(
            IMemcachedClientConfiguration configuration,
            IOperationFactory opFactory,
            ILogger logger)
        {
            if (configuration == null) throw new ArgumentNullException("socketConfig");
            if (opFactory == null) throw new ArgumentNullException("opFactory");

            this.configuration = configuration;
            this.factory = opFactory;

            this.deadTimeoutMsec = (int)this.configuration.SocketPool.DeadTimeout.TotalMilliseconds;

            _logger = logger;
        }

        ~DefaultServerPool()
        {
            try { ((IDisposable)this).Dispose(); }
            catch { }
        }

        protected virtual IMemcachedNode CreateNode(EndPoint endpoint)
        {
            return new MemcachedNode(endpoint, this.configuration.SocketPool, _logger);
        }

        private void rezCallback(object state)
        {
            var isDebug = _logger.IsEnabled(LogLevel.Debug);

            if (isDebug) _logger.LogDebug("Checking the dead servers.");

            // how this works:
            // 1. timer is created but suspended
            // 2. Locate encounters a dead server, so it starts the timer which will trigger after deadTimeout has elapsed
            // 3. if another server goes down before the timer is triggered, nothing happens in Locate (isRunning == true).
            //		however that server will be inspected sooner than Dead Timeout.
            //		   S1 died   S2 died    dead timeout
            //		|----*--------*------------*-
            //           |                     |
            //          timer start           both servers are checked here
            // 4. we iterate all the servers and record it in another list
            // 5. if we found a dead server whihc responds to Ping(), the locator will be reinitialized
            // 6. if at least one server is still down (Ping() == false), we restart the timer
            // 7. if all servers are up, we set isRunning to false, so the timer is suspended
            // 8. GOTO 2
            lock (this.DeadSync)
            {
                if (this.isDisposed)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                        _logger.LogWarning("IsAlive timer was triggered but the pool is already disposed. Ignoring.");

                    return;
                }

                var nodes = this.allNodes;
                var aliveList = new List<IMemcachedNode>(nodes.Length);
                var changed = false;
                var deadCount = 0;

                for (var i = 0; i < nodes.Length; i++)
                {
                    var n = nodes[i];
                    if (n.IsAlive)
                    {
                        if (isDebug) _logger.LogDebug("Alive: {0}", n.EndPoint);

                        aliveList.Add(n);
                    }
                    else
                    {
                        if (isDebug) _logger.LogDebug("Dead: {0}", n.EndPoint);

                        if (n.Ping())
                        {
                            changed = true;
                            aliveList.Add(n);

                            if (isDebug) _logger.LogDebug("Ping ok.");
                        }
                        else
                        {
                            if (isDebug) _logger.LogDebug("Still dead.");

                            deadCount++;
                        }
                    }
                }

                // reinit the locator
                if (changed)
                {
                    if (isDebug) _logger.LogDebug("Reinitializing the locator.");

                    this.nodeLocator.Initialize(aliveList);
                }

                // stop or restart the timer
                if (deadCount == 0)
                {
                    if (isDebug) _logger.LogDebug("deadCount == 0, stopping the timer.");

                    this.isTimerActive = false;
                }
                else
                {
                    if (isDebug) _logger.LogDebug("deadCount == {0}, starting the timer.", deadCount);

                    this.resurrectTimer.Change(this.deadTimeoutMsec, Timeout.Infinite);
                }
            }
        }

        private void NodeFail(IMemcachedNode node)
        {
            var isDebug = _logger.IsEnabled(LogLevel.Debug);
            if (isDebug) _logger.LogDebug("Node {0} is dead.", node.EndPoint);

            // the timer is stopped until we encounter the first dead server
            // when we have one, we trigger it and it will run after DeadTimeout has elapsed
            lock (this.DeadSync)
            {
                if (this.isDisposed)
                {
                    if (_logger.IsEnabled(LogLevel.Warning)) _logger.LogWarning("Got a node fail but the pool is already disposed. Ignoring.");

                    return;
                }

                // bubble up the fail event to the client
                var fail = this.nodeFailed;
                if (fail != null)
                    fail(node);

                // re-initialize the locator
                var newLocator = this.configuration.CreateNodeLocator();
                newLocator.Initialize(allNodes.Where(n => n.IsAlive).ToArray());
                Interlocked.Exchange(ref this.nodeLocator, newLocator);

                // the timer is stopped until we encounter the first dead server
                // when we have one, we trigger it and it will run after DeadTimeout has elapsed
                if (!this.isTimerActive)
                {
                    if (isDebug) _logger.LogDebug("Starting the recovery timer.");

                    if (this.resurrectTimer == null)
                        this.resurrectTimer = new Timer(this.rezCallback, null, this.deadTimeoutMsec, Timeout.Infinite);
                    else
                        this.resurrectTimer.Change(this.deadTimeoutMsec, Timeout.Infinite);

                    this.isTimerActive = true;

                    if (isDebug) _logger.LogDebug("Timer started.");
                }
            }
        }

        #region [ IServerPool                  ]

        IMemcachedNode IServerPool.Locate(string key)
        {
            var node = this.nodeLocator.Locate(key);

            return node;
        }

        IOperationFactory IServerPool.OperationFactory
        {
            get { return this.factory; }
        }

        IEnumerable<IMemcachedNode> IServerPool.GetWorkingNodes()
        {
            return this.nodeLocator.GetWorkingNodes();
        }

        void IServerPool.Start()
        {
            this.allNodes = this.configuration.Servers.
                                Select(ep =>
                                {
                                    var node = this.CreateNode(ep);
                                    node.Failed += this.NodeFail;

                                    return node;
                                }).
                                ToArray();

            // initialize the locator
            var locator = this.configuration.CreateNodeLocator();
            locator.Initialize(allNodes);

            this.nodeLocator = locator;
        }

        event Action<IMemcachedNode> IServerPool.NodeFailed
        {
            add { this.nodeFailed += value; }
            remove { this.nodeFailed -= value; }
        }

        #endregion
        #region [ IDisposable                  ]

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);

            lock (this.DeadSync)
            {
                if (this.isDisposed) return;

                this.isDisposed = true;

                // dispose the locator first, maybe it wants to access 
                // the nodes one last time
                var nd = this.nodeLocator as IDisposable;
                if (nd != null)
                    try { nd.Dispose(); }
                    catch (Exception e) { _logger.LogError(nameof(DefaultServerPool), e); }

                this.nodeLocator = null;

                for (var i = 0; i < this.allNodes.Length; i++)
                    try { this.allNodes[i].Dispose(); }
                    catch (Exception e) { _logger.LogError(nameof(DefaultServerPool), e); }

                // stop the timer
                if (this.resurrectTimer != null)
                    using (this.resurrectTimer)
                        this.resurrectTimer.Change(Timeout.Infinite, Timeout.Infinite);

                this.allNodes = null;
                this.resurrectTimer = null;
            }
        }

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
