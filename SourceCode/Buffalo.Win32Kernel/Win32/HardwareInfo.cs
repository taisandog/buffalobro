using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Kernel.Win32;

namespace Buffalo.Win32Kernel.Win32
{
        /// <summary>
        /// 得到硬盘与CPU的ID及网卡MAC
        /// </summary>
    public class HardwareInfo
    {
        /// <summary>
        /// 取机器名 
        /// </summary>
        /// <returns></returns>
        public string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }

        /// <summary>
        /// 取CPU编号
        /// </summary>
        /// <returns></returns>
        public static String GetCpuID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return null;
            }

        }//end method

        /// <summary>
        /// 取第一块硬盘编号
        /// </summary>
        /// <returns></returns>
        public static String GetHardDiskID()
        {
            try
            {
                String strHardDiskID = null;
                ManagementClass cimobject = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = cimobject.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    strHardDiskID = (string)mo.Properties["Model"].Value;
                    break;
                }
                return strHardDiskID;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到网卡MAC
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            string addr = "";

            int cb;
            ASTAT adapter;
            NCB Ncb = new NCB();
            char uRetCode;
            LANA_ENUM lenum;

            Ncb.ncb_command = (byte)NCBCONST.NCBENUM;
            cb = Marshal.SizeOf(typeof(LANA_ENUM));
            Ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
            Ncb.ncb_length = (ushort)cb;
            uRetCode = WindowsAPI.Netbios(ref Ncb);
            lenum = (LANA_ENUM)Marshal.PtrToStructure(Ncb.ncb_buffer, typeof(LANA_ENUM));
            Marshal.FreeHGlobal(Ncb.ncb_buffer);
            if (uRetCode != (short)NCBCONST.NRC_GOODRET)
                return "";

            for (int i = 0; i < lenum.length; i++)
            {
                Ncb.ncb_command = (byte)NCBCONST.NCBRESET;
                Ncb.ncb_lana_num = lenum.lana[i];
                uRetCode = WindowsAPI.Netbios(ref Ncb);
                if (uRetCode != (short)NCBCONST.NRC_GOODRET)
                    return "";

                Ncb.ncb_command = (byte)NCBCONST.NCBASTAT;
                Ncb.ncb_lana_num = lenum.lana[i];
                Ncb.ncb_callname[0] = (byte)'*';
                cb = Marshal.SizeOf(typeof(ADAPTER_STATUS)) + Marshal.SizeOf(typeof(NAME_BUFFER)) * (int)NCBCONST.NUM_NAMEBUF;
                Ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
                Ncb.ncb_length = (ushort)cb;
                uRetCode = WindowsAPI.Netbios(ref Ncb);
                adapter.adapt = (ADAPTER_STATUS)Marshal.PtrToStructure(Ncb.ncb_buffer, typeof(ADAPTER_STATUS));
                Marshal.FreeHGlobal(Ncb.ncb_buffer);

                if (uRetCode == (short)NCBCONST.NRC_GOODRET)
                {
                    if (i > 0)
                        addr += ":";
                    addr = string.Format("{0,2:X}{1,2:X}{2,2:X}{3,2:X}{4,2:X}{5,2:X}",
                     adapter.adapt.adapter_address[0],
                     adapter.adapt.adapter_address[1],
                     adapter.adapt.adapter_address[2],
                     adapter.adapt.adapter_address[3],
                     adapter.adapt.adapter_address[4],
                     adapter.adapt.adapter_address[5]);
                }

            }

            return addr.Replace(' ', '0');
        }

        /// <summary>
        /// 根据IP获取MAC
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetMacByIP(string ip)
        {
            Int32 ldest = WindowsAPI.inet_addr(ip); //目的地的ip 
            Int32 lhost = WindowsAPI.inet_addr("");    //本地服务器的ip 
            Int64 macinfo = new Int64();
            Int32 len = 6;
            int res = WindowsAPI.SendARP(ldest, 0, ref macinfo, ref len);
            string mac_src = macinfo.ToString("X");
            while (mac_src.Length < 12)
            {
                mac_src = mac_src.Insert(0, "0");
            }
            string mac_dest = "";
            for (int i = 0; i < 11; i++)
            {
                if (0 == (i % 2))
                {
                    if (i == 10)
                    {
                        mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                    }
                    else
                    {
                        mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                    }
                }
            }

            return mac_dest;
        }
    }
}
