using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.FastReflection
{
    /// <summary>
    /// �������Ե���Ϣ��
    /// </summary>
    public class PropertyInfoHandle
    {
        private FastPropertyHandler _getHandle;
        private FastPropertyHandler _setHandle;
        private Type _propertyType;
        private string _propertyName;
        private Type _belong;
        /// <summary>
        /// �������Ե���Ϣ��
        /// </summary>
        /// <param name="getHandle">getί��</param>
        /// <param name="setHandle">setί��</param>
        /// <param name="propertyType">������������</param>
        public PropertyInfoHandle(Type belong, FastPropertyHandler getHandle, FastPropertyHandler setHandle, Type propertyType, string propertyName)
        {
            this._getHandle = getHandle;
            this._setHandle = setHandle;
            this._propertyType = propertyType;
            this._propertyName = propertyName;
            _belong = belong;
        }
        /// <summary>
        /// ������������
        /// </summary>
        public Type Belong
        {
            get
            {
                return _belong;
            }
        }
        /// <summary>
        /// ��ȡ���Ե�����
        /// </summary>
        public Type PropertyType
        {
            get
            {
                return _propertyType;
            }
        }
        /// <summary>
        /// ��ȡ���Ե�����
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }

        /// <summary>
        /// ����������ֵ
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="value">ֵ</param>
        public void SetValue(object args, object value) 
        {
            if (_setHandle == null) 
            {
                throw new Exception("������û��Set����");
            }
            _setHandle(args,  value );
        }

        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        /// <param name="args">����</param>
        public object GetValue(object args)
        {
            if (_getHandle == null)
            {
                throw new Exception("������û��Get����");
            }
            return _getHandle(args, null);
        }
        ///// <summary>
        ///// ����������ֵ
        ///// </summary>
        ///// <param name="args">����</param>
        ///// <param name="indexKey">����</param>
        //public object GetValue(object args,object[] indexKey)
        //{
        //    if (getHandle == null)
        //    {
        //        throw new Exception("������û��Get����");
        //    }
        //    return getHandle(args, new object[] { });
        //}
        /// <summary>
        /// �Ƿ���Get����
        /// </summary>
        public bool HasGetHandle 
        {
            get 
            {
                return _getHandle != null;
            }
        }

        /// <summary>
        /// �Ƿ���Set����
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
