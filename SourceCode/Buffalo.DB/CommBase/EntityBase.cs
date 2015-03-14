using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using Buffalo.DB.ListExtends;
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
    public class EntityBase:ICloneable
    {
        private EntityInfoHandle _thisInfo = null;//当前类的信息
        internal Dictionary<string, bool> _dicUpdateProperty___ = new Dictionary<string,bool>();//记录被修改过值的属性

        /// <summary>
        /// 修改过的属性名
        /// </summary>
        public List<string> GetChangedPropertyName()
        {
            List<string> pNames = new List<string>(_dicUpdateProperty___.Count);
            foreach (KeyValuePair<string, bool> kvp in _dicUpdateProperty___) 
            {
                pNames.Add(kvp.Key);
            }
            return pNames;
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
            _dicUpdateProperty___[propertyName] = true;
        }

        /// <summary>
        /// 通知映射属性已经被修改
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal virtual void OnMapPropertyUpdated(string propertyName) 
        {

            EntityInfoHandle entityInfo = GetEntityInfo();
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
        /// 获取属性是否已经被修改
        /// </summary>
        /// <param name="propertyName"></param>
        public bool HasPropertyChange(string propertyName)
        {
            return _dicUpdateProperty___.ContainsKey(propertyName);
        }

        /// <summary>
        /// 获取当前实体的信息
        /// </summary>
        /// <returns></returns>
        public EntityInfoHandle GetEntityInfo()
        {
            if (_thisInfo == null)
            {
                _thisInfo = EntityInfoManager.GetEntityHandle(CH.GetRealType(this));
            }
            return _thisInfo;
        }

        /// <summary>
        /// 根据属性名获取或设置属性的值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public object this[string propertyName]
        {
            get
            {
                return GetEntityInfo().PropertyInfo[propertyName].GetValue(this);
            }
            set 
            {
                GetEntityInfo().PropertyInfo[propertyName].SetValue(this,value);
            }
        }

        /// <summary>
        /// 判断是否有此属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public bool HasProperty(string propertyName) 
        {
            return GetEntityInfo().PropertyInfo[propertyName] != null;
        }

        /// <summary>
        /// 是否可以拷贝属性的数值类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private bool IsAllowType(Type type)
        {
            return DefaultType.GetDefaultValue(type) != null;
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
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(CH.GetRealType(this));
            CacheReader.FillInfoFromReader(reader, entityInfo, this);
        }

        /// <summary>
        /// 把本类的字段拷贝到目标类
        /// </summary>
        /// <param name="target"></param>
        public void CopyTo(object target) 
        {
            FieldCloneHelper.CopyTo(this, target);
            EntityBase tar=target as EntityBase;
            if (tar != null)
            {
                tar._dicUpdateProperty___.Clear();

                foreach (KeyValuePair<string, bool> kvp in this._dicUpdateProperty___)
                {
                    tar._dicUpdateProperty___[kvp.Key] = kvp.Value;
                }

            }
        }

        /// <summary>
        /// 提交属性更新通知
        /// </summary>
        /// <param name="propertys">属性集合(null则所有属性都通知更新)</param>
        public void SubmitUpdateProperty(IEnumerable propertys) 
        {
            
            if (propertys == null)
            {
                EntityInfoHandle eHandle = GetEntityInfo();
                foreach (EntityPropertyInfo pinfo in eHandle.PropertyInfo)
                {
                    _dicUpdateProperty___[pinfo.PropertyName] = true;
                }
                return;
            }
            foreach (object oproName in propertys)
            {
                string proName = oproName as string;
                if (!string.IsNullOrEmpty(proName))
                {
                    _dicUpdateProperty___[proName]=true;
                }
            }
        }

        /// <summary>
        /// 撤销哪些属性的更新通知
        /// </summary>
        /// <param name="propertys">属性集合(null则所有属性的更新通知都被撤销)</param>
        public void CancelUpdateProperty(IEnumerable propertys) 
        {
            if (propertys == null) 
            {
                _dicUpdateProperty___.Clear();
                return;
            }
            foreach (object oproName in propertys) 
            {
                string proName = oproName as string;
                if (!string.IsNullOrEmpty(proName)) 
                {
                    _dicUpdateProperty___.Remove(proName);
                }
            }
        }

        #region ICloneable 成员

        /// <summary>
        /// 拷贝对象
        /// </summary>
        /// <param name="allPropertyUpdate">是否识别为所有属性都被修改</param>
        /// <returns></returns>
        public object Clone()
        {
            object target = CH.Create(this.GetType());

            CopyTo(target);

            return target;
        }
        #endregion
    }
}
