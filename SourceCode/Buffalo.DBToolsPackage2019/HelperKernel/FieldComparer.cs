using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.HelperKernel
{
    public class FieldComparer<T> : IComparer<T> where T : EntityFieldBase
    {

        #region IComparer<EntityFieldBase> ��Ա

        //�Ƚ��������󲢷���һ��ֵ��ָʾһ��������С�ڡ����ڻ��Ǵ�����һ������
        public int Compare(T x, T y)
        {
            //��Ϊ�ǵ����������x��ʱ�����y��ʱ�䣬���ظ���
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
