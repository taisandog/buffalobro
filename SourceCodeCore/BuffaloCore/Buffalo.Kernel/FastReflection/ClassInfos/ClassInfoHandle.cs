using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// 类的信息
    /// </summary>
    public class ClassInfoHandle 
    {
        private Type _classType;
        private CreateInstanceHandler _createInstanceHandler;
        private ClassPropertyInfoCollection _propertyInfoHandles;
        private ClassFieldInfoCollection _fieldInfoHandles;

        /// <summary>
        /// 类的信息
        /// </summary>
        /// <param name="classType">类类型</param>
        /// <param name="createInstanceHandler">实例化类的句柄</param>
        /// <param name="propertyInfoHandles">属性集合</param>
        /// <param name="fieldInfoHandles">字段集合</param>
        internal ClassInfoHandle(Type classType, CreateInstanceHandler createInstanceHandler,
            Dictionary<string, PropertyInfoHandle> propertyInfoHandles, Dictionary<string, FieldInfoHandle> fieldInfoHandles) 
        {
            this._classType = classType;
            this._createInstanceHandler = createInstanceHandler;
            this._propertyInfoHandles = new ClassPropertyInfoCollection(propertyInfoHandles);
            this._fieldInfoHandles = new ClassFieldInfoCollection(fieldInfoHandles);
        }
        /// <summary>
        /// 本类的类型
        /// </summary>
        public Type ClassType 
        {
            get 
            {
                return _classType;
                
            }
            
        }

        /// <summary>
        /// 获取属性的信息
        /// </summary>
        public ClassPropertyInfoCollection PropertyInfo
        {
            get 
            {
                return _propertyInfoHandles;
            }
        }
        /// <summary>
        /// 获取字段的信息
        /// </summary>
        public ClassFieldInfoCollection FieldInfo
        {
            get
            {
                return _fieldInfoHandles;
            }
        }
        /// <summary>
        /// 返回此类型的实例
        /// </summary>
        /// <returns></returns>
        public object CreateInstance() 
        {
            if (_createInstanceHandler != null) 
            {
                return _createInstanceHandler.Invoke();
            }
            return null;
        }
    }
}
