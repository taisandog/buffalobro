using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.Win32;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 快速类型转换(修改类型标志，非拷贝)
    /// </summary>
    public class ArrayTypeConvert
    {
        /// <summary>
        /// 整型数组转换成字节数组
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static unsafe byte[] IntArrayToByteArray(int[] arr)
        {
            Type objType = typeof(byte[]);
            object ret = null;
            
            fixed (int* parr = arr)
            {
                byte* pbarr = (byte*)parr;
                ChangeArrayTypeHandle(objType, pbarr, arr.Length * 4);
                ret = arr;
            }
            return (byte[])ret;
        }

        /// <summary>
        /// 字节数组转换成整型数组
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static unsafe int[] ByteArrayToIntArray(byte[] arr)
        {
            Type objType = typeof(int[]);
            object ret = null;

            fixed (byte* parr = arr)
            {
                ChangeArrayTypeHandle(objType, parr, arr.Length /4);
                ret = arr;
            }
            return (int[])ret;
        }
        /// <summary>
        /// 字节数组转换成
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static unsafe char[] ByteArrayToCharArray(byte[] arr)
        {
            Type objType = typeof(char[]);
            object ret = null;

            fixed (byte* parr = arr)
            {
                ChangeArrayTypeHandle(objType, parr, arr.Length / 2);
                ret = arr;
            }
            return (char[])ret;
        }
        /// <summary>
        /// 字符数组转换成整型数组
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static unsafe byte[] CharArrayToByteArray(char[] arr)
        {
            Type objType = typeof(byte[]);
            object ret = null;

            fixed (char* parr = arr)
            {
                byte* pbarr = (byte*)parr;
                ChangeArrayTypeHandle(objType, pbarr, arr.Length * 2);
                ret = arr;
            }
            return (byte[])ret;
        }
        /// <summary>
        /// 转换数组类型
        /// </summary>
        /// <param name="objType">新类型</param>
        /// <param name="pbarr">原类型指针</param>
        /// <param name="newLength">新数组长度</param>
        public static unsafe void ChangeArrayTypeHandle(Type objType, byte* pbarr, int newLength) 
        {
            int pointerLen = IntPtr.Size;//指针长度，X86程序是4，X64程序是8
            byte* phandle = (byte*)objType.TypeHandle.Value.ToPointer();//指向类型句柄的指针

            phandle = phandle + pointerLen - 2;//修正位置

            
            for (int i = 0; i < pointerLen; i++)
            {
                *(pbarr - pointerLen * 2 + i) = phandle[i];
            }
            *((int*)(pbarr - pointerLen)) = newLength;
            
        }


    }
}


/*----------示例-------------

int[] arrInt ={ 0x003456AE, 0x00456789, 0x45678912, 0x78561E23, 0x0A345678};

            foreach (int v in arrInt)
            {
                richTextBox1.AppendText(v.ToString("X") + "\n");
            }

            byte[] arr = ArrayTypeConvert.IntArrayToByteArray(arrInt);

            //arrInt[0] = 0x0FFFFFFF;
            
            richTextBox1.AppendText("-----\n");
            foreach (byte v in arr)
            {
                richTextBox1.AppendText(v.ToString("X") + "\n");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] arrByte ={ 0x23, 0xEF, 0x45, 0x6E, 0x24, 0x8F, 0x46, 0x6F };
            int[] arr = ArrayTypeConvert.ByteArrayToIntArray(arrByte);
            richTextBox1.AppendText("-----\n");
            foreach (int v in arr)
            {
                richTextBox1.AppendText(v.ToString("X") + "\n");
            }

            arrByte[1] = 0x36;
            richTextBox1.AppendText("-----\n");
            foreach (int v in arr)
            {
                richTextBox1.AppendText(v.ToString("X") + "\n");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            char[] c ={ 'a','b' };
            richTextBox1.Clear();
            byte[] arrByte ={  0x41, 0x00, 0x42, 0x00, 0x43, 0x00, 0x44,
                0x00, 0x45, 0x00, 0x46, 0x00, 0x47, 0x00, 0x48,0x00, 0x00, 0x00 };
            char[] str = ArrayTypeConvert.ByteArrayToCharArray(arrByte);
            richTextBox1.AppendText(new string(str));
        }
-----------------------------*/