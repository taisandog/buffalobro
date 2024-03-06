using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.IOCP.DataProtocol
{
    /// <summary>
    /// 连接的证书配置
    /// </summary>
    public class SocketCertConfig
    {

        /// <summary>
        /// 作为服务端证书
        /// </summary>
        private X509Certificate _serverCert;
        /// <summary>
        /// 作为服务端证书
        /// </summary>
        public X509Certificate ServerCertificate
        {
            get { return _serverCert; }
        }
        /// <summary>
        /// 客户端需要证书
        /// </summary>
        private bool _clientCertificateRequired;

        /// <summary>
        /// 客户端需要证书
        /// </summary>
        public bool ClientCertificateRequired 
        {
            get 
            {
                return _clientCertificateRequired;
            }
        }

        /// <summary>
        /// SSL类型
        /// </summary>
        private SslProtocols _enabledSslProtocols;
        /// <summary>
        /// SSL类型
        /// </summary>
        public SslProtocols SslProtocols
        {
            get { return _enabledSslProtocols; }
        }
        private bool _checkCertificateRevocation;
        /// <summary>
        /// 检查证书吊销
        /// </summary>
        public bool CheckCertificateRevocation
        {
            get { return _checkCertificateRevocation; }
        }
        private bool _leaveInnerStreamOpen;
        /// <summary>
        /// 保持流打开
        /// </summary>
        public bool LeaveInnerStreamOpen
        {
            get { return _leaveInnerStreamOpen; }
        }
        private RemoteCertificateValidationCallback _userCertificateValidationCallback;
        /// <summary>
        /// 用户证书验证回调
        /// </summary>
        public RemoteCertificateValidationCallback UserCertificateValidationCallback
        {
            get { return _userCertificateValidationCallback; }
        }
        private LocalCertificateSelectionCallback _userCertificateSelectionCallback;
        /// <summary>
        /// 用户证书选择回调
        /// </summary>
        public LocalCertificateSelectionCallback UserCertificateSelectionCallback
        {
            get { return _userCertificateSelectionCallback; }
        }
        private EncryptionPolicy _encryptionPolicy;
        /// <summary>
        /// 加密策略
        /// </summary>
        public EncryptionPolicy EncryptionPolicy
        {
            get { return _encryptionPolicy; }
        }
        private string _targetHost;
        /// <summary>
        /// 目标机器
        /// </summary>
        public string TargetHost 
        {
            get { return _targetHost; }
        }
        private X509CertificateCollection _clientCertificates;
        /// <summary>
        /// 作为客户端证书
        /// </summary>
        public X509CertificateCollection ClientCertificates
        {
            get { return _clientCertificates; }
        }

        private SocketCertConfig() { }

        /// <summary>
        /// 创建Socket证书配置(服务端)
        /// </summary>
        /// <param name="serverCert">服务端证书</param>
        /// <param name="clientCertificateRequired">客户端需要证书</param>
        /// <param name="enabledSslProtocols">SSL类型</param>
        /// <param name="checkCertificateRevocation">检查证书吊销</param>
        /// <param name="leaveInnerStreamOpen">保持流打开</param>
        /// <param name="userCertificateValidationCallback">用户证书验证回调</param>
        /// <param name="userCertificateSelectionCallback">用户证书选择回调</param>
        /// <param name="encryptionPolicy">加密策略</param>
        public static SocketCertConfig CreateServerConfig(X509Certificate serverCert, bool clientCertificateRequired,
            SslProtocols enabledSslProtocols, bool checkCertificateRevocation = false,
             bool leaveInnerStreamOpen = false,
            RemoteCertificateValidationCallback userCertificateValidationCallback = null,
            LocalCertificateSelectionCallback userCertificateSelectionCallback = null,
            EncryptionPolicy encryptionPolicy = EncryptionPolicy.NoEncryption)
        {
            SocketCertConfig config=new SocketCertConfig();
            config._serverCert = serverCert;

            config._clientCertificateRequired = clientCertificateRequired;
            config._checkCertificateRevocation = checkCertificateRevocation;
            config._enabledSslProtocols = enabledSslProtocols;
            config._leaveInnerStreamOpen = leaveInnerStreamOpen;
            if (userCertificateValidationCallback != null)
            {
                config._userCertificateValidationCallback = userCertificateValidationCallback;
            }
            else 
            {
                config._userCertificateValidationCallback = DefaultServerRemoteCertificateValidation;
            }
            config._userCertificateSelectionCallback = userCertificateSelectionCallback;
            config._encryptionPolicy = encryptionPolicy;
            return config;
        }

        /// <summary>
        /// 默认的服务器验证方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool DefaultServerRemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 创建Socket证书配置(客户端)
        /// </summary>
        /// <param name="targetHost">地址</param>
        /// <param name="clientCertificates">客户端需要证书</param>
        /// <param name="enabledSslProtocols">SSL类型</param>
        /// <param name="checkCertificateRevocation">检查证书吊销</param>
        /// <param name="leaveInnerStreamOpen">保持流打开</param>
        /// <param name="userCertificateValidationCallback">用户证书验证回调</param>
        /// <param name="userCertificateSelectionCallback">用户证书选择回调</param>
        /// <param name="encryptionPolicy">加密策略</param>
        public static SocketCertConfig CreateClientConfig(string targetHost, X509CertificateCollection clientCertificates,
            SslProtocols enabledSslProtocols, bool checkCertificateRevocation = false,
             bool leaveInnerStreamOpen = false,
            RemoteCertificateValidationCallback userCertificateValidationCallback = null,
            LocalCertificateSelectionCallback userCertificateSelectionCallback = null,
            EncryptionPolicy encryptionPolicy = EncryptionPolicy.AllowNoEncryption)
        {
            SocketCertConfig config = new SocketCertConfig();
            config._targetHost = targetHost;
            if (clientCertificates != null)
            {
                config._clientCertificates = clientCertificates;
            }
            else
            {
                config._clientCertificates = new X509CertificateCollection();
            }
            config._checkCertificateRevocation = checkCertificateRevocation;
            config._enabledSslProtocols = enabledSslProtocols;

            config._leaveInnerStreamOpen = leaveInnerStreamOpen;
            if (userCertificateValidationCallback != null)
            {
                config._userCertificateValidationCallback = userCertificateValidationCallback;
            }
            else
            {
                config._userCertificateValidationCallback = DefaultClientRemoteCertificateValidation;
            }
            config._userCertificateSelectionCallback = userCertificateSelectionCallback;
            config._encryptionPolicy = encryptionPolicy;
            return config;
        }

        /// <summary>
        /// 创建SslStream
        /// </summary>
        /// <param name="stream">内存Stream</param>
        /// <param name="isServerSocket">是否服务器</param>
        /// <returns></returns>
        public SslStream CreateStream(Stream stream,bool isServerSocket) 
        {
            SslStream sslStream = new SslStream(stream, _leaveInnerStreamOpen, _userCertificateValidationCallback,
                _userCertificateSelectionCallback, _encryptionPolicy);
            sslStream.ReadTimeout = 5000;
            sslStream.WriteTimeout = 5000;
            if (isServerSocket)
            {
                sslStream.AuthenticateAsServer(_serverCert, _clientCertificateRequired,
                    _enabledSslProtocols, _checkCertificateRevocation);
            }
            else
            {

                sslStream.AuthenticateAsClient(_targetHost, _clientCertificates, _enabledSslProtocols, _checkCertificateRevocation);

            }
            
            return sslStream;
        }

        private static bool DefaultClientRemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            return false;

        }
    }
}
