using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Collections
{
    /// <summary>
    /// LRU字典枚举器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class LinkedDictionaryCollection<T, K> : IEnumerable, IEnumerable<KeyValuePair<T, K>>, IDisposable
    {

        private LinkedList<KeyValuePair<T, K>> _lk;
        private bool _moveNext = false;
        /// <summary>
        /// 是否向后枚举
        /// </summary>
        public bool IsMoveNext
        {
            get { return _moveNext; }
            set { _moveNext = value; }
        }

        public LinkedDictionaryCollection(LinkedList<KeyValuePair<T, K>> lk, bool moveNext)
        {
            _lk = lk;
            _moveNext = moveNext;
        }


        public IEnumerator GetEnumerator()
        {
            if (_moveNext)
            {
                return new LinkedDictionaryEnumerator<T, K>(_lk);
            }
            return new LinkedDictionaryEnumeratorMovePrevious<T, K>(_lk);
        }

        IEnumerator<KeyValuePair<T, K>> IEnumerable<KeyValuePair<T, K>>.GetEnumerator()
        {
            if (_moveNext)
            {
                return new LinkedDictionaryEnumerator<T, K>(_lk);
            }
            return new LinkedDictionaryEnumeratorMovePrevious<T, K>(_lk);
        }

        public void Dispose()
        {
            _lk = null;
        }
    }
}
