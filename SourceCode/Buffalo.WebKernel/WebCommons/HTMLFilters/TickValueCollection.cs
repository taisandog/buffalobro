using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    /// <summary>
    /// 映射信息集合
    /// </summary>
    public class TickValueCollection : IEnumerable<TickValue>
    {
        private Dictionary<string, TickValue> tickValues;

        /// <summary>
        /// 映射信息集合
        /// </summary>
        /// <param name="propertyInfoHandles">映射信息的哈希表</param>
        public TickValueCollection(Dictionary<string, TickValue> tickValues)
        {
            this.tickValues = tickValues;
        }

        /// <summary>
        /// 根据属性名获取映射属性信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
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
        /// 当前属性的数量
        /// </summary>
        public int Count
        {
            get
            {
                return tickValues.Count;
            }
        }

        #region IEnumerable<TickValue> 成员

        public IEnumerator<TickValue> GetEnumerator()
        {
            return new TickValueEnumerator(tickValues);
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new TickValueEnumerator(tickValues);
        }

        #endregion
    }
}
