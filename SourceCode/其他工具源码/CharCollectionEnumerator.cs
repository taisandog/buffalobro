using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Buffalo.Kernel
{
    public class CharCollectionEnumerator : ICloneable, IEnumerator<char>, IDisposable
    {
        private string _chrs = null;
        private int _curIndex;
        /// <summary>
        /// ��ǰ����
        /// </summary>
        public int CurIndex
        {
            get { return _curIndex; }
        }
        /// <summary>
        /// �ַ����ϵ�ö����
        /// </summary>
        /// <param name="chrs"></param>
        public CharCollectionEnumerator(string chrs) 
        {
            _curIndex = -1;
            _chrs = chrs;
        }
        /// <summary>
        /// ��ǰ�ַ�
        /// </summary>
        public char CurrentChar
        {
            get { return _chrs[_curIndex]; }
        }


        #region IEnumerator ��Ա

        public object Current
        {
            get { return _chrs[_curIndex]; }
        }

        public bool MoveNext()
        {
            if (_curIndex >= _chrs.Length - 1) 
            {
                return false;
            }
            _curIndex++;
            return true;
        }

        public void Reset()
        {
            _curIndex = -1;
        }

        #endregion

        #region IEnumerator<char> ��Ա

        char IEnumerator<char>.Current
        {
            get { return _chrs[_curIndex]; }
        }

        #endregion

        #region IDisposable ��Ա

        public void Dispose()
        {
            
        }

        #endregion

        #region ICloneable ��Ա

        public object Clone()
        {
            return new CharCollectionEnumerator(_chrs);
        }

        #endregion

        #region IDisposable ��Ա

        void IDisposable.Dispose()
        {
            
        }

        #endregion
    }
}
