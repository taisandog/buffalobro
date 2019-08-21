using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.Kernel.Defaults
{
    public class TypeItem
    {
        public TypeItem(Type itemType, object defaultValue,DbType dbType) 
        {
            _itemType = itemType;
            _defaultValue = defaultValue;
            _dbType = dbType;
        }

        private Type _itemType;

        /// <summary>
        /// ��ֵ����
        /// </summary>
        public Type ItemType
        {
            get { return _itemType; }
        }
        private object _defaultValue;

        /// <summary>
        /// ���ݵ�Ĭ��ֵ
        /// </summary>
        public object DefaultValue
        {
            get { return _defaultValue; }
        }


        private DbType _dbType;
        public DbType DbType 
        {
            get 
            {
                return _dbType;
            }
        }
    }
}
