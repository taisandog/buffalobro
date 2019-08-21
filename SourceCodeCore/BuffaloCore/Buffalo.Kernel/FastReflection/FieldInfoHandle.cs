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
        /// 创建属性的信息类
        /// </summary>
        /// <param name="belong">字段所属的类类型</param>
        /// <param name="getHandle">get委托</param>
        /// <param name="setHandle">set委托</param>
        /// <param name="fieldType">字段数据类型</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="belongFieldInfo">所属的字段反射信息</param>
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
        /// 判断类型是否跟本属性一致
        /// </summary>
        /// <param name="reader">读取器</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public bool TypeEqual(IDataReader reader, int index) 
        {
            return this._realFieldType.Equals(reader.GetFieldType(index));
        }

        /// <summary>
        /// 加载真正的数据类型
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
        /// 所属的字段反射信息
        /// </summary>
        public FieldInfo BelongFieldInfo
        {
            get { return _belongFieldInfo; }
        }
        /// <summary>
        /// 字段所属的类
        /// </summary>
        public Type Belong
        {
            get
            {
                return _belong;
            }
        }
        /// <summary>
        /// 真正的数值类型
        /// </summary>
        public Type RealFieldType
        {
            get { return _realFieldType; }
        }
        /// <summary>
        /// Set句柄
        /// </summary>
        public SetFieldValueHandle SetHandle
        {
            get { return _setHandle; }
        }
        /// <summary>
        /// Get句柄
        /// </summary>
        public GetFieldValueHandle GetHandle
        {
            get { return _getHandle; }
        }
        /// <summary>
        /// 获取属性的类型
        /// </summary>
        public Type FieldType
        {
            get
            {
                return _fieldType;
            }
        }
        /// <summary>
        /// 获取属性的名字
        /// </summary>
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }

        /// <summary>
        /// 给对象设置值
        /// </summary>
        /// <param name="args">对象</param>
        /// <param name="value">值</param>
        public virtual void SetValue(object args, object value) 
        {
            if (_setHandle == null) 
            {
                throw new Exception("此类型没有Set方法");
            }
            _setHandle(args, value);
        }

        /// <summary>
        /// 获取对象值
        /// </summary>
        /// <param name="args">对象</param>
        /// <param name="value">值</param>
        public virtual object GetValue(object args)
        {
            if (_getHandle == null)
            {
                throw new Exception("此类型没有Get方法");
            }
            return _getHandle(args);
        }

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

        /// <summary>
        /// 获取字段集合
        /// </summary>
        /// <param name="objType">类型</param>
        /// <param name="inner">是否</param>
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
        /// 填充值
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
