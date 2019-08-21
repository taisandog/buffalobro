using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.CommBase
{
    /// <summary>
    /// ���ԵĹ���������Ϣ
    /// </summary>
    public class UpdatePropertyInfo
    {
        public UpdatePropertyInfo() { }

        /// <summary>
        /// ���ԵĹ���������Ϣ
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="isEntityProperty"></param>
        public UpdatePropertyInfo(EntityMappingInfo mapInfo, bool isEntityProperty) 
        {
            _mapInfo = mapInfo;
            _isEntityProperty = isEntityProperty;
        }


        private bool _isEntityProperty;

        /// <summary>
        /// �Ƿ�ʵ������
        /// </summary>
        public bool IsEntityProperty
        {
            get { return _isEntityProperty; }
            set { _isEntityProperty = value; }
        }

        

        private EntityMappingInfo _mapInfo;
        /// <summary>
        /// ������ӳ��
        /// </summary>
        public EntityMappingInfo MapInfo
        {
            get { return _mapInfo; }
            set { _mapInfo = value; }
        }

        /// <summary>
        /// ����ʵ������
        /// </summary>
        /// <param name="entity"></param>
        public string UpdateProperty(EntityBase entity) 
        {
            if (_isEntityProperty) 
            {
                return UpdateChildProperty(entity);
            }
            return ClearParentProperty(entity);
        }

        /// <summary>
        /// ��ո�����
        /// </summary>
        private string ClearParentProperty(EntityBase entity)
        {
            bool ret = false;
            _mapInfo.SetValue(entity, null);
            entity.GetEntityBaseInfo()._dicUpdateProperty___.TryRemove(_mapInfo.PropertyName,out ret);
            return null;
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="mapInfo"></param>
        private string UpdateChildProperty(EntityBase entity)
        {

            object parentObject = _mapInfo.GetValue(entity);
            object pkValue = null;
            if (parentObject != null)
            {
                pkValue = _mapInfo.TargetProperty.GetValue(parentObject);//��ȡID
            }
            else 
            {
                pkValue = _mapInfo.SourceProperty.FieldType.IsValueType ? Activator.CreateInstance(_mapInfo.SourceProperty.FieldType) : null;
            }
            if (pkValue != null && pkValue.GetType() != _mapInfo.SourceProperty.FieldType)
            {
                pkValue = Convert.ChangeType(pkValue, _mapInfo.SourceProperty.FieldType);
            }
            _mapInfo.SourceProperty.SetValue(entity, pkValue);
            return _mapInfo.TargetProperty.PropertyName;

        }
    }
}
