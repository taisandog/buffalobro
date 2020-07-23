using System;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Server
{
    public interface IMqttServerStoppedHandler
    {
        Task HandleServerStoppedAsync(EventArgs eventArgs);
    }
}
