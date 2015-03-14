using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.Commons;
using System.Reflection;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 实体的属性信息
    /// </summary>
    public class EntityPropertyInfo:FieldInfoHandle
    {
        private EntityParam _paramInfo;
        private bool _readOnly;
        private EntityInfoHandle _belong;
        private PropertyInfo _belongPropertyInfo;


        /// <summary>
        /// 创建属性的信息类
        /// </summary>
        /// <param name="belong">所属的实体信息</param>
        /// <param name="getHandle">get委托</param>
        /// <param name="setHandle">set委托</param>
        /// <param name="ep">字段标识类</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="sourceType">源字段类型</param>
        public EntityPropertyInfo(EntityInfoHandle belong, GetFieldValueHandle getHandle, SetFieldValueHandle setHandle,
            EntityParam paramInfo, Type fieldType, string fieldName, FieldInfo belongFieldInfo, PropertyInfo belongPropertyInfo)
            : base(belong.EntityType, getHandle, setHandle, fieldType, fieldName, belongFieldInfo)
        {
            paramInfo.SqlType = belong.DBInfo.CurrentDbAdapter.ToCurrentDbType(paramInfo.SqlType);//转换成本数据库支持的数据类型
            this._paramInfo = paramInfo;
            _belong = belong;
            _belongPropertyInfo = belongPropertyInfo;
        }



        /// <summary>
        /// 所属的属性信息
        /// </summary>
        public PropertyInfo BelongPropertyInfo
        {
            get { return _belongPropertyInfo; }
        }

        /// <summary>
        /// 只读
        /// </summary>
        public bool ReadOnly 
        {
            get
            {
                return _paramInfo.ReadOnly;
            }
        }
        


        /// <summary>
        /// 字段配置信息
        /// </summary>
        public EntityParam ParamInfo
        {
            get { return _paramInfo; }
            internal set { _paramInfo = value; }
        }
        /// <summary>
        /// 返回拷贝副本
        /// </summary>
        /// <param name="belong">新副本所属的实体</param>
        /// <returns></returns>
        public EntityPropertyInfo Copy(EntityInfoHandle belong) 
        {

            EntityPropertyInfo info = new EntityPropertyInfo(belong, GetHandle,
                SetHandle, _paramInfo, _fieldType, _fieldName,BelongFieldInfo,BelongPropertyInfo);
            return info;
        }

        /// <summary>
        /// 所属实体类型
        /// </summary>
        public EntityInfoHandle BelongInfo
        {
            get 
            {
                return _belong;
            }
        }

        /// <summary>
        /// 对应的属性名
        /// </summary>
        public string PropertyName 
        {
            get 
            {
                return _paramInfo.PropertyName;
            }
        }

        /// <summary>
        /// 属性注释
        /// </summary>
        public string Description
        {
            get
            {
                return _paramInfo.Description;
            }
        }
        /// <summary>
        /// 获取对应的字段的SQL类型
        /// </summary>
        public DbType SqlType
        {
            get
            {
                return _paramInfo.SqlType;
            }
        }

        /// <summary>
        /// 获取对应的字段的名字
        /// </summary>
        public string ParamName
        {
            get
            {
                return _paramInfo.ParamName;
            }
        }

        /// <summary>
        /// 获取对应的字段是否主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return _paramInfo.IsPrimaryKey;
            }
        }

        /// <summary>
        /// 是否自动增长字段
        /// </summary>
        public bool Identity
        {
            get
            {
                return _paramInfo.Identity;
            }
        }

        /// <summary>
        /// 是否自动普通字段
        /// </summary>
        public bool IsNormal
        {
            get
            {
                return _paramInfo.IsNormal;
            }
        }
        
        /// <summary>
        /// 是否版本信息字段
        /// </summary>
        public bool IsVersion
        {
            get
            {
                return _paramInfo.IsVersion;
            }
        }
    }
}
