using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    /// <summary>
    /// ӳ���ö����
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

        #region IEnumerator<EntityPropertyInfo> ��Ա

        public TickValue Current
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
            Dispose();
            enumCurrent = dicCurrent.GetEnumerator();
        }

        #endregion
    }
}
