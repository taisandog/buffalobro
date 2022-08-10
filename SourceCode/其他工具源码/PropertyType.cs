using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Commons
{
    public class PropertyType
    {
        
        private byte[] data;
        private int numberLen;

        #region ����ֵ����
        public byte[] Data
        {
            get { return data; }
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        public int IntValue
        {
            get { return BitConverter.ToInt32(data, 0); }
        }
        
        /// <summary>
        /// �ַ���
        /// </summary>
        public char CharValue
        {
            get { return BitConverter.ToChar(data, 0); }
        }

        /// <summary>
        /// ˫������
        /// </summary>
        public double DoubleValue
        {
            get { return BitConverter.ToDouble(data, 0); }
        }

        /// <summary>
        /// ������
        /// </summary>
        public short ShortValue
        {
            get { return BitConverter.ToInt16(data, 0); }
        }

        /// <summary>
        /// ������
        /// </summary>
        public long LongValue
        {
            get { return BitConverter.ToInt64(data, 0); }
        }

        /// <summary>
        /// ������
        /// </summary>
        public float FloatValue
        {
            get { return BitConverter.ToSingle(data, 0); }
        }

        /// <summary>
        /// �޷��ų�����
        /// </summary>
        public ulong ULongValue
        {
            get { return BitConverter.ToUInt64(data, 0); }
        }

        /// <summary>
        /// �޷�������ֵ
        /// </summary>
        public uint UIntValue
        {
            get { return BitConverter.ToUInt32(data, 0); }
        }

        /// <summary>
        /// �޷��Ŷ�����
        /// </summary>
        public ushort UShortValue
        {
            get { return BitConverter.ToUInt16(data, 0); }
        }

        #endregion
        
        #region ��ʼ������
        public PropertyType(byte[] value) 
        {
            this.data = value;
            this.numberLen = value.Length;
        }
        public PropertyType(int value)
        {
            this.data = BitConverter.GetBytes(value);
            this.numberLen = data.Length;
        }
        public PropertyType(uint value)
        {
            this.data = BitConverter.GetBytes(value);
            this.numberLen = data.Length;
        }
        public PropertyType(char value)
        {
            this.data = BitConverter.GetBytes(value);
            this.numberLen = data.Length;
        }
        public PropertyType(double value)
        {
            this.data = BitConverter.GetBytes(value);
            this.numberLen = data.Length;
        }
        public PropertyType(float value)
        {
            this.data = BitConverter.GetBytes(value);
            this.numberLen = data.Length;
        }
        public PropertyType(long value)
        {
            this.data = BitConverter.GetBytes(value);
            this.numberLen = data.Length;
        }
        public PropertyType(short value)
        {
            this.data = BitConverter.GetBytes(value);
            this.numberLen = data.Length;
        }
        public PropertyType(ulong value)
        {
            this.data = BitConverter.GetBytes(value);
            this.numberLen = data.Length;
        }
        public PropertyType(ushort value)
        {
            this.data = BitConverter.GetBytes(value);
            this.numberLen = data.Length;
        }
        
        public PropertyType()
        {
            
            this.data = new byte[4];
            this.numberLen = data.Length;
        }
        #endregion

        /// <summary>
        /// ��ȡѭ���ĳ���
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private int GetForLen(byte[] arr) 
        {
            int len = arr.Length;
            if (len > data.Length) 
            {
                throw new Exception("ֵ���ȱ�ԭ���ȴ��޷�����λ����!");
            }
            return len;
        }

        /// <summary>
        /// ����and����
        /// </summary>
        /// <param name="val"></param>
        private void DoAnd(byte[] arr) 
        {
            //byte[] tmpArr = BitConverter.GetBytes(val);
            int len=GetForLen(arr);
            for (int i = 0; i < len; i++)
            {
                data[i] = (byte)(data[i] & arr[i]);
            }
            for(int i=len;i<numberLen;i++)
            {
                data[i]=0;
            }
        }

       
        /// <summary>
        /// ����Or����
        /// </summary>
        /// <param name="val"></param>
        private void DoOr(byte[] arr)
        {
            int len=GetForLen(arr);
            if (arr != null)
            {
                for (int i = 0; i < len; i++)
                {
                    data[i] = (byte)(data[i] | arr[i]);
                }
            }
        }

        /// <summary>
        /// ����Or����
        /// </summary>
        /// <param name="val"></param>
        private void DoXor(byte[] arr)
        {
            //byte[] tmpArr = BitConverter.GetBytes(val);
            for (int i = 0; i < numberLen; i++)
            {
                data[i] = (byte)(data[i] ^ arr[i]);
            }
        }

        #region �������
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(byte[] value)
        {
            DoOr(value);
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(uint value) 
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(int value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(double value)
        {
            DoOr(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(float value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(long value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(short value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(ulong value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(ushort value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(char value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(Enum value)
        {
            DoOr(BitConverter.GetBytes(Convert.ToInt32(value)));
        }
        #endregion

        #region ɾ������
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(byte[] arr)
        {
            if (arr != null)
            {
                byte tmp = 0;
                int len=GetForLen(arr);
                for (int i = 0; i < len; i++)
                {
                    tmp = (byte)~arr[i];
                    data[i] = (byte)(data[i] & tmp);
                }
            }
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(int value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(uint value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(char value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(double value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(float value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(long value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(short value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(ulong value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(ushort value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }


        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(Enum value)
        {
            DeleteProperty(BitConverter.GetBytes(Convert.ToInt32(value)));
        }
        #endregion

        #region �Ƿ��������

        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(byte[] arr)
        {
            
            //uint propertys = BitConverter.ToUInt32(this.data, 0);
            int len=GetForLen(arr);
            byte tmp = 0;
            for (int i = 0; i < len; i++)
            {
                tmp = (byte)(data[i] | arr[i]);
                if (tmp != data[i]) 
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(double value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(char value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(float value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(int value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(long value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(short value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(uint value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(ulong value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(ushort value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// ����һ�������ͣ�ָʾ�Ƿ���ָ��ֵ
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(Enum value)
        {
            return Contains(BitConverter.GetBytes(Convert.ToUInt32(value)));
        }
        #endregion
    }
}
