using System;
using System.Collections.Generic;
using System.Text;
using AutoServices;
using System.IO;

namespace AutoTest
{
    public class EmployeeServices : ServicesBase
    {
        protected override bool RunContext()
        {
            TimeSpan ts = DateTime.Now.Subtract(LastAliveTime);
            if (ts.TotalSeconds >= 10) 
            {
                File.AppendAllText("D:\\log.txt","EmployeeServices£º"+DateTime.Now+"\r\n");
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
        public EmployeeServices LEmployeeServices 
        {
            get 
            {
                return new EmployeeServices();
            }
        }
    }
}