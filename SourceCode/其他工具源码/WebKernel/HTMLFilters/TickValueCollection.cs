using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    /// <summary>
    /// ӳ����Ϣ����
    /// </summary>
    public class TickValueCollection : IEnumerable<TickValue>
    {
        private Dictionary<string, TickValue> tickValues;

        /// <summary>
        /// ӳ����Ϣ����
        /// </summary>
        /// <param name="propertyInfoHandles">ӳ����Ϣ�Ĺ�ϣ��</param>
        public TickValueCollection(Dictionary<string, TickValue> tickValues)
        {
            this.tickValues = tickValues;
        }

        /// <summary>
        /// ������������ȡӳ��������Ϣ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public TickValue this[string propertyName]
        {
            get
            {
                if (tickValues != null)
                {
                    TickValue ret = null;
                    if (tickValues.TryGetValue(propertyName, out ret))
                    {
                        return ret;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// ��ǰ���Ե�����
        /// </summary>
        public int Count
        {
            get
            {
                return tickValues.Count;
            }
        }

        #region IEnumerable<TickValue> ��Ա

        public IEnumerator<TickValue> GetEnumerator()
        {
            return new TickValueEnumerator(tickValues);
        }

        #endregion

        #region IEnumerable ��Ա

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new TickValueEnumerator(tickValues);
        }

        #endregion
    }
}
