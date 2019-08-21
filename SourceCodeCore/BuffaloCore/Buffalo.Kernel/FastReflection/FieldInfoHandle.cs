using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Buffalo.Kernel.Defaults;
using System.Data;

namespace Buffalo.Kernel.FastReflection
{
    public class FieldInfoHandle
    {
        private GetFieldValueHandle _getHandle;


        private SetFieldValueHandle _setHandle;

        private FieldInfo _belongFieldInfo;


        protected Type _fieldType;
        protected string _fieldName;
        protected Type _belong;
        private Type _realFieldType;

        
        /// <summary>
        /// �������Ե���Ϣ��
        /// </summary>
        /// <param name="belong">�ֶ�������������</param>
        /// <param name="getHandle">getί��</param>
        /// <param name="setHandle">setί��</param>
        /// <param name="fieldType">�ֶ���������</param>
        /// <param name="fieldName">�ֶ���</param>
        /// <param name="belongFieldInfo">�������ֶη�����Ϣ</param>
        public FieldInfoHandle(Type belong, GetFieldValueHandle getHandle,
            SetFieldValueHandle setHandle, Type fieldType, string fieldName, FieldInfo belongFieldInfo)
        {
            this._getHandle = getHandle;
            this._setHandle = setHandle;
            this._fieldType = fieldType;
            this._fieldName = fieldName;
            this._belong = belong;
            this._belongFieldInfo = belongFieldInfo;

            LoadRealFieldType();
        }

        /// <summary>
        /// �ж������Ƿ��������һ��
        /// </summary>
        /// <param name="reader">��ȡ��</param>
        /// <param name="index">����</param>
        /// <returns></returns>
        public bool TypeEqual(IDataReader reader, int index) 
        {
            return this._realFieldType.Equals(reader.GetFieldType(index));
        }

        /// <summary>
        /// ������������������
        /// </summary>
        private void LoadRealFieldType() 
        {
            if (_fieldType.IsEnum)
            {
                _realFieldType =Enum.GetUnderlyingType(_fieldType);
                return;
            }

            this._realFieldType = DefaultType.GetRealValueType(_fieldType);
            
        }

        /// <summary>
        /// �������ֶη�����Ϣ
        /// </summary>
        public FieldInfo BelongFieldInfo
        {
            get { return _belongFieldInfo; }
        }
        /// <summary>
        /// �ֶ���������
        /// </summary>
        public Type Belong
        {
            get
            {
                return _belong;
            }
        }
        /// <summary>
        /// ��������ֵ����
        /// </summary>
        public Type RealFieldType
        {
            get { return _realFieldType; }
        }
        /// <summary>
        /// Set���
        /// </summary>
        public SetFieldValueHandle SetHandle
        {
            get { return _setHandle; }
        }
        /// <summary>
        /// Get���
        /// </summary>
        public GetFieldValueHandle GetHandle
        {
            get { return _getHandle; }
        }
        /// <summary>
        /// ��ȡ���Ե�����
        /// </summary>
        public Type FieldType
        {
            get
            {
                return _fieldType;
            }
        }
        /// <summary>
        /// ��ȡ���Ե�����
        /// </summary>
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }

        /// <summary>
        /// ����������ֵ
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="value">ֵ</param>
        public virtual void SetValue(object args, object value) 
        {
            if (_setHandle == null) 
            {
                throw new Exception("������û��Set����");
            }
            _setHandle(args, value);
        }

        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="value">ֵ</param>
        public virtual object GetValue(object args)
        {
            if (_getHandle == null)
            {
                throw new Exception("������û��Get����");
            }
            return _getHandle(args);
        }

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

        /// <summary>
        /// ��ȡ�ֶμ���
        /// </summary>
        /// <param name="objType">����</param>
        /// <param name="inner">�Ƿ�</param>
        /// <returns></returns>
        public static List<FieldInfoHandle> GetFieldInfos(Type objType, BindingFlags flags, bool fillBase)
        {
            List<FieldInfoHandle> lstRet = new List<FieldInfoHandle>();

            Type curType = objType;

            //Queue<Type> stkType = new Stack<Type>();
            //stkType.Push(curType);
            //if (fillBase) 
            //{
            //    curType = curType.BaseType;
            //    while (curType != null) 
            //    {
            //        stkType.Push(curType);
            //        curType = curType.BaseType;
            //    }
            //}
            Dictionary<string, bool> dicExists = new Dictionary<string, bool>();
            while (curType!=null)
            {
                
                FillFieldInfos(curType, flags, lstRet,dicExists);
                curType = curType.BaseType;
            }
            return lstRet;
        }
        /// <summary>
        /// ���ֵ
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="flags"></param>
        /// <param name="fillBase"></param>
        private static void FillFieldInfos(Type objType, BindingFlags flags, List<FieldInfoHandle> lstRet, Dictionary<string, bool> dicExists) 
        {
            FieldInfo[] infos = objType.GetFields(flags);
            foreach (FieldInfo info in infos)
            {
                if (dicExists.ContainsKey(info.Name)) 
                {
                    continue;
                }
                
                GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(info);
                SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(info);
                FieldInfoHandle handle = new FieldInfoHandle(objType, getHandle, setHandle, info.FieldType, info.Name,info);
                lstRet.Add(handle);
                dicExists[info.Name] = true;
            }
        }
    }
}
