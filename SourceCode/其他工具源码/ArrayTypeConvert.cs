using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ��������ת��(�޸����ͱ�־���ǿ���)
    /// </summary>
    public class ArrayTypeConvert
    {
        /// <summary>
        /// ��������ת�����ֽ�����
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
        /// �ֽ�����ת������������
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
        /// �ֽ�����ת����
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
        /// �ַ�����ת������������
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
        /// ת����������
        /// </summary>
        /// <param name="objType">������</param>
        /// <param name="pbarr">ԭ����ָ��</param>
        /// <param name="newLength">�����鳤��</param>
        public static unsafe void ChangeArrayTypeHandle(Type objType, byte* pbarr, int newLength) 
        {
            int pointerLen = IntPtr.Size;//ָ�볤�ȣ�X86������4��X64������8
            byte* phandle = (byte*)objType.TypeHandle.Value.ToPointer();//ָ�����;����ָ��

            phandle = phandle + pointerLen - 2;//����λ��

            
            for (int i = 0; i < pointerLen; i++)
            {
                *(pbarr - pointerLen * 2 + i) = phandle[i];
            }
            *((int*)(pbarr - pointerLen)) = newLength;
            
        }


    }
}


/*----------ʾ��-------------

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