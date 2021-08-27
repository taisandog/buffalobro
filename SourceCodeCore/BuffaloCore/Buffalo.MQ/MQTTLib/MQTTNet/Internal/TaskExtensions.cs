using Buffalo.MQ.MQTTLib.MQTTnet.Diagnostics;
using System.Threading.Tasks;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Internal
{
    public static class TaskExtensions
    {
        public static void Forget(this Task task, IMqttNetScopedLogger logger)
        {
            task?.ContinueWith(t =>
                {
                    logger.Error(t.Exception, "Unhandled exception.");
                },
                TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
