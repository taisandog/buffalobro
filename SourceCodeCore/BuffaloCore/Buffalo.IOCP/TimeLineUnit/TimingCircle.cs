using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Kernel.Collections
{
    /// <summary>
    /// 时间轴
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TimingCircle<T>:IDisposable
    {
        private ConcurrentDictionary<T, bool>[]  _arrTimings;
        /// <summary>
        /// 所有数据
        /// </summary>
        public ConcurrentDictionary<T, bool>[] AllData 
        {
            get 
            {
                return _arrTimings;
            }
        }
        /// <summary>
        /// 刻度
        /// </summary>
        private long _scale;

        /// <summary>
        /// 总时间
        /// </summary>
        private long _totalTime;
        /// <summary>
        /// 当前索引
        /// </summary>
        private int _currentIndex;
        /// <summary>
        /// 上次一的刻度
        /// </summary>
        private long _lastTick;
        /// <summary>
        /// 时间轴
        /// </summary>
        /// <param name="totalTime">走完一圈的总时间</param>
        /// <param name="scale">刻度</param>
        public TimingCircle(long totalTime,long scale) 
        {
            _totalTime = totalTime;
            _scale = scale;
            int len = (int)(_totalTime / _scale);
            if (len <1) 
            {
                len = 1;
            }
            if (_scale < 1) 
            {
                _scale = 1;
            }
            _arrTimings = new ConcurrentDictionary<T, bool>[len];
            for(int i = 0; i < len; i++) 
            {
                _arrTimings[i] = new ConcurrentDictionary<T, bool>();
            }
        }

        /// <summary>
        /// 重置到此时间
        /// </summary>
        /// <param name="time">跳到此时间刻度</param>
        public void Reset(long time) 
        {
            _lastTick = time/_scale;
        }

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public bool DeleteValue(T value, int index)
        {

            ConcurrentDictionary<T, bool> item = _arrTimings[index];
            return item.TryRemove(value, out _);
        }

        /// <summary>
        /// 清除值
        /// </summary>
        public void Clear() 
        {
            ConcurrentDictionary<T, bool> item = null;
            for (int i=0;i< _arrTimings.Length; i++) 
            {
                item = _arrTimings[i];
                if (item == null)
                {
                    continue;
                }
                item.Clear();
                
            }
        }

        /// <summary>
        /// 移动到此时间刻度
        /// </summary>
        /// <param name="curTime">时间</param>
        /// <param name="queItems">填充列表</param>
        public void MoveToTime(long curTime, Queue<ConcurrentDictionary<T, bool>> queItems)
        {
            long curTick = 0;
            int moveItems = GetMoveCount(curTime, out curTick);
            int index = 0;
            if (moveItems <= 0)
            {
                return;
            }

            ConcurrentDictionary<T, bool> item = null;
            lock (_arrTimings)
            {
                for (int i = 0; i < moveItems; i++)
                {
                    index = GetAddIndex(i);
                    item = _arrTimings[index];
                    if (item == null)
                    {
                        continue;
                    }
                    queItems.Enqueue(item);
                }
                _currentIndex = GetAddIndex(moveItems);
                _lastTick = curTick;
            }
        }
        /// <summary>
        /// 添加值到指定时间
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="time">时间(-1则为当前时间)</param>
        /// <returns>返回加到刻度索引</returns>
        public int AddValue(T value, long time = -1)
        {
            int moveItems = 0;
            if (time > 0)
            {
                moveItems = GetMoveCount(time, out _);
            }
            
            ConcurrentDictionary<T, bool> dic = null;

            int index= GetAddIndex(moveItems);

            dic = _arrTimings[index];

            dic[value] = true;
            return index;
        }




        /// <summary>
        /// 获取上一次判断到现在为止，经历了几个时间刻度
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <param name="curTick">计算出来的当前刻度</param>
        /// <returns>返回经历了几个刻度</returns>
        private int GetMoveCount(long time,out long curTick) 
        {
            curTick = time / _scale;
            int moveItems = (int)Math.Abs(curTick - _lastTick);
            if (moveItems > _arrTimings.Length)
            {
                moveItems = _arrTimings.Length;
            }
            return moveItems;
        }

        /// <summary>
        /// 获取加值后的当前索引
        /// </summary>
        /// <param name="addCount"></param>
        /// <returns></returns>
        private int GetAddIndex(int addCount) 
        {
            int ret = _currentIndex + addCount;
            return ret % _arrTimings.Length;
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            ConcurrentDictionary<T, bool> item = null;
            lock (_arrTimings)
            {
                for (int i = 0; i < _arrTimings.Length; i++)
                {
                    item = _arrTimings[i];
                    if (item == null)
                    {
                        continue;
                    }
                    item.Clear();
                    _arrTimings[i] = null;
                }
                _arrTimings = null;
            }
        }
    }
}
