using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel.FastReflection;
using System.Reflection;
using Buffalo.DB.CommBase;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 实体映射的属性信息
    /// </summary>
    public class EntityMappingInfo : FieldInfoHandle
    {
        private TableRelationAttribute _mappingInfo;

        private PropertyInfo _belongPropertyInfo;
        /// <summary>
        /// 创建属性的信息类
        /// </summary>
        /// <param name="belong">所属的实体</param>
        /// <param name="getHandle">get委托</param>
        /// <param name="setHandle">set委托</param>
        /// <param name="tableMappingAtt">映射标识类</param>
        /// <param name="fieldName">属性名</param>
        /// <param name="fieldType">属性类型</param>
        public EntityMappingInfo(Type belong, GetFieldValueHandle getHandle, SetFieldValueHandle setHandle,
            TableRelationAttribute mappingInfo, string fieldName, Type fieldType, FieldInfo belongFieldInfo, 
            PropertyInfo belongPropertyInfo)

            : base(belong, getHandle, setHandle, fieldType, fieldName, belongFieldInfo)
        {
            this._mappingInfo = mappingInfo;
            this._belongPropertyInfo = belongPropertyInfo;
        }
        /// <summary>
        /// 所属的属性信息
        /// </summary>
        public PropertyInfo BelongPropertyInfo
        {
            get { return _belongPropertyInfo; }
        }
        public override void SetValue(object args, object value)
        {
            EntityBase obj = args as EntityBase;
            if (obj != null)
            {
                obj.OnPropertyUpdated(PropertyName);
            }
            base.SetValue(args, value);
        }
        /// <summary>
        /// 映射信息
        /// </summary>
        public TableRelationAttribute MappingInfo
        {
            get { return _mappingInfo; }
            internal set { _mappingInfo = value; }
        }
        /// <summary>
        /// 拷贝副本
        /// </summary>
        /// <param name="belong">所属实体</param>
        /// <returns></returns>
        public EntityMappingInfo Copy(Type belong) 
        {
            EntityMappingInfo info = new EntityMappingInfo(
                belong, GetHandle, SetHandle, _mappingInfo, _fieldName, _fieldType,BelongFieldInfo,BelongPropertyInfo);
            return info;
        }

        /// <summary>
        /// 对应的属性名
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _mappingInfo.PropertyName;
            }
        }
        /// <summary>
        /// 对应的属性注释
        /// </summary>
        public string Description
        {
            get
            {
                return _mappingInfo.Description;
            }
        }
        /// <summary>
        /// 是否主表属性
        /// </summary>
        public bool IsParent
        {
            get
            {
                return _mappingInfo.IsParent;
            }
        }

        /// <summary>
        /// 源属性
        /// </summary>
        public EntityPropertyInfo SourceProperty
        {
            get
            {

                return _mappingInfo.SourceProperty;
            }
        }
        /// <summary>
        /// 目标属性
        /// </summary>
        public EntityPropertyInfo TargetProperty
        {
            get
            {

                return _mappingInfo.TargetProperty;
            }
        }
    }
}
