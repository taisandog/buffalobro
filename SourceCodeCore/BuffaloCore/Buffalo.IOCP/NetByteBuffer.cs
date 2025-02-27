using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 网络内容缓冲
    /// </summary>
    public class NetByteBuffer:IEnumerable<byte>,IDisposable
    {
        private byte[] _buffer = null;
        private int _headIndex = 0;
        private int _count= 0;
        public NetByteBuffer(int capacity)
        {
            _buffer = new byte[capacity];
            _headIndex = 0;
            _count = 0;
        }
        

        /// <summary>
        /// 当前个数
        /// </summary>
        public int Count 
        {
            get 
            {
                return _count;
            }
        }
        /// <summary>
        /// 缓冲数组的容量
        /// </summary>
        public int Capacity
        {
            get 
            {
                return _buffer.Length;
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte this[int index] 
        {
            get 
            {
                if (index >= _count) 
                {
                    throw new IndexOutOfRangeException("数组越界");
                }
                int cur = GetIndex(index);
                return _buffer[cur];
            }
            set 
            {
                if (index >= _count)
                {
                    throw new IndexOutOfRangeException("数组越界");
                }
                int cur = GetIndex(index);
                _buffer[cur] = value;
            }
        }
        /// <summary>
        /// 获取转换后的索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetIndex(int index) 
        {
            int cur = _headIndex + index;
            cur = cur % _buffer.Length;
            return cur;
        }

        /// <summary>
        /// 复制数组到本缓冲尾部
        /// </summary>
        /// <param name="array">需要被复制的数组</param>
        /// <param name="sourceIndex">起始索引</param>
        /// <param name="length">复制长度</param>
        /// <returns></returns>
        public void AppendBytes(byte[] array, int sourceIndex,int length) 
        {
            CheckLength(length);

            int startIndex = _headIndex + _count;
            startIndex = startIndex % _buffer.Length;

            int leftLength =_buffer.Length- startIndex;//现在头部位置到结尾还剩多少长度
            
            if (leftLength> length) //当前位置足够
            {
                
                Array.Copy(array, sourceIndex, _buffer, startIndex, length);
            }
            else//分段复制 
            {
                
                Array.Copy(array, sourceIndex, _buffer, startIndex, leftLength);
                int partLen = length - leftLength;//剩余的长度
                int partIndex = leftLength;//剩余元素的头部索引
                Array.Copy(array, partIndex, _buffer, 0, partLen);//到头部复制
            }
            _count += length;
        }
        /// <summary>
        /// 转成字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray() 
        {
            byte[] ret=new byte[_count];
            ReadBytes(0, ret, 0, ret.Length);
            return ret;
        }

        /// <summary>
        /// 复制字节到本缓冲尾部
        /// </summary>
        /// <param name="value">字节</param>
        /// <returns></returns>
        public void AppendByte(byte value)
        {
            CheckLength(1);

            int lastIndex = _headIndex + _count;
            lastIndex=lastIndex % _buffer.Length;
            _buffer[lastIndex] = value;
            _count++;
        }
        /// <summary>
        /// 检测是否还足够复制此容量的元素
        /// </summary>
        /// <param name="addcount">增加的元素</param>
        private void CheckLength(int addcount) 
        {
            if (_buffer.Length > _count+addcount) 
            {
                return;
            }
            //数组扩容
            int newLength = (_buffer.Length + addcount) * 15 / 10;
            if (newLength < 8) 
            {
                newLength = 8;
            }
            byte[] newBuffer = new byte[newLength];
            ReadBytes(0, newBuffer, 0, _count);
            _headIndex = 0;
            _buffer = newBuffer;
        }

        /// <summary>
        /// 从本缓冲读出字节到指定数组
        /// </summary>
        /// <param name="sourceIndex">本缓冲的起始索引</param>
        /// <param name="destinationArray">目标数组</param>
        /// <param name="destinationIndex">目标数组的起始索引</param>
        /// <param name="length">读出字节数</param>
        public void ReadBytes(int sourceIndex, byte[] destinationArray, int destinationIndex, int length) 
        {
            int index = sourceIndex + _headIndex;
            index = index % _buffer.Length;
            if (sourceIndex+length > _count)
            {
                throw new IndexOutOfRangeException("数组越界");
            }

            int leftLength = _buffer.Length - index;//现在头部位置到结尾还剩多少长度
            if (leftLength >= length)//前段长度足够
            {
               
                Array.Copy(_buffer, index, destinationArray, destinationIndex, length);
            }
            else//分段读出
            {
                

                Array.Copy(_buffer, index, destinationArray, destinationIndex, leftLength);
                int partLen = length - leftLength;//剩余的长度
                int partIndex = destinationIndex + leftLength;//剩余元素的头部索引
                Array.Copy(_buffer, 0, destinationArray, partIndex, partLen);//到头部复制
            }
        }

        

        /// <summary>
        /// 删除头部字节
        /// </summary>
        /// <param name="length">要删除的字节长度</param>
        /// <returns></returns>
        public void RemoveHeadBytes(int length)
        {
            if (length <= 0) 
            {
                return;
            }
            if (length > _count) 
            {
                length = _count;
            }
            _headIndex += length;
            _headIndex = _headIndex % _buffer.Length;
            _count -= length;
        }
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void Clear() 
        {
            _headIndex = 0;
            _count = 0;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return new NetByteBufferIEnumerable(_buffer, _headIndex, _count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new NetByteBufferIEnumerable(_buffer, _headIndex, _count);
        }

        public void Dispose()
        {
            _buffer = null;
        }
    }

    public class NetByteBufferIEnumerable : IEnumerator<byte>
    {
        private byte[] _buffer = null;
        private int _currentIndex;
        private int _headIndex;
        private int _endIndex = 0;
        private int _arrLen;
        public NetByteBufferIEnumerable(byte[] buffer,int headIndex, int count) 
        {
            _headIndex = headIndex;
            _currentIndex =-1;
            _buffer = buffer;
            _arrLen = _buffer.Length;
            _endIndex = (headIndex + count-1) % _arrLen;
        }
        public byte Current 
        {
            get 
            {
                return _buffer[_currentIndex];
            }
        }

        object IEnumerator.Current 
        {
            get { return _buffer[_currentIndex]; }
        }

        public void Dispose()
        {
            _buffer = null;
        }

        public bool MoveNext()
        {
            if (_currentIndex < 0) 
            {
                _currentIndex = _headIndex;
                return true;
            }
            if (_currentIndex == _endIndex)
            {
                return false;
            }
            _currentIndex++;
            _currentIndex = _currentIndex % _arrLen;
            return true;
        }

        public void Reset()
        {
            _currentIndex = -1;
            
        }
    }
}
