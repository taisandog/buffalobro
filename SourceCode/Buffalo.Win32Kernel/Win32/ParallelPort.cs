using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace Buffalo.Win32Kernel.Win32
{
    /**
     ԭ���ߣ�http://www.cnblogs.com/geyunfei/archive/2009/03/17/1414659.html
     */


    /// <summary>
    /// ���ڿ�����
    /// </summary>
    public class ParallelPort:IDisposable
    {
        private IntPtr iHandle;

        private bool _IsOpen;
        /// <summary>
        /// �Ƿ���
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
        /// ������
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
        /// д������
        /// </summary>
        /// <param name="data">����</param>
        /// <returns></returns>
        public bool WriteData(byte[] data)
        {
            //for (int i = 0; i < Data.Length; i++)
            //    Output(BasePort, Data[i]); ����ԭ��Ҳ����inpout32ʵ�֣����Ǵ��ֽڵ�intת���Ƚ��鷳�����˼���û�ﵽЧ��
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
                //throw new Exception("�������ӵ���ӡ��! ");
                return false;
            }
        }
        /// <summary>
        /// ��״̬
        /// </summary>
        /// <param name="len">����</param>
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
        /// �򿪶˿�
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
        /// �رն˿�
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
        /// �˿ڻ�ַ
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
        /// ����ͨ��
        /// </summary>
        /// <param name="portName"></param>
        public ParallelPort(String portName)
        {
            this.Name = portName;
            iHandle = IntPtr.Zero;

            ///��wql��ѯ���ڻ�ַ
            ManagementObjectSearcher search2 =
                    new ManagementObjectSearcher(
                        "ASSOCIATORS OF {Win32_ParallelPort.DeviceID='" + this.Name + "'}");
            //������ѵ�wql��ASSOCIATORS OF {Win32_ParallelPort.DeviceID='" + this.Name  + "'} WHERE ASSCICLASS = Win32_PortResource
            //���ǲ�֪��Ϊʲô�����ؽ�������Ծ���һ���򵥵ı�����
            foreach (ManagementBaseObject i in search2.Get())
            {

                if (i.ClassPath.ClassName == "Win32_PortResource")
                {
                    //�õ����ڻ�ַ �������0x378H
                    this.BasePort = System.Convert.ToUInt32(i.Properties["StartingAddress"].Value.ToString());

                    break;
                }
            }
            if (BasePort == 0)
                throw new Exception("������Ч�˿�");
            IsOpen = false;
        }

        #region IDisposable ��Ա

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
