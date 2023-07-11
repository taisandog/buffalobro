
using Buffalo.Kernel;
using MQTTnet.Client;
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

            string name= _configs.GetDicValue<string, string>("uid");
            string pwd = _configs.GetDicValue<string, string>("pwd");
            if (!string.IsNullOrWhiteSpace(name))
            {
                Options.WithCredentials(name, pwd);
            }
            string clientId= _configs.GetDicValue<string, string>("clientId");
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                Options.WithClientId(clientId);
            }
            else 
            {
                Options.WithClientId(CommonMethods.GuidToString(Guid.NewGuid(),true));
            }
            string webSocketServer = _configs.GetDicValue<string, string>("webSocketServer");
            if (!string.IsNullOrWhiteSpace(webSocketServer))
            {
                Options.WithWebSocketServer(webSocketServer);
            }
            string sessionExpiry = _configs.GetDicValue<string, string>("sessionExpiry");//超时，秒数
            if (!string.IsNullOrWhiteSpace(sessionExpiry))
            {
                Options.WithSessionExpiryInterval(sessionExpiry.ConvertTo<uint>());
            }
            string keepAlivePeriod = _configs.GetDicValue<string, string>("keepAlivePeriod");//(秒)当超过设置的时间间隔必须回复PONG报文，否则服务器认定为掉线。默认120秒
            if (!string.IsNullOrWhiteSpace(keepAlivePeriod))
            {
                Options.WithKeepAlivePeriod(TimeSpan.FromSeconds(keepAlivePeriod.ConvertTo<long>()));
            }
            string keepAlive = _configs.GetDicValue<string, string>("keepAlive");//(秒)用于保持连接的心跳时间的发送间隔
            if (keepAlive=="0")
            {
                Options.WithNoKeepAlive();
            }

            //Options.WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311);

            string proxy = _configs.GetDicValue<string, string>("proxy");//代理地址
            string proxyUserName = _configs.GetDicValue<string, string>("proxyUserName");//代理用户
            string proxyPassword = _configs.GetDicValue<string, string>("proxyPassword");//代理用户密码
            string domain = _configs.GetDicValue<string, string>("domain");//代理domain
            if (!string.IsNullOrWhiteSpace(proxy))
            {
                Options.WithProxy(proxy, proxyUserName, proxyPassword, domain);
            }

            string qualityOfServiceLevel= _configs.GetDicValue<string, string>("QualityOfServiceLevel");
            if (!string.IsNullOrWhiteSpace(qualityOfServiceLevel))
            {
                QualityOfServiceLevel = (MqttQualityOfServiceLevel)qualityOfServiceLevel.ConvertTo<int>();
            }


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
