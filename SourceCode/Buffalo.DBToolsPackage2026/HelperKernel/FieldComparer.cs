using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.HelperKernel
{
    public class FieldComparer<T> : IComparer<T> where T : EntityFieldBase
    {

        #region IComparer<EntityFieldBase> 成员

        //比较两个对象并返回一个值，指示一个对象是小于、等于还是大于另一个对象。
        public int Compare(T x, T y)
        {
            //因为是倒序，所以如果x的时间大于y的时间，返回负数
            if (x.StarLine < y.StarLine)
            {
                return -1;
            }
            else if (x.StarLine > y.StarLine)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        #endregion
    }
}
