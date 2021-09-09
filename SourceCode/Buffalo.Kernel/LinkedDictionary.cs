using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 保存了活跃度的Dictionary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class LinkedDictionary<T, K> : IDictionary<T, K>
    {
        /// <summary>
        /// 存储数据的字典
        /// </summary>
        private IDictionary<T, NodeValue<T, K>> _dic = null;
        /// <summary>
        /// 头部节点
        /// </summary>
        private NodeValue<T, K> _headNode;
        /// <summary>
        /// 尾部节点
        /// </summary>
        private NodeValue<T, K> _lastNode;

        /// <summary>
        /// 最老的节点
        /// </summary>
        public NodeValue<T, K> OldestNode
        {
            get 
            {
                return _headNode;
            }
        }
        /// <summary>
        /// 最新的节点
        /// </summary>
        public NodeValue<T, K> ActiveNode
        {
            get
            {
                return _lastNode;
            }
        }

        /// <summary>
        /// get值时候是否触发
        /// </summary>
        private bool _isGetToUpdate;
        /// <summary>
        /// 保存了活跃度的Dictionary
        /// </summary>
        /// <param name="dic">托管的字典</param>
        /// <param name="isGetToUpdate">Get值时候是否要更新活跃度</param>
        public LinkedDictionary(IDictionary<T, NodeValue<T, K>> dic, bool isGetToUpdate = true)
        {
            _dic = dic;

            _isGetToUpdate = isGetToUpdate;
        }
        /// <summary>
        /// 保存了活跃度的Dictionary
        /// </summary>
        public LinkedDictionary(bool isGetToUpdate = true) : this(new Dictionary<T, NodeValue<T, K>>())
        {
        }



        /// <summary>
        /// 存取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public K this[T key]
        {
            get
            {
                NodeValue<T, K> node = _dic[key];
                if (_isGetToUpdate)
                {
                    MoveToLast(node);
                }
                return node.Value;
            }
            set
            {
                NodeValue<T, K> node = null;
                if (_dic.TryGetValue(key, out node))
                {
                    node.Value = value;

                    MoveToLast(node);
                }
                else
                {
                    node = new NodeValue<T, K>(key, value);
                    AddToLast(node);
                    _dic[key] = node;
                }

            }
        }

        /// <summary>
        /// 把节点移动到最新
        /// </summary>
        /// <param name="node"></param>
        private void MoveToLast(NodeValue<T, K> node)
        {
            //_lk.Remove(node);
            //_lk.AddLast(node);
            RemoveNode(node);
            AddToLast(node);
        }

        /// <summary>
        /// 增加到末尾
        /// </summary>
        /// <param name="node"></param>
        private void AddToLast(NodeValue<T, K> node)
        {
            if (HeadNode == null)
            {
                HeadNode = node;
            }
            if (_lastNode == null)
            {
                _lastNode = node;
            }
            else
            {
                AddKey(_lastNode, node, false);
            }
        }
        /// <summary>
        /// 增加到末尾
        /// </summary>
        /// <param name="node"></param>
        private void AddToHead(NodeValue<T, K> node)
        {
            if (HeadNode == null)
            {
                HeadNode = node;
            }
            else
            {
                AddKey(HeadNode, node, true);
            }

            if (_lastNode == null)
            {
                _lastNode = node;
            }

        }
        /// <summary>
        /// 增加到之前
        /// </summary>
        /// <param name="node">上一个节点</param>
        /// <param name="needInsertNode">当前要插入的节点</param>
        /// <returns></returns>
        private bool AddKey(NodeValue<T, K> node, NodeValue<T, K> needInsertNode, bool insertToFont)
        {
            if (insertToFont)
            {
                if (node.Previous != null)
                {
                    node.Previous.Next = needInsertNode;
                }
                needInsertNode.Previous = node.Previous;
                node.Previous = needInsertNode;

                needInsertNode.Next = node;

                if (HeadNode == node)
                {
                    HeadNode = needInsertNode;
                }
            }
            else
            {
                if (node.Next != null)
                {
                    node.Next.Previous = needInsertNode;
                }
                needInsertNode.Next = node.Next;
                node.Next = needInsertNode;
                needInsertNode.Previous = node;
                if (_lastNode == node)
                {
                    _lastNode = needInsertNode;
                }
            }
            return true;

        }

        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool RemoveNode(NodeValue<T, K> node)
        {
            if (HeadNode == node)
            {
                HeadNode = node.Next;
            }
            if (_lastNode == node)
            {
                _lastNode = node.Previous;
            }
            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }
            node.Previous = null;
            node.Next = null;
            return true;
        }

        /// <summary>
        /// 所有键
        /// </summary>
        public ICollection<T> Keys
        {
            get
            {

                return _dic.Keys;
            }
        }

        /// <summary>
        /// 所有值
        /// </summary>
        public ICollection<K> Values
        {
            get
            {
                List<K> lst = new List<K>(_dic.Count);
                foreach (KeyValuePair<T, NodeValue<T, K>> kvp in _dic)
                {
                    lst.Add(kvp.Value.Value);
                }
                return lst;
            }
        }

        /// <summary>
        /// 字典个数
        /// </summary>
        public int Count
        {
            get
            {
                return _dic.Count;
            }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _dic.IsReadOnly;
            }
        }

        /// <summary>
        /// get值时候是否触发更新
        /// </summary>
        public bool IsGetToUpdate
        {
            get { return _isGetToUpdate; }
            set { _isGetToUpdate = value; }
        }

        public NodeValue<T, K> HeadNode { get => _headNode; set => _headNode = value; }


        /// <summary>
        /// 添加一个带有所提供的键和值的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(T key, K value)
        {
            NodeValue<T, K> node = new NodeValue<T, K>(key, value);

            _dic.Add(key, node);
            AddToLast(node);
        }
        /// <summary>
        /// 添加一个带有所提供的键和值的元素
        /// </summary>
        /// <param name="item">项</param>
        public void Add(KeyValuePair<T, K> item)
        {
            NodeValue<T, K> node = new NodeValue<T, K>(item.Key, item.Value);
            _dic.Add(item.Key, node);
            AddToLast(node);
        }

        /// <summary>
        /// 清空所有
        /// </summary>
        public void Clear()
        {
            _dic.Clear();
            HeadNode = null;
            _lastNode = null;
        }

        /// <summary>
        /// 是否包含此项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<T, K> item)
        {
            return ContainsKey(item.Key);
        }
        /// <summary>
        ///  是否包含此键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(T key)
        {
            NodeValue<T, K> ret = null;

            if (_dic.TryGetValue(key, out ret))
            {
                if (_isGetToUpdate)
                {
                    MoveToLast(ret);
                }
                return true;
            }

            return false;
        }
        /// <summary>
        /// 把值复制到数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<T, K>[] array, int arrayIndex)
        {
            int curIndex = arrayIndex;
            foreach (KeyValuePair<T, NodeValue<T, K>> kvp in _dic)
            {
                if (curIndex >= array.Length)
                {
                    break;
                }
                KeyValuePair<T, K> retKvp = new KeyValuePair<T, K>(kvp.Key, kvp.Value.Value);
                array[curIndex] = retKvp;
                curIndex++;
            }


        }
        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<T, K>> GetEnumerator()
        {
            return new LinkedDictionaryEnumerator<T, K>(_dic.GetEnumerator());
        }

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(T key)
        {
            NodeValue<T, K> ret = null;
            bool isRemove = false;
            if (_dic.TryGetValue(key, out ret))
            {
                isRemove = _dic.Remove(key);
                //_lk.Remove(ret);
                RemoveNode(ret);
            }
            return isRemove;
        }
        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<T, K> item)
        {

            return Remove(item.Key);

        }

        /// <summary>
        /// 获取与指定键关联的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(T key, out K value)
        {
            NodeValue<T, K> ret = null;

            if (_dic.TryGetValue(key, out ret))
            {
                if (_isGetToUpdate)
                {
                    MoveToLast(ret);
                }
                value = ret.Value;
                return true;
            }
            value = default(K);
            return false;
        }
        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LinkedDictionaryEnumerator<T, K>(_dic.GetEnumerator());
        }
        /// <summary>
        ///  按最低活跃度开始裁剪元素集合
        /// </summary>
        /// <param name="count">保留个数</param>
        public void TrimCount(int count)
        {
            DateTime dt = DateTime.Now;
            NodeValue<T, K> curNode = null;
            while (_dic.Count > count)
            {
                curNode = HeadNode;
                if (curNode == null)
                {
                    break;
                }
                if (!Remove(curNode.Key))
                {
                    break;
                }
            }
        }


    }



    /// <summary>
    /// LRU字典的枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class LinkedDictionaryEnumerator<T, K> : IEnumerator<KeyValuePair<T, K>>
    {
        private IEnumerator<KeyValuePair<T, NodeValue<T, K>>> _enumTk;
        /// <summary>
        /// LRU字典的枚举
        /// </summary>
        /// <param name="enumTk">枚举器</param>
        public LinkedDictionaryEnumerator(IEnumerator<KeyValuePair<T, NodeValue<T, K>>> enumTk)
        {
            _enumTk = enumTk;
        }
        public KeyValuePair<T, K> Current
        {
            get
            {
                return new KeyValuePair<T, K>(_enumTk.Current.Key, _enumTk.Current.Value.Value);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return new KeyValuePair<T, K>(_enumTk.Current.Key, _enumTk.Current.Value.Value);
            }
        }


        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            _enumTk.Dispose();
        }
        /// <summary>
        /// 下一个
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            return _enumTk.MoveNext();
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _enumTk.Reset();
        }
    }

    /// <summary>
    /// 节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class NodeValue<T, K>
    {
        /// <summary>
        /// 节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public NodeValue(T key, K value)
        {
            _key = key;
            _value = value;
        }

        private K _value;

        private T _key;

        private NodeValue<T, K> _next;
        /// <summary>
        /// 下一节点
        /// </summary>
        public NodeValue<T, K> Next
        {
            get
            {
                return _next;
            }

            internal set
            {
                _next = value;
            }
        }
        private NodeValue<T, K> _previous;
        /// <summary>
        /// 上一节点
        /// </summary>
        public NodeValue<T, K> Previous
        {
            get
            {
                return _previous;
            }

            internal set
            {
                _previous = value;
            }
        }
        /// <summary>
        /// 键
        /// </summary>
        public T Key
        {
            get
            {
                return _key;
            }

            
        }
        /// <summary>
        /// 值
        /// </summary>
        public K Value
        {
            get
            {
                return _value;
            }

            internal set
            {
                this._value = value;
            }
        }

        public override string ToString()
        {
            return "[" + Key + "," + Value + "]";
        }
    }
}
