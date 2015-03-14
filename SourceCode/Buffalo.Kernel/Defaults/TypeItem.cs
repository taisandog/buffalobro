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
        /// 数值类型
        /// </summary>
        public Type ItemType
        {
            get { return _itemType; }
        }
        private object _defaultValue;

        /// <summary>
        /// 数据的默认值
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
