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
    /// ʵ��ӳ���������Ϣ
    /// </summary>
    public class EntityMappingInfo : FieldInfoHandle
    {
        private TableRelationAttribute _mappingInfo;

        private PropertyInfo _belongPropertyInfo;
        /// <summary>
        /// �������Ե���Ϣ��
        /// </summary>
        /// <param name="belong">������ʵ��</param>
        /// <param name="getHandle">getί��</param>
        /// <param name="setHandle">setί��</param>
        /// <param name="tableMappingAtt">ӳ���ʶ��</param>
        /// <param name="fieldName">������</param>
        /// <param name="fieldType">��������</param>
        public EntityMappingInfo(Type belong, GetFieldValueHandle getHandle, SetFieldValueHandle setHandle,
            TableRelationAttribute mappingInfo, string fieldName, Type fieldType, FieldInfo belongFieldInfo, 
            PropertyInfo belongPropertyInfo)

            : base(belong, getHandle, setHandle, fieldType, fieldName, belongFieldInfo)
        {
            this._mappingInfo = mappingInfo;
            this._belongPropertyInfo = belongPropertyInfo;
        }
        /// <summary>
        /// ������������Ϣ
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
        /// ӳ����Ϣ
        /// </summary>
        public TableRelationAttribute MappingInfo
        {
            get { return _mappingInfo; }
            internal set { _mappingInfo = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="belong">����ʵ��</param>
        /// <returns></returns>
        public EntityMappingInfo Copy(Type belong) 
        {
            EntityMappingInfo info = new EntityMappingInfo(
                belong, GetHandle, SetHandle, _mappingInfo, _fieldName, _fieldType,BelongFieldInfo,BelongPropertyInfo);
            return info;
        }

        /// <summary>
        /// ��Ӧ��������
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _mappingInfo.PropertyName;
            }
        }
        /// <summary>
        /// ��Ӧ������ע��
        /// </summary>
        public string Description
        {
            get
            {
                return _mappingInfo.Description;
            }
        }
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsParent
        {
            get
            {
                return _mappingInfo.IsParent;
            }
        }

        /// <summary>
        /// Դ����
        /// </summary>
        public EntityPropertyInfo SourceProperty
        {
            get
            {

                return _mappingInfo.SourceProperty;
            }
        }
        /// <summary>
        /// Ŀ������
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
