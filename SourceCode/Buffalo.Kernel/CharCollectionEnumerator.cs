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
        /// 当前索引
        /// </summary>
        public int CurIndex
        {
            get { return _curIndex; }
        }
        /// <summary>
        /// 字符集合的枚举器
        /// </summary>
        /// <param name="chrs"></param>
        public CharCollectionEnumerator(string chrs) 
        {
            _curIndex = -1;
            _chrs = chrs;
        }
        /// <summary>
        /// 当前字符
        /// </summary>
        public char CurrentChar
        {
            get { return _chrs[_curIndex]; }
        }


        #region IEnumerator 成员

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

        #region IEnumerator<char> 成员

        char IEnumerator<char>.Current
        {
            get { return _chrs[_curIndex]; }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            
        }

        #endregion

        #region ICloneable 成员

        public object Clone()
        {
            return new CharCollectionEnumerator(_chrs);
        }

        #endregion

        #region IDisposable 成员

        void IDisposable.Dispose()
        {
            
        }

        #endregion
    }
}
