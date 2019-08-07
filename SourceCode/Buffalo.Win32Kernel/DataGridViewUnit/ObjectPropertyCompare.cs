using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

/** 
 * @ԭ����:�շɡ�Perky Su
 * @����ʱ��:2010-02-04 02:05
 * @����:http://www.cnblogs.com/sufei/archive/2010/02/04/1663125.html
 * @˵��:ʵ�����ԶԱ���
*/
namespace Buffalo.Win32Kernel.DataGridViewUnit
{
    /// <summary>
    /// ʵ�����ԶԱ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPropertyCompare<T> : IComparer<T>
    {
        private PropertyDescriptor property;
        private ListSortDirection direction;

        public ObjectPropertyCompare(PropertyDescriptor property, ListSortDirection direction)
        {
            this.property = property;
            this.direction = direction;
        }

        #region IComparer<T>

        /// <summary>
        /// �ȽϷ���
        /// </summary>
        /// <param name="x">�������x</param>
        /// <param name="y">�������y</param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            object xValue = x.GetType().GetProperty(property.Name).GetValue(x, null);
            object yValue = y.GetType().GetProperty(property.Name).GetValue(y, null);

            int returnValue;

            if (xValue is IComparable)
            {
                returnValue = ((IComparable)xValue).CompareTo(yValue);
            }
            else if (xValue.Equals(yValue))
            {
                returnValue = 0;
            }
            else
            {
                returnValue = xValue.ToString().CompareTo(yValue.ToString());
            }

            if (direction == ListSortDirection.Ascending)
            {
                return returnValue;
            }
            else
            {
                return returnValue * -1;
            }
        }

        public bool Equals(T xWord, T yWord)
        {
            return xWord.Equals(yWord);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
