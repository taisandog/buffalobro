using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.FastReflection
{
    /// <summary>
    /// 创建属性的信息类
    /// </summary>
    public class PropertyInfoHandle
    {
        private FastPropertyHandler _getHandle;
        private FastPropertyHandler _setHandle;
        private Type _propertyType;
        private string _propertyName;
        private Type _belong;
        /// <summary>
        /// 创建属性的信息类
        /// </summary>
        /// <param name="getHandle">get委托</param>
        /// <param name="setHandle">set委托</param>
        /// <param name="propertyType">属性数据类型</param>
        public PropertyInfoHandle(Type belong, FastPropertyHandler getHandle, FastPropertyHandler setHandle, Type propertyType, string propertyName)
        {
            this._getHandle = getHandle;
            this._setHandle = setHandle;
            this._propertyType = propertyType;
            this._propertyName = propertyName;
            _belong = belong;
        }
        /// <summary>
        /// 属性所属的类
        /// </summary>
        public Type Belong
        {
            get
            {
                return _belong;
            }
        }
        /// <summary>
        /// 获取属性的类型
        /// </summary>
        public Type PropertyType
        {
            get
            {
                return _propertyType;
            }
        }
        /// <summary>
        /// 获取属性的名字
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }

        /// <summary>
        /// 给对象设置值
        /// </summary>
        /// <param name="args">对象</param>
        /// <param name="value">值</param>
        public void SetValue(object args, object value) 
        {
            if (_setHandle == null) 
            {
                throw new Exception("此类型没有Set方法");
            }
            _setHandle(args,  value );
        }

        /// <summary>
        /// 获取对象值
        /// </summary>
        /// <param name="args">对象</param>
        public object GetValue(object args)
        {
            if (_getHandle == null)
            {
                throw new Exception("此类型没有Get方法");
            }
            return _getHandle(args, null);
        }
        ///// <summary>
        ///// 给对象设置值
        ///// </summary>
        ///// <param name="args">对象</param>
        ///// <param name="indexKey">索引</param>
        //public object GetValue(object args,object[] indexKey)
        //{
        //    if (getHandle == null)
        //    {
        //        throw new Exception("此类型没有Get方法");
        //    }
        //    return getHandle(args, new object[] { });
        //}
        /// <summary>
        /// 是否有Get方法
        /// </summary>
        public bool HasGetHandle 
        {
            get 
            {
                return _getHandle != null;
            }
        }

        /// <summary>
        /// 是否有Set方法
        /// </summary>
        public bool HasSetHandle
        {
            get
            {
                return _setHandle != null;
            }
        }
    }
}
