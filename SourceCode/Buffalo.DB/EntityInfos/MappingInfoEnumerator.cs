using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 映射的枚举类
    /// </summary>
    public class MappingInfoEnumerator : IEnumerator<EntityMappingInfo>
    {
        private Dictionary<string, EntityMappingInfo>.Enumerator enumCurrent;
        private Dictionary<string, EntityMappingInfo> dicCurrent;
        public MappingInfoEnumerator(Dictionary<string, EntityMappingInfo> dicCurrent)
        {
            this.dicCurrent = dicCurrent;
            this.enumCurrent = dicCurrent.GetEnumerator();
        }

        #region IEnumerator<EntityPropertyInfo> 成员

        public EntityMappingInfo Current
        {
            get
            {
                return enumCurrent.Current.Value;
            }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            enumCurrent.Dispose();
        }

        #endregion

        #region IEnumerator 成员

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
            Dispose();
            enumCurrent = dicCurrent.GetEnumerator();
        }

        #endregion
    }
}
