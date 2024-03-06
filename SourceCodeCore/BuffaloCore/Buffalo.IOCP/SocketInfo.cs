using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoxIOCP.Sources
{
    /// <summary>
    /// Socket信息
    /// </summary>
    public class SocketInfo
    {
        private string _ip;

        
        private int _port;

        private DateTime _lastSend;

        private DateTime _lastReceive;
    }
}
