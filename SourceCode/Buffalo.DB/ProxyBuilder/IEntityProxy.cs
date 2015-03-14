using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.ProxyBuilder
{
    public interface IEntityProxy
    {
        /// <summary>
        /// 获取被代理的实体类型
        /// </summary>
        /// <returns></returns>
        Type GetEntityType();
    }
}
