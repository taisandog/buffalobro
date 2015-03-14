using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 属性的枚举类
    /// </summary>
    public class PropertyEnumerator : IEnumerator<EntityPropertyInfo>
    {
        private Dictionary<string, EntityPropertyInfo>.Enumerator enumCurrent;
        private Dictionary<string, EntityPropertyInfo> dicCurrent;
        public PropertyEnumerator(Dictionary<string, EntityPropertyInfo> dicCurrent)
        {
            this.dicCurrent = dicCurrent;
            this.enumCurrent = dicCurrent.GetEnumerator();
        }

        #region IEnumerator<EntityPropertyInfo> 成员

        public EntityPropertyInfo Current
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
            enumCurrent = dicCurrent.GetEnumerator();
        }

        #endregion
    }
}
