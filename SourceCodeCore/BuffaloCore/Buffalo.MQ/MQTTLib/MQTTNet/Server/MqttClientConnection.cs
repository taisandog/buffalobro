﻿using Buffalo.MQ.MQTTLib.MQTTnet.Adapter;
using Buffalo.MQ.MQTTLib.MQTTnet.Client;
using Buffalo.MQ.MQTTLib.MQTTnet.Diagnostics;
using Buffalo.MQ.MQTTLib.MQTTnet.Exceptions;
using Buffalo.MQ.MQTTLib.MQTTnet.Formatter;
using Buffalo.MQ.MQTTLib.MQTTnet.Implementations;
using Buffalo.MQ.MQTTLib.MQTTnet.Internal;
using Buffalo.MQ.MQTTLib.MQTTnet.PacketDispatcher;
using Buffalo.MQ.MQTTLib.MQTTnet.Packets;
using Buffalo.MQ.MQTTLib.MQTTnet.Protocol;
using Buffalo.MQ.MQTTLib.MQTTnet.Server.Status;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public sealed class MqttClientConnection : IDisposable
    {
        readonly MqttPacketIdentifierProvider _packetIdentifierProvider = new MqttPacketIdentifierProvider();
        readonly MqttPacketDispatcher _packetDispatcher = new MqttPacketDispatcher();
        readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        readonly IMqttRetainedMessagesManager _retainedMessagesManager;
        readonly Func<Task> _onStart;
        readonly Func<MqttClientDisconnectType, Task> _onStop;
        readonly MqttClientKeepAliveMonitor _keepAliveMonitor;
        readonly MqttClientSessionsManager _sessionsManager;

        readonly IMqttNetScopedLogger _logger;
        readonly IMqttServerOptions _serverOptions;

        readonly IMqttChannelAdapter _channelAdapter;
        readonly IMqttDataConverter _dataConverter;
        readonly string _endpoint;
        readonly DateTime _connectedTimestamp;

        volatile Task _packageReceiverTask;
        DateTime _lastPacketReceivedTimestamp;
        DateTime _lastNonKeepAlivePacketReceivedTimestamp;

        long _receivedPacketsCount;
        long _sentPacketsCount = 1; // Start with 1 because the CONNECT packet is not counted anywhere.
        long _receivedApplicationMessagesCount;
        long _sentApplicationMessagesCount;

        volatile bool _isTakeover;

        public MqttClientConnection(
            MqttConnectPacket connectPacket,
            IMqttChannelAdapter channelAdapter,
            MqttClientSession session,
            IMqttServerOptions serverOptions,
            MqttClientSessionsManager sessionsManager,
            IMqttRetainedMessagesManager retainedMessagesManager,
            Func<Task> onStart,
            Func<MqttClientDisconnectType, Task> onStop,
            IMqttNetLogger logger)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
            _serverOptions = serverOptions ?? throw new ArgumentNullException(nameof(serverOptions));
            _sessionsManager = sessionsManager ?? throw new ArgumentNullException(nameof(sessionsManager));
            _retainedMessagesManager = retainedMessagesManager ?? throw new ArgumentNullException(nameof(retainedMessagesManager));
            _onStart = onStart ?? throw new ArgumentNullException(nameof(onStart));
            _onStop = onStop ?? throw new ArgumentNullException(nameof(onStop));

            _channelAdapter = channelAdapter ?? throw new ArgumentNullException(nameof(channelAdapter));
            _dataConverter = _channelAdapter.PacketFormatterAdapter.DataConverter;
            _endpoint = _channelAdapter.Endpoint;
            ConnectPacket = connectPacket ?? throw new ArgumentNullException(nameof(connectPacket));

            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger.CreateScopedLogger(nameof(MqttClientConnection));

            _keepAliveMonitor = new MqttClientKeepAliveMonitor(ConnectPacket.ClientId, () => StopAsync(), logger);

            _connectedTimestamp = DateTime.UtcNow;
            _lastPacketReceivedTimestamp = _connectedTimestamp;
            _lastNonKeepAlivePacketReceivedTimestamp = _lastPacketReceivedTimestamp;
        }

        public MqttConnectPacket ConnectPacket { get; }

        public string ClientId => ConnectPacket.ClientId;

        public MqttClientSession Session { get; }

        public Task StopAsync(bool isTakeover = false)
        {
            _isTakeover = isTakeover;
            var task = _packageReceiverTask;

            StopInternal();

            if (task != null)
            {
                return task;
            }

            return PlatformAbstractionLayer.CompletedTask;
        }

        public void ResetStatistics()
        {
            _channelAdapter.ResetStatistics();
        }

        public void FillStatus(MqttClientStatus status)
        {
            status.ClientId = ClientId;
            status.Endpoint = _endpoint;
            status.ProtocolVersion = _channelAdapter.PacketFormatterAdapter.ProtocolVersion;

            status.ReceivedApplicationMessagesCount = Interlocked.Read(ref _receivedApplicationMessagesCount);
            status.SentApplicationMessagesCount = Interlocked.Read(ref _sentApplicationMessagesCount);

            status.ReceivedPacketsCount = Interlocked.Read(ref _receivedPacketsCount);
            status.SentPacketsCount = Interlocked.Read(ref _sentPacketsCount);

            status.ConnectedTimestamp = _connectedTimestamp;
            status.LastPacketReceivedTimestamp = _lastPacketReceivedTimestamp;
            status.LastNonKeepAlivePacketReceivedTimestamp = _lastNonKeepAlivePacketReceivedTimestamp;

            status.BytesSent = _channelAdapter.BytesSent;
            status.BytesReceived = _channelAdapter.BytesReceived;
        }

        public void Dispose()
        {
            _cancellationToken.Dispose();
        }

        public Task RunAsync(MqttConnectionValidatorContext connectionValidatorContext)
        {
            _packageReceiverTask = RunInternalAsync(connectionValidatorContext);
            return _packageReceiverTask;
        }

        async Task RunInternalAsync(MqttConnectionValidatorContext connectionValidatorContext)
        {
            var disconnectType = MqttClientDisconnectType.NotClean;
            try
            {
                await _onStart();
                _logger.Info("Client '{0}': Session started.", ClientId);

                _channelAdapter.ReadingPacketStartedCallback = OnAdapterReadingPacketStarted;
                _channelAdapter.ReadingPacketCompletedCallback = OnAdapterReadingPacketCompleted;

                Session.WillMessage = ConnectPacket.WillMessage;

                Task.Run(() => SendPendingPacketsAsync(_cancellationToken.Token), _cancellationToken.Token).Forget(_logger);

                // TODO: Change to single thread in SessionManager. Or use SessionManager and stats from KeepAliveMonitor.
                _keepAliveMonitor.Start(ConnectPacket.KeepAlivePeriod, _cancellationToken.Token);

                await SendAsync(
                    _channelAdapter.PacketFormatterAdapter.DataConverter.CreateConnAckPacket(connectionValidatorContext)
                    ).ConfigureAwait(false);

                Session.IsCleanSession = false;

                while (!_cancellationToken.IsCancellationRequested)
                {
                    var packet = await _channelAdapter.ReceivePacketAsync(TimeSpan.Zero, _cancellationToken.Token).ConfigureAwait(false);
                    if (packet == null)
                    {
                        // The client has closed the connection gracefully.
                        break;
                    }

                    Interlocked.Increment(ref _sentPacketsCount);
                    _lastPacketReceivedTimestamp = DateTime.UtcNow;

                    if (!(packet is MqttPingReqPacket || packet is MqttPingRespPacket))
                    {
                        _lastNonKeepAlivePacketReceivedTimestamp = _lastPacketReceivedTimestamp;
                    }

                    _keepAliveMonitor.PacketReceived();

                    if (packet is MqttPublishPacket publishPacket)
                    {
                        await HandleIncomingPublishPacketAsync(publishPacket).ConfigureAwait(false);
                        continue;
                    }

                    if (packet is MqttPubRelPacket pubRelPacket)
                    {
                        var pubCompPacket = new MqttPubCompPacket
                        {
                            PacketIdentifier = pubRelPacket.PacketIdentifier,
                            ReasonCode = MqttPubCompReasonCode.Success
                        };

                        await SendAsync(pubCompPacket).ConfigureAwait(false);
                        continue;
                    }

                    if (packet is MqttSubscribePacket subscribePacket)
                    {
                        await HandleIncomingSubscribePacketAsync(subscribePacket).ConfigureAwait(false);
                        continue;
                    }

                    if (packet is MqttUnsubscribePacket unsubscribePacket)
                    {
                        await HandleIncomingUnsubscribePacketAsync(unsubscribePacket).ConfigureAwait(false);
                        continue;
                    }

                    if (packet is MqttPingReqPacket)
                    {
                        await SendAsync(new MqttPingRespPacket()).ConfigureAwait(false);
                        continue;
                    }

                    if (packet is MqttDisconnectPacket)
                    {
                        Session.WillMessage = null;
                        disconnectType = MqttClientDisconnectType.Clean;

                        StopInternal();
                        break;
                    }

                    _packetDispatcher.Dispatch(packet);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                if (exception is MqttCommunicationException)
                {
                    _logger.Warning(exception, "Client '{0}': Communication exception while receiving client packets.", ClientId);
                }
                else
                {
                    _logger.Error(exception, "Client '{0}': Error while receiving client packets.", ClientId);
                }

                StopInternal();
            }
            finally
            {
                if (_isTakeover)
                {
                    disconnectType = MqttClientDisconnectType.Takeover;
                }
                
                if (Session.WillMessage != null)
                {
                    _sessionsManager.DispatchApplicationMessage(Session.WillMessage, this);
                    Session.WillMessage = null;
                }

                _packetDispatcher.Reset();

                _channelAdapter.ReadingPacketStartedCallback = null;
                _channelAdapter.ReadingPacketCompletedCallback = null;

                _logger.Info("Client '{0}': Connection stopped.", ClientId);

                _packageReceiverTask = null;

                try
                {
                    await _onStop(disconnectType);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "client '{0}': Error while cleaning up", ClientId);
                }
            }
        }

        void StopInternal()
        {
            _cancellationToken.Cancel(false);
        }

        async Task EnqueueSubscribedRetainedMessagesAsync(ICollection<MqttTopicFilter> topicFilters)
        {
            var retainedMessages = await _retainedMessagesManager.GetSubscribedMessagesAsync(topicFilters).ConfigureAwait(false);
            foreach (var applicationMessage in retainedMessages)
            {
                Session.EnqueueApplicationMessage(applicationMessage, ClientId, true);
            }
        }

        async Task HandleIncomingSubscribePacketAsync(MqttSubscribePacket subscribePacket)
        {
            // TODO: Let the channel adapter create the packet.
            var subscribeResult = await Session.SubscriptionsManager.SubscribeAsync(subscribePacket, ConnectPacket).ConfigureAwait(false);

            await SendAsync(subscribeResult.ResponsePacket).ConfigureAwait(false);

            if (subscribeResult.CloseConnection)
            {
                StopInternal();
                return;
            }

            await EnqueueSubscribedRetainedMessagesAsync(subscribePacket.TopicFilters).ConfigureAwait(false);
        }

        async Task HandleIncomingUnsubscribePacketAsync(MqttUnsubscribePacket unsubscribePacket)
        {
            // TODO: Let the channel adapter create the packet.
            var unsubscribeResult = await Session.SubscriptionsManager.UnsubscribeAsync(unsubscribePacket).ConfigureAwait(false);
            await SendAsync(unsubscribeResult).ConfigureAwait(false);
        }

        Task HandleIncomingPublishPacketAsync(MqttPublishPacket publishPacket)
        {
            Interlocked.Increment(ref _sentApplicationMessagesCount);

            switch (publishPacket.QualityOfServiceLevel)
            {
                case MqttQualityOfServiceLevel.AtMostOnce:
                    {
                        return HandleIncomingPublishPacketWithQoS0Async(publishPacket);
                    }
                case MqttQualityOfServiceLevel.AtLeastOnce:
                    {
                        return HandleIncomingPublishPacketWithQoS1Async(publishPacket);
                    }
                case MqttQualityOfServiceLevel.ExactlyOnce:
                    {
                        return HandleIncomingPublishPacketWithQoS2Async(publishPacket);
                    }
                default:
                    {
                        throw new MqttCommunicationException("Received a not supported QoS level.");
                    }
            }
        }

        Task HandleIncomingPublishPacketWithQoS0Async(MqttPublishPacket publishPacket)
        {
            var applicationMessage = _dataConverter.CreateApplicationMessage(publishPacket);

            _sessionsManager.DispatchApplicationMessage(applicationMessage, this);

            return PlatformAbstractionLayer.CompletedTask;
        }

        Task HandleIncomingPublishPacketWithQoS1Async(MqttPublishPacket publishPacket)
        {
            var applicationMessage = _dataConverter.CreateApplicationMessage(publishPacket);
            _sessionsManager.DispatchApplicationMessage(applicationMessage, this);

            var pubAckPacket = _dataConverter.CreatePubAckPacket(publishPacket);
            return SendAsync(pubAckPacket);
        }

        Task HandleIncomingPublishPacketWithQoS2Async(MqttPublishPacket publishPacket)
        {
            var applicationMessage = _dataConverter.CreateApplicationMessage(publishPacket);
            _sessionsManager.DispatchApplicationMessage(applicationMessage, this);

            var pubRecPacket = new MqttPubRecPacket
            {
                PacketIdentifier = publishPacket.PacketIdentifier,
                ReasonCode = MqttPubRecReasonCode.Success
            };

            return SendAsync(pubRecPacket);
        }

        async Task SendPendingPacketsAsync(CancellationToken cancellationToken)
        {
            MqttQueuedApplicationMessage queuedApplicationMessage = null;
            MqttPublishPacket publishPacket = null;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    queuedApplicationMessage = await Session.ApplicationMessagesQueue.TakeAsync(cancellationToken).ConfigureAwait(false);
                    if (queuedApplicationMessage == null)
                    {
                        return;
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    publishPacket = _dataConverter.CreatePublishPacket(queuedApplicationMessage.ApplicationMessage);
                    publishPacket.QualityOfServiceLevel = queuedApplicationMessage.QualityOfServiceLevel;

                    // Set the retain flag to true according to [MQTT-3.3.1-8] and [MQTT-3.3.1-9].
                    publishPacket.Retain = queuedApplicationMessage.IsRetainedMessage;

                    if (publishPacket.QualityOfServiceLevel > 0)
                    {
                        publishPacket.PacketIdentifier = _packetIdentifierProvider.GetNextPacketIdentifier();
                    }

                    if (_serverOptions.ClientMessageQueueInterceptor != null)
                    {
                        var context = new MqttClientMessageQueueInterceptorContext(
                            queuedApplicationMessage.SenderClientId,
                            ClientId,
                            queuedApplicationMessage.ApplicationMessage);

                        if (_serverOptions.ClientMessageQueueInterceptor != null)
                        {
                            await _serverOptions.ClientMessageQueueInterceptor.InterceptClientMessageQueueEnqueueAsync(context).ConfigureAwait(false);
                        }

                        if (!context.AcceptEnqueue || context.ApplicationMessage == null)
                        {
                            return;
                        }

                        publishPacket.Topic = context.ApplicationMessage.Topic;
                        publishPacket.Payload = context.ApplicationMessage.Payload;
                        publishPacket.QualityOfServiceLevel = context.ApplicationMessage.QualityOfServiceLevel;
                    }

                    if (publishPacket.QualityOfServiceLevel == MqttQualityOfServiceLevel.AtMostOnce)
                    {
                        await SendAsync(publishPacket).ConfigureAwait(false);
                    }
                    else if (publishPacket.QualityOfServiceLevel == MqttQualityOfServiceLevel.AtLeastOnce)
                    {
                        var awaiter = _packetDispatcher.AddAwaiter<MqttPubAckPacket>(publishPacket.PacketIdentifier);
                        await SendAsync(publishPacket).ConfigureAwait(false);
                        await awaiter.WaitOneAsync(_serverOptions.DefaultCommunicationTimeout).ConfigureAwait(false);
                    }
                    else if (publishPacket.QualityOfServiceLevel == MqttQualityOfServiceLevel.ExactlyOnce)
                    {
                        using (var awaiter1 = _packetDispatcher.AddAwaiter<MqttPubRecPacket>(publishPacket.PacketIdentifier))
                        using (var awaiter2 = _packetDispatcher.AddAwaiter<MqttPubCompPacket>(publishPacket.PacketIdentifier))
                        {
                            await SendAsync(publishPacket).ConfigureAwait(false);
                            await awaiter1.WaitOneAsync(_serverOptions.DefaultCommunicationTimeout).ConfigureAwait(false);

                            await SendAsync(new MqttPubRelPacket { PacketIdentifier = publishPacket.PacketIdentifier }).ConfigureAwait(false);
                            await awaiter2.WaitOneAsync(_serverOptions.DefaultCommunicationTimeout).ConfigureAwait(false);
                        }
                    }

                    _logger.Verbose("Queued application message sent (ClientId: {0}).", ClientId);
                }
            }
            catch (Exception exception)
            {
                if (exception is MqttCommunicationTimedOutException)
                {
                    _logger.Warning(exception, "Sending publish packet failed: Timeout (ClientId: {0}).", ClientId);
                }
                else if (exception is MqttCommunicationException)
                {
                    _logger.Warning(exception, "Sending publish packet failed: Communication exception (ClientId: {0}).", ClientId);
                }
                else if (exception is OperationCanceledException)
                {
                }
                else
                {
                    _logger.Error(exception, "Sending publish packet failed (ClientId: {0}).", ClientId);
                }

                if (publishPacket?.QualityOfServiceLevel > MqttQualityOfServiceLevel.AtMostOnce)
                {
                    queuedApplicationMessage.IsDuplicate = true;

                    Session.ApplicationMessagesQueue.Enqueue(queuedApplicationMessage);
                }

                if (!_cancellationToken.Token.IsCancellationRequested)
                {
                    await StopAsync().ConfigureAwait(false);
                }
            }
        }

        async Task SendAsync(MqttBasePacket packet)
        {
            await _channelAdapter.SendPacketAsync(packet, _serverOptions.DefaultCommunicationTimeout, _cancellationToken.Token).ConfigureAwait(false);

            Interlocked.Increment(ref _receivedPacketsCount);

            if (packet is MqttPublishPacket)
            {
                Interlocked.Increment(ref _receivedApplicationMessagesCount);
            }
        }

        void OnAdapterReadingPacketCompleted()
        {
            _keepAliveMonitor?.Resume();
        }

        void OnAdapterReadingPacketStarted()
        {
            _keepAliveMonitor?.Pause();
        }
    }
}
