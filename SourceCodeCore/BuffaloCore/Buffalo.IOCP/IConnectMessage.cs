using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.IOCP
{
    public interface IConnectMessage
    {
        void Log(string message);
        void LogError(string message);
        void LogWarning(string message);

        bool ShowLog { get; }
        bool ShowError { get; }
        bool ShowWarning { get; }

    }
}
