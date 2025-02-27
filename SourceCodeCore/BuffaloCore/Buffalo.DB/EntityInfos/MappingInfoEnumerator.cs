using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 映射的枚举类
    /// </summary>
    public class MappingInfoEnumerator : IEnumerator<EntityMappingInfo>
    {
        
        private IEnumerator<KeyValuePair<string, EntityMappingInfo>> _enumCurrent;
        private Dictionary<string, EntityMappingInfo> _dicCurrent;
        public MappingInfoEnumerator(Dictionary<string, EntityMappingInfo> dicCurrent)
        {
            this._dicCurrent = dicCurrent;
            this._enumCurrent = dicCurrent.GetEnumerator();
        }

        #region IEnumerator<EntityPropertyInfo> 成员

        public EntityMappingInfo Current
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
            Dispose();
            _enumCurrent = _dicCurrent.GetEnumerator();
        }

        #endregion
    }
}
