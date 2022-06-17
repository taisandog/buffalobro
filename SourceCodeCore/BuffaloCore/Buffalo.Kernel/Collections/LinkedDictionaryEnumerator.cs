using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Collections
{
    /// <summary>
    /// LRU字典的枚举,向后枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class LinkedDictionaryEnumerator<T, K> : IEnumerator<KeyValuePair<T, K>>
    {
        protected LinkedList<KeyValuePair<T, K>> _list;

        /// <summary>
        /// 当前节点
        /// </summary>
        protected LinkedListNode<KeyValuePair<T, K>> _currentNode;

        /// <summary>
        /// 是否已经开始移动了
        /// </summary>
        protected bool _isStart = false;
        /// <summary>
        /// LRU字典的枚举
        /// </summary>
        /// <param name="enumTk">枚举器</param>
        public LinkedDictionaryEnumerator(LinkedList<KeyValuePair<T, K>> list)
        {
            _isStart = false;
            _list = list;
        }
        public KeyValuePair<T, K> Current
        {
            get
            {
                if (this._currentNode == null)
                {
                    return default(KeyValuePair<T, K>);
                }
                return _currentNode.Value;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                if (this._currentNode == null)
                {
                    return null;
                }
                return _currentNode.Value;
            }
        }

        public void Dispose()
        {
            _list = null;
            _currentNode = null;
        }

        public virtual bool MoveNext()
        {
            if (!_isStart)
            {
                _isStart = true;
                _currentNode = _list.First;
            }
            else
            {
                if (_currentNode == null)
                {
                    return false;
                }
                _currentNode = _currentNode.Next;
            }

            return _currentNode != null;
        }

        public virtual void Reset()
        {
            _isStart = false;
            _currentNode = null;
        }
    }
    /// <summary>
    /// LRU字典的枚举,向前枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class LinkedDictionaryEnumeratorMovePrevious<T, K> : LinkedDictionaryEnumerator<T, K>
    {
        /// <summary>
        /// LRU字典的枚举
        /// </summary>
        /// <param name="enumTk">枚举器</param>
        public LinkedDictionaryEnumeratorMovePrevious(LinkedList<KeyValuePair<T, K>> list) : base(list)
        {

        }
        public override bool MoveNext()
        {
            if (!_isStart)
            {
                _isStart = true;
                _currentNode = _list.Last;
            }
            else
            {
                if (_currentNode == null)
                {
                    return false;
                }
                _currentNode = _currentNode.Previous;
            }

            return _currentNode != null;
        }

    }
}
