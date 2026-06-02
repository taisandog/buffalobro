
using Buffalo.Kernel;
using MQTTnet;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib
{
    public class MQTTConfig : MQConfigBase
    {
        public readonly MqttClientOptionsBuilder Options;

        public MqttQualityOfServiceLevel QualityOfServiceLevel= MqttQualityOfServiceLevel.AtMostOnce;
        /// <summary>
        /// This is only supported when using MQTTv5.
        /// </summary>
        public bool? RetainAsPublished = null;
        /// <summary>
        /// This is only supported when using MQTTv5.
        /// </summary>
        public MqttRetainHandling? RetainHandling;
        /// <summary>
        /// This is only supported when using MQTTv5.
        /// </summary>
        public bool? NoLocal;

        public MqttProtocolVersion ProtocolVersion;
        public MQTTConfig(string connString) : base(connString)
        {
            Options = new MqttClientOptionsBuilder();
            string server = _configs.GetDicValue<string, string>("server");
            if (!string.IsNullOrWhiteSpace(server))
            {
                string[] serPart = server.Split(':');
                string strserver = serPart[0];

                if (serPart.Length > 1)
                {
                    Options.WithTcpServer(strserver, serPart[1].ConvertTo<int>());
                    
                }
                else
                {
                    Options.WithTcpServer(strserver);
                }
            }

            string name = _configs.GetDicValue<string, string>("uid");
            string pwd = _configs.GetDicValue<string, string>("pwd");
            if (!string.IsNullOrWhiteSpace(name))
            {
                Options.WithCredentials(name, pwd);
            }
            string clientId = _configs.GetDicValue<string, string>("clientId");
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                Options.WithClientId(clientId);
            }
            else
            {
                Options.WithClientId(CommonMethods.GuidToString(Guid.NewGuid(), true));
            }
            string webSocketServer = _configs.GetDicValue<string, string>("webSocketServer");
            string proxy = _configs.GetDicValue<string, string>("proxy");//代理地址
            string proxyUserName = _configs.GetDicValue<string, string>("proxyUserName");//代理用户
            string proxyPassword = _configs.GetDicValue<string, string>("proxyPassword");//代理用户密码
            string domain = _configs.GetDicValue<string, string>("domain");//代理domain

            if (!string.IsNullOrWhiteSpace(webSocketServer))
            {
                Options.WithWebSocketServer(ws =>
                {
                    ws.WithUri(webSocketServer);
                    if (!string.IsNullOrWhiteSpace(proxy))
                    {
                        ws.WithProxyOptions(p => p
                            .WithAddress(proxy)
                            .WithUsername(proxyUserName)
                            .WithPassword(proxyPassword)
                            .WithDomain(domain));
                    }

                });
            }
            else if (!string.IsNullOrWhiteSpace(proxy))
            {
                // v5 中纯 TCP 连接不再支持 WithProxy，proxy 仅适用于 WebSocket
                // 如果原来就是 TCP+proxy 的场景，只能用 v4
                throw new NotSupportedException("MQTTnet v5 不支持 TCP 连接的 Proxy，请用 WebSocket 或降级到 v4");
            }
            string sessionExpiry = _configs.GetDicValue<string, string>("sessionExpiry");//超时，秒数
            if (!string.IsNullOrWhiteSpace(sessionExpiry))
            {
                Options.WithSessionExpiryInterval(sessionExpiry.ConvertTo<uint>());
            }
            
            string keepAlive = _configs.GetDicValue<string, string>("keepAlive");//(秒)用于保持连接的心跳时间的发送间隔
            if (keepAlive == "0")
            {
                Options.WithNoKeepAlive();
            }
            
            string keepAlivePeriod = _configs.GetDicValue<string, string>("keepAlivePeriod");//(秒)当超过设置的时间间隔必须回复PONG报文，否则服务器认定为掉线。默认120秒
            if (!string.IsNullOrWhiteSpace(keepAlivePeriod))
            {
                Options.WithKeepAlivePeriod(TimeSpan.FromSeconds(keepAlivePeriod.ConvertTo<long>()));
            }


           
          

            string qualityOfServiceLevel = _configs.GetDicValue<string, string>("QualityOfServiceLevel");
            if (!string.IsNullOrWhiteSpace(qualityOfServiceLevel))
            {
                QualityOfServiceLevel = (MqttQualityOfServiceLevel)qualityOfServiceLevel.ConvertTo<int>();
            }

            ProtocolVersion = MqttProtocolVersion.V311;
            string protocolVersion = _configs.GetDicValue<string, string>("ProtocolVersion");
            if (!string.IsNullOrWhiteSpace(protocolVersion))
            {
                ProtocolVersion = (MqttProtocolVersion)protocolVersion.ConvertTo<int>();
                Options.WithProtocolVersion(ProtocolVersion); ;
            }

            string cleanSession = _configs.GetDicValue<string, string>("CleanSession");//(秒)用于保持连接的心跳时间的发送间隔
            
            

            if (ProtocolVersion == MqttProtocolVersion.V500)
            {
                string retainAsPublished = _configs.GetDicValue<string, string>("RetainAsPublished");
                if (!string.IsNullOrWhiteSpace(retainAsPublished))
                {
                    RetainAsPublished = retainAsPublished == "1";
                }

                string retainHandling = _configs.GetDicValue<string, string>("RetainHandling");
                if (!string.IsNullOrWhiteSpace(retainHandling))
                {
                    RetainHandling = (MqttRetainHandling)retainHandling.ConvertTo<int>();
                }

                string noLocal = _configs.GetDicValue<string, string>("NoLocal");
                if (!string.IsNullOrWhiteSpace(noLocal))
                {
                    NoLocal = noLocal == "1";
                }
                if (cleanSession != "0")
                {
                    Options.WithCleanStart();
                    uint sessionExpiryInterval = _configs.GetDicValue<string, string>("SessionExpiryInterval").ConvertTo<uint>();
                    if(sessionExpiryInterval > 0) 
                    {
                        Options.WithSessionExpiryInterval(sessionExpiryInterval);
                    }
                }
                
            }
            else 
            {
                if (cleanSession != "0")
                {
                    Options.WithCleanSession(true);
                }
                else 
                {
                    Options.WithCleanSession(false);
                }
               
            }
            
        }


        public override MQConnection CreateConnection()
        {
            return new MQTTConnection(this);
        }

        public override MQListener CreateListener()
        {
            return new MQTTListener(this);
        }
    }

}
