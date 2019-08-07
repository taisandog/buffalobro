using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.IO;

namespace Buffalo.Win32
{
    public class HardwareInfo
    {
        /// <summary>
        /// 获取CPUID
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCPUID()
        {
            List<string> lstID = new List<string>();
            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        lstID.Add(ObjectToString(mo.Properties["ProcessorId"]));
                    }
                }
            }
            return lstID;
        }

        /// <summary>
        /// object转成字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string ObjectToString(object obj) 
        {
            if (obj == null) 
            {
                return null;
            }
           
            return obj.ToString().Trim();
        }

        /// <summary>
        /// 获取主板ID
        /// </summary>
        /// <returns></returns>
        public static string GetBaseBoardID()
        {
            using (ManagementClass mc = new ManagementClass("Win32_BaseBoard"))
            {

                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    
                    foreach (ManagementObject mo in moc)
                    {
                        return ObjectToString(mo.Properties["SerialNumber"].Value);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHardDiskSerialNumber()
        {
            List<string> lstID = new List<string>();
            using (ManagementClass mc = new ManagementClass("Win32_PhysicalMedia"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        lstID.Add(ObjectToString(mo.Properties["SerialNumber"].Value));
                    }
                }
            }
            return lstID;
        }

        /// <summary>
        /// 获取主盘序列号
        /// </summary>
        /// <returns></returns>
        public static string GetPrimaryDriverNumber() 
        {
            List<string> lstID = new List<string>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia"))
            {
                foreach(ManagementObject mo in searcher.Get())
                {
                    return ObjectToString(mo["SerialNumber"]);       
                }
            }
            return null;
        } 

        /// <summary>
        /// 获取硬盘型号ID
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHardDiskID()
        {
            List<string> lstID = new List<string>();
            using (ManagementClass mc = new ManagementClass("Win32_DiskDrive"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        lstID.Add(ObjectToString(mo.Properties["Model"].Value));
                    }
                }
            }
            return lstID;
        }
        /// <summary>
        /// 获取机器名
        /// </summary>
        /// <returns></returns>
        public string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }

        /// <summary>
        /// 获取BIOS编号
        /// </summary>
        /// <returns></returns>
        public static string GetBIOSNumber()
        {
            List<string> lstID = new List<string>();
            using (ManagementClass mc = new ManagementClass("Win32_BIOS"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        return ObjectToString(mo.Properties["SerialNumber"].Value);
                    }
                }
            }
            return null;
        }
    }
}
