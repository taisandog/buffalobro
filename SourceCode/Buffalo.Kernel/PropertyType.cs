using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Commons
{
    public class PropertyType
    {
        
        private byte[] data;
        private int numberLen;

        #region 返回值函数
        public byte[] Data
        {
            get { return data; }
        }
        /// <summary>
        /// 整型值
        /// </summary>
        public int IntValue
        {
            get { return BitConverter.ToInt32(data, 0); }
        }
        
        /// <summary>
        /// 字符型
        /// </summary>
        public char CharValue
        {
            get { return BitConverter.ToChar(data, 0); }
        }

        /// <summary>
        /// 双精度型
        /// </summary>
        public double DoubleValue
        {
            get { return BitConverter.ToDouble(data, 0); }
        }

        /// <summary>
        /// 短整型
        /// </summary>
        public short ShortValue
        {
            get { return BitConverter.ToInt16(data, 0); }
        }

        /// <summary>
        /// 长整型
        /// </summary>
        public long LongValue
        {
            get { return BitConverter.ToInt64(data, 0); }
        }

        /// <summary>
        /// 浮点型
        /// </summary>
        public float FloatValue
        {
            get { return BitConverter.ToSingle(data, 0); }
        }

        /// <summary>
        /// 无符号长整型
        /// </summary>
        public ulong ULongValue
        {
            get { return BitConverter.ToUInt64(data, 0); }
        }

        /// <summary>
        /// 无符号整型值
        /// </summary>
        public uint UIntValue
        {
            get { return BitConverter.ToUInt32(data, 0); }
        }

        /// <summary>
        /// 无符号短整型
        /// </summary>
        public ushort UShortValue
        {
            get { return BitConverter.ToUInt16(data, 0); }
        }

        #endregion
        
        #region 初始化函数
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
        /// 获取循环的长度
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private int GetForLen(byte[] arr) 
        {
            int len = arr.Length;
            if (len > data.Length) 
            {
                throw new Exception("值长度比原长度大，无法进行位运算!");
            }
            return len;
        }

        /// <summary>
        /// 进行and运算
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
        /// 进行Or运算
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
        /// 进行Or运算
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

        #region 添加属性
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(byte[] value)
        {
            DoOr(value);
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(uint value) 
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(int value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(double value)
        {
            DoOr(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(float value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(long value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(short value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(ulong value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(ushort value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(char value)
        {
            DoOr(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="value"></param>
        public void AddProperty(Enum value)
        {
            DoOr(BitConverter.GetBytes(Convert.ToInt32(value)));
        }
        #endregion

        #region 删除属性
        /// <summary>
        /// 删除一个属性
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
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(int value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }

        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(uint value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(char value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(double value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(float value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(long value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(short value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(ulong value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }
        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(ushort value)
        {
            byte[] tmpArr = BitConverter.GetBytes(value);
            DeleteProperty(tmpArr);
        }


        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="value"></param>
        public void DeleteProperty(Enum value)
        {
            DeleteProperty(BitConverter.GetBytes(Convert.ToInt32(value)));
        }
        #endregion

        #region 是否包含属性

        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
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
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(double value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(char value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(float value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(int value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(long value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(short value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(uint value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(ulong value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(ushort value)
        {
            return Contains(BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 返回一个布尔型，指示是否含有指定值
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(Enum value)
        {
            return Contains(BitConverter.GetBytes(Convert.ToUInt32(value)));
        }
        #endregion
    }
}
