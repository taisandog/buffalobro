using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using System.Reflection;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel.FastReflection.ClassInfos;
using Buffalo.DB.DataFillers;
using Buffalo.Kernel.Defaults;
using System.Data;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;
namespace Buffalo.DB.CommBase
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class EntityBase
    {
        private EntityBaseInfo ___baseInfo___=null;

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <returns></returns>
        public EntityBaseInfo GetEntityBaseInfo() 
        {
            if (___baseInfo___ != null) 
            {
                return ___baseInfo___;
            }
            ___baseInfo___ = new EntityBaseInfo(this);
            return ___baseInfo___;
        }
        //private IList _baseListInfo;
        ///// <summary>
        ///// 所属列表集合以便在延迟加载时候读出
        ///// </summary>
        //internal IList GetBaseList()
        //{
        //     return _baseListInfo;
        //}
        ///// <summary>
        ///// 所属列表集合以便在延迟加载时候读出
        ///// </summary>
        //internal void SetBaseList(IList lst)
        //{
        //     _baseListInfo=lst;
        //}

        /// <summary>
        /// 通知属性已经被修改
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal virtual void OnPropertyUpdated(string propertyName) 
        {
            GetEntityBaseInfo()._dicUpdateProperty___[propertyName] = true;
        }

        /// <summary>
        /// 通知映射属性已经被修改
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal virtual void OnMapPropertyUpdated(string propertyName) 
        {

            EntityInfoHandle entityInfo = GetEntityBaseInfo().EntityInfo;
            UpdatePropertyInfo updateInfo = entityInfo.GetUpdatePropertyInfo(propertyName);
            if (updateInfo != null)
            {
                string updatePropertyName = updateInfo.UpdateProperty(this);
                if (!string.IsNullOrEmpty(updatePropertyName))
                {
                    OnPropertyUpdated(updatePropertyName);
                }
                else 
                {
                    OnPropertyUpdated(propertyName);
                }
            }
        }

        ///// <summary>
        ///// 通知主键已经修改
        ///// </summary>
        ///// <returns></returns>
        //internal bool PrimaryKeyChange() 
        //{
        //    EntityInfoHandle entityInfo = GetEntityInfo();
        //    List<EntityPropertyInfo> lstPk = entityInfo.PrimaryProperty;
        //    if (lstPk == null || lstPk.Count == 0) 
        //    {
        //        return false;
        //    }
        //    foreach (EntityPropertyInfo pinfo in lstPk) 
        //    {
        //        OnPropertyUpdated(pinfo.PropertyName);
        //    }
        //    return true;
        //}

        /// <summary>
        /// 根据属性名获取或设置属性的值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public object this[string propertyName]
        {
            get
            {
                return GetEntityBaseInfo().EntityInfo.PropertyInfo[propertyName].GetValue(this);
            }
            set 
            {
                GetEntityBaseInfo().EntityInfo.PropertyInfo[propertyName].SetValue(this, value);
            }
        }

        

        

        /// <summary>
        /// 填充子类
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void FillChild(string propertyName)
        {
            MappingContorl.FillChildList(propertyName, this);
        }

        /// <summary>
        /// 填充父类
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void FillParent(string propertyName)
        {
            MappingContorl.FillParent(propertyName, this);
        }

        /// <summary>
        /// 当填充子类时候
        /// </summary>
        protected internal virtual void OnFillChild(string propertyName, ScopeList lstScope) 
        {

        }

        /// <summary>
        /// 当填充子类时候
        /// </summary>
        protected internal virtual void OnFillParent(string propertyName, ScopeList lstScope)
        {

        } 

        /// <summary>
        /// 从DataReader加载
        /// </summary>
        /// <param name="reader"></param>
        public void LoadFromReader(IDataReader reader)
        {
            EntityInfoHandle entityInfo = GetEntityBaseInfo().EntityInfo;
            CacheReader.FillInfoFromReader(reader, entityInfo, this);
        }

        ///// <summary>
        ///// 把本类的字段拷贝到目标类
        ///// </summary>
        ///// <param name="target"></param>
        //public void CopyTo(object target)
        //{
        //    FieldCloneHelper.CopyTo(this, target);
        //    EntityBase tar = target as EntityBase;
        //    if (tar != null)
        //    {
        //        tar._dicUpdateProperty___.Clear();

        //        foreach (KeyValuePair<string, bool> kvp in this._dicUpdateProperty___)
        //        {
        //            tar._dicUpdateProperty___[kvp.Key] = kvp.Value;
        //        }

        //    }
        //}

        

        //#region ICloneable 成员

        ///// <summary>
        ///// 拷贝对象
        ///// </summary>
        ///// <param name="allPropertyUpdate">是否识别为所有属性都被修改</param>
        ///// <returns></returns>
        //public object Clone()
        //{
        //    object target = CH.Create(this.GetType());

        //    CopyTo(target);

        //    return target;
        //}
        //#endregion
    }
}
