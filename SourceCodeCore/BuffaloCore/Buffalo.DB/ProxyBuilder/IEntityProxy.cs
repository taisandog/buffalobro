using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.ProxyBuilder
{
    public interface IEntityProxy
    {
        /// <summary>
        /// ��ȡ�������ʵ������
        /// </summary>
        /// <returns></returns>
        Type GetEntityType();
    }
}
