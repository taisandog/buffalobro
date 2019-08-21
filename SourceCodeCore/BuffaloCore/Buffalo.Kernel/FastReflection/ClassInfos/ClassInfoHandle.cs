using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// �����Ϣ
    /// </summary>
    public class ClassInfoHandle 
    {
        private Type _classType;
        private CreateInstanceHandler _createInstanceHandler;
        private ClassPropertyInfoCollection _propertyInfoHandles;
        private ClassFieldInfoCollection _fieldInfoHandles;

        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="classType">������</param>
        /// <param name="createInstanceHandler">ʵ������ľ��</param>
        /// <param name="propertyInfoHandles">���Լ���</param>
        /// <param name="fieldInfoHandles">�ֶμ���</param>
        internal ClassInfoHandle(Type classType, CreateInstanceHandler createInstanceHandler,
            Dictionary<string, PropertyInfoHandle> propertyInfoHandles, Dictionary<string, FieldInfoHandle> fieldInfoHandles) 
        {
            this._classType = classType;
            this._createInstanceHandler = createInstanceHandler;
            this._propertyInfoHandles = new ClassPropertyInfoCollection(propertyInfoHandles);
            this._fieldInfoHandles = new ClassFieldInfoCollection(fieldInfoHandles);
        }
        /// <summary>
        /// ���������
        /// </summary>
        public Type ClassType 
        {
            get 
            {
                return _classType;
                
            }
            
        }

        /// <summary>
        /// ��ȡ���Ե���Ϣ
        /// </summary>
        public ClassPropertyInfoCollection PropertyInfo
        {
            get 
            {
                return _propertyInfoHandles;
            }
        }
        /// <summary>
        /// ��ȡ�ֶε���Ϣ
        /// </summary>
        public ClassFieldInfoCollection FieldInfo
        {
            get
            {
                return _fieldInfoHandles;
            }
        }
        /// <summary>
        /// ���ش����͵�ʵ��
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
