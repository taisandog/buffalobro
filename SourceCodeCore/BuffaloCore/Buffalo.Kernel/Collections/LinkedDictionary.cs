using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Kernel.Collections
{
    /// <summary>
    /// 保存了活跃度的Dictionary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class LinkedDictionary<T, K> : IDictionary<T, K>, IDisposable
    {
        /// <summary>
        /// 存储数据的字典
        /// </summary>
        private IDictionary<T, LinkedListNode<LinkedValueNode<T, K>>> _dic = null;
        /// <summary>
        /// 存储数据的字典
        /// </summary>
        private LinkedList<LinkedValueNode<T, K>> _lk = null;
        /// <summary>
        /// get值时候是否触发
        /// </summary>
        private bool _isGetToUpdate;

        private LinkedDictionaryCollection<T, K> _collect;
        /// <summary>
        /// 保存了活跃度的Dictionary
        /// </summary>
        /// <param name="dic">托管的字典</param>
        /// <param name="isGetToUpdate">Get值时候是否要更新活跃度</param>
        public LinkedDictionary(IDictionary<T, LinkedListNode<LinkedValueNode<T, K>>> dic, bool isGetToUpdate = true)
        {
            _dic = dic;
            _lk = new LinkedList<LinkedValueNode<T, K>>();
            _collect = new LinkedDictionaryCollection<T, K>(_lk, false);
            _isGetToUpdate = isGetToUpdate;
        }
        /// <summary>
        /// 保存了活跃度的Dictionary
        /// </summary>
        public LinkedDictionary(bool isGetToUpdate = true) : this(new Dictionary<T, LinkedListNode<LinkedValueNode<T, K>>>(), isGetToUpdate)
        {
        }



        /// <summary>
        /// 存取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual K this[T key]
        {
            get
            {
                LinkedListNode<LinkedValueNode<T, K>> node = _dic[key];
                if (_isGetToUpdate)
                {
                    MoveToLast(node);
                }
                return node.Value.Value;
            }
            set
            {
                LinkedListNode<LinkedValueNode<T, K>> node = null;
                if (_dic.TryGetValue(key, out node))
                {
                    node.Value = new LinkedValueNode<T, K>(key, value);

                    MoveToLast(node);
                }
                else
                {
                    //node = _lk.AddLast(new LinkedValueNode<T, K>(key, value));
                    //_dic[key] = node;
                    Add(key,value);
                }

            }
        }

        /// <summary>
        /// 把节点移动到最新
        /// </summary>
        /// <param name="node"></param>
        protected virtual void MoveToLast(LinkedListNode<LinkedValueNode<T, K>> node)
        {
            OnUpdate(node.Value);
            _lk.Remove(node);
            _lk.AddLast(node);
        }



        public ICollection<T> Keys
        {
            get
            {

                return _dic.Keys;
            }
        }

        public ICollection<K> Values
        {
            get
            {
                List<K> lst = new List<K>(_dic.Count);
                foreach (KeyValuePair<T, LinkedListNode<LinkedValueNode<T, K>>> kvp in _dic)
                {
                    lst.Add(kvp.Value.Value.Value);
                }
                return lst;
            }
        }

        public int Count
        {
            get
            {
                return _dic.Count;
            }
        }

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

        /// <summary>
        /// 活跃度信息
        /// </summary>
        public LinkedList<LinkedValueNode<T, K>> TimeInfos
        {
            get { return _lk; }
        }
        /// <summary>
        /// 添加一个带有所提供的键和值的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(T key, K value)
        {
            LinkedValueNode < T, K > node= new LinkedValueNode<T, K>(key, value);
            Add(node);
        }
        /// <summary>
        /// 添加一个带有所提供的键和值的元素
        /// </summary>
        /// <param name="item">项</param>
        public void Add(LinkedValueNode<T, K> item)
        {
            OnAddNew(item);
            LinkedListNode<LinkedValueNode<T, K>> node = _lk.AddLast(item);
            _dic.Add(item.Key, node);
        }

        
        /// <summary>
        /// 当发生AddNew时候
        /// </summary>
        /// <param name="node"></param>
        protected virtual void OnAddNew(LinkedValueNode<T, K> node) 
        {

        }
        /// <summary>
        /// 当发生AddNew时候
        /// </summary>
        /// <param name="node"></param>
        protected virtual void OnUpdate(LinkedValueNode<T, K> node)
        {

        }
        /// <summary>
        /// 添加一个带有所提供的键和值的元素
        /// </summary>
        /// <param name="item">项</param>
        public void Add(KeyValuePair<T, K> item)
        {
            LinkedValueNode < T, K> linkedValueNode = new LinkedValueNode<T, K>(item.Key, item.Value);
            Add(linkedValueNode);
        }
        /// <summary>
        /// 清空所有
        /// </summary>
        public void Clear()
        {
            _dic.Clear();
            _lk.Clear();

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
            LinkedListNode<LinkedValueNode<T, K>> ret = null;

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
            foreach (KeyValuePair<T, LinkedListNode<LinkedValueNode<T, K>>> kvp in _dic)
            {
                if (curIndex >= array.Length)
                {
                    break;
                }
                KeyValuePair<T, K> retKvp = new KeyValuePair<T, K>(kvp.Key, kvp.Value.Value.Value);
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
            return new LinkedKeyValueEnumerator<T, K>(_dic.GetEnumerator());
        }

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(T key)
        {
            LinkedListNode<LinkedValueNode<T, K>> ret = null;
            bool isRemove = false;
            if (_dic.TryGetValue(key, out ret))
            {
                isRemove = _dic.Remove(key);
                _lk.Remove(ret);
            }
            ret = null;
            return isRemove;
        }
        /// <summary>
        /// 删除键并返回值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public K RemoveKey(T key)
        {
            LinkedListNode<LinkedValueNode<T, K>> retNode = null;
            bool isRemove = false;
            K ret = default(K);
            if (_dic.TryGetValue(key, out retNode))
            {
                ret = retNode.Value.Value;
                isRemove = _dic.Remove(key);
                _lk.Remove(retNode);
                
            }
            
            ClearLinkedListNode(retNode);
            retNode = null;
            return ret;
        }

        private void ClearLinkedListNode(LinkedListNode<LinkedValueNode<T, K>> retNode) 
        {
            if(retNode == null) 
            {
                return;
            }
            retNode.Value = null;
        }

        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(LinkedListNode<LinkedValueNode<T, K>> item)
        {
            if (item == null)
            {
                return true;
            }
            bool isRemove = _dic.Remove(item.Value.Key);
            _lk.Remove(item);
            ClearLinkedListNode(item);
            return isRemove;

        }

        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<T, K> item)
        {
            T key = item.Key;
            LinkedListNode<LinkedValueNode<T, K>> retNode = null;
            bool isRemove = false;
            if (_dic.TryGetValue(key, out retNode))
            {
                isRemove = _dic.Remove(key);
                _lk.Remove(retNode);
            }
            ClearLinkedListNode(retNode);
            if (!isRemove || retNode == null)
            {
                return false;
            }
            
            return true;

        }
        /// <summary>
        /// 获取与指定键关联的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(T key, out K value)
        {
            LinkedListNode<LinkedValueNode<T, K>> ret = null;

            if (_dic.TryGetValue(key, out ret))
            {
                if (_isGetToUpdate)
                {
                    MoveToLast(ret);
                }
                value = ret.Value.Value;
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
            return _lk.GetEnumerator();
        }
        /// <summary>
        ///  按最低活跃度开始裁剪元素集合
        /// </summary>
        /// <param name="count">保留个数</param>
        public void TrimCount(int count)
        {
            DateTime dt = DateTime.Now;
            LinkedListNode<LinkedValueNode<T, K>> curNode = null;
            while (_dic.Count > count)
            {
                curNode = _lk.First;
                if (curNode == null)
                {
                    break;
                }
                if (!Remove(curNode))
                {
                    //break;
                }
            }
        }

        /// <summary>
        /// 最老的节点
        /// </summary>
        public LinkedListNode<LinkedValueNode<T, K>> OldestNode
        {
            get
            {
                return _lk.First;
            }
        }

        /// <summary>
        /// 最新的节点
        /// </summary>
        public LinkedListNode<LinkedValueNode<T, K>> LatestNode
        {
            get
            {
                return _lk.Last;
            }
        }

        /// <summary>
        /// 获取一个从旧到新的枚举器
        /// </summary>
        /// <returns></returns>
        public LinkedDictionaryCollection<T, K> GetEnumeratorOldToNew()
        {
            _collect.IsMoveNext = true;
            return _collect;
        }
        /// <summary>
        /// 获取一个从新到旧的枚举器
        /// </summary>
        /// <returns></returns>
        public LinkedDictionaryCollection<T, K> GetEnumeratorNewToOld()
        {
            _collect.IsMoveNext = false;
            return _collect;
        }

        public void Dispose()
        {
            if (_dic != null)
            {
                _dic.Clear();
            }
            _dic = null;
            if (_lk != null)
            {
                _lk.Clear();
            }
            _lk = null;
            if (_collect != null)
            {
                _collect.Dispose();
            }
            _collect = null;

        }
    }



    

}
