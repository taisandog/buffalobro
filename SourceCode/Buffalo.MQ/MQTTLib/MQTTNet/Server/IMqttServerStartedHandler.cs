using System;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerStartedHandler
    {
        Task HandleServerStartedAsync(EventArgs eventArgs);
    }
}
