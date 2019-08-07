using Buffalo.DB.EntityInfos;
using Buffalo.Kernel.Defaults;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CommBase
{
    /// <summary>
    /// 属性实体信息
    /// </summary>
    public class EntityBaseInfo
    {
        internal EntityInfoHandle _thisInfo = null;//当前类的信息
        private EntityBase _entity;
        internal ConcurrentDictionary<string, bool> _dicUpdateProperty___ = new ConcurrentDictionary<string, bool>();//记录被修改过值的属性
        internal ConcurrentDictionary<string, bool> _dicFilledParent___ = new ConcurrentDictionary<string, bool>();//记录被查询过的父表属性


        private bool _hasPKValue;
        /// <summary>
        /// 主键是否被赋值
        /// </summary>
        internal bool HasPKValue
        {
            get { return _hasPKValue; }
            set { _hasPKValue = value; }
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
        /// <summary>
        /// 实体信息
        /// </summary>
        /// <param name="type">实体类型</param>
        public EntityBaseInfo(EntityBase entity) 
        {
            _entity = entity;
            _thisInfo = EntityInfoManager.GetEntityHandle(CH.GetRealType(entity));
        }
        /// <summary>
        /// 获取属性是否已经被修改
        /// </summary>
        /// <param name="propertyName"></param>
        public bool HasPropertyChange(string propertyName)
        {
            return _dicUpdateProperty___.ContainsKey(propertyName);
        }
        /// <summary>
        /// 判断是否有此属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public bool HasProperty(string propertyName)
        {
            return EntityInfo.PropertyInfo[propertyName] != null;
        }
        /// <summary>
        /// 获取当前实体的信息
        /// </summary>
        /// <returns></returns>
        public EntityInfoHandle EntityInfo
        {
            get
            {
                return _thisInfo;
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
                EntityInfoHandle eHandle = EntityInfo;
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
                    _dicUpdateProperty___[proName] = true;
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
            bool ret = false;
            foreach (object oproName in propertys)
            {
                string proName = oproName as string;
                if (!string.IsNullOrEmpty(proName))
                {
                    _dicUpdateProperty___.TryRemove(proName,out ret);
                }
            }
        }

    }
}
