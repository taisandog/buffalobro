using System.Security.Cryptography.X509Certificates;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Certificates
{
    public interface ICertificateProvider
    {
        X509Certificate2 GetCertificate();
    }
}
