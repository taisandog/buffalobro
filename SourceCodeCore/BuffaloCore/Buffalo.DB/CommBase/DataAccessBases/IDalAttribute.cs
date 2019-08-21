using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    
    /// <summary>
    /// ���ݲ�ӿڵı�ǩ
    /// </summary>
    public class IDalAttribute:System.Attribute
    {
        private Type _interfaceType;
        public IDalAttribute(Type interfaceType) 
        {
            _interfaceType = interfaceType;
        }

        /// <summary>
        /// ʵ������
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
