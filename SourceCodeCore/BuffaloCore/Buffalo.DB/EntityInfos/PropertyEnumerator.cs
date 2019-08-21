using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// ���Ե�ö����
    /// </summary>
    public class PropertyEnumerator : IEnumerator<EntityPropertyInfo>
    {
        private IEnumerator<KeyValuePair<string, EntityPropertyInfo>> _enumCurrent;
        private Dictionary<string, EntityPropertyInfo> _dicCurrent;
        public PropertyEnumerator(Dictionary<string, EntityPropertyInfo> dicCurrent)
        {
            this._dicCurrent = dicCurrent;
            this._enumCurrent = dicCurrent.GetEnumerator();
        }

        #region IEnumerator<EntityPropertyInfo> ��Ա

        public EntityPropertyInfo Current
        {
            get 
            {
                return _enumCurrent.Current.Value;
            }
        }

        #endregion

        #region IDisposable ��Ա

        public void Dispose()
        {
            _enumCurrent.Dispose();
        }

        #endregion

        #region IEnumerator ��Ա

        object System.Collections.IEnumerator.Current
        {
            get 
            {
                return _enumCurrent.Current.Value;
            }
        }

        public bool MoveNext()
        {
            return _enumCurrent.MoveNext();
        }

        public void Reset()
        {
            _enumCurrent = _dicCurrent.GetEnumerator();
        }

        #endregion
    }
}
