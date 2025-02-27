using System;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// 字段信息集合
    /// </summary>
    public class ClassFieldInfoCollection : IEnumerable<FieldInfoHandle>
    {
        private Dictionary<string, FieldInfoHandle> _fieldInfoHandles;
        /// <summary>
        /// 属性信息集合
        /// </summary>
        /// <param name="FieldInfoHandles">字段信息的哈希表</param>
        public ClassFieldInfoCollection(Dictionary<string, FieldInfoHandle> fieldInfoHandles) 
        {
            this._fieldInfoHandles = fieldInfoHandles;

        }

        /// <summary>
        /// 根据字段名获取字段信息
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public FieldInfoHandle this[string fieldName]
        {
            get
            {
                if (_fieldInfoHandles != null)
                {
                    FieldInfoHandle ret = null;
                    if (_fieldInfoHandles.TryGetValue(fieldName, out ret))
                    {
                        return ret;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 当前属性的数量
        /// </summary>
        public int Count
        {
            get
            {
                return _fieldInfoHandles.Count;
            }
        }


        #region IEnumerable<FieldInfoHandle> 成员

        public IEnumerator<FieldInfoHandle> GetEnumerator()
        {
            return new ClassFieldEnumerator(_fieldInfoHandles);
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ClassFieldEnumerator(_fieldInfoHandles);
        }

        #endregion
    }
}
