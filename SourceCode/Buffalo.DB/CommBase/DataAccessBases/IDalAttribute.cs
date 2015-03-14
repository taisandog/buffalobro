using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    
    /// <summary>
    /// 数据层接口的标签
    /// </summary>
    public class IDalAttribute:System.Attribute
    {
        private Type _interfaceType;
        public IDalAttribute(Type interfaceType) 
        {
            _interfaceType = interfaceType;
        }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type InterfaceType 
        {
            get 
            {
                return _interfaceType;
            }
        }
    }
}
