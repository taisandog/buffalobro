using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 属性的枚举类
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

        #region IEnumerator<EntityPropertyInfo> 成员

        public EntityPropertyInfo Current
        {
            get 
            {
                return _enumCurrent.Current.Value;
            }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            _enumCurrent.Dispose();
        }

        #endregion

        #region IEnumerator 成员

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
