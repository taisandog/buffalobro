
using Buffalo.IOCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class ServerMessageLog : IConnectMessage
    {


        public ServerMessageLog() 
        {
            
        }

        private bool _showLog=false;

        public bool ShowLog
        {
            get
            {
                return _showLog;
            }
            set 
            {
                _showLog = value;
            }
        }
        private bool _showError=false;
        public bool ShowError
        {
            get
            {
                return _showError;
            }
            set 
            {
                _showError = value;
            }
        }
        public bool _showWarning=false;
        public bool ShowWarning
        {
            get
            {
                return _showWarning;
            }
            set 
            {
                _showWarning = value;
            }
        }

        public void Log(string message)
        {
            if (_showLog)
            {
                Console.WriteLine(message);
            }
        }

        

        public void LogError(string message)
        {
            Console.WriteLine("Error:"+message);
            
        }

        

        public void LogWarning(string message)
        {
            if (_showError)
            {
                Console.WriteLine(message);
            }
            
        }

       
    }

