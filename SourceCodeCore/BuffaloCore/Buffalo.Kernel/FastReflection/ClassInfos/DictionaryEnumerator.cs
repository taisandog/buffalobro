using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    public class DictionaryEnumerator<T,K>:IEnumerator<K>
    {
        private Dictionary<T, K>.Enumerator enumCurrent;
        private Dictionary<T, K> dicCurrent;
        public DictionaryEnumerator(Dictionary<T, K> dicCurrent)
        {
            this.dicCurrent = dicCurrent;
            this.enumCurrent = dicCurrent.GetEnumerator();
        }

        #region IEnumerator<K> ��Ա

        public K Current
        {
            get 
            {
                return enumCurrent.Current.Value;
            }
        }

        #endregion

        #region IDisposable ��Ա

        public void Dispose()
        {
            enumCurrent.Dispose();
        }

        #endregion

        #region IEnumerator ��Ա

        object System.Collections.IEnumerator.Current
        {
            get 
            {
                return enumCurrent.Current.Value;
            }
        }

        public bool MoveNext()
        {
            return enumCurrent.MoveNext();
        }

        public void Reset()
        {
            enumCurrent = dicCurrent.GetEnumerator();
        }

        #endregion
    }
}
