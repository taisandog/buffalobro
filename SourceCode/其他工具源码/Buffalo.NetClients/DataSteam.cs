using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.NetClients
{
    /// <summary>
    /// 数据流
    /// </summary>
    public class DataSteam
    {
        /// <summary>
        /// 收到的数据缓存区
        /// </summary>
        private byte[] _bufferData ;
        public DataSteam(int length)
        {
            _bufferData = new byte[length];
        }
        

        /// <summary>
        /// 缓冲区数据
        /// </summary>
        public byte[] BufferData
        {
            get { return _bufferData; }
        }

        public byte this[int index] 
        {
            get 
            {
                return _bufferData[index];
            }
            set 
            {
                _bufferData[index] = value;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="length">删除长度</param>
        public void Remove(int index,int length)
        {
            _bufferDataLenght -= length;
            Array.Copy(_bufferData, index, _bufferData, 0, _bufferDataLenght - index);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear() 
        {
            _bufferDataLenght = 0;
        }

        /// <summary>
        /// 数据缓存区当前存放的数据长度
        /// </summary>
        private int _bufferDataLenght = 0;

        /// <summary>
        /// 当前数据长度
        /// </summary>
        public int DataLenght
        {
            get { return _bufferDataLenght; }
            
        }

        /// <summary>
        /// 添加到流末尾
        /// </summary>
        /// <param name="source"></param>
        /// <param name="offest"></param>
        /// <param name="length"></param>
        public void AddRange(byte[] source, int offest, int length) 
        {
            Array.Copy(source, offest, _bufferData, _bufferDataLenght, length);
            _bufferDataLenght += length;
        }
    }
}
