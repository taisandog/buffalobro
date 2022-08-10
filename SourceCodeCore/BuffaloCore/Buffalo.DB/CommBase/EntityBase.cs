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
    /// ʵ�����
    /// </summary>
    public class EntityBase
    {
        private EntityBaseInfo ___baseInfo___=null;

        /// <summary>
        /// ��ȡʵ����Ϣ
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
        ///// �����б����Ա����ӳټ���ʱ�����
        ///// </summary>
        //internal IList GetBaseList()
        //{
        //     return _baseListInfo;
        //}
        ///// <summary>
        ///// �����б����Ա����ӳټ���ʱ�����
        ///// </summary>
        //internal void SetBaseList(IList lst)
        //{
        //     _baseListInfo=lst;
        //}

        /// <summary>
        /// ֪ͨ�����Ѿ����޸�
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal virtual void OnPropertyUpdated(string propertyName) 
        {
            GetEntityBaseInfo()._dicUpdateProperty___[propertyName] = true;
        }

        /// <summary>
        /// ֪ͨӳ�������Ѿ����޸�
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
        ///// ֪ͨ�����Ѿ��޸�
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
        /// ������������ȡ���������Ե�ֵ
        /// </summary>
        /// <param name="propertyName">������</param>
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
        /// �������
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void FillChild(string propertyName)
        {
            MappingContorl.FillChildList(propertyName, this);
        }

        /// <summary>
        /// ��丸��
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void FillParent(string propertyName)
        {
            MappingContorl.FillParent(propertyName, this);
        }

        /// <summary>
        /// ���������ʱ��
        /// </summary>
        protected internal virtual void OnFillChild(string propertyName, ScopeList lstScope) 
        {

        }

        /// <summary>
        /// ���������ʱ��
        /// </summary>
        protected internal virtual void OnFillParent(string propertyName, ScopeList lstScope)
        {

        } 

        /// <summary>
        /// ��DataReader����
        /// </summary>
        /// <param name="reader"></param>
        public void LoadFromReader(IDataReader reader)
        {
            EntityInfoHandle entityInfo = GetEntityBaseInfo().EntityInfo;
            CacheReader.FillInfoFromReader(reader, entityInfo, this);
        }

        ///// <summary>
        ///// �ѱ�����ֶο�����Ŀ����
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

        

        //#region ICloneable ��Ա

        ///// <summary>
        ///// ��������
        ///// </summary>
        ///// <param name="allPropertyUpdate">�Ƿ�ʶ��Ϊ�������Զ����޸�</param>
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
