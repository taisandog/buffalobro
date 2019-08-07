using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    /// <summary>
    /// 映射的枚举类
    /// </summary>
    public class TickValueEnumerator : IEnumerator<TickValue>
    {
        private Dictionary<string, TickValue>.Enumerator enumCurrent;
        private Dictionary<string, TickValue> dicCurrent;
        public TickValueEnumerator(Dictionary<string, TickValue> dicCurrent)
        {
            this.dicCurrent = dicCurrent;
            this.enumCurrent = dicCurrent.GetEnumerator();
        }

        #region IEnumerator<EntityPropertyInfo> 成员

        public TickValue Current
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
