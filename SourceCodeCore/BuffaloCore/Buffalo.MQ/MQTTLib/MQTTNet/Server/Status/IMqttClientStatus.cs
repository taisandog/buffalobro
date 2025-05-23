﻿using Buffalo.MQ.MQTTLib.MQTTnet.Formatter;
using System;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server.Status
{
    public interface IMqttClientStatus
    {
        string ClientId { get; }

        string Endpoint { get; }

        MqttProtocolVersion ProtocolVersion { get; }

        DateTime LastPacketReceivedTimestamp { get; }

        DateTime LastNonKeepAlivePacketReceivedTimestamp { get; }

        long ReceivedApplicationMessagesCount { get; }

        long SentApplicationMessagesCount { get; }

        long ReceivedPacketsCount { get; }

        long SentPacketsCount { get; }

        IMqttSessionStatus Session { get; }

        long BytesSent { get; }

        long BytesReceived { get; }

        Task DisconnectAsync();

        void ResetStatistics();
    }
}
