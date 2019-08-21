using System;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// �ֶ���Ϣ����
    /// </summary>
    public class ClassFieldInfoCollection : IEnumerable<FieldInfoHandle>
    {
        private Dictionary<string, FieldInfoHandle> _fieldInfoHandles;
        /// <summary>
        /// ������Ϣ����
        /// </summary>
        /// <param name="FieldInfoHandles">�ֶ���Ϣ�Ĺ�ϣ��</param>
        public ClassFieldInfoCollection(Dictionary<string, FieldInfoHandle> fieldInfoHandles) 
        {
            this._fieldInfoHandles = fieldInfoHandles;

        }

        /// <summary>
        /// �����ֶ�����ȡ�ֶ���Ϣ
        /// </summary>
        /// <param name="fieldName">�ֶ���</param>
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
        /// ��ǰ���Ե�����
        /// </summary>
        public int Count
        {
            get
            {
                return _fieldInfoHandles.Count;
            }
        }


        #region IEnumerable<FieldInfoHandle> ��Ա

        public IEnumerator<FieldInfoHandle> GetEnumerator()
        {
            return new ClassFieldEnumerator(_fieldInfoHandles);
        }

        #endregion

        #region IEnumerable ��Ա

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ClassFieldEnumerator(_fieldInfoHandles);
        }

        #endregion
    }
}
