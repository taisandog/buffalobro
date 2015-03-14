using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using Buffalo.Kernel.Win32;

namespace Buffalo.Win32Kernel.Win32
{
    /**
     原作者：http://www.cnblogs.com/geyunfei/archive/2009/03/17/1414659.html
     */


    /// <summary>
    /// 并口控制类
    /// </summary>
    public class ParallelPort:IDisposable
    {
        private IntPtr iHandle;

        private bool _IsOpen;
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return _IsOpen;
            }
            private set
            {
                _IsOpen = value;
            }
        }

        private string _Name;
        /// <summary>
        /// 并口名
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public bool WriteData(byte[] data)
        {
            //for (int i = 0; i < Data.Length; i++)
            //    Output(BasePort, Data[i]); 这里原来也想用inpout32实现，但是从字节到int转换比较麻烦，试了几次没达到效果
            //return true;

            if (iHandle != IntPtr.Zero)
            {
                OVERLAPPED x = new OVERLAPPED();
                int i = 0;
                WindowsAPI.WriteFile(iHandle, data, data.Length,
                  ref   i, ref   x);
                return true;
            }
            else
            {
                //throw new Exception("不能连接到打印机! ");
                return false;
            }
        }
        /// <summary>
        /// 读状态
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public byte[] ReadData(int len)
        {
            byte[] result;//= new byte[Len];
            result = new byte[len];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)WindowsAPI.Input(BasePort + 1);
            }
            return result;
        }


        /// <summary>
        /// 打开端口
        /// </summary>
        public void Open()
        {
            iHandle = WindowsAPI.CreateFile(Name, GENERICFileAccess.GENERIC_WRITE, 0, 0, 3, 0, 0);
            if (iHandle != IntPtr.Zero)
            {
                this.IsOpen = true;
            }
            else
            {
                this.IsOpen = false;
            }
        }


        /// <summary>
        /// 关闭端口
        /// </summary>
        public void Close()
        {
            if (IsOpen && iHandle!=IntPtr.Zero)
            {
                WindowsAPI.CloseHandle(iHandle);
                this.IsOpen =false;
            }
        }

        private uint _basePort;

        /// <summary>
        /// 端口基址
        /// </summary>
        private uint BasePort 
        {
            get 
            {
                return _basePort;
            }
            set 
            {
                _basePort = value;
            }
        }

        /// <summary>
        /// 并口通信
        /// </summary>
        /// <param name="portName"></param>
        public ParallelPort(String portName)
        {
            this.Name = portName;
            iHandle = IntPtr.Zero;

            ///用wql查询串口基址
            ManagementObjectSearcher search2 =
                    new ManagementObjectSearcher(
                        "ASSOCIATORS OF {Win32_ParallelPort.DeviceID='" + this.Name + "'}");
            //本来最佳的wql是ASSOCIATORS OF {Win32_ParallelPort.DeviceID='" + this.Name  + "'} WHERE ASSCICLASS = Win32_PortResource
            //但是不知道为什么不返回结果，所以就做一个简单的遍历吧
            foreach (ManagementBaseObject i in search2.Get())
            {

                if (i.ClassPath.ClassName == "Win32_PortResource")
                {
                    //得到串口基址 大多数是0x378H
                    this.BasePort = System.Convert.ToUInt32(i.Properties["StartingAddress"].Value.ToString());

                    break;
                }
            }
            if (BasePort == 0)
                throw new Exception("不是有效端口");
            IsOpen = false;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
