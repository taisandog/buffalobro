using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB.ProxyBase
{
    /// <summary>
    /// Mongo数据库实体基类
    /// </summary>
    public class MongoEntityBase
    {
        private MongoEntityBaseInfo ___baseInfo___ = null;

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <returns></returns>
        public MongoEntityBaseInfo GetEntityBaseInfo()
        {
            if (___baseInfo___ != null)
            {
                return ___baseInfo___;
            }
            ___baseInfo___ = new MongoEntityBaseInfo(this);
            return ___baseInfo___;
        }
       
        /// <summary>
        /// 通知属性已经被修改
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal virtual void OnPropertyUpdated(string propertyName)
        {
            GetEntityBaseInfo()._dicUpdateProperty___[propertyName] = true;
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
                return GetEntityBaseInfo().EntityInfo.PropertyInfo[propertyName].PropertyHandle.GetValue(this);
            }
            set
            {
                GetEntityBaseInfo().EntityInfo.PropertyInfo[propertyName].PropertyHandle.SetValue(this, value);
            }
        }
    }
}
