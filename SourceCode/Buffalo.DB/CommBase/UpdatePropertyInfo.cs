using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.CommBase
{
    /// <summary>
    /// 属性的关联更新信息
    /// </summary>
    public class UpdatePropertyInfo
    {
        public UpdatePropertyInfo() { }

        /// <summary>
        /// 属性的关联更新信息
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
        /// 是否父实体属性
        /// </summary>
        public bool IsEntityProperty
        {
            get { return _isEntityProperty; }
            set { _isEntityProperty = value; }
        }

        

        private EntityMappingInfo _mapInfo;
        /// <summary>
        /// 关联的映射
        /// </summary>
        public EntityMappingInfo MapInfo
        {
            get { return _mapInfo; }
            set { _mapInfo = value; }
        }

        /// <summary>
        /// 更新属性
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
        /// 清空父属性
        /// </summary>
        private string ClearParentProperty(EntityBase entity)
        {
            _mapInfo.SetValue(entity, null);
            entity._dicUpdateProperty___.Remove(_mapInfo.PropertyName);
            return null;
        }

        /// <summary>
        /// 更新子属性
        /// </summary>
        /// <param name="mapInfo"></param>
        private string UpdateChildProperty(EntityBase entity)
        {

            object parentObject = _mapInfo.GetValue(entity);
            object pkValue = null;
            if (parentObject != null)
            {
                pkValue = _mapInfo.TargetProperty.GetValue(parentObject);//获取ID
            }
            else 
            {
                pkValue = _mapInfo.SourceProperty.FieldType.IsValueType ? Activator.CreateInstance(_mapInfo.SourceProperty.FieldType) : null;
            }
            _mapInfo.SourceProperty.SetValue(entity, pkValue);
            return _mapInfo.TargetProperty.PropertyName;

        }
    }
}
