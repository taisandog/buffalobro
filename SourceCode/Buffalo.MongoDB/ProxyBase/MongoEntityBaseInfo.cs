using Buffalo.Kernel.FastReflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB.ProxyBase
{
    public class MongoEntityBaseInfo
    {
        internal MongoEntityInfo _thisInfo = null;//当前类的信息
        private MongoEntityBase _entity;
        internal Dictionary<string, bool> _dicUpdateProperty___ = new Dictionary<string, bool>();//记录被修改过值的属性

        // <summary>
        /// 实体信息
        /// </summary>
        /// <param name="type">实体类型</param>
        public MongoEntityBaseInfo(MongoEntityBase entity)
        {
            _entity = entity;
            _thisInfo = MongoEntityInfoManager.GetEntityHandle(entity.GetType());
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
        public MongoEntityInfo EntityInfo
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
                MongoEntityInfo eHandle = EntityInfo;
                foreach (MongoPropertyInfo pinfo in eHandle.PropertyInfo)
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
    }
}
