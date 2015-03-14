using System;
using System.Collections.Generic;
using System.Text;
using AutoServices;
using System.IO;

namespace AutoTest
{
    public class ManagerServices : ServicesBase
    {
        protected override bool RunContext()
        {
            TimeSpan ts = DateTime.Now.Subtract(LastAliveTime);
            if (ts.TotalSeconds >= 5) 
            {
                File.AppendAllText("D:\\log.txt", "ManagerServices£º" + DateTime.Now + "\r\n");
                return true;
            }
            return base.RunContext();
        }
    }
    
}

namespace AutoTest
{
    public partial class AutoServicesInfo
    {
        public ManagerServices LManagerServices
        {
            get 
            {
                return new ManagerServices();
            }
        }
    }
}